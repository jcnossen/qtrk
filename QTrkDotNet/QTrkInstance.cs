using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
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
            QTrkDLL.QTrkGetComputedConfig(inst, out cc);
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

        public void Dispose()
        {
            Destroy();
        }
    }
}
