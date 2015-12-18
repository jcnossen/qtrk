using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            QTrkDLL.SelectNativeLibrary(baseDir == "" ? Directory.GetCurrentDirectory() : baseDir, useDebug, useCUDA);
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

        public void ProcessLUTFrame(ImageData image, Int2[] roiPos, int plane)
        {
            QTrkDLL.QTrkBuildLUTFromFrame(inst, ref image, QTRK_PixelDataType.Float, plane, roiPos, roiPos.Length);
        }

        public void FinalizeLUT()
        {
            QTrkDLL.QTrkFinalizeLUT(inst);
        }

        public void Dispose()
        {
            Destroy();
        }

        public FloatImg[] GetRadialZLUT()
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

        public void SetRadialZLUTSize(int nbeads, int numLUTSteps)
        {
            QTrkDLL.QTrkSetRadialZLUT(inst, IntPtr.Zero, nbeads, numLUTSteps);
        }
    }
}
