/*
 * 
 * Mikrotron camera support
 * 
 * Make sure to use NI Camera File Generator to put the Serial termination string to \n
 * Tested with EoSens CL MC1362
 * 
 * 
 * Default baud rate: 9600
 * Termination char: \r (0x0D) Carriage Return
 * 
 * Responses: ACK (0x6) on success. NAK (0x15) for unrecognized commands or errors.
 */

using NationalInstruments.Vision;
using NationalInstruments.Vision.Acquisition.Imaq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RTTracker
{
    class MikrotronCamera : IMAQCamera
    {
        public MikrotronCamera(CameraSettings settings)
        {
            Initialize(settings);
        }

        void Initialize(CameraSettings settings)
        {
            session = new ImaqSession(settings.deviceName);

            Framerate = settings.framerate;

            Trace.WriteLine("Framerate: " + Framerate);

            //  Create a buffer collection for the acquisition with the requested
            //  number of images, and configure the buffers to loop continuously.
            bufList = session.CreateBufferCollection(settings.numBuffers);
            for (int i = 0; i < bufList.Count; ++i)
            {
                bufList[i].Command = (i == bufList.Count - 1) ? ImaqBufferCommand.Loop : ImaqBufferCommand.Next;
            }
            //  Configure and start the acquisition.
            session.Acquisition.Configure(bufList);
            session.Acquisition.AcquireAsync();
        }

        public override void Close()
        {
            base.Close();
        }

        public void UpdateAcquisition(VisionImage image )
        {
            uint bufferNumber = 0;

            //  Try to get the next image.  If it has been overwritten then get
            //  the newest image.
            bufferNumber = session.Acquisition.Copy(bufferNumber, ImaqOverwriteMode.GetNewest, image);
            bufferNumber++;
            //  Update the UI by calling ReportProgress on the background worker.
        }
        
        public override void SetROI(QTrkDotNet.Int2 size, QTrkDotNet.Int2[] roiPositions)
        {
            throw new NotImplementedException();
        }

        internal void Start()
        {
            session.Start();
        }

        internal void Stop()
        {
            session.Stop();
        }

        public override int Framerate
        {
            get
            {
                // frame min max
             
                //%06x %02x-%06x       

                string resp = SerialCmd(":q?");
                string[] values=resp.Split(' ');
                return Convert.ToInt32(values[0], 16);
            }
            set
            {
                SerialCmd(string.Format(":q{0:X06}", value));
            }
        }

        string SerialCmd(string p)
        {
            var cmd=ASCIIEncoding.ASCII.GetBytes(p+"\r");

            session.SerialConnection.Write(cmd,1000);
            session.SerialConnection.Flush();

            string resp = "";
            bool err = false;
            while (true)
            {
                byte r = session.SerialConnection.ReadBytes(1, 1000)[0];
                if (r == '\r') break;
                if (r == 0x06)
                {
                    resp = "ACK.";
                    return "";
                }
                if (r == 0x15)
                {
                    resp = "NAK.";
                    err = true;
                    break;
                }
                resp += ASCIIEncoding.ASCII.GetString(new byte[] { r });
            }

            Trace.WriteLine("IMAQ Camera Resp: " + resp);

            if (err)
                throw new ApplicationException("IMAQ Camera error: " + resp);
            return resp;
        }
    }
}
