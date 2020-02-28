using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NanoBLOC
{
	public partial class IntroDlg : Form
	{
		public IntroDlg()
		{
			InitializeComponent();
			timer.Tick += timer_Tick;
		}

		void timer_Tick(object sender, EventArgs e)
		{
			timer.Stop();
			new OfflineTrackerDlg(this).Show();
		}

		private void IntroDlg_Load(object sender, EventArgs e)
		{
		}

		private void IntroDlg_Shown(object sender, EventArgs e)
		{
			timer.Interval = 50;
			timer.Start();
		}
	}
}
