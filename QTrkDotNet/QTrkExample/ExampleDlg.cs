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
        public ExampleDlg()
        {
            InitializeComponent();

            QTrkInstance.SelectDLL(true, false);

            QTrkDLL.TestDLLCallConv(10);

            QTrkConfig cfg = QTrkConfig.Default;
            cfg.width = cfg.height = 60;
            
            propertyGridSettings.SelectedObject = cfg;
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
                Image img = Image.FromFile(ofd.FileName);
            }
                
        }


    }
}
