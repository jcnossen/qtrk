#pragma once

#include "std_incl.h"

template<typename T> bool isNAN(const T& v) { 
	return !(v == v); 
}

void dbgout(const std::string& s);
std::string SPrintf(const char *fmt, ...);
void dbgprintf(const char *fmt,...);
void dbgsetlogfile(const char*path);

template<typename T>
void DeleteAllElems(T& c) {
	for(typename T::iterator i=c.begin();i!=c.end();++i)
		delete *i;
	c.clear();
}


template<typename TPixel>
void normalize(TPixel* d, uint w,uint h)
{
	TPixel maxv = d[0];
	TPixel minv = d[0];
	for (uint k=0;k<w*h;k++) {
		maxv = std::max(maxv, d[k]);
		minv = std::min(minv, d[k]);
	}
	for (uint k=0;k<w*h;k++)
		d[k]=(d[k]-minv)/(maxv-minv);
}

inline float Lerp(float a, float b, float x) { return a + (b-a)*x; }

inline float Interpolate(float* image, int width, int height, float x,float y, bool* outside=0)
{
	int rx=x, ry=y;
	if (rx<0 || ry <0 || rx+1 >= width || ry+1>=height) {
		if (outside) *outside=true;
		return 0.0f;
	}
	if (outside) *outside=false;

	float v00 = image[width*ry+rx];
	float v10 = image[width*ry+rx+1];
	float v01 = image[width*(ry+1)+rx];
	float v11 = image[width*(ry+1)+rx+1];

	float v0 = Lerp(v00, v10, x-rx);
	float v1 = Lerp(v01, v11, x-rx);

	return Lerp(v0, v1, y-ry);
}

template<typename T>
inline T Interpolate1D(T* d, int len, float x)
{
	int fx = (int)x;
	if (fx < 0) return d[0];
	if (fx >= len-1) return d[len-1];
	return (d[fx+1]-d[fx]) * (x-fx) + d[fx];
}

template<typename T>
inline T Interpolate1D(const std::vector<T>& d, float x)
{
	return Interpolate1D(&d[0],d.size(),x);
}

struct ImageData;

void WriteImageAsCSV(const char* file, float* d, int w,int h, const char *labels[]=0);

struct ImageData {
	float* data;
	int w,h;
	ImageData() { data=0;w=h=0;}
	ImageData(float *d, int w, int h) : data(d), w(w),h(h) {}
	float& at(int x, int y) { return data[w*y+x]; }
	float interpolate(float x, float y, bool *outside=0) { return Interpolate(data, w,h, x,y,outside); }
	int numPixels() { return w*h; }
	int pitch() { return sizeof(float)*w; } // bytes per line
	void normalize() { ::normalize(data,w,h); }
	float mean() {
		float s=0.0f;
		for(int x=0;x<w*h;x++)
			s+=data[x];
		return s/(w*h);
	}
	float& operator[](int i) { return data[i]; }

	static ImageData alloc(int w,int h) { return ImageData(new float[w*h], w,h); }
	void free() { delete[] data; }
	void writeAsCSV(const char *filename, const char *labels[]=0) { WriteImageAsCSV(filename, data, w,h,labels); }
};

std::vector<float> ComputeStetsonWindow(int rsteps);
float ComputeBgCorrectedCOM1D(float *data, int len, float cf=2.0f);
void ComputeCRP(float* dst, int radialSteps, int angularSteps, float minradius, float maxradius, vector2f center, ImageData* src,float mean, float*crpmap=0);
void ComputeRadialProfile(float* dst, int radialSteps, int angularSteps, float minradius, float maxradius, vector2f center, ImageData* src, float mean, bool normalize);
void NormalizeRadialProfile(float* prof, int rsteps);
void NormalizeZLUT(float *zlut, int numLUTs, int planes, int radialsteps);
void GenerateImageFromLUT(ImageData* image, ImageData* zlut, float minradius, float maxradius, vector3f pos);
void ApplyPoissonNoise(ImageData& img, float poissonMax, float maxValue=255);
void ApplyGaussianNoise(ImageData& img, float sigma);
void WriteComplexImageAsCSV(const char* file, std::complex<float>* d, int w,int h, const char *labels[]=0);
void WriteArrayAsCSVRow(const char *file, float* d, int len, bool append);
std::vector< std::vector<float> > ReadCSV(const char *filename, char sep='\t');
std::vector<vector3f> ReadVector3CSV( const char *file, char sep='\t');

