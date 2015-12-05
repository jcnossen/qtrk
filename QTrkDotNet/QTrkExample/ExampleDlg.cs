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
        QTrkInstance qtrk;

        public ExampleDlg()
        {
            InitializeComponent();

            QTrkInstance.SelectDLL(true, false);

            QTrkDLL.TestDLLCallConv(10);

            QTrkConfig cfg = QTrkConfig.Default;
            cfg.width = cfg.height = 60;
            qtrk = new QTrkInstance(cfg);

        }
    }
}
