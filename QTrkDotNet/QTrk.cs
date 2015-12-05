using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using System.Text;

namespace QTrkDotNet
{

enum LocalizeModeEnum {
	// Flags for selecting 2D localization type
	LT_OnlyCOM = 0, // use only COM
	LT_XCor1D = 1, // COM+XCor1D
	LT_QI = 2, // COM+QI
	LT_Gaussian2D = 4, // 2D Gaussian localization
	LT_ZLUTAlign = 8, // XYZ Alignment with ZLUT

	LT_LocalizeZ = 16,
	LT_NormalizeProfile = 64,
	LT_ClearFirstFourPixels = 128,
	LT_FourierLUT = 256,
	LT_LocalizeZWeighted = 512,

	LT_Force32Bit = 0xffffffff
};

typedef int LocMode_t; // LocalizationModeEnum

enum QTRK_PixelDataType
{
	QTrkU8 = 0,
	QTrkU16 = 1,
	QTrkFloat = 2
};


#pragma pack(push, 1)

// 24 bytes
struct LocalizationJob {
	LocalizationJob() {
		frame=timestamp=zlutIndex=0; 
	}
	LocalizationJob(uint frame, uint timestamp, uint zlutPlane, uint zlutIndex) :
		frame (frame), timestamp(timestamp), zlutIndex(zlutIndex) 
	{}
	uint frame, timestamp;   
	int zlutIndex; // or bead#
	vector3f initialPos;
};


// DONT CHANGE, Mapped to labview clusters!
// 13*4 = 52 bytes
struct LocalizationResult {
	LocalizationJob job; //24
	vector3f pos;
	vector2f pos2D() { return vector2f(pos.x,pos.y); }
	vector2f firstGuess; // COM pos
	uint error;
	float imageMean;
};
// DONT CHANGE, Mapped to labview clusters (QTrkSettings.ctl)!
struct QTrkSettings {

	int width, height;
	int numThreads;

#define QTrkCUDA_UseList -3   // Use list defined by SetCUDADevices
#define QTrkCUDA_UseAll -2
#define QTrkCUDA_UseBest -1
	// cuda_device < 0: use flags above
	// cuda_device >= 0: use as hardware device index
	int cuda_device;

	float com_bgcorrection; // 0.0f to disable

	float zlut_minradius;
	float zlut_radial_coverage;
	float zlut_angular_coverage;
	float zlut_roi_coverage; // maxradius = ROI/2*roi_coverage

	int qi_iterations;
	float qi_minradius;
	float qi_radial_coverage;
	float qi_angular_coverage;
	float qi_roi_coverage;
	float qi_angstep_factor;

	int xc1_profileLength;
	int xc1_profileWidth;
	int xc1_iterations;

	int gauss2D_iterations;
	float gauss2D_sigma;

	int downsample; // 0 = original, 1 = 1x (W=W/2,H=H/2)
};

struct ROIPosition
{
	int x,y; // top-left coordinates. ROI is [ x .. x+w ; y .. y+h ]
};



    public struct QTrkSettings
    {        
	    int width, height;
	    int numThreads;
	    // cuda_device < 0: use flags above
	    // cuda_device >= 0: use as hardware device index
	    int cuda_device;

	    float com_bgcorrection; // 0.0f to disable

	    float zlut_minradius;
	    float zlut_radial_coverage;
	    float zlut_angular_coverage;
	    float zlut_roi_coverage; // maxradius = ROI/2*roi_coverage

	    int qi_iterations;
	    float qi_minradius;
	    float qi_radial_coverage;
	    float qi_angular_coverage;
	    float qi_roi_coverage;
	    float qi_angstep_factor;

	    int xc1_profileLength;
	    int xc1_profileWidth;
	    int xc1_iterations;

	    int gauss2D_iterations;
	    float gauss2D_sigma;

    	int downsample; // 0 = original, 1 = 1x (W=W/2,H=H/2)

    };

    public struct QTrkComputedSettings : QTrkSettings
    {
	    // Computed from QTrkSettings
	    int zlut_radialsteps;
	    int zlut_angularsteps;
	    float zlut_maxradius;
	
	    int qi_radialsteps;
	    int qi_angstepspq;
	    float qi_maxradius;
    }

    public class QTrk
    {
        public const string DllName = "qtrkcuda.dll";

