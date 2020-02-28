namespace TrackerDlgUtils
{
	partial class BeadSelectorDlg
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
			this.pictureBox = new System.Windows.Forms.PictureBox();
			this.buttonOK = new System.Windows.Forms.Button();
			this.buttonCancel = new System.Windows.Forms.Button();
			this.buttonRemoveAll = new System.Windows.Forms.Button();
			this.buttonAutofind = new System.Windows.Forms.Button();
			this.textBoxROI = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.labelAcceptance = new System.Windows.Forms.Label();
			this.labelMinDist = new System.Windows.Forms.Label();
			this.trackBarAcceptance = new System.Windows.Forms.TrackBar();
			this.trackBarMinDist = new System.Windows.Forms.TrackBar();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBarAcceptance)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBarMinDist)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBox
			// 
			this.pictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBox.Location = new System.Drawing.Point(121, 12);
			this.pictureBox.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.pictureBox.Name = "pictureBox";
			this.pictureBox.Size = new System.Drawing.Size(649, 498);
			this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox.TabIndex = 0;
			this.pictureBox.TabStop = false;
			this.pictureBox.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBox_Paint);
			this.pictureBox.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseDown);
			this.pictureBox.MouseLeave += new System.EventHandler(this.pictureBox_MouseLeave);
			this.pictureBox.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBox_MouseMove);
			// 
			// buttonOK
			// 
			this.buttonOK.Location = new System.Drawing.Point(12, 12);
			this.buttonOK.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(103, 28);
			this.buttonOK.TabIndex = 1;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Location = new System.Drawing.Point(12, 46);
			this.buttonCancel.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(103, 28);
			this.buttonCancel.TabIndex = 1;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// buttonRemoveAll
			// 
			this.buttonRemoveAll.Location = new System.Drawing.Point(12, 80);
			this.buttonRemoveAll.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.buttonRemoveAll.Name = "buttonRemoveAll";
			this.buttonRemoveAll.Size = new System.Drawing.Size(103, 28);
			this.buttonRemoveAll.TabIndex = 1;
			this.buttonRemoveAll.Text = "Remove all";
			this.buttonRemoveAll.UseVisualStyleBackColor = true;
			this.buttonRemoveAll.Click += new System.EventHandler(this.buttonRemoveAll_Click);
			// 
			// buttonAutofind
			// 
			this.buttonAutofind.Location = new System.Drawing.Point(12, 465);
			this.buttonAutofind.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.buttonAutofind.Name = "buttonAutofind";
			this.buttonAutofind.Size = new System.Drawing.Size(103, 28);
			this.buttonAutofind.TabIndex = 1;
			this.buttonAutofind.Text = "Autofind";
			this.buttonAutofind.UseVisualStyleBackColor = true;
			this.buttonAutofind.Click += new System.EventHandler(this.buttonAutofind_Click);
			// 
			// textBoxROI
			// 
			this.textBoxROI.Location = new System.Drawing.Point(12, 139);
			this.textBoxROI.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.textBoxROI.Name = "textBoxROI";
			this.textBoxROI.Size = new System.Drawing.Size(64, 22);
			this.textBoxROI.TabIndex = 2;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 119);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(36, 17);
			this.label1.TabIndex = 3;
			this.label1.Text = "ROI:";
			// 
			// labelAcceptance
			// 
			this.labelAcceptance.Location = new System.Drawing.Point(12, 245);
			this.labelAcceptance.Name = "labelAcceptance";
			this.labelAcceptance.Size = new System.Drawing.Size(90, 42);
			this.labelAcceptance.TabIndex = 4;
			// 
			// labelMinDist
			// 
			this.labelMinDist.Location = new System.Drawing.Point(12, 373);
			this.labelMinDist.Name = "labelMinDist";
			this.labelMinDist.Size = new System.Drawing.Size(90, 28);
			this.labelMinDist.TabIndex = 4;
			// 
			// trackBarAcceptance
			// 
			this.trackBarAcceptance.Location = new System.Drawing.Point(3, 290);
			this.trackBarAcceptance.Maximum = 200;
			this.trackBarAcceptance.Name = "trackBarAcceptance";
			this.trackBarAcceptance.Size = new System.Drawing.Size(112, 56);
			this.trackBarAcceptance.TabIndex = 5;
			this.trackBarAcceptance.TickFrequency = 5;
			this.trackBarAcceptance.Scroll += new System.EventHandler(this.trackBarAcceptance_Scroll);
			// 
			// trackBarMinDist
			// 
			this.trackBarMinDist.Location = new System.Drawing.Point(3, 404);
			this.trackBarMinDist.Maximum = 200;
			this.trackBarMinDist.Name = "trackBarMinDist";
			this.trackBarMinDist.Size = new System.Drawing.Size(112, 56);
			this.trackBarMinDist.TabIndex = 5;
			this.trackBarMinDist.TickFrequency = 5;
			this.trackBarMinDist.Scroll += new System.EventHandler(this.trackBarMinDist_Scroll);
			// 
			// BeadSelectorDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(783, 522);
			this.Controls.Add(this.trackBarMinDist);
			this.Controls.Add(this.trackBarAcceptance);
			this.Controls.Add(this.labelMinDist);
			this.Controls.Add(this.labelAcceptance);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textBoxROI);
			this.Controls.Add(this.buttonAutofind);
			this.Controls.Add(this.buttonRemoveAll);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.pictureBox);
			this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.Name = "BeadSelectorDlg";
			this.Text = "BeadSelectorDlg";
			this.Load += new System.EventHandler(this.BeadSelectorDlg_Load);
			this.Resize += new System.EventHandler(this.BeadSelectorDlg_Resize);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBarAcceptance)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.trackBarMinDist)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.PictureBox pictureBox;
		private System.Windows.Forms.Button buttonOK;
		private System.Windows.Forms.Button buttonCancel;
		private System.Windows.Forms.Button buttonRemoveAll;
		private System.Windows.Forms.Button buttonAutofind;
		private System.Windows.Forms.TextBox textBoxROI;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label labelAcceptance;
		private System.Windows.Forms.Label labelMinDist;
		private System.Windows.Forms.TrackBar trackBarAcceptance;
		private System.Windows.Forms.TrackBar trackBarMinDist;
	}
}