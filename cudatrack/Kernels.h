#pragma once



template<typename T>
static __device__ T interpolate(T a, T b, float x) { return a + (b-a)*x; }

// Types used by QI algorithm
typedef float qivalue_t;
typedef float2 qicomplex_t;


template<typename TImageSampler>
__device__ void ComputeQuadrantProfile(cudaImageListf& images, int idx, float* dst, const QIParams& params, int quadrant, float2 center, float mean)
{
	const int qmat[] = {
		1, 1,
		-1, 1,
		-1, -1,
		1, -1 };
	int mx = qmat[2*quadrant+0];
	int my = qmat[2*quadrant+1];

	for (int i=0;i<params.radialSteps;i++)
		dst[i]=0.0f;
	
	float asf = (float)params.trigtablesize / params.angularSteps;
	float total = 0.0f;
	float rstep = (params.maxRadius - params.minRadius) / params.radialSteps;
	for (int i=0;i<params.radialSteps; i++) {
		float sum = 0.0f;
		float r = params.minRadius + rstep * i;
		int count=0;

		for (int a=0;a<params.angularSteps;a++) {
			int j = (int)(asf * a);
			float x = center.x + mx*params.cos_sin_table[j].x * r;
			float y = center.y + my*params.cos_sin_table[j].y * r;
			bool outside=false;
			float v = TImageSampler::Interpolated(images, x,y, idx, outside);
			if (!outside) {
				sum += v;
				count ++;
			}
		}

		dst[i] = count >= MIN_RADPROFILE_SMP_COUNT ? sum/count : mean;
		total += dst[i];
	}
}

template<typename TImageSampler>
__global__ void QI_ComputeProfile(int count, cudaImageListf images, float3* positions, float* quadrants, float2* profiles, float2* reverseProfiles, QIParams params, float *imgmeans)
{
	int idx = threadIdx.x + blockDim.x * blockIdx.x;
	if (idx < count) {
		int fftlen = params.radialSteps*2;
		float* img_qdr = &quadrants[ idx * params.radialSteps * 4 ];
		for (int q=0;q<4;q++) {
			ComputeQuadrantProfile<TImageSampler> (images, idx, &img_qdr[q*params.radialSteps], params, q, 
				make_float2(positions[idx].x, positions[idx].y), imgmeans[idx]);
		}

		int nr = params.radialSteps;
		qicomplex_t* imgprof = (qicomplex_t*) &profiles[idx * fftlen*2];
		qicomplex_t* x0 = imgprof;
		qicomplex_t* x1 = imgprof + nr*1;
		qicomplex_t* y0 = imgprof + nr*2;
		qicomplex_t* y1 = imgprof + nr*3;

		qicomplex_t* revprof = (qicomplex_t*)&reverseProfiles[idx*fftlen*2];
		qicomplex_t* xrev = revprof;
		qicomplex_t* yrev = revprof + nr*2;

		float* q0 = &img_qdr[0];
		float* q1 = &img_qdr[nr];
		float* q2 = &img_qdr[nr*2];
		float* q3 = &img_qdr[nr*3];

		// Build Ix = qL(-r) || qR(r)
		// qL = q1 + q2   (concat0)
		// qR = q0 + q3   (concat1)
		for(int r=0;r<nr;r++) {
			x0[nr-r-1] = make_float2(q1[r]+q2[r], 0);
			x1[r] = make_float2(q0[r]+q3[r],0);
		}

		// Build Iy = [ qB(-r)  qT(r) ]
		// qT = q0 + q1
		// qB = q2 + q3
		for(int r=0;r<nr;r++) {
			y1[r] = make_float2(q0[r]+q1[r],0);
			y0[nr-r-1] = make_float2(q2[r]+q3[r],0);
		}

		for(int r=0;r<nr*2;r++)
			xrev[r] = x0[nr*2-r-1];
		for(int r=0;r<nr*2;r++)
			yrev[r] = y0[nr*2-r-1];
	}
}


