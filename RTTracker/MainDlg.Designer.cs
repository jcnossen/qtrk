namespace RTTracker
{
    partial class MainDlg
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;


        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.buttonStartStop = new System.Windows.Forms.Button();
            this.imageViewer = new NationalInstruments.Vision.WindowsForms.ImageViewer();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.buttonStartStop);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.imageViewer);
            this.splitContainer1.Size = new System.Drawing.Size(766, 451);
            this.splitContainer1.SplitterDistance = 255;
            this.splitContainer1.TabIndex = 0;
            // 
            // buttonStartStop
            // 
            this.buttonStartStop.Location = new System.Drawing.Point(12, 12);
            this.buttonStartStop.Name = "buttonStartStop";
            this.buttonStartStop.Size = new System.Drawing.Size(75, 23);
            this.buttonStartStop.TabIndex = 0;
            this.buttonStartStop.Text = "Start";
            this.buttonStartStop.UseVisualStyleBackColor = true;
            this.buttonStartStop.Click += new System.EventHandler(this.buttonStartStop_Click);
            // 
            // imageViewer
            // 
            this.imageViewer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imageViewer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.imageViewer.Location = new System.Drawing.Point(0, 0);
            this.imageViewer.Name = "imageViewer";
            this.imageViewer.ShowImageInfo = true;
            this.imageViewer.ShowScrollbars = true;
            this.imageViewer.ShowToolbar = true;
            this.imageViewer.Size = new System.Drawing.Size(507, 451);
            this.imageViewer.TabIndex = 0;
            this.imageViewer.SizeChanged += new System.EventHandler(this.imageViewer_SizeChanged);
            // 
            // MainDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(766, 451);
            this.Controls.Add(this.splitContainer1);
            this.Name = "MainDlg";
            this.Text = "Realtime Spherical Bead Tracker";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainDlg_FormClosing);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private NationalInstruments.Vision.WindowsForms.ImageViewer imageViewer;
        private System.Windows.Forms.Button buttonStartStop;
    }
}

