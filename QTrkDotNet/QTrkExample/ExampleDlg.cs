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

namespace QTrkExample
{
    public partial class ExampleDlg : Form
    {
		FloatImg zlut;
		float zpos;

        public ExampleDlg()
        {
            InitializeComponent();

            QTrkInstance.SelectDLL(true, false);

            QTrkDLL.TestDLLCallConv(10);

            QTrkConfig cfg = QTrkConfig.Default;
            cfg.width = cfg.height = 60;
            
            propertyGridSettings.SelectedObject = cfg;
        }


		QTrkConfig QTrkConfig
		{
			get { return (QTrkConfig)propertyGridSettings.SelectedObject; }
		}
        

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
//                qtrk.Dispose();
            }

            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void buttonOpenLUT_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd=new OpenFileDialog()
            {
                Title="Select LUT image"
            };

            if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                Bitmap img = new Bitmap(ofd.FileName);
//				pictureBoxLUT.Image = img;
				zlut = new FloatImg(img, 0);
				pictureBoxLUT.Image = zlut.ToImage();
            }
        }


		private void pictureBoxLUT_MouseMove(object sender, MouseEventArgs e)
		{
			if (e.Button.HasFlag(System.Windows.Forms.MouseButtons.Left))
			{
				zpos = zlut.h * e.Y / (float)pictureBoxLUT.Height;
				GenerateImageFromLUT();
			}
		}

		private void pictureBoxLUT_MouseDown(object sender, MouseEventArgs e)
		{
			zpos = zlut.h * e.Y / (float)pictureBoxLUT.Height;
			GenerateImageFromLUT();
		}

		private void GenerateImageFromLUT()
		{
			QTrkConfig cfg=QTrkConfig;

			using (var dst = new FloatImg(100, 100))
			{
				QTrkUtil.GenerateImageFromLUT(dst, zlut, 3, 40, new Vector3(dst.w / 2, dst.h / 2, zpos), false, 1);
				if(trackBarNoise.Value>0) QTrkUtil.ApplyPoissonNoise(dst, trackBarNoise.Value, 255);
				pictureBoxFrameView.Image = dst.ToImage();
			}
		}

		private void pictureBoxFrameView_Click(object sender, EventArgs e)
		{

		}

		private void trackBarNoise_Scroll(object sender, EventArgs e)
		{
			GenerateImageFromLUT();
		}


    }
}