__global__ void QI_MultiplyWithConjugate(int n, cufftComplex* a, cufftComplex* b)
{
	int idx = threadIdx.x + blockIdx.x * blockDim.x;
	if (idx < n) {
		cufftComplex A = a[idx];
		cufftComplex B = b[idx];
	
		a[idx] = make_float2(A.x*B.x + A.y*B.y, A.y*B.x -A.x*B.y); // multiplying with conjugate
	}
}

__device__ float QI_ComputeAxisOffset(cufftComplex* autoconv, int fftlen, float* shiftbuf)
{
	typedef float compute_t;
	int nr = fftlen/2;
	for(int x=0;x<fftlen;x++)  {
		shiftbuf[x] = autoconv[(x+nr)%(nr*2)].x;
	}

	compute_t maxPos = ComputeMaxInterp<compute_t>::Compute(shiftbuf, fftlen);
	compute_t offset = (maxPos - nr) / (3.14159265359f * 0.5f);
	return offset;
}

__global__ void QI_OffsetPositions(int njobs, float3* current, float3* dst, cufftComplex* autoconv, int fftLength, float2* offsets, float pixelsPerProfLen, float* shiftbuf)
{
	int idx = threadIdx.x + blockIdx.x * blockDim.x;

	if (idx < njobs) {
		float* shifted = &shiftbuf[ idx * fftLength ];		

		// X
		cufftComplex* autoconvX = &autoconv[idx * fftLength * 2];
		float xoffset = QI_ComputeAxisOffset(autoconvX, fftLength, shifted);

		cufftComplex* autoconvY = autoconvX + fftLength;
		float yoffset = QI_ComputeAxisOffset(autoconvY, fftLength, shifted);

		dst[idx].x = current[idx].x + xoffset * pixelsPerProfLen;
		dst[idx].y = current[idx].y + yoffset * pixelsPerProfLen;

		if (offsets) 
			offsets[idx] = make_float2( xoffset, yoffset);
	}
}



/*
		q0: xprof[r], yprof[r]
		q1: xprof[len-r-1], yprof[r]
		q2: xprof[len-r-1], yprof[len-r-1]
		q3: xprof[r], yprof[len-r-1]

	kernel gets called with dim3(images.count, radialsteps, 4) elements
*/
template<typename TImageSampler>
__global__ void QI_ComputeQuadrants(int njobs, cudaImageListf images, float3* positions, float* dst_quadrants, const QIParams params, float* imgmeans)
{
	int jobIdx = threadIdx.x + blockIdx.x * blockDim.x;
	int rIdx = threadIdx.y + blockIdx.y * blockDim.y;
	int quadrant = threadIdx.z;

	if (jobIdx < njobs && rIdx < params.radialSteps && quadrant < 4) {

		const int qmat[] = {
			1, 1,
			-1, 1,
			-1, -1,
			1, -1 };

		int mx = qmat[2*quadrant+0];
		int my = qmat[2*quadrant+1];
		float* qdr = &dst_quadrants[ (jobIdx * 4 + quadrant) * params.radialSteps ];

		float rstep = (params.maxRadius - params.minRadius) / params.radialSteps;
		float sum = 0.0f;
		float r = params.minRadius + rstep * rIdx;
		float3 pos = positions[jobIdx];
//		float mean = imgmeans[jobIdx];

		int count=0;
		for (int a=0;a<params.angularSteps;a++) {
			float x = pos.x + mx*params.cos_sin_table[a].x * r;
			float y = pos.y + my*params.cos_sin_table[a].y * r;
			bool outside=false;
			sum += TImageSampler::Interpolated(images, x,y,jobIdx, outside);
			if (!outside) count++;
		}
		qdr[rIdx] = count>MIN_RADPROFILE_SMP_COUNT ? sum/count : imgmeans[jobIdx];
	}
}