        [DllImport(DllName)] public static extern IntPtr QTrkCreateInstance(QTrkSettings *cfg);
        [DllImport(DllName)] public static extern void QTrkFreeInstance(IntPtr qtrk);

        // C API, mainly intended to allow binding to .NET
        [DllImport(DllName)] public static extern void QTrkSetLocalizationMode(IntPtr qtrk, int locType);

	    // These are per-bead! So both gain and offset are sized [width*height*numbeads], similar to ZLUT
	    // result=gain*(pixel+offset)
        [DllImport(DllName)] public static extern void QTrkSetPixelCalibrationImages(IntPtr qtrk, float* offset, float* gain);
        [DllImport(DllName)] public static extern void QTrkSetPixelCalibrationFactors(float offsetFactor, float gainFactor);

        // Frame and timestamp are ignored by tracking code itself, but usable for the calling code
        // Pitch: Distance in bytes between two successive rows of pixels (e.g. address of (0,0) -  address of (0,1) )
        // ZlutIndex: Which ZLUT to use for ComputeZ/BuildZLUT
        [DllImport(DllName)] public static extern void QTrkScheduleLocalization(IntPtr qtrk, void* data, int pitch, QTRK_PixelDataType pdt, const LocalizationJob *jobInfo);
        [DllImport(DllName)] public static extern void QTrkClearResults(IntPtr qtrk);
        [DllImport(DllName)] public static extern void QTrkFlush(IntPtr qtrk); // stop waiting for more jobs to do, and just process the current batch

        // Schedule an entire frame at once, allowing for further optimizations
        [DllImport(DllName)] public static extern int QTrkScheduleFrame(IntPtr qtrk, void *imgptr, int pitch, int width, int height, ROIPosition *positions, int numROI, QTRK_PixelDataType pdt, const LocalizationJob *jobInfo);

        [DllImport(DllName)] public static extern void QTrkEnableRadialZLUTCompareProfile(bool enabled);
        [DllImport(DllName)] public static extern void QTrkGetRadialZLUTCompareProfile(float* dst); // dst = [count * planes]

        // data can be zero to allocate ZLUT data. zcmp has to have 'zlut_radialsteps' elements
        [DllImport(DllName)] public static extern void QTrkSetRadialZLUT(IntPtr qtrk, float* data, int count, int planes); 
        [DllImport(DllName)] public static extern void QTrkGetRadialZLUT(IntPtr qtrk, float* dst);
        [DllImport(DllName)] public static extern void QTrkGetRadialZLUTSize(IntPtr qtrk, int* count, int* planes, int* radialsteps);

        // Set radial weights used for comparing LUT profiles, zcmp has to have 'zlut_radialsteps' elements
        [DllImport(DllName)] public static extern void QTrkSetRadialWeights(IntPtrqtrk,  float* zcmp);

        #define BUILDLUT_NORMALIZE 4
        #define BUILDLUT_BIASCORRECT 8
        [DllImport(DllName)] public static extern void QTrkBeginLUT(IntPtr qtrk, uint flags);
        [DllImport(DllName)] public static extern void QTrkBuildLUT(IntPtr qtrk, void* data, int pitch, QTRK_PixelDataType pdt, int plane, vector2f* known_pos=0);
        [DllImport(DllName)] public static extern void QTrkFinalizeLUT(IntPtr qtrk);

        [DllImport(DllName)] public static extern int QTrkGetResultCount(IntPtr qtrk);
        [DllImport(DllName)] public static extern int QTrkFetchResults(IntPtr qtrk, LocalizationResult* results, int maxResults);

        [DllImport(DllName)] public static extern int QTrkGetQueueLength(IntPtr qtrk, int *maxQueueLen);
        [DllImport(DllName)] public static extern bool QTrkIsIdle(IntPtr qtrk);

        /*	virtual void SetConfigValue(std::string name, std::string value) = 0;
	        typedef std::map<std::string, std::string> ConfigValueMap;
	        virtual ConfigValueMap GetConfigValues() = 0;
	        */

        [DllImport(DllName)] public static extern void QTrkGetProfileReport(IntPtr qtrk, char *dst, int maxStrLen);
        [DllImport(DllName)] public static extern void QTrkGetWarnings(IntPtr qtrk, char *dst, int maxStrLen);

        [DllImport(DllName)] public static extern void QTrkGetComputedConfig(IntPtr qtrk, QTrkComputedConfig* cfg);


    }
}