void WriteTrace(std::string file, vector3f* results, int nResults);
void GenerateTestImage(ImageData& img, float xp, float yp, float size, float MaxPhotons);

std::string GetLocalModuleFilename();
std::string GetLocalModulePath();
std::string GetDirectoryFromPath(std::string fullpath);

struct PathSeperator {
	PathSeperator(std::string fullpath);
	std::string filename, extension, directory;
};

std::string file_ext(const char *f);

ImageData ReadJPEGFile(const char *fn);
int ReadJPEGFile(uchar* srcbuf, int srclen, uchar** data, int* width, int*height);
void WriteJPEGFile(uchar* data,int w,int h, const char * filename, int quality);
void FloatToJPEGFile (const char *name, const float* d, int w,int h);
inline void WriteJPEGFile(const char *name, const ImageData& img) { FloatToJPEGFile(name, img.data, img.w,img.h); }
int NearestPowerOf2(int v);
int NearestPowerOf3(int v);
void GenerateGaussianSpotImage(ImageData* img, vector2f pos, float sigma, float I0, float Ibg);

std::vector<uchar> ReadToByteBuffer(const char* filename);
double GetPreciseTime();

template<typename T>
void floatToNormalizedInt(T* dst, const float *src, uint w,uint h, T maxValue)
{
	float maxv = src[0];
	float minv = src[0];
	for (uint k=0;k<w*h;k++) {
		maxv = std::max(maxv, src[k]);
		minv = std::min(minv, src[k]);
	}
	for (uint k=0;k<w*h;k++)
		dst[k] = maxValue * (src[k]-minv) / (maxv-minv);
}


template<typename T>
T* floatToNormalizedInt(const float *src, uint w,uint h, T maxValue)
{ 
	T* r = new T[w*h]; 
	floatToNormalizedInt(r,src,w,h, maxValue);
	return r; 
}

template<typename T>
T ComputeStdDev(T* data, int len)
{
	T sum = 0.0, sum2=0.0;
	for (int a=0;a<len;a++) {
		sum+=data[a];
		sum2+=data[a]*data[a];
	}
	T mean = sum / len;
	return sqrt(sum2 / len- mean * mean);
}

template<typename T>
T qselect(T* data, int start, int end, int k)
{
	if (end-start==1)
		return data[start];

	// select one of the elements as pivot
	int p = 0;
	T value = data[p+start];
	// swap with last value
	std::swap(data[p+start], data[end-1]);

	// move all items < pivot to the left
	int nSmallerItems=0;
	for(int i=start;i<end-1;i++)
		if(data[i]<value) {
			std::swap(data[i], data[start+nSmallerItems]);
			nSmallerItems++;
		}
	// pivot is now at [# items < pivot]
	std::swap(data[start+nSmallerItems], data[end-1]);

	// we are trying to find the kth element
	// so if pivotpos == k, we found it
	// if k < pivotpos, we need to recurse left side
	// if k > pivotpos, we need to recurse right side
	int pivotpos = start+nSmallerItems;
	if (k == pivotpos)
		return data[k];
	else if (k < pivotpos)
		return qselect(data, start, pivotpos, k);
	else 
		return qselect(data, pivotpos+1, end, k);
}

class Matrix3X3
{
public:
	Matrix3X3() { for(int i=0;i<9;i++) m[i]=0.0f; }
	Matrix3X3(vector3f x,vector3f y,vector3f z) { 
		row(0) = x;
		row(1) = y;
		row(2) = z;
	}

	float& operator[](int i) { return m[i]; }
	const float& operator[](int i) const { return m[i]; }

	vector3f& row(int i) { return *(vector3f*)&m[i*3]; }
	const vector3f& row(int i) const { return *(vector3f*)&m[i*3]; }

	float& operator()(int i, int j) { return m[3*i+j]; }
	const float& operator()(int i, int j) const { return m[3*i+j]; }

	float& at(int i,int j) {  return m[3*i+j]; }
	const float& at(int i,int j) const { return m[3*i+j]; }

	float Determinant() const 
	{
		return at(0,0)*(at(1,1)*at(2,2)-at(2,1)*at(1,2))
							-at(0,1)*(at(1,0)*at(2,2)-at(1,2)*at(2,0))
						+at(0,2)*(at(1,0)*at(2,1)-at(1,1)*at(2,0));
	}

