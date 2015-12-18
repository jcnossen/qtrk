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

namespace OfflineTracker
{
	public partial class OfflineTrackerDlg : Form
	{
        List<Int2> beadPosList = new List<Int2>();

		public OfflineTrackerDlg()
		{
			InitializeComponent();

            if (!DesignMode)
            {
                QTrkInstance.SelectDLL(true, false);

                var d = QTrkConfig.Default;
                d.width = d.height = 80;
                propertyGridQTrkSettings.SelectedObject = d;
                UpdateInfo();
            }
		}

		QTrkConfig Config
		{
			get { return (QTrkConfig)propertyGridQTrkSettings.SelectedObject; }
            set { propertyGridQTrkSettings.SelectedObject = value; }
		}

		private void buttonSelectExpDir_Click(object sender, EventArgs e)
		{
			var fbd = new FolderBrowserDialog() { Description = "Select experiment folder with images" };
			if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				textExpDir.Text = fbd.SelectedPath;
                UpdateInfo();
            }
		}

		private void buttonSelectLUTDir_Click(object sender, EventArgs e)
		{
			var fbd = new FolderBrowserDialog() { Description = "Select LUT folder" };
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
					var cfg=Config;
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
			using(Bitmap bmp=new Bitmap("selectortest.png")) {
				using (var fimg = new FloatImg(bmp, 0))
				{
					var dlg = new BeadSelectorDlg.BeadSelectorDlg(fimg, Config.width, beadPosList.ToArray());
					if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
					{
                        beadPosList = dlg.ROIPositions.ToList();
                        UpdateInfo();
					}
				}
			}
		}

        private void propertyGridQTrkSettings_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {

        }

        public void UpdateInfo()
        {
            labelNumBeads.Text = "#Beads: " + beadPosList.Count;
        }
	}
}

