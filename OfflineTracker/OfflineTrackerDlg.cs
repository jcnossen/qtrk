using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using QTrkDotNet;
using System.IO;
using System.Diagnostics;
using System.Configuration;
using System.Xml.Serialization;

namespace OfflineTracker
{
	public partial class OfflineTrackerDlg : Form
	{
        List<Int2> beadPosList = new List<Int2>();
        Bitmap frameViewImage;
        FloatImg[] luts;

        int numFrames, numLUTSteps;

		public OfflineTrackerDlg()
		{
			InitializeComponent();

            if (!DesignMode)
            {
                QTrkInstance.SelectDLL(true, false);

                var d = QTrkConfig.Default;
                d.width = d.height = Settings.ROI;
                propertyGridQTrkSettings.SelectedObject = d;
                UpdateInfo();

                textLUTDir.Text = Settings.LUTDir;
                textExpDir.Text = Settings.ExpDir;
                UpdateDirectory();
            }
		}

        Properties.Settings Settings
        {
            get { return Properties.Settings.Default; }
        }


        QTrkConfig GetTrackerConfig()
        {
            var cfg = (QTrkConfig)propertyGridQTrkSettings.SelectedObject;
            cfg.height = cfg.width = Settings.ROI;
            return cfg;
        }


		private void buttonSelectExpDir_Click(object sender, EventArgs e)
		{
			var fbd = new FolderBrowserDialog() { Description = "Select experiment folder with images", SelectedPath=textExpDir.Text };
			if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				textExpDir.Text = fbd.SelectedPath;
                UpdateInfo();
            }
		}

		private void buttonSelectLUTDir_Click(object sender, EventArgs e)
		{
			var fbd = new FolderBrowserDialog() { Description = "Select LUT folder" ,SelectedPath=textLUTDir.Text };
			if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				textLUTDir.Text = fbd.SelectedPath;
                UpdateInfo();
			}
		}

		private void buttonGenerateTestLUT_Click(object sender, EventArgs e)
		{
			var ofd = new OpenFileDialog();
			if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				string basePath = textLUTDir.Text;
				if (basePath.Length > 0)
					basePath += Path.DirectorySeparatorChar;

				using (var lut = new Bitmap(ofd.FileName))
				{
					var cfg=GetTrackerConfig();
					var cc=QTrkComputedConfig.FromConfig(cfg);

					FloatImg lutf = new FloatImg(lut, 0);
					FloatImg img = new FloatImg(cfg.width,cfg.height);
					for (int i = 0; i < lut.Height; i++) {
						QTrkUtil.GenerateImageFromLUT(img, lutf, cfg.zlut_minradius, cc.zlut_maxradius, new Vector3(cfg.width / 2, cfg.height / 2, i), false, 1);
						string fn = basePath + string.Format("lut{0:000}.png", i);
						Trace.WriteLine("Writing " + fn);
						using (var planeimg = img.ToImage())
							planeimg.Save(fn);
					}

					lutf.Dispose();
					img.Dispose();
				}
			}
		}

		private void buttonSelectBeads_Click(object sender, EventArgs e)
		{
            if (frameViewImage==null)return;

			using (var fimg = new FloatImg(frameViewImage, 0))
			{
				var dlg = new BeadSelectorDlg.BeadSelectorDlg(fimg, Settings.ROI, beadPosList.ToArray());
				if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
                    beadPosList = dlg.ROIPositions.ToList();
                    Settings.ROI = dlg.ROISize;
                    UpdateInfo();
				}
			}
		}

        private void propertyGridQTrkSettings_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {

        }

        public void UpdateInfo()
        {
            UpdateDirectory();
            labelNumBeads.Text = string.Format( "#Beads: {0}. Frame: {1}.  LUT Steps: {2}", beadPosList.Count, numFrames,numLUTSteps);
        }

        private void textExpDir_TextChanged(object sender, EventArgs e)
        {
            Settings.ExpDir = textExpDir.Text;
            UpdateDirectory();
        }

        private void textLUTDir_TextChanged(object sender, EventArgs e)
        {
            Settings.LUTDir = textLUTDir.Text;
            UpdateDirectory();
        }

        void UpdateDirectory()
        {
            if (Directory.Exists(Settings.LUTDir))
            {
                var jpgs = Directory.GetFiles(Settings.ExpDir, "*.jpg");
                numLUTSteps = jpgs.Length;
            }

            if (Directory.Exists(Settings.ExpDir))
            {
                var jpgs = Directory.GetFiles(Settings.ExpDir, "*.jpg");

                numFrames = jpgs.Length;
                trackBarFrameView.Maximum = jpgs.Length;
                UpdateFrameView();
            }
        }

        void UpdateFrameView()
        {
            string path = Settings.ExpDir + Path.DirectorySeparatorChar + string.Format("{0:D8}.jpg", trackBarFrameView.Value);
            if (File.Exists(path))
            {
                var bmp = new Bitmap(path);
                frameView.Image = bmp;
                if (frameViewImage != null) frameViewImage.Dispose();
                frameViewImage = bmp;
            }
        }

        private void propertyGridQTrkSettings_Click(object sender, EventArgs e)
        {

        }

        private void trackBarFrameView_Scroll(object sender, EventArgs e)
        {
            UpdateFrameView();
        }

        private void buttonBuildLUT_Click(object sender, EventArgs e)
        {
            using (QTrkInstance inst = new QTrkInstance(GetTrackerConfig()))
            {
                inst.BeginLUT(true);

                for (int i = 0; i < numLUTSteps; i++)
                {
                    string path = GetImagePath(Settings.LUTDir, i);
                    if (!File.Exists(path))
                    {
                        MessageBox.Show("File " + path + " missing");
                        break;
                    }
                    using (Bitmap bmp = new Bitmap(path))
                    {
                        using (var fi = new FloatImg(bmp, 0))
                        {
                            inst.ProcessLUTFrame(fi.ImageData, beadPosList.ToArray(), i);
                        }
                    }
                }

                inst.FinalizeLUT();
                luts = inst.GetRadialZLUT();
            }
        }

        string GetImagePath(string basepath, int jpgIndex)
        {
            return basepath + Path.DirectorySeparatorChar + string.Format("{0:D8}.jpg", jpgIndex);
        }

        private void OfflineTrackerDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            Settings.Save();
        }

        string BeadListPath
        {
            get { return Settings.ExpDir +  Path.DirectorySeparatorChar + "beadlist.xml"; }
        }

        private void buttonLoadBeadlist_Click(object sender, EventArgs e)
        {
            using (Stream stream = File.OpenRead(BeadListPath))
            {
                XmlSerializer s = new XmlSerializer(typeof(Int2[]));
                var result=(Int2[])s.Deserialize(stream);
                beadPosList = result.ToList();
            }
        }

        private void buttonSaveBeadlist_Click(object sender, EventArgs e)
        {
            XmlSerializer s = new XmlSerializer(typeof(Int2[]));
            using (Stream stream = File.OpenWrite(BeadListPath))
            {
                s.Serialize(stream, beadPosList.ToArray());
            }
        }
	}
}

