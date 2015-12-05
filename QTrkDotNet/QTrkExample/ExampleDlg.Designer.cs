namespace QTrkExample
{
    partial class ExampleDlg
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
            this.buttonOpenLUT = new System.Windows.Forms.Button();
            this.buttonSpeedTest = new System.Windows.Forms.Button();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.pictureBoxLUT = new System.Windows.Forms.PictureBox();
            this.pictureBoxFrameView = new System.Windows.Forms.PictureBox();
            this.propertyGridSettings = new System.Windows.Forms.PropertyGrid();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLUT)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFrameView)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonOpenLUT
            // 
            this.buttonOpenLUT.Location = new System.Drawing.Point(3, 39);
            this.buttonOpenLUT.Name = "buttonOpenLUT";
            this.buttonOpenLUT.Size = new System.Drawing.Size(75, 23);
            this.buttonOpenLUT.TabIndex = 0;
            this.buttonOpenLUT.Text = "Open LUT";
            this.buttonOpenLUT.UseVisualStyleBackColor = true;
            this.buttonOpenLUT.Click += new System.EventHandler(this.buttonOpenLUT_Click);
            // 
            // buttonSpeedTest
            // 
            this.buttonSpeedTest.Location = new System.Drawing.Point(3, 10);
            this.buttonSpeedTest.Name = "buttonSpeedTest";
            this.buttonSpeedTest.Size = new System.Drawing.Size(75, 23);
            this.buttonSpeedTest.TabIndex = 0;
            this.buttonSpeedTest.Text = "Speed test";
            this.buttonSpeedTest.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.propertyGridSettings);
            this.splitContainer1.Panel1.Controls.Add(this.pictureBoxLUT);
            this.splitContainer1.Panel1.Controls.Add(this.buttonOpenLUT);
            this.splitContainer1.Panel1.Controls.Add(this.buttonSpeedTest);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.pictureBoxFrameView);
            this.splitContainer1.Size = new System.Drawing.Size(694, 450);
            this.splitContainer1.SplitterDistance = 230;
            this.splitContainer1.TabIndex = 4;
            // 
            // pictureBoxLUT
            // 
            this.pictureBoxLUT.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxLUT.Location = new System.Drawing.Point(12, 68);
            this.pictureBoxLUT.Name = "pictureBoxLUT";
            this.pictureBoxLUT.Size = new System.Drawing.Size(167, 148);
            this.pictureBoxLUT.TabIndex = 1;
            this.pictureBoxLUT.TabStop = false;
            // 
            // pictureBoxFrameView
            // 
            this.pictureBoxFrameView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBoxFrameView.Location = new System.Drawing.Point(3, 3);
            this.pictureBoxFrameView.Name = "pictureBoxFrameView";
            this.pictureBoxFrameView.Size = new System.Drawing.Size(454, 444);
            this.pictureBoxFrameView.TabIndex = 1;
            this.pictureBoxFrameView.TabStop = false;
            // 
            // propertyGridSettings
            // 
            this.propertyGridSettings.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.propertyGridSettings.Location = new System.Drawing.Point(3, 237);
            this.propertyGridSettings.Name = "propertyGridSettings";
            this.propertyGridSettings.Size = new System.Drawing.Size(224, 210);
            this.propertyGridSettings.TabIndex = 2;
            // 
            // ExampleDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(694, 450);
            this.Controls.Add(this.splitContainer1);
            this.Name = "ExampleDlg";
            this.Text = "Form1";
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxLUT)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFrameView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button buttonOpenLUT;
        private System.Windows.Forms.Button buttonSpeedTest;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.PictureBox pictureBoxFrameView;
        private System.Windows.Forms.PictureBox pictureBoxLUT;
        private System.Windows.Forms.PropertyGrid propertyGridSettings;
    }
}

