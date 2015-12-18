namespace OfflineTracker
{
	partial class OfflineTrackerDlg
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageSetup = new System.Windows.Forms.TabPage();
            this.labelNumBeads = new System.Windows.Forms.Label();
            this.buttonSelectBeads = new System.Windows.Forms.Button();
            this.buttonDiscardBead = new System.Windows.Forms.Button();
            this.textLUTDir = new System.Windows.Forms.TextBox();
            this.textExpDir = new System.Windows.Forms.TextBox();
            this.buttonSelectLUTDir = new System.Windows.Forms.Button();
            this.buttonSelectExpDir = new System.Windows.Forms.Button();
            this.propertyGridQTrkSettings = new System.Windows.Forms.PropertyGrid();
            this.buttonGenerateTestLUT = new System.Windows.Forms.Button();
            this.buttonBuildLUT = new System.Windows.Forms.Button();
            this.tabPageTrack = new System.Windows.Forms.TabPage();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxPixelSize = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.textBoxLUTStep = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.textBoxZCorrection = new System.Windows.Forms.TextBox();
            this.frameView = new System.Windows.Forms.PictureBox();
            this.trackBarFrameView = new System.Windows.Forms.TrackBar();
            this.lutViewer = new OfflineTracker.LUTViewer();
            this.buttonSaveBeadlist = new System.Windows.Forms.Button();
            this.buttonLoadBeadlist = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPageSetup.SuspendLayout();
            this.tabPageTrack.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.frameView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFrameView)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageSetup);
            this.tabControl1.Controls.Add(this.tabPageTrack);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(2);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(880, 528);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPageSetup
            // 
            this.tabPageSetup.Controls.Add(this.buttonLoadBeadlist);
            this.tabPageSetup.Controls.Add(this.buttonSaveBeadlist);
            this.tabPageSetup.Controls.Add(this.trackBarFrameView);
            this.tabPageSetup.Controls.Add(this.frameView);
            this.tabPageSetup.Controls.Add(this.textBoxZCorrection);
            this.tabPageSetup.Controls.Add(this.textBoxLUTStep);
            this.tabPageSetup.Controls.Add(this.textBoxPixelSize);
            this.tabPageSetup.Controls.Add(this.label3);
            this.tabPageSetup.Controls.Add(this.label2);
            this.tabPageSetup.Controls.Add(this.label1);
            this.tabPageSetup.Controls.Add(this.labelNumBeads);
            this.tabPageSetup.Controls.Add(this.buttonSelectBeads);
            this.tabPageSetup.Controls.Add(this.buttonDiscardBead);
            this.tabPageSetup.Controls.Add(this.lutViewer);
            this.tabPageSetup.Controls.Add(this.textLUTDir);
            this.tabPageSetup.Controls.Add(this.textExpDir);
            this.tabPageSetup.Controls.Add(this.buttonSelectLUTDir);
            this.tabPageSetup.Controls.Add(this.buttonSelectExpDir);
            this.tabPageSetup.Controls.Add(this.buttonGenerateTestLUT);
            this.tabPageSetup.Controls.Add(this.buttonBuildLUT);
            this.tabPageSetup.Location = new System.Drawing.Point(4, 22);
            this.tabPageSetup.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageSetup.Name = "tabPageSetup";
            this.tabPageSetup.Padding = new System.Windows.Forms.Padding(2);
            this.tabPageSetup.Size = new System.Drawing.Size(872, 502);
            this.tabPageSetup.TabIndex = 0;
            this.tabPageSetup.Text = "Configure";
            this.tabPageSetup.UseVisualStyleBackColor = true;
            // 
            // labelNumBeads
            // 
            this.labelNumBeads.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.labelNumBeads.Location = new System.Drawing.Point(493, 85);
            this.labelNumBeads.Name = "labelNumBeads";
            this.labelNumBeads.Size = new System.Drawing.Size(222, 19);
            this.labelNumBeads.TabIndex = 6;
            // 
            // buttonSelectBeads
            // 
            this.buttonSelectBeads.Location = new System.Drawing.Point(426, 63);
            this.buttonSelectBeads.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSelectBeads.Name = "buttonSelectBeads";
            this.buttonSelectBeads.Size = new System.Drawing.Size(108, 19);
            this.buttonSelectBeads.TabIndex = 5;
            this.buttonSelectBeads.Text = "Select beads";
            this.buttonSelectBeads.UseVisualStyleBackColor = true;
            this.buttonSelectBeads.Click += new System.EventHandler(this.buttonSelectBeads_Click);
            // 
            // buttonDiscardBead
            // 
            this.buttonDiscardBead.Location = new System.Drawing.Point(538, 63);
            this.buttonDiscardBead.Margin = new System.Windows.Forms.Padding(2);
            this.buttonDiscardBead.Name = "buttonDiscardBead";
            this.buttonDiscardBead.Size = new System.Drawing.Size(108, 19);
            this.buttonDiscardBead.TabIndex = 5;
            this.buttonDiscardBead.Text = "Discard Bead";
            this.buttonDiscardBead.UseVisualStyleBackColor = true;
            // 
            // textLUTDir
            // 
            this.textLUTDir.Location = new System.Drawing.Point(426, 39);
            this.textLUTDir.Margin = new System.Windows.Forms.Padding(2);
            this.textLUTDir.Name = "textLUTDir";
            this.textLUTDir.Size = new System.Drawing.Size(269, 20);
            this.textLUTDir.TabIndex = 3;
            this.textLUTDir.TextChanged += new System.EventHandler(this.textLUTDir_TextChanged);
            // 
            // textExpDir
            // 
            this.textExpDir.Location = new System.Drawing.Point(426, 15);
            this.textExpDir.Margin = new System.Windows.Forms.Padding(2);
            this.textExpDir.Name = "textExpDir";
            this.textExpDir.Size = new System.Drawing.Size(269, 20);
            this.textExpDir.TabIndex = 3;
            this.textExpDir.TextChanged += new System.EventHandler(this.textExpDir_TextChanged);
            // 
            // buttonSelectLUTDir
            // 
            this.buttonSelectLUTDir.Location = new System.Drawing.Point(288, 38);
            this.buttonSelectLUTDir.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSelectLUTDir.Name = "buttonSelectLUTDir";
            this.buttonSelectLUTDir.Size = new System.Drawing.Size(134, 19);
            this.buttonSelectLUTDir.TabIndex = 2;
            this.buttonSelectLUTDir.Text = "Select LUT dir";
            this.buttonSelectLUTDir.UseVisualStyleBackColor = true;
            this.buttonSelectLUTDir.Click += new System.EventHandler(this.buttonSelectLUTDir_Click);
            // 
            // buttonSelectExpDir
            // 
            this.buttonSelectExpDir.Location = new System.Drawing.Point(288, 15);
            this.buttonSelectExpDir.Margin = new System.Windows.Forms.Padding(2);
            this.buttonSelectExpDir.Name = "buttonSelectExpDir";
            this.buttonSelectExpDir.Size = new System.Drawing.Size(134, 19);
            this.buttonSelectExpDir.TabIndex = 2;
            this.buttonSelectExpDir.Text = "Select experiment dir";
            this.buttonSelectExpDir.UseVisualStyleBackColor = true;
            this.buttonSelectExpDir.Click += new System.EventHandler(this.buttonSelectExpDir_Click);
            // 
            // propertyGridQTrkSettings
            // 
            this.propertyGridQTrkSettings.CategoryForeColor = System.Drawing.SystemColors.InactiveCaptionText;
            this.propertyGridQTrkSettings.Location = new System.Drawing.Point(7, 4);
            this.propertyGridQTrkSettings.Margin = new System.Windows.Forms.Padding(2);
            this.propertyGridQTrkSettings.Name = "propertyGridQTrkSettings";
            this.propertyGridQTrkSettings.Size = new System.Drawing.Size(272, 207);
            this.propertyGridQTrkSettings.TabIndex = 1;
            this.propertyGridQTrkSettings.PropertyValueChanged += new System.Windows.Forms.PropertyValueChangedEventHandler(this.propertyGridQTrkSettings_PropertyValueChanged);
            this.propertyGridQTrkSettings.Click += new System.EventHandler(this.propertyGridQTrkSettings_Click);
            // 
            // buttonGenerateTestLUT
            // 
            this.buttonGenerateTestLUT.Location = new System.Drawing.Point(288, 85);
            this.buttonGenerateTestLUT.Margin = new System.Windows.Forms.Padding(2);
            this.buttonGenerateTestLUT.Name = "buttonGenerateTestLUT";
            this.buttonGenerateTestLUT.Size = new System.Drawing.Size(200, 19);
            this.buttonGenerateTestLUT.TabIndex = 0;
            this.buttonGenerateTestLUT.Text = "Generate test images from LUT file";
            this.buttonGenerateTestLUT.UseVisualStyleBackColor = true;
            this.buttonGenerateTestLUT.Click += new System.EventHandler(this.buttonGenerateTestLUT_Click);
            // 
            // buttonBuildLUT
            // 
            this.buttonBuildLUT.Location = new System.Drawing.Point(288, 62);
            this.buttonBuildLUT.Margin = new System.Windows.Forms.Padding(2);
            this.buttonBuildLUT.Name = "buttonBuildLUT";
            this.buttonBuildLUT.Size = new System.Drawing.Size(134, 19);
            this.buttonBuildLUT.TabIndex = 0;
            this.buttonBuildLUT.Text = "Build LUT";
            this.buttonBuildLUT.UseVisualStyleBackColor = true;
            this.buttonBuildLUT.Click += new System.EventHandler(this.buttonBuildLUT_Click);
            // 
            // tabPageTrack
            // 
            this.tabPageTrack.Controls.Add(this.propertyGridQTrkSettings);
            this.tabPageTrack.Location = new System.Drawing.Point(4, 22);
            this.tabPageTrack.Margin = new System.Windows.Forms.Padding(2);
            this.tabPageTrack.Name = "tabPageTrack";
            this.tabPageTrack.Padding = new System.Windows.Forms.Padding(2);
            this.tabPageTrack.Size = new System.Drawing.Size(872, 502);
            this.tabPageTrack.TabIndex = 1;
            this.tabPageTrack.Text = "Tracking";
            this.tabPageTrack.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "Pixel size:  (nm)";
            // 
            // textBoxPixelSize
            // 
            this.textBoxPixelSize.Location = new System.Drawing.Point(138, 14);
            this.textBoxPixelSize.Name = "textBoxPixelSize";
            this.textBoxPixelSize.Size = new System.Drawing.Size(100, 20);
            this.textBoxPixelSize.TabIndex = 8;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "LUT Step distance (nm)";
            // 
            // textBoxLUTStep
            // 
            this.textBoxLUTStep.Location = new System.Drawing.Point(138, 41);
            this.textBoxLUTStep.Name = "textBoxLUTStep";
            this.textBoxLUTStep.Size = new System.Drawing.Size(100, 20);
            this.textBoxLUTStep.TabIndex = 8;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 69);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(98, 13);
            this.label3.TabIndex = 7;
            this.label3.Text = "Z Correction factor:";
            // 
            // textBoxZCorrection
            // 
            this.textBoxZCorrection.Location = new System.Drawing.Point(138, 66);
            this.textBoxZCorrection.Name = "textBoxZCorrection";
            this.textBoxZCorrection.Size = new System.Drawing.Size(100, 20);
            this.textBoxZCorrection.TabIndex = 8;
            this.textBoxZCorrection.Text = "0.88";
            // 
            // frameView
            // 
            this.frameView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.frameView.Location = new System.Drawing.Point(286, 107);
            this.frameView.Name = "frameView";
            this.frameView.Size = new System.Drawing.Size(583, 359);
            this.frameView.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.frameView.TabIndex = 9;
            this.frameView.TabStop = false;
            // 
            // trackBarFrameView
            // 
            this.trackBarFrameView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.trackBarFrameView.Location = new System.Drawing.Point(284, 472);
            this.trackBarFrameView.Name = "trackBarFrameView";
            this.trackBarFrameView.Size = new System.Drawing.Size(583, 45);
            this.trackBarFrameView.TabIndex = 10;
            this.trackBarFrameView.Scroll += new System.EventHandler(this.trackBarFrameView_Scroll);
            // 
            // lutViewer
            // 
            this.lutViewer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.lutViewer.Location = new System.Drawing.Point(7, 256);
            this.lutViewer.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.lutViewer.Name = "lutViewer";
            this.lutViewer.Size = new System.Drawing.Size(272, 239);
            this.lutViewer.TabIndex = 4;
            // 
            // buttonSaveBeadlist
            // 
            this.buttonSaveBeadlist.Location = new System.Drawing.Point(745, 15);
            this.buttonSaveBeadlist.Name = "buttonSaveBeadlist";
            this.buttonSaveBeadlist.Size = new System.Drawing.Size(104, 23);
            this.buttonSaveBeadlist.TabIndex = 11;
            this.buttonSaveBeadlist.Text = "Save beadlist";
            this.buttonSaveBeadlist.UseVisualStyleBackColor = true;
            this.buttonSaveBeadlist.Click += new System.EventHandler(this.buttonSaveBeadlist_Click);
            // 
            // buttonLoadBeadlist
            // 
            this.buttonLoadBeadlist.Location = new System.Drawing.Point(745, 41);
            this.buttonLoadBeadlist.Name = "buttonLoadBeadlist";
            this.buttonLoadBeadlist.Size = new System.Drawing.Size(104, 23);
            this.buttonLoadBeadlist.TabIndex = 11;
            this.buttonLoadBeadlist.Text = "Load beadlist";
            this.buttonLoadBeadlist.UseVisualStyleBackColor = true;
            this.buttonLoadBeadlist.Click += new System.EventHandler(this.buttonLoadBeadlist_Click);
            // 
            // OfflineTrackerDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(880, 528);
            this.Controls.Add(this.tabControl1);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "OfflineTrackerDlg";
            this.Text = "Spherical bead image tracker";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OfflineTrackerDlg_FormClosing);
            this.tabControl1.ResumeLayout(false);
            this.tabPageSetup.ResumeLayout(false);
            this.tabPageSetup.PerformLayout();
            this.tabPageTrack.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.frameView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarFrameView)).EndInit();
            this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPageSetup;
		private System.Windows.Forms.TabPage tabPageTrack;
		private System.Windows.Forms.TextBox textLUTDir;
		private System.Windows.Forms.TextBox textExpDir;
		private System.Windows.Forms.Button buttonSelectLUTDir;
		private System.Windows.Forms.Button buttonSelectExpDir;
		private System.Windows.Forms.PropertyGrid propertyGridQTrkSettings;
		private System.Windows.Forms.Button buttonGenerateTestLUT;
		private System.Windows.Forms.Button buttonBuildLUT;
		private System.Windows.Forms.Button buttonDiscardBead;
		private LUTViewer lutViewer;
        private System.Windows.Forms.Button buttonSelectBeads;
        private System.Windows.Forms.Label labelNumBeads;
        private System.Windows.Forms.TextBox textBoxZCorrection;
        private System.Windows.Forms.TextBox textBoxLUTStep;
        private System.Windows.Forms.TextBox textBoxPixelSize;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox frameView;
        private System.Windows.Forms.TrackBar trackBarFrameView;
        private System.Windows.Forms.Button buttonLoadBeadlist;
        private System.Windows.Forms.Button buttonSaveBeadlist;
	}
}

