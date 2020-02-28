using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TrackerDlgUtils
{
	public partial class ProgressBarDlg : Form
	{
		public ProgressBarDlg()
		{
			InitializeComponent();
			progressBar.Maximum = 100;
		}

		public void Update(float prog, string text, string title)
		{
			this.Invoke(new Action(delegate
			{
				progressBar.Value = (int)(prog * 100);

				labelPercentage.Text = string.Format("{0}%", (int)(prog * 100));
				labelInfo.Text = text;
				Text = title;
			}));
		}

		public void Show(Action op)
		{
			System.Threading.ThreadPool.QueueUserWorkItem(new System.Threading.WaitCallback(
				delegate
				{
					op();
					Invoke(new Action(delegate { Close(); }));
				}));

			ShowDialog();
		}

		private void ProgressBarDlg_Load(object sender, EventArgs e)
		{
		}

	}
}
