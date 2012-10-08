/*

CPU only tracker

*/

#include <Windows.h>

#include "cpu_tracker.h"
#include "../cudatrack/LsqQuadraticFit.h"



const float XCorScale = 0.5f;

CPUTracker::CPUTracker(int w, int h)
{
	width = w;
	height = h;

	xcorw = 128;

	X_xc.resize(xcorw);
	X_xcr.resize(xcorw);
	X_result.resize(xcorw);
	Y_xc.resize(xcorw);
	Y_xcr.resize(xcorw);
	Y_result.resize(xcorw);

	fft_revout = new complexf[xcorw];
	fft_out = new complexf[xcorw];

	fft_plan_fw = fftwf_plan_dft_r2c_1d(xcorw, &X_xc[0], (fftwf_complex*) fft_out, FFTW_ESTIMATE);
	fft_plan_bw = fftwf_plan_dft_c2r_1d(xcorw, (fftwf_complex*)fft_out, &X_result[0], FFTW_ESTIMATE);
	
	srcImage = new float [w*h];
}

CPUTracker::~CPUTracker()
{
	if (fft_plan_fw) 
		fftwf_destroy_plan(fft_plan_fw);
	if (fft_plan_bw)
		fftwf_destroy_plan(fft_plan_bw);

	delete[] fft_revout;
	delete[] fft_out;
	delete[] srcImage;
}

const inline float interp(float a, float b, float x) { return a + (b-a)*x; }

float CPUTracker::interpolate(float x,float y)
{
	int rx=x, ry=y;
	float v00 = getPixel(rx,ry);
	float v10 = getPixel(rx+1,ry);
	float v01 = getPixel(rx,ry+1);
	float v11 = getPixel(rx+1,ry+1);

	float v0 = interp (v00, v10, x-rx);
	float v1 = interp (v01, v11, x-rx);

	return interp (v0, v1, y-ry);
}

vector2f CPUTracker::ComputeXCor(vector2f initial, int iterations)
{
	// extract the image
	float scale = (1.0f/(XCorProfileLen*float(1<<16)-1));
	vector2f pos = initial;

	for (int k=0;k<iterations;k++) {

		float xmin = pos.x - XCorScale * xcorw/2;
		float ymin = pos.y - XCorScale * xcorw/2;

	//	dbgout(SPrintf("[%d]: xmin: %.1f, ymin: %.1f, \n", k, xmin, ymin));

		// generate X position xcor array (summing over y range)
		for (int x=0;x<xcorw;x++) {
			float s = 0.0f;
			for (int y=0;y<XCorProfileLen;y++)
				s += interpolate(x * XCorScale + xmin, pos.y + XCorScale * (y - XCorProfileLen/2));
			X_xc [x] = s*scale;
			X_xcr [xcorw-x-1] = X_xc[x];
		}

	//	dbgout(SPrintf("\t: X FFT\n"));

		XCorFFTHelper(&X_xc[0], &X_xcr[0], &X_result[0]);
		float offsetX = ComputeMaxInterp(X_result) - (float)xcorw/2 - 1;

//dbgout(SPrintf("\t: offsetX: %f\n", offsetX));

		// generate Y position xcor array (summing over x range)
		for (int y=0;y<xcorw;y++) {
			float s = 0.0f; 
			for (int x=0;x<XCorProfileLen;x++) 
				s += interpolate(pos.x + XCorScale * (x - XCorProfileLen/2), y * XCorScale + ymin);
			Y_xc[y] = s*scale;
			Y_xcr [xcorw-y-1] = Y_xc[y];
		}

	//	dbgout(SPrintf("\t: Y FFT\n", offsetX));
		XCorFFTHelper(&Y_xc[0], &Y_xcr[0], &Y_result[0]);
		float offsetY = ComputeMaxInterp(Y_result) - (float)xcorw/2 - 1;

	//	dbgout(SPrintf("[%d] offsetX: %f, offsetY: %f\n", k, offsetX, offsetY));
		pos.x -= offsetX * XCorScale;
		pos.y -= offsetY * XCorScale;

	}


	return pos;
}

