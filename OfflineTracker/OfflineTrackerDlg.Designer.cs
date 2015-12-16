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
			this.buttonDiscardBead = new System.Windows.Forms.Button();
			this.lutViewer = new OfflineTracker.LUTViewer();
			this.textLUTDir = new System.Windows.Forms.TextBox();
			this.textExpDir = new System.Windows.Forms.TextBox();
			this.buttonSelectLUTDir = new System.Windows.Forms.Button();
			this.buttonSelectExpDir = new System.Windows.Forms.Button();
			this.propertyGridQTrkSettings = new System.Windows.Forms.PropertyGrid();
			this.buttonGenerateTestLUT = new System.Windows.Forms.Button();
			this.buttonBuildLUT = new System.Windows.Forms.Button();
			this.tabPageTrack = new System.Windows.Forms.TabPage();
			this.buttonSelectBeads = new System.Windows.Forms.Button();
			this.tabControl1.SuspendLayout();
			this.tabPageSetup.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.tabPageSetup);
			this.tabControl1.Controls.Add(this.tabPageTrack);
			this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.tabControl1.Location = new System.Drawing.Point(0, 0);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(1006, 517);
			this.tabControl1.TabIndex = 0;
			// 
			// tabPageSetup
			// 
			this.tabPageSetup.Controls.Add(this.buttonSelectBeads);
			this.tabPageSetup.Controls.Add(this.buttonDiscardBead);
			this.tabPageSetup.Controls.Add(this.lutViewer);
			this.tabPageSetup.Controls.Add(this.textLUTDir);
			this.tabPageSetup.Controls.Add(this.textExpDir);
			this.tabPageSetup.Controls.Add(this.buttonSelectLUTDir);
			this.tabPageSetup.Controls.Add(this.buttonSelectExpDir);
			this.tabPageSetup.Controls.Add(this.propertyGridQTrkSettings);
			this.tabPageSetup.Controls.Add(this.buttonGenerateTestLUT);
			this.tabPageSetup.Controls.Add(this.buttonBuildLUT);
			this.tabPageSetup.Location = new System.Drawing.Point(4, 25);
			this.tabPageSetup.Name = "tabPageSetup";
			this.tabPageSetup.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageSetup.Size = new System.Drawing.Size(998, 488);
			this.tabPageSetup.TabIndex = 0;
			this.tabPageSetup.Text = "Configure";
			this.tabPageSetup.UseVisualStyleBackColor = true;
			// 
			// buttonDiscardBead
			// 
			this.buttonDiscardBead.Location = new System.Drawing.Point(568, 105);
			this.buttonDiscardBead.Name = "buttonDiscardBead";
			this.buttonDiscardBead.Size = new System.Drawing.Size(144, 23);
			this.buttonDiscardBead.TabIndex = 5;
			this.buttonDiscardBead.Text = "Discard Bead";
			this.buttonDiscardBead.UseVisualStyleBackColor = true;
			// 
			// lutViewer
			// 
			this.lutViewer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.lutViewer.Location = new System.Drawing.Point(396, 134);
			this.lutViewer.Name = "lutViewer";
			this.lutViewer.Size = new System.Drawing.Size(583, 342);
			this.lutViewer.TabIndex = 4;
			// 
			// textLUTDir
			// 
			this.textLUTDir.Location = new System.Drawing.Point(568, 48);
			this.textLUTDir.Name = "textLUTDir";
			this.textLUTDir.Size = new System.Drawing.Size(357, 22);
			this.textLUTDir.TabIndex = 3;
			// 
			// textExpDir
			// 
			this.textExpDir.Location = new System.Drawing.Point(568, 19);
			this.textExpDir.Name = "textExpDir";
			this.textExpDir.Size = new System.Drawing.Size(357, 22);
			this.textExpDir.TabIndex = 3;
			// 
			// buttonSelectLUTDir
			// 
			this.buttonSelectLUTDir.Location = new System.Drawing.Point(384, 47);
			this.buttonSelectLUTDir.Name = "buttonSelectLUTDir";
			this.buttonSelectLUTDir.Size = new System.Drawing.Size(178, 23);
			this.buttonSelectLUTDir.TabIndex = 2;
			this.buttonSelectLUTDir.Text = "Select LUT dir";
			this.buttonSelectLUTDir.UseVisualStyleBackColor = true;
			this.buttonSelectLUTDir.Click += new System.EventHandler(this.buttonSelectLUTDir_Click);
			// 
			// buttonSelectExpDir
			// 
			this.buttonSelectExpDir.Location = new System.Drawing.Point(384, 18);
			this.buttonSelectExpDir.Name = "buttonSelectExpDir";
			this.buttonSelectExpDir.Size = new System.Drawing.Size(178, 23);
			this.buttonSelectExpDir.TabIndex = 2;
			this.buttonSelectExpDir.Text = "Select experiment dir";
			this.buttonSelectExpDir.UseVisualStyleBackColor = true;
			this.buttonSelectExpDir.Click += new System.EventHandler(this.buttonSelectExpDir_Click);
			// 
			// propertyGridQTrkSettings
			// 
			this.propertyGridQTrkSettings.Location = new System.Drawing.Point(8, 221);
			this.propertyGridQTrkSettings.Name = "propertyGridQTrkSettings";
			this.propertyGridQTrkSettings.Size = new System.Drawing.Size(363, 255);
			this.propertyGridQTrkSettings.TabIndex = 1;
			// 
			// buttonGenerateTestLUT
			// 
			this.buttonGenerateTestLUT.Location = new System.Drawing.Point(384, 105);
			this.buttonGenerateTestLUT.Name = "buttonGenerateTestLUT";
			this.buttonGenerateTestLUT.Size = new System.Drawing.Size(178, 23);
			this.buttonGenerateTestLUT.TabIndex = 0;
			this.buttonGenerateTestLUT.Text = "Generate test LUT";
			this.buttonGenerateTestLUT.UseVisualStyleBackColor = true;
			this.buttonGenerateTestLUT.Click += new System.EventHandler(this.buttonGenerateTestLUT_Click);
			// 
			// buttonBuildLUT
			// 
			this.buttonBuildLUT.Location = new System.Drawing.Point(384, 76);
			this.buttonBuildLUT.Name = "buttonBuildLUT";
			this.buttonBuildLUT.Size = new System.Drawing.Size(178, 23);
			this.buttonBuildLUT.TabIndex = 0;
			this.buttonBuildLUT.Text = "Compute LUT from files";
			this.buttonBuildLUT.UseVisualStyleBackColor = true;
			// 
			// tabPageTrack
			// 
			this.tabPageTrack.Location = new System.Drawing.Point(4, 25);
			this.tabPageTrack.Name = "tabPageTrack";
			this.tabPageTrack.Padding = new System.Windows.Forms.Padding(3);
			this.tabPageTrack.Size = new System.Drawing.Size(998, 488);
			this.tabPageTrack.TabIndex = 1;
			this.tabPageTrack.Text = "Tracking";
			this.tabPageTrack.UseVisualStyleBackColor = true;
			// 
			// buttonSelectBeads
			// 
			this.buttonSelectBeads.Location = new System.Drawing.Point(146, 48);
			this.buttonSelectBeads.Name = "buttonSelectBeads";
			this.buttonSelectBeads.Size = new System.Drawing.Size(178, 23);
			this.buttonSelectBeads.TabIndex = 6;
			this.buttonSelectBeads.Text = "Select beads";
			this.buttonSelectBeads.UseVisualStyleBackColor = true;
			this.buttonSelectBeads.Click += new System.EventHandler(this.buttonSelectBeads_Click);
			// 
			// OfflineTrackerDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1006, 517);
			this.Controls.Add(this.tabControl1);
			this.Name = "OfflineTrackerDlg";
			this.Text = "Spherical bead image tracker";
			this.tabControl1.ResumeLayout(false);
			this.tabPageSetup.ResumeLayout(false);
			this.tabPageSetup.PerformLayout();
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
	}
}