	Matrix3X3 Inverse() const
	{
		float det = Determinant();
		if (det != 0.0f) {
			float invdet = 1/det;
			Matrix3X3 result;
			result(0,0) =  (at(1,1)*at(2,2)-at(2,1)*at(1,2))*invdet;
			result(1,0) = -(at(0,1)*at(2,2)-at(0,2)*at(2,1))*invdet;
			result(2,0) =  (at(0,1)*at(1,2)-at(0,2)*at(1,1))*invdet;
			result(0,1) = -(at(1,0)*at(2,2)-at(1,2)*at(2,0))*invdet;
			result(1,1) =  (at(0,0)*at(2,2)-at(0,2)*at(2,0))*invdet;
			result(2,1) = -(at(0,0)*at(1,2)-at(1,0)*at(0,2))*invdet;
			result(0,2) =  (at(1,0)*at(2,1)-at(2,0)*at(1,1))*invdet;
			result(1,2) = -(at(0,0)*at(2,1)-at(2,0)*at(0,1))*invdet;
			result(2,2) =  (at(0,0)*at(1,1)-at(1,0)*at(0,1))*invdet;
			return result;
		}
		return Matrix3X3();
	}
	Matrix3X3 InverseTranspose() const
	{
		float det = Determinant();
		if (det != 0.0f) {
			float invdet = 1/det;
			Matrix3X3 result;
			result(0,0) =  (at(1,1)*at(2,2)-at(2,1)*at(1,2))*invdet;
			result(0,1) = -(at(0,1)*at(2,2)-at(0,2)*at(2,1))*invdet;
			result(0,2) =  (at(0,1)*at(1,2)-at(0,2)*at(1,1))*invdet;
			result(1,0) = -(at(1,0)*at(2,2)-at(1,2)*at(2,0))*invdet;
			result(1,1) =  (at(0,0)*at(2,2)-at(0,2)*at(2,0))*invdet;
			result(1,2) = -(at(0,0)*at(1,2)-at(1,0)*at(0,2))*invdet;
			result(2,0) =  (at(1,0)*at(2,1)-at(2,0)*at(1,1))*invdet;
			result(2,1) = -(at(0,0)*at(2,1)-at(2,0)*at(0,1))*invdet;
			result(2,2) =  (at(0,0)*at(1,1)-at(1,0)*at(0,1))*invdet;
			return result;
		}
		return Matrix3X3();
	}

	Matrix3X3& operator*=(float a) {
		for(int i=0;i<9;i++) 
			m[i]*=a;
		return *this;
	}

	Matrix3X3& operator+=(const Matrix3X3& b) {
		for(int i=0;i<9;i++)
			m[i]+=b[i];
		return *this;
	}

	float m[9];

	static void test() 
	{
		Matrix3X3 t;

		for (int i=0;i<9;i++)
			t[i]=i*i;
		t.Inverse().dbgprint();
	}

	void dbgprint()
	{
		dbgprintf("%f\t%f\t%f\n", m[0],m[1],m[2]);
		dbgprintf("%f\t%f\t%f\n", m[3],m[4],m[5]);
		dbgprintf("%f\t%f\t%f\n", m[6],m[7],m[8]);
	}

};




template<typename T>
T erf(T x)
{
    // constants
    T a1 =  0.254829592f;
    T a2 = -0.284496736f;
    T a3 =  1.421413741f;
    T a4 = -1.453152027f;
    T a5 =  1.061405429f;
    T p  =  0.3275911f;

    // Save the sign of x
    int sign = 1;
    if (x < 0)
        sign = -1;
    x = fabs(x);

    // A&S formula 7.1.26
    T t = 1.0f/(1.0f + p*x);
    T y = 1.0f - (((((a5*t + a4)*t) + a3)*t + a2)*t + a1)*t*exp(-x*x);

    return sign*y;
}



/*
Cubic B-spline. Has continuous derivative when knots are distinct
	  B(i - 1) = 1/6 * (1 - t) ^ 3
	  B(i + 0) = 1/6 * (3*t^3 - 6*t^2 + 4)
	  B(i + 1) = 1/6 * (-3*t^3 + 3*t^2 + 3t + 1)
	  B(i + 2) = 1/6 * t^3
*/

inline void ComputeBSplineWeights(float w[], float t)
{
	float t2=t*t;
	float t3=t2*t;
	float omt = 1-t;
	w[0] = 1.0f/6 * omt*omt*omt;
	w[1] = 1.0f/6 * (3*t3 - 6*t2 + 4);
	w[2] = 1.0f/6 * (-3*t3 + 3*t2 + 3*t + 1);
	w[3] = 1.0f/6 * t3;
}


