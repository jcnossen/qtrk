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

namespace OfflineTracker
{
	public partial class LUTViewer : UserControl
	{
		public LUTViewer()
		{
			InitializeComponent();
		}

        FloatImg[] lutList;

        public QTrkDotNet.FloatImg[] LUTs
        {
            get
            {
                return lutList;
            }
            set
            {
                lutList = value;
                trackBarBeadIndex.Maximum = lutList.Length - 1;
            }
        }

        private void trackBarBeadIndex_Scroll(object sender, EventArgs e)
        {
            pictureBoxLUT.Image = lutList[trackBarBeadIndex.Value].ToImage();
        }
    }
}
