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

		public BeadSelectorDlg(FloatImg image)
		{
			InitializeComponent();

			if (!DesignMode)
			{
				dispImage = image.ToImage();
				this.image = image;
				pictureBox.Image = dispImage;
			}
			DialogResult = System.Windows.Forms.DialogResult.Cancel;
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

		}

		private void buttonAutofind_Click(object sender, EventArgs e)
		{

		}
	}
}
