using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace QTrkDotNet
{
    public unsafe class QTrkInstance : IDisposable
    {
        IntPtr inst;
        QTrkComputedConfig cc;

        static bool IsDLLSelected;

        public QTrkComputedConfig Config
        {
            get { return cc; }
        }

        public static void SelectDLL(bool useDebug, bool useCUDA, string baseDir="")
        {
            QTrkDLL.SelectNativeLibrary(baseDir == "" ? Directory.GetCurrentDirectory() : baseDir, useDebug, useCUDA, IntPtr.Size==8);
            IsDLLSelected = true;
        }

        public QTrkInstance(QTrkConfig config)
        {
            if (!IsDLLSelected)
                throw new ApplicationException("Use QTrkInstance::SelectDLL before creating an instance");

            inst = QTrkDLL.QTrkCreateInstance(ref config);
			QTrkDLL.QTrkGetComputedConfig(ref config, out cc);
        }

        public void Destroy()
        {
            if (inst != IntPtr.Zero)
            {
                Trace.WriteLine("Disposing QTrk instance...");

                QTrkDLL.QTrkFreeInstance(inst);
                inst = IntPtr.Zero;
            }
        }

        public void BeginLUT(bool normalize)
        {
            QTrkDLL.QTrkBeginLUT(inst, (uint)( normalize ? 4 : 0 ));
        }

		public void BuildLUT(FloatImg image, int plane)
		{
			ImageData data=image.ImageData;
			QTrkDLL.QTrkBuildLUT(inst, data.data, data.Pitch, QTRK_PixelDataType.Float, plane, null);
		}

		public void SetLocalizationMode(LocalizeModeEnum locMode)
		{
			QTrkDLL.QTrkSetLocalizationMode(inst, (int)locMode);
		}

		public void Flush()
		{
			QTrkDLL.QTrkFlush(inst);
		}

		public void ScheduleLocalization(ref ImageData img, LocalizationJob* job)
		{
			QTrkDLL.QTrkScheduleLocalization(inst, img.data, img.Pitch, QTRK_PixelDataType.Float, job);
		}

		public void ScheduleLocalization(FloatImg img, uint frame, int zlut, uint timestamp)
		{
			ImageData d =img.ImageData;
			LocalizationJob job=new LocalizationJob() { frame=frame, timestamp=timestamp, zlutIndex=zlut };
			QTrkDLL.QTrkScheduleLocalization(inst, d.data, d.Pitch, QTRK_PixelDataType.Float, &job);
		}

		public void ScheduleFrameBitmap(Bitmap bmp, Int2[] positions, LocalizationJob[] jobInfo)
		{
//        public static extern int QTrkScheduleFrame(IntPtr qtrk, void* imgptr, int pitch, int width, int height, Int2* positions, int numROI, QTRK_PixelDataType pdt, LocalizationJob* jobInfo);
			BitmapData bmpData=bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.ReadOnly, PixelFormat.Format8bppIndexed);
			QTrkDLL.QTrkScheduleFrame(inst, bmpData.Scan0, bmpData.Stride, bmpData.Width, bmpData.Height, positions, positions.Length, QTRK_PixelDataType.U8, jobInfo);

			bmp.UnlockBits(bmpData);

//			QTrkDLL.QTrkScheduleFrame(inst, 
		}

        public void FinalizeLUT()
        {
            QTrkDLL.QTrkFinalizeLUT(inst);
        }

        public void Dispose()
        {
            Destroy();
        }

        public FloatImg[] GetRadialZLUTImages()
        {
            int count,planes,radialsteps;
            QTrkDLL.QTrkGetRadialZLUTSize(inst, out count, out planes, out radialsteps);

            IntPtr lutspace= Marshal.AllocHGlobal(sizeof(float) * count * planes* radialsteps);
            QTrkDLL.QTrkGetRadialZLUT(inst, lutspace);

            FloatImg[] luts = new FloatImg[count];
            float* src=(float*)lutspace.ToPointer();
            for (int i = 0; i < count; i++)
            {
                float *srcimg=&src[i*planes*radialsteps];
                luts[i] = new FloatImg(radialsteps, planes, srcimg);
            }

            Marshal.FreeHGlobal(lutspace);

            return luts;
        }


		public FloatImg GetRadialZLUT()
		{
			int count, planes, radialsteps;
			QTrkDLL.QTrkGetRadialZLUTSize(inst, out count, out planes, out radialsteps);

			FloatImg lut = new FloatImg(radialsteps, planes * count);
			QTrkDLL.QTrkGetRadialZLUT(inst, lut.pixels);
			return lut;
		}

		public struct LUTSize
		{
			public int count, planes, radialsteps;
		}
		public LUTSize GetRadialZLUTSize()
		{
			LUTSize s;
			QTrkDLL.QTrkGetRadialZLUTSize(inst, out s.count, out s.planes, out s.radialsteps);
			return s;
		}

        public void SetRadialZLUTSize(int nbeads, int numLUTSteps)
        {
            QTrkDLL.QTrkSetRadialZLUT(inst, IntPtr.Zero, nbeads, numLUTSteps);
        }
		public void SetRadialZLUT(FloatImg lut, int count, int planes)
		{
			Debug.Assert(count*planes==lut.h);
			QTrkDLL.QTrkSetRadialZLUT(inst, lut.pixels, count, planes);
		}

		public int GetResultCount()
		{
			return QTrkDLL.QTrkGetResultCount(inst);
		}

		public void GetResults(LocalizationResult[] results)
		{
			QTrkDLL.QTrkFetchResults(inst, results, results.Length);
		}

		public IntPtr InstancePtr
		{
			get { return inst; } 
		}

		public bool IsIdle()
		{
			return QTrkDLL.QTrkIsIdle(inst);
		}

		public int GetQueueLength()
		{
			int maxQueueLen;
			return QTrkDLL.QTrkGetQueueLength(inst, out maxQueueLen);
		}

		public int GetMaxQueueLength()
		{
			int maxQueueLen;
			QTrkDLL.QTrkGetQueueLength(inst, out maxQueueLen);
			return maxQueueLen;
		}
	}
}