__global__ void QI_QuadrantsToProfiles(int njobs, cudaImageListf images, float* quadrants, float2* profiles, float2* reverseProfiles, const QIParams params)
{
//ComputeQuadrantProfile(cudaImageListf& images, int idx, float* dst, const QIParams& params, int quadrant, float2 center)
	int idx = threadIdx.x + blockDim.x * blockIdx.x;
	if (idx < njobs) {
		int fftlen = params.radialSteps*2;
		float* img_qdr = &quadrants[ idx * params.radialSteps * 4 ];
	//	for (int q=0;q<4;q++)
			//ComputeQuadrantProfile<TImageSampler> (images, idx, &img_qdr[q*params.radialSteps], params, q, img_means[idx], make_float2(positions[idx].x, positions[idx].y));

		int nr = params.radialSteps;
		qicomplex_t* imgprof = (qicomplex_t*) &profiles[idx * fftlen*2];
		qicomplex_t* x0 = imgprof;
		qicomplex_t* x1 = imgprof + nr*1;
		qicomplex_t* y0 = imgprof + nr*2;
		qicomplex_t* y1 = imgprof + nr*3;

		qicomplex_t* revprof = (qicomplex_t*)&reverseProfiles[idx*fftlen*2];
		qicomplex_t* xrev = revprof;
		qicomplex_t* yrev = revprof + nr*2;

		float* q0 = &img_qdr[0];
		float* q1 = &img_qdr[nr];
		float* q2 = &img_qdr[nr*2];
		float* q3 = &img_qdr[nr*3];

		// Build Ix = qL(-r) || qR(r)
		// qL = q1 + q2   (concat0)
		// qR = q0 + q3   (concat1)
		for(int r=0;r<nr;r++) {
			x0[nr-r-1] = make_float2(q1[r]+q2[r],0);
			x1[r] = make_float2(q0[r]+q3[r],0);
		}
		// Build Iy = [ qB(-r)  qT(r) ]
		// qT = q0 + q1
		// qB = q2 + q3
		for(int r=0;r<nr;r++) {
			y1[r] = make_float2(q0[r]+q1[r],0);
			y0[nr-r-1] = make_float2(q2[r]+q3[r],0);
		}

		for(int r=0;r<nr*2;r++)
			xrev[r] = x0[nr*2-r-1];
		for(int r=0;r<nr*2;r++)
			yrev[r] = y0[nr*2-r-1];
	}
}

template<typename TImageSampler>
__device__ float2 BgCorrectedCOM(int idx, cudaImageListf images, float correctionFactor, float* pMean)
{
	int imgsize = images.w*images.h;
	float sum=0, sum2=0;
	float momentX=0;
	float momentY=0;

	for (int y=0;y<images.h;y++)
		for (int x=0;x<images.w;x++) {
			float v = TImageSampler::Index(images, x, y, idx);
			sum += v;
			sum2 += v*v;
		}

	float invN = 1.0f/imgsize;
	float mean = sum * invN;
	float stdev = sqrtf(sum2 * invN - mean * mean);
	sum = 0.0f;

	for (int y=0;y<images.h;y++)
		for(int x=0;x<images.w;x++)
		{
			float v = TImageSampler::Index(images, x,y,idx);
			v = fabsf(v-mean)-correctionFactor*stdev;
			if(v<0.0f) v=0.0f;
			sum += v;
			momentX += x*v;
			momentY += y*v;
		}

	if (pMean)
		*pMean = mean;

	float2 com;
	com.x = momentX / (float)sum;
	com.y = momentY / (float)sum;
	return com;
}

template<typename TImageSampler>
__global__ void BgCorrectedCOM(int count, cudaImageListf images,float3* d_com, float bgCorrectionFactor, float* d_imgmeans) {
	int idx = threadIdx.x + blockDim.x * blockIdx.x;
	if (idx < count) {
		float mean;
		float2 com = BgCorrectedCOM<TImageSampler> (idx, images, bgCorrectionFactor, &mean);
		d_com[idx] = make_float3(com.x,com.y,0.0f);
		d_imgmeans[idx] = mean;
	}
}