void CPUTracker::XCorFFTHelper(float* xc, float* xcr, float* result)
{
	// need to optimize this: the DFT of the reverse sequence should be calculatable from the known DFT (right?)
	fftwf_execute_dft_r2c(fft_plan_fw, xc, (fftwf_complex*)fft_out);
	fftwf_execute_dft_r2c(fft_plan_fw, xcr, (fftwf_complex*)fft_revout);

	// Multiply with conjugate of reverse
	for (int x=0;x<xcorw;x++) {
		fft_out[x] *= complexf(fft_revout[x].real(), -fft_revout[x].imag());
	}

	fftwf_execute_dft_c2r(fft_plan_bw, (fftwf_complex*)fft_out, xc);
	for (int x=0;x<xcorw;x++)
		result[x] = xc[ (x+xcorw/2) % xcorw ];
}

float CPUTracker::ComputeMaxInterp(const std::vector<float>& r)
{
	uint iMax=0;
	float vMax=0;
	for (uint k=0;k<r.size();k++) {
		if (r[k]>vMax) {
			vMax = r[k];
			iMax = k;
		}
	}
	
	float xs[] = {-2, -1, 0, 1, 2};
	LsqSqQuadFit<float> qfit(5, xs, &r[iMax-2]);
	float interpMax = qfit.maxPos();

	return (float)iMax - interpMax * 0.5f;
}


vector2f CPUTracker::ComputeCOM(float median)
{
	float sum=0;
	float momentX=0;
	float momentY=0;

	for (uint y=0;y<height;y++)
		for(uint x=0;x<width;x++)
		{
			float v = getPixel(x,y)-median;
			v *= v;
			sum += v;
			momentX += x*v;
			momentY += y*v;
		}
	vector2f com;
	com.x = momentX / (float)sum;
	com.y = momentY / (float)sum;
	return com;
}



template<typename TPixel>
void normalize(TPixel* d, uint w,uint h)
{
	TPixel maxv = d[0];
	TPixel minv = d[0];
	for (uint k=0;k<w*h;k++) {
		maxv = max(maxv, d[k]);
		minv = min(minv, d[k]);
	}
	for (uint k=0;k<w*h;k++)
		d[k]=(d[k]-minv)/(maxv-minv);
}

void CPUTracker::Normalize()
{
	normalize(srcImage, width, height);
}


void CPUTracker::ComputeRadialProfile(float* dst, int radialSteps, int angularSteps, float range, vector2f center)
{
	if (radialDirs.size() != angularSteps) {
		radialDirs.resize(angularSteps);
		for (int j=0;j<angularSteps;j++) {
			float ang = 2*3.141593f*j/(float)angularSteps;
			vector2f d = { cosf(ang), sinf(ang) };
			radialDirs[j] = d;
		}
	}

	for (int i=0;i<radialSteps;i++)
		dst[i]=0.0f;

	float rstep = range / radialSteps;
	for (int i=0;i<radialSteps; i++) {
		float sum = 0.0f;

		for (int a=0;a<angularSteps;a++) {
			float x = center.x + radialDirs[a].x * rstep*i;
			float y = center.y + radialDirs[a].y * rstep*i;
			sum += interpolate(x,y)/float(1+i);
		}

		dst[i] = sum;
	}
}


ushort* floatToNormalizedUShort(float *data, uint w,uint h)
{
	float maxv = data[0];
	float minv = data[0];
	for (uint k=0;k<w*h;k++) {
		maxv = max(maxv, data[k]);
		minv = min(minv, data[k]);
	}
	ushort *norm = new ushort[w*h];
	for (uint k=0;k<w*h;k++)
		norm[k] = ((1<<16)-1) * (data[k]-minv) / (maxv-minv);
	return norm;
}


