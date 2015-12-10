using NationalInstruments.Vision;
using NationalInstruments.Vision.Acquisition.Imaq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RTTracker
{
    public partial class MainDlg : Form
    {
        VisionImage displayImage;
        MikrotronCamera camera;
        CameraSettings settings = new CameraSettings();
        BackgroundWorker acquisitionWorker;
        bool acquiring = false;

        public MainDlg()
        {
            InitializeComponent();

            camera = new MikrotronCamera(settings);

            //  Configure the image display.
            displayImage = new VisionImage((ImageType)camera.Session.Attributes[ImaqStandardAttribute.ImageType].GetValue());
            imageViewer.Attach(displayImage);

            acquisitionWorker = new BackgroundWorker();
            acquisitionWorker.DoWork += acquisitionWorker_DoWork;
            acquisitionWorker.RunWorkerCompleted += acquisitionWorker_RunWorkerCompleted;
            acquisitionWorker.ProgressChanged += acquisitionWorker_ProgressChanged;

            acquisitionWorker.WorkerSupportsCancellation = true;
        }
        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {

                components.Dispose();
            }
            base.Dispose(disposing);
        }

        void Acquire(bool acq)
        {
            if (acq)
                acquisitionWorker.RunWorkerAsync();
            else
                acquisitionWorker.CancelAsync();
            acquiring = acq;
        }

        void acquisitionWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            //  This is the main function of the acquisition background worker thread.
            //  Perform image processing here instead of the UI thread to avoid a
            //  sluggish or unresponsive UI.
            BackgroundWorker worker = (BackgroundWorker)sender;

            camera.Start();

            try
            {
                //  Loop until we tell the thread to cancel or we get an error.  When this
                //  function completes the acquisitionWorker_RunWorkerCompleted method will
                //  be called.
                while (!worker.CancellationPending)
                {
                    camera.UpdateAcquisition(displayImage);
                    //  This will call the acquisition_ProgressChanged method in the UI
                    //  thread, where it is safe to update UI elements.  Do not update UI
                    //  elements directly in this thread as doing so could result in a
                    //  deadlock.
//                    worker.ReportProgress(0, );
                }
            }
            catch (ImaqException ex)
            {
                //  If an error occurs and the background worker thread is not being
                //  cancelled, then pass the exception along in the result so that
                //  it can be handled in the acquisition_RunWorkerCompleted method.
                if (!worker.CancellationPending)
                    e.Result = ex;
            }

            camera.Stop();
            Trace.WriteLine("Quitting acquisition loop ..");
        }

        void acquisitionWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            //  Update the UI with the information passed from the background worker thread.
            uint bufferNumber = (uint)e.UserState;
//            bufNumTextBox.Text = bufferNumber.ToString();
        }

        void acquisitionWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            //  The background worker thread has completed its execution.  Perform any cleanup here.
            if (e.Result is ImaqException)
            {
                //  If we get here it means that we had an error in the background worker thread
                //  that we need to handle.
                MessageBox.Show(((ImaqException)e.Result).ToString(), "NI-IMAQ Error");
            }
        }

        void Cleanup()
        {
            if (camera != null)
            {
                camera.Close();
                camera = null;
            }
        }

        private void buttonStartStop_Click(object sender, EventArgs e)
        {
            Acquire(!acquiring);
            if (acquiring)
                buttonStartStop.Text = "Stop";
            else
                buttonStartStop.Text = "Start";
        }

        private void MainDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (acquiring)
            {
                Acquire(false);
            }                

            while (acquisitionWorker.CancellationPending) ;

            Cleanup();
        }

        private void imageViewer_SizeChanged(object sender, EventArgs e)
        {
            imageViewer.ShowToolbar=true; 
        }
    }
}