__global__ void ZLUT_ProfilesToZLUT(int njobs, cudaImageListf images, ZLUTParams params, float3* positions, LocalizationParams* locParams, float* profiles)
{
	int idx = threadIdx.x + blockDim.x * blockIdx.x;

	if (idx < njobs) {
		auto m = locParams[idx];
		if (m.locType & LT_BuildZLUT) {
			float* dst = params.GetZLUT(m.zlutIndex, m.zlutPlane );

			for (int i=0;i<params.radialSteps();i++)
				dst [i] = profiles [ params.radialSteps()*idx + i ];
		}
	}
}

/*

void ComputeRadialProfile(float* dst, int radialSteps, int angularSteps, float minradius, float maxradius,
	vector2f center, ImageData* img, float paddingValue)
{
	vector2f* radialDirs = (vector2f*)ALLOCA(sizeof(vector2f)*angularSteps);
	for (int j=0;j<angularSteps;j++) {
		float ang = 2*3.141593f*j/(float)angularSteps;
		radialDirs[j] = vector2f(cosf(ang), sinf(ang));
	}

	for (int i=0;i<radialSteps;i++)
		dst[i]=0.0f;

	double totalrmssum2 = 0.0f, totalsum=0.0;
	float rstep = (maxradius-minradius) / radialSteps;
	int totalsmp = 0;
	for (int i=0;i<radialSteps; i++) {
		double sum = 0.0f;

		int nsamples = 0;
		float r = minradius+rstep*i;
		for (int a=0;a<angularSteps;a++) {
			float x = center.x + radialDirs[a].x * r;
			float y = center.y + radialDirs[a].y * r;
			bool outside;
			float v = img->interpolate(x,y, &outside);
			if (!outside) {
				sum += v;
				nsamples++;
			}
		}

		dst[i] = sum/nsamples;
		totalsum += sum;
		totalsmp += nsamples;
	}
	float substr = totalsum/totalsmp;
	for (int i=0;i<radialSteps;i++)
		dst[i] -= substr;
	double sum=0.0f;
	for (int i=0;i<radialSteps;i++)
		totalrmssum2 += dst[i]*dst[i];
//		sum += dst[i];
	double invSum = 1.0/sum;
	//	totalrmssum2 += dst[i]*dst[i];
	double invTotalrms = 1.0f/sqrt(totalrmssum2/radialSteps);
	for (int i=0;i<radialSteps;i++) {
		dst[i] *= invTotalrms;
	}
}

*/

// Compute a single ZLUT radial profile element (looping through all the pixels at constant radial distance)
template<typename TImageSampler>
__global__ void ZLUT_RadialProfileKernel(int njobs, cudaImageListf images, ZLUTParams params, float3* positions, float* profiles, float* means)
{
	int jobIdx = threadIdx.x + blockIdx.x * blockDim.x;
	int radialIdx = threadIdx.y + blockIdx.y * blockDim.y;

	if (jobIdx >= njobs || radialIdx >= params.radialSteps()) 
		return;

	float* dstprof = &profiles[params.radialSteps() * jobIdx];
	float r = params.minRadius + (params.maxRadius-params.minRadius)*radialIdx/params.radialSteps();
	float sum = 0.0f;
	int count = 0;
	
	for (int i=0;i<params.angularSteps;i++) {
		float x = positions[jobIdx].x + params.trigtable[i].x * r;
		float y = positions[jobIdx].y + params.trigtable[i].y * r;

		bool outside=false;
		sum += TImageSampler::Interpolated(images, x,y, jobIdx, outside);
		if (!outside) count++;
	}
	dstprof [radialIdx] = count>MIN_RADPROFILE_SMP_COUNT ? sum/count : means[jobIdx];
}


