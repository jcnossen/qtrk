using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using System.Text;
using System.IO;
using System.Diagnostics;

namespace QTrkDotNet
{
    public enum LocalizeModeEnum {
	    // Flags for selecting 2D localization type
	    OnlyCOM = 0, // use only COM
	    XCor1D = 1, // COM+XCor1D
	    QI = 2, // COM+QI
	    Gaussian2D = 4, // 2D Gaussian localization
	    ZLUTAlign = 8, // XYZ Alignment with ZLUT

	    LocalizeZ = 16,
	    NormalizeProfile = 64,
	    ClearFirstFourPixels = 128,
	    FourierLUT = 256,
	    LocalizeZWeighted = 512,
    };

    public enum QTRK_PixelDataType
    {
	    U8 = 0,
	    U16 = 1,
	    Float = 2
    };


// 24 bytes
    [StructLayout(LayoutKind.Sequential)]
    public struct LocalizationJob {
	    public uint frame, timestamp;   
	    public int zlutIndex; // or bead#
	    public Vector3 initialPos;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct Vector2
    {
        public float x,y;
    }


    [StructLayout(LayoutKind.Sequential)]
    public struct LocalizationResult {
	    public LocalizationJob job; //24
	    public Vector3 pos;
	    public Vector2 firstGuess; // COM pos
	    public uint error;
        public float imageMean;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct ROIPosition
    {
	    public int x,y; // top-left coordinates. ROI is [ x .. x+w ; y .. y+h ]
    };


    [StructLayout(LayoutKind.Sequential)]
    public struct QTrkConfig
    {
        public int width, height;

        public int Height
        {
            get { return height; }
            set { height = value; }
        }

        public int Width
        {
            get { return width; }
            set { width = value; }
        }
	    public int numThreads;
	    // cuda_device < 0: use flags above
	    // cuda_device >= 0: use as hardware device index
	    public int cuda_device;

	    public float com_bgcorrection; // 0.0f to disable

	    public float zlut_minradius;
	    public float zlut_radial_coverage;
	    public float zlut_angular_coverage;
	    public float zlut_roi_coverage; // maxradius = ROI/2*roi_coverage

        private int qi_iterations;

        public int Qi_iterations
        {
            get { return qi_iterations; }
            set { qi_iterations = value; }
        }
	    public float qi_minradius;
        private float qi_radial_coverage;

        public float Qi_radial_coverage
        {
            get { return qi_radial_coverage; }
            set { qi_radial_coverage = value; }
        }
        private float qi_angular_coverage;

        public float Qi_angular_coverage
        {
            get { return qi_angular_coverage; }
            set { qi_angular_coverage = value; }
        }
        private float qi_roi_coverage;

        public float Qi_roi_coverage
        {
            get { return qi_roi_coverage; }
            set { qi_roi_coverage = value; }
        }
	    public float qi_angstep_factor;

	    public int xc1_profileLength;
	    public int xc1_profileWidth;
	    public int xc1_iterations;

        private int gauss2D_iterations;

        public int Gauss2D_iterations
        {
            get { return gauss2D_iterations; }
            set { gauss2D_iterations = value; }
        }
	    public float gauss2D_sigma;

    	public int downsample; // 0 = original, 1 = 1x (W=W/2,H=H/2)

        public  unsafe static QTrkConfig Default
        {
            get
            {
                QTrkConfig val;
                QTrkDLL.QTrkGetDefaultConfig(out val);
                return val;
            }
        }
    };

	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct ImageData
	{
		public float* data;
		public int width,height;
	}

    [StructLayout(LayoutKind.Sequential)]
    public struct QTrkComputedConfig
    {
        public QTrkConfig config;
	    // Computed from QTrkSettings
	    public int zlut_radialsteps;
	    public int zlut_angularsteps;
	    public float zlut_maxradius;
	
	    public int qi_radialsteps;
	    public int qi_angstepspq;
	    public float qi_maxradius;

		public static QTrkComputedConfig FromConfig(QTrkConfig cfg)
		{
			QTrkComputedConfig cc;
			QTrkDLL.QTrkGetComputedConfig(ref cfg, out cc);
			return cc;
		}
	}

    public unsafe class QTrkDLL
    {
        public const string DllName = "qtrk.dll";

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void TestDLLCallConv(int a);

        [DllImport(DllName, CallingConvention=CallingConvention.Cdecl)]
        public static extern void QTrkGetDefaultConfig([Out] out QTrkConfig cfg);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr QTrkCreateInstance([In] ref QTrkConfig cfg);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void QTrkFreeInstance(IntPtr qtrk);

