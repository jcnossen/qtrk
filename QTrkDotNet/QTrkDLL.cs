using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;

using System.Text;
using System.IO;
using System.Diagnostics;
using System.Security;
using System.Xml.Serialization;

namespace QTrkDotNet
{
	[Flags]
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
    public struct QTrkConfig
    {
        [XmlIgnore]
        public int width, height;

        public int ROI
        {
            get { return width; }
            set { width = height = value; }
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

        public int qi_iterations;

        [XmlIgnore]
        [Description("Number of iterations done by QI algorithm. 3 is typically good enough for small ROI (~60)")]
        public int QI_iterations
        {
            get { return qi_iterations; }
            set { qi_iterations = value; }
        }
        [XmlIgnore]
        public float QI_minradius
        {
            get { return qi_minradius; }
            set { qi_minradius = value; }
        }
	    public float qi_minradius;
        public float qi_radial_coverage;

        [XmlIgnore]
        public float QI_radial_coverage
        {
            get { return qi_radial_coverage; }
            set { qi_radial_coverage = value; }
        }
        public float qi_angular_coverage;

        [XmlIgnore]
        public float QI_angular_coverage
        {
            get { return qi_angular_coverage; }
            set { qi_angular_coverage = value; }
        }
        public float qi_roi_coverage;

        [XmlIgnore]
        public float QI_roi_coverage
        {
            get { return qi_roi_coverage; }
            set { qi_roi_coverage = value; }
        }
	    public float qi_angstep_factor;

	    public int xc1_profileLength;
	    public int xc1_profileWidth;
	    public int xc1_iterations;

        public int gauss2D_iterations;

        [XmlIgnore]
        public int Gauss2D_iterations
        {
            get { return gauss2D_iterations; }
            set { gauss2D_iterations = value; }
        }
	    public float gauss2D_sigma;

    	public int downsample; // 0 = original, 1 = 1x (W=W/2,H=H/2)

        public unsafe static QTrkConfig Default
        {
            get
            {
                QTrkConfig val;
                QTrkDLL.QTrkGetDefaultConfig(out val);
                return val;
            }
        }
                
        [XmlIgnore]
        [Description("How many pixels away from the bead center do we start creating the ZLUT radial profile?")]
        public float ZLUT_minradius
        {
            get { return zlut_minradius; }
            set { zlut_minradius = value; }
        }

        [XmlIgnore]
        [Description("Which fraction of the ROI is used to compute the radial profile. " +
                     "Example: For a 60x60 ROI, 0.5 means sampling the radial profile using a circle of 30 pixels wide.")]
        public float ZLUT_roi_coverage
        {
            get { return zlut_roi_coverage; }
            set { zlut_roi_coverage = value; }
        }

        [XmlIgnore]
        public float ZLUT_radial_coverage
        {
            get { return zlut_radial_coverage; }
            set { zlut_radial_coverage = value; }
        }

        [XmlIgnore]
        public float ZLUT_angular_coverage
        {
            get { return zlut_angular_coverage; }
            set { zlut_angular_coverage = value; }
        }
    };

	[StructLayout(LayoutKind.Sequential)]
	public unsafe struct ImageData
	{
		public float* data;
		public int width,height;

		public int Pitch { get { return 4 * width; } }

		public IntPtr Pointer { get { return new IntPtr(data); } }
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

	[StructLayout(LayoutKind.Sequential)]
	public struct RMFrameCounters {
		public int startFrame; // startFrame for frameResults
		public int processedFrames; // frame where all data is retrieved (all beads)
		public int lastSaveFrame;
		public int capturedFrames;  // lock by resultMutex
		public int localizationsDone;
		public int lostFrames;
		public int fileError;
	}

	[StructLayout( LayoutKind.Sequential)]
	public struct ResultManagerConfig
	{
		public int numBeads, numFrameInfoColumns;
		public Vector3 scaling;
		public Vector3 offset; // output will be (position + offset) * scaling
		public int writeInterval; // [frames]
		public uint maxFramesInMemory; // 0 for infinite
		public byte binaryOutput;
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
		[SuppressUnmanagedCodeSecurity]
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
		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void QTrkScheduleLocalization(IntPtr qtrk, void* data, int pitch, QTRK_PixelDataType pdt, LocalizationJob* jobInfo);
		
		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void QTrkClearResults(IntPtr qtrk);
		
		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void QTrkFlush(IntPtr qtrk); // stop waiting for more jobs to do, and just process the current batch

