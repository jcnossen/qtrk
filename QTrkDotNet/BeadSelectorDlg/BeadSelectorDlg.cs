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

namespace TrackerDlgUtils
{
	public partial class BeadSelectorDlg : Form
	{
		Bitmap dispImage;
		FloatImg image;
		bool autoFindMode, drawSelectionRect;
        List<Int2> roiPositions = new List<Int2>();

        public Int2[] ROIPositions
        {
            get { return roiPositions.ToArray(); }
        }

		public BeadSelectorDlg(FloatImg image, int ROI, Int2[] beadCenterPos)
		{
			InitializeComponent();

			if (!DesignMode)
			{
				dispImage = image.ToImage();
				this.image = image;
				pictureBox.Image = dispImage;
                roiPositions = beadCenterPos.ToList();
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
			SwitchAutoFind();
		}

		void SwitchAutoFind()
		{
			if (autoFindMode)
			{
				autoFindMode = false;
				buttonAutofind.BackColor = SystemColors.ButtonFace;
				buttonAutofind.Text = "Autofind";
			}
			else
			{
				autoFindMode = true;
				buttonAutofind.BackColor = SystemColors.ButtonShadow;
				buttonAutofind.Text = "Select bead";
			}
		}

		void AutoFindBeads(Int2 beadCornerPos)
		{
			int roi=GetROI();
			beadCornerPos.Clamp(image.w - roi, image.h - roi);

			Cursor.Current = Cursors.WaitCursor;
			Int2[] beadpos = QTrkUtil.FindBeads(image, beadCornerPos, ROISize, AutoFindMinDist, AutoFindAcceptance);
			Cursor.Current = Cursors.Default;

			MessageBox.Show("beads: "+ beadpos.Length);
			roiPositions.AddRange(Array.ConvertAll(beadpos, corner => new Int2(corner.x + roi / 2, corner.y + roi / 2)));

			SwitchAutoFind();
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

			if (e.Button == MouseButtons.Left && !ModifierKeys.HasFlag(Keys.Control)) {
				Int2 beadPos = ClampCenterPos(clickPos);
				if (autoFindMode)
					AutoFindBeads(new Int2(beadPos.x - roi / 2, beadPos.y - roi / 2));
				else
				{
					roiPositions.Add(beadPos);
					pictureBox.Invalidate();
				}
			}

            if (e.Button == MouseButtons.Middle || ( e.Button== MouseButtons.Left && ModifierKeys.HasFlag(Keys.Control)))
            {
				roiPositions.RemoveAll(pos => clickPos.x >= pos.x - roi / 2 && clickPos.x < pos.x + roi / 2 && clickPos.y >= pos.y - roi / 2 && clickPos.y < pos.y + roi / 2);
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
			float roix = roi * sx, roiy = roi * sy;
            foreach (var pos in roiPositions)
                e.Graphics.DrawRectangle(Pens.White, (pos.x - roi/2) * sx, (pos.y -roi/2)* sy, roix, roiy);

			if (drawSelectionRect)
			{
				Point mousePos = pictureBox.PointToClient(MousePosition);
//				e.Graphics.DrawRectangle(Pens.Blue, mousePos.X,mousePos.Y, roix, roiy); 
				
				Int2 p = GetMousePosInImage(new Int2(mousePos.X, mousePos.Y));

				p = ClampCenterPos(new Int2(p.x, p.y));
				e.Graphics.DrawRectangle(Pens.Blue, (p.x - roi / 2)*sx, (p.y - roi / 2)*sy, roix, roiy);
			}
		}
		Int2 ClampCenterPos(Int2 p)
		{
			int roi = GetROI();
			if(p.x<roi/2) p.x=roi/2;
			if(p.y<roi/2) p.y=roi/2;
			if (p.x + roi/2 >= image.w) p.x = image.w - roi/2;
			if (p.y + roi/2 >= image.h) p.y = image.h - roi/2;
			return p;
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

		private void pictureBox_MouseMove(object sender, MouseEventArgs e)
		{
			drawSelectionRect = true;
			pictureBox.Invalidate();
		}

		private void pictureBox_MouseLeave(object sender, EventArgs e)
		{
			drawSelectionRect = false;
			pictureBox.Invalidate();
		}

		private void BeadSelectorDlg_Load(object sender, EventArgs e)
		{

		}

		void UpdateAutoFindConfig()
		{
			labelAcceptance.Text = string.Format("Acceptance:{0}", AutoFindAcceptance);
			labelMinDist.Text = string.Format("MinDist:{0}", AutoFindMinDist);
		}

		public float AutoFindAcceptance
		{
			get
			{
				return trackBarAcceptance.Value / 100.0f;
			}
			set
			{
				trackBarAcceptance.Value = (int)(value * 100);
				UpdateAutoFindConfig();
			}
		}
		public float AutoFindMinDist
		{
			get
			{
				return trackBarMinDist.Value / 100.0f;
			}
			set
			{
				trackBarMinDist.Value = (int)(value * 100);
				UpdateAutoFindConfig();
			}
		}

		private void trackBarAcceptance_Scroll(object sender, EventArgs e)
		{
			UpdateAutoFindConfig();
		}

		private void trackBarMinDist_Scroll(object sender, EventArgs e)
		{
			UpdateAutoFindConfig();
		}
    }
}
