using NationalInstruments.Vision;
using NationalInstruments.Vision.Acquisition.Imaq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using QTrkDotNet;

namespace RTTracker
{
    public class CameraSettings
    {
        public string deviceName = "img0";
        public int width=1280, height=1024;
        public int numBuffers=30;
        public int framerate=50;
    }

    abstract class IMAQCamera
    {
        protected ImaqSession session;
        protected ImaqBufferCollection bufList;

        public ImaqSession Session
        {
            get { return session; }
        }

        public ImaqSerialConnection Serial
        {
            get { return session.SerialConnection; }
        }

        public abstract int Framerate
        {
            get;
            set;
        }
        public abstract void SetROI(Int2 size, Int2[] roiPositions);

        
        public virtual void Close()
        {
            if (session != null)
            {
                session.Close();
                session = null;
            }
        }
    }
}