__global__ void ZLUT_ComputeZ (int njobs, ZLUTParams params, float3* positions, float* compareScoreBuf, LocalizationParams *locParams)
{
	int jobIdx = threadIdx.x + blockIdx.x * blockDim.x;

	if (jobIdx < njobs && (locParams[jobIdx].locType & LT_LocalizeZ)) {
		float* cmp = &compareScoreBuf [params.planes * jobIdx];

		float maxPos = ComputeMaxInterp<float>::Compute(cmp, params.planes);
		positions[jobIdx].z = maxPos;
	}
}

__global__ void ZLUT_ComputeProfileMatchScores(int njobs, ZLUTParams params, float *profiles, float* compareScoreBuf, LocalizationParams *locParams)
{
	int jobIdx = threadIdx.x + blockIdx.x * blockDim.x;
	int zPlaneIdx = threadIdx.y + blockIdx.y * blockDim.y;

	if (jobIdx >= njobs || zPlaneIdx >= params.planes)
		return;

	float* prof = &profiles [jobIdx * params.radialSteps()];
	auto mapping = locParams[jobIdx];
	if (mapping.locType & LT_LocalizeZ) {
		float diffsum = 0.0f;
		for (int r=0;r<params.radialSteps();r++) {
			float d = prof[r] - params.img.pixel(r, zPlaneIdx, mapping.zlutIndex);
			if (params.zcmpwindow)
				d *= params.zcmpwindow[r];
			diffsum += d*d;
		}

		compareScoreBuf[ params.planes * jobIdx + zPlaneIdx ] = -diffsum;
	}
}

__global__ void ZLUT_NormalizeProfiles(int njobs, ZLUTParams params, float* profiles)
{
	int jobIdx = threadIdx.x + blockIdx.x * blockDim.x;

	if (jobIdx < njobs) {
		float* prof = &profiles[params.radialSteps()*jobIdx];

		// First, subtract mean
		float mean = 0.0f;
		for (int i=0;i<params.radialSteps();i++) {
			mean += prof[i];
		}
		mean /= params.radialSteps();

		float rmsSum2 = 0.0f;
		for (int i=0;i<params.radialSteps();i++){
			prof[i] -= mean;
			rmsSum2 += prof[i]*prof[i];
		}

		// And make RMS power equal 1
		float invTotalRms = 1.0f / sqrt(rmsSum2/params.radialSteps());
		for (int i=0;i<params.radialSteps();i++)
			prof[i] *= invTotalRms;
	}
}


__global__ void ApplyOffsetGain (cudaImageListf images, LocalizationParams* locParams, cudaImageListf calib_gain, cudaImageListf calib_offset)
{
	int x = threadIdx.x + blockIdx.x * blockDim.x;
	int y = threadIdx.y + blockIdx.y * blockDim.y;
	int jobIdx = threadIdx.z + blockIdx.z * blockDim.z;

	int bead = locParams[jobIdx].zlutIndex;

	float value = images.pixel(x,y,jobIdx);
	images.pixel(x,y,jobIdx) = calib_gain.pixel(x,y,bead) * value + calib_offset.pixel(x,y,bead);
}

