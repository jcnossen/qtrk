using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using QTrkDotNet;

namespace NanoBLOC
{
	public partial class LUTViewer : UserControl
	{
		public LUTViewer()
		{
			InitializeComponent();
		}

		int zplanes;
		FloatImg lutImage;

		public void SetLUT(FloatImg lutimg, int zplanes)
		{
			lutImage = lutimg;
			this.zplanes = zplanes;

			if (lutimg != null)
			{
				int lutcount = lutimg.h / zplanes;
				trackBarBeadIndex.Maximum = Math.Max(0, lutcount - 1);
			}
			UpdateImage();
		}

        private void trackBarBeadIndex_Scroll(object sender, EventArgs e)
        {
			UpdateImage();
        }

		void UpdateImage()
		{
			if (lutImage!=null) {
				using (var img = lutImage.ExtractSubsection(trackBarBeadIndex.Value * zplanes, zplanes))
				{
					img.Normalize();
					pictureBoxLUT.Image = img.ToImage();
				}
			}
			else
				pictureBoxLUT.Image = null;
		}
    }
}