        // C API, mainly intended to allow binding to .NET
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void QTrkSetLocalizationMode(IntPtr qtrk, int locType);

	    // These are per-bead! So both gain and offset are sized [width*height*numbeads], similar to ZLUT
	    // result=gain*(pixel+offset)
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void QTrkSetPixelCalibrationImages(IntPtr qtrk, float* offset, float* gain);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void QTrkSetPixelCalibrationFactors(float offsetFactor, float gainFactor);

        // Frame and timestamp are ignored by tracking code itself, but usable for the calling code
        // Pitch: Distance in bytes between two successive rows of pixels (e.g. address of (0,0) -  address of (0,1) )
        // ZlutIndex: Which ZLUT to use for ComputeZ/BuildZLUT
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void QTrkScheduleLocalization(IntPtr qtrk, void* data, int pitch, QTRK_PixelDataType pdt, LocalizationJob* jobInfo);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void QTrkClearResults(IntPtr qtrk);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void QTrkFlush(IntPtr qtrk); // stop waiting for more jobs to do, and just process the current batch

        // Schedule an entire frame at once, allowing for further optimizations
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int QTrkScheduleFrame(IntPtr qtrk, void* imgptr, int pitch, int width, int height, ROIPosition* positions, int numROI, QTRK_PixelDataType pdt, LocalizationJob* jobInfo);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void QTrkEnableRadialZLUTCompareProfile(bool enabled);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void QTrkGetRadialZLUTCompareProfile(float* dst); // dst = [count * planes]

        // data can be zero to allocate ZLUT data. zcmp has to have 'zlut_radialsteps' elements
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void QTrkSetRadialZLUT(IntPtr qtrk, float* data, int count, int planes);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void QTrkGetRadialZLUT(IntPtr qtrk, float* dst);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void QTrkGetRadialZLUTSize(IntPtr qtrk, out int count, out int planes, out int radialsteps);

        // Set radial weights used for comparing LUT profiles, zcmp has to have 'zlut_radialsteps' elements
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void QTrkSetRadialWeights(IntPtr qtrk, float* zcmp);

//        #define BUILDLUT_NORMALIZE 4
        //#define BUILDLUT_BIASCORRECT 8
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void QTrkBeginLUT(IntPtr qtrk, uint flags);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void QTrkBuildLUT(IntPtr qtrk, void* data, int pitch, QTRK_PixelDataType pdt, int plane, Vector2[] known_pos);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void QTrkFinalizeLUT(IntPtr qtrk);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int QTrkGetResultCount(IntPtr qtrk);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int QTrkFetchResults(IntPtr qtrk, out LocalizationResult[] results, int maxResults);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int QTrkGetQueueLength(IntPtr qtrk, out int maxQueueLen);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern bool QTrkIsIdle(IntPtr qtrk);

        /*	virtual void SetConfigValue(std::string name, std::string value) = 0;
	        typedef std::map<std::string, std::string> ConfigValueMap;
	        virtual ConfigValueMap GetConfigValues() = 0;
	        */

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void QTrkGetProfileReport(IntPtr qtrk, char* dst, int maxStrLen);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void QTrkGetWarnings(IntPtr qtrk, char* dst, int maxStrLen);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void QTrkGetComputedConfig([In] ref QTrkConfig cfg, [Out] out QTrkComputedConfig cc);


        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void ComputeRadialProfile(float[] dst, int radialSteps, int angularSteps, float minradius, float maxradius, Vector2 center, ImageData* src, float mean, bool normalize);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void NormalizeRadialProfile([MarshalAs(UnmanagedType.LPArray)] float[] prof, int rsteps);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void GenerateImageFromLUT(ref ImageData image, [In] ref ImageData zlut, float minradius, float maxradius, Vector3 pos, bool useSplineInterp, int ovs);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void ApplyPoissonNoise([In] ref ImageData img, float poissonMax, float maxValue);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void ApplyGaussianNoise([In] ref ImageData img, float sigma);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetDllDirectory(string lpPathName);

        public static void SelectNativeLibrary(string baseDir, bool debugMode, bool useCUDA)
        {
            string dirname = debugMode ? "debug" : "release";
            if (useCUDA) dirname = "cuda_" + dirname;

            string finalDir = baseDir+Path.DirectorySeparatorChar +"QTrkDLLs" + Path.DirectorySeparatorChar + dirname + Path.DirectorySeparatorChar;

            Trace.WriteLine("Selected QTrk native dll directory: " + finalDir);
            SetDllDirectory(finalDir);
        }

    }
}