// Simple gaussian 2D MLE implementation. A better solution would be to distribute CUDA threads over each pixel, but this is a very straightforward implementation
template<typename TImageSampler>
__global__ void G2MLE_Compute(int njobs, cudaImageListf images, LocalizationParams* locParam, float sigma, int iterations, float* imgmeans, float3* initial, float3 *positions, float* I_bg, float* I_0)
{
	int jobIdx = threadIdx.x + blockIdx.x * blockDim.x;

	if (jobIdx >= njobs)
		return;
	int loc2d = locParam[jobIdx].locType&LT_2DMask;
	// this is based on the idea that usually, the localization type will be the same for each task. If not, this code will have really bad performance
	if (loc2d != LT_Gaussian2D && loc2d != LT_Gaussian2DSigmaFit) 
		return;

	float2 pos = make_float2(initial[jobIdx].x, initial[jobIdx].y);
	float mean = imgmeans[jobIdx];
	float I0 = mean*0.5f*images.w*images.h;
	float bg = mean*0.5f;

	const float _1oSq2Sigma = 1.0f / (sqrtf(2) * sigma);
	const float _1oSq2PiSigma = (1.0f / (sqrtf(2*3.14159265359f))) / sigma;
	const float _1oSq2PiSigma3 = (1.0f / (sqrtf(2*3.14159265359f))) / (sigma*sigma*sigma);

	for (int i=0;i<iterations;i++)
	{
		float dL_dx = 0.0; 
		float dL_dy = 0.0; 
		float dL_dI0 = 0.0;
		float dL_dIbg = 0.0;
		float dL2_dx = 0.0;
		float dL2_dy = 0.0;
		float dL2_dI0 = 0.0;
		float dL2_dIbg = 0.0;
				
		for (int y=0;y<images.h;y++)
		{
			for (int x=0;x<images.w;x++)
			{
		        float Xexp0 = (x-pos.x + .5f) * _1oSq2Sigma;
				float Yexp0 = (y-pos.y + .5f) * _1oSq2Sigma;
        
				float Xexp1 = (x-pos.x - .5f) * _1oSq2Sigma;
				float Yexp1 = (y-pos.y - .5f) * _1oSq2Sigma;
				
				float DeltaX = 0.5f * erff(Xexp0) - 0.5f * erff(Xexp1);
				float DeltaY = 0.5f * erff(Yexp0) - 0.5f * erff(Yexp1);
				float mu = bg + I0 * DeltaX * DeltaY;
				
				float dmu_dx = I0*_1oSq2PiSigma * ( expf(-Xexp1*Xexp1) - expf(-Xexp0*Xexp0)) * DeltaY;

				float dmu_dy = I0*_1oSq2PiSigma * ( expf(-Yexp1*Yexp1) - expf(-Yexp0*Yexp0)) * DeltaX;
				float dmu_dI0 = DeltaX*DeltaY;
				float dmu_dIbg = 1;
        
				float smp = TImageSampler::Index(images, x,y, jobIdx);
				float f = smp / mu - 1;
				dL_dx += dmu_dx * f;
				dL_dy += dmu_dy * f;
				dL_dI0 += dmu_dI0 * f;
				dL_dIbg += dmu_dIbg * f;

				float d2mu_dx = I0*_1oSq2PiSigma3 * ( (x - pos.x - .5f) * expf (-Xexp1*Xexp1) - (x - pos.x + .5) * expf(-Xexp0*Xexp0) ) * DeltaY;
				float d2mu_dy = I0*_1oSq2PiSigma3 * ( (y - pos.y - .5f) * expf (-Yexp1*Yexp1) - (y - pos.y + .5) * expf(-Yexp0*Yexp0) ) * DeltaX;
				dL2_dx += d2mu_dx * f - dmu_dx*dmu_dx * smp / (mu*mu);
				dL2_dy += d2mu_dy * f - dmu_dy*dmu_dy * smp / (mu*mu);
				dL2_dI0 += -dmu_dI0*dmu_dI0 * smp / (mu*mu);
				dL2_dIbg += -smp / (mu*mu);
			}
		}

		pos.x -= dL_dx / dL2_dx;
		pos.y -= dL_dy / dL2_dy;
		I0 -= dL_dI0 / dL2_dI0;
		bg -= dL_dIbg / dL2_dIbg;
	}
	

	positions[jobIdx].x = pos.x;
	positions[jobIdx].y = pos.y;
	if (I_bg) I_bg[jobIdx] = bg;
	if (I_0) I_0[jobIdx] = I0;

	return r;
}
