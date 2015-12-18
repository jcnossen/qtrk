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

namespace BeadSelectorDlg
{
	public partial class BeadSelectorDlg : Form
	{
		Bitmap dispImage;
		FloatImg image;
        List<Int2> roiPositions = new List<Int2>();

        public Int2[] ROIPositions
        {
            get { return roiPositions.ToArray(); }
        }

		public BeadSelectorDlg(FloatImg image, int ROI, Int2[] positions)
		{
			InitializeComponent();

			if (!DesignMode)
			{
				dispImage = image.ToImage();
				this.image = image;
				pictureBox.Image = dispImage;
                roiPositions = positions.ToList();
			}
			DialogResult = System.Windows.Forms.DialogResult.Cancel;
            textBoxROI.Text = ROI.ToString();
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{
			DialogResult = System.Windows.Forms.DialogResult.OK;
			Close();
		}

		private void buttonCancel_Click(object sender, EventArgs e)
		{
			Close();
		}

		private void buttonRemoveAll_Click(object sender, EventArgs e)
		{
            roiPositions.Clear();
            pictureBox.Invalidate();

		}

		private void buttonAutofind_Click(object sender, EventArgs e)
		{

		}

        Int2 GetMousePosInImage(Int2 clientPos)
        {
            int x= (int)(clientPos.x / (float)pictureBox.Width * image.w);
            int y = (int)(clientPos.y / (float)pictureBox.Height * image.h);
            return new Int2(x, y);
        }

        private void pictureBox_MouseDown(object sender, MouseEventArgs e)
        {
            int roi = GetROI();
            Int2 clickPos = GetMousePosInImage(new Int2(e.X, e.Y));
            if (e.Button == MouseButtons.Left)
            {
                roiPositions.Add(clickPos - new Int2(roi/2, roi/2));
                pictureBox.Invalidate();
            }

            if (e.Button == MouseButtons.Middle)
            {
                roiPositions.RemoveAll(pos => clickPos.x >= pos.x && clickPos.x < pos.x + roi && clickPos.y >= pos.y && clickPos.y < pos.y + roi);
                pictureBox.Invalidate();
            }
        }

        int GetROI()
        {
            int roi;
            if (!int.TryParse(textBoxROI.Text, out roi))
            {
                textBoxROI.Text = "50";
                roi = 50;
            }
            return roi;
        }

        private void pictureBox_Paint(object sender, PaintEventArgs e)
        {
            int roi = GetROI();
            float sx = pictureBox.Width / (float)image.w;
            float sy = pictureBox.Height / (float)image.h;
            foreach (var pos in roiPositions)
            {
                e.Graphics.DrawRectangle(Pens.White, pos.x * sx, pos.y * sy, roi * sx, roi * sy);
            }
        }

        private void BeadSelectorDlg_Resize(object sender, EventArgs e)
        {
            pictureBox.Invalidate();
        }

        public int ROISize
        {
            get { return GetROI(); }
            set { textBoxROI.Text = value.ToString(); }
        }
    }
}