        // Schedule an entire frame at once, allowing for further optimizations
		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int QTrkScheduleFrame(IntPtr qtrk, IntPtr imgptr, int pitch, int width, int height, [In] Int2[] positions, int numROI, QTRK_PixelDataType pdt, [In] LocalizationJob[] jobInfo);

        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void QTrkEnableRadialZLUTCompareProfile(bool enabled);
        [DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern unsafe void QTrkGetRadialZLUTCompareProfile(float* dst); // dst = [count * planes]

        // data can be zero to allocate ZLUT data. zcmp has to have 'zlut_radialsteps' elements
		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void QTrkSetRadialZLUT(IntPtr qtrk, IntPtr data, int count, int planes);
		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void QTrkGetRadialZLUT(IntPtr qtrk, IntPtr dst);
		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void QTrkGetRadialZLUTSize(IntPtr qtrk, out int count, out int planes, out int radialsteps);

        // Set radial weights used for comparing LUT profiles, zcmp has to have 'zlut_radialsteps' elements
		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void QTrkSetRadialWeights(IntPtr qtrk, [In] float[] zcmp);

//        #define BUILDLUT_NORMALIZE 4
        //#define BUILDLUT_BIASCORRECT 8
		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void QTrkBeginLUT(IntPtr qtrk, uint flags);
		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void QTrkBuildLUT(IntPtr qtrk, void* data, int pitch, QTRK_PixelDataType pdt, int plane, Vector2[] known_pos);
		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void QTrkFinalizeLUT(IntPtr qtrk);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int QTrkGetResultCount(IntPtr qtrk);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int QTrkFetchResults(IntPtr qtrk, [Out] LocalizationResult[] results, int maxResults);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern int QTrkGetQueueLength(IntPtr qtrk, out int maxQueueLen);
		
		[SuppressUnmanagedCodeSecurity]
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


		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void ComputeRadialProfile(float[] dst, int radialSteps, int angularSteps, float minradius, float maxradius, Vector2 center, ImageData* src, float mean, bool normalize);
		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void NormalizeRadialProfile([MarshalAs(UnmanagedType.LPArray)] float[] prof, int rsteps);
		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void GenerateImageFromLUT(ref ImageData image, [In] ref ImageData zlut, float minradius, float maxradius, Vector3 pos, bool useSplineInterp, int ovs);
		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void NormalizeImage([In] ref ImageData img);
		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void ApplyPoissonNoise([In] ref ImageData img, float poissonMax, float maxValue);
		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void ApplyGaussianNoise([In] ref ImageData img, float sigma);

		// sample needs to be preallocated with a [roi,roi] image in order for QTrkFindBeads to copy the data into it, otherwise it is ignored.
		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr QTrkFindBeads(ref ImageData image, int smpCornerPosX, int smpCornerPosY, int roi, float imgRelDist, float acceptance, out int beadCount, ref ImageData sample);
		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
        public static extern void QTrkFreeROIPositions(IntPtr data);

		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern IntPtr RMCreate(string file, string frameinfo, [In] ref ResultManagerConfig cfg, string semicolonSeparatedNames);
		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void RMSetTracker(IntPtr rm, IntPtr qtrk);
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void RMDestroy(IntPtr rm);
		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void RMStoreFrameInfo(IntPtr rm, int frame, double timestamp, float[] cols);
		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int RMGetBeadResults(IntPtr rm, int start, int numFrames, int bead, [MarshalAs(UnmanagedType.LPArray)] out LocalizationResult[] results);
		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void RMGetFrameCounters(IntPtr rm, out RMFrameCounters dst);
		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void RMFlush(IntPtr rm);
		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern int RMGetResults(IntPtr rm, int startFrame, int numFrames, [MarshalAs(UnmanagedType.LPArray)] out LocalizationResult[] results);
		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void RMRemoveBead(IntPtr rm, int bead);
		[SuppressUnmanagedCodeSecurity]
		[DllImport(DllName, CallingConvention = CallingConvention.Cdecl)]
		public static extern void RMGetConfig(IntPtr rm, out ResultManagerConfig cfg);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool SetDllDirectory(string lpPathName);

        public static void SelectNativeLibrary(string baseDir, bool debugMode, bool useCUDA, bool use64Bit)
        {
            string dirname = debugMode ? "debug" : "release";
            if (useCUDA) dirname = "cuda_" + dirname;
            if (use64Bit) dirname = dirname + "_64";

			string finalDir = baseDir + Path.DirectorySeparatorChar + "QTrkDLLs" + Path.DirectorySeparatorChar + dirname + Path.DirectorySeparatorChar;

            Trace.WriteLine("Selected QTrk native dll directory: " + finalDir);
            SetDllDirectory(finalDir);
        }


	}
}
