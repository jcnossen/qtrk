namespace BeadSelectorDlg
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
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// pictureBox
			// 
			this.pictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBox.Location = new System.Drawing.Point(109, 12);
			this.pictureBox.Name = "pictureBox";
			this.pictureBox.Size = new System.Drawing.Size(507, 412);
			this.pictureBox.TabIndex = 0;
			this.pictureBox.TabStop = false;
			// 
			// buttonOK
			// 
			this.buttonOK.Location = new System.Drawing.Point(12, 12);
			this.buttonOK.Name = "buttonOK";
			this.buttonOK.Size = new System.Drawing.Size(91, 28);
			this.buttonOK.TabIndex = 1;
			this.buttonOK.Text = "OK";
			this.buttonOK.UseVisualStyleBackColor = true;
			this.buttonOK.Click += new System.EventHandler(this.buttonOK_Click);
			// 
			// buttonCancel
			// 
			this.buttonCancel.Location = new System.Drawing.Point(12, 46);
			this.buttonCancel.Name = "buttonCancel";
			this.buttonCancel.Size = new System.Drawing.Size(91, 28);
			this.buttonCancel.TabIndex = 1;
			this.buttonCancel.Text = "Cancel";
			this.buttonCancel.UseVisualStyleBackColor = true;
			this.buttonCancel.Click += new System.EventHandler(this.buttonCancel_Click);
			// 
			// buttonRemoveAll
			// 
			this.buttonRemoveAll.Location = new System.Drawing.Point(12, 80);
			this.buttonRemoveAll.Name = "buttonRemoveAll";
			this.buttonRemoveAll.Size = new System.Drawing.Size(91, 28);
			this.buttonRemoveAll.TabIndex = 1;
			this.buttonRemoveAll.Text = "Remove all";
			this.buttonRemoveAll.UseVisualStyleBackColor = true;
			this.buttonRemoveAll.Click += new System.EventHandler(this.buttonRemoveAll_Click);
			// 
			// buttonAutofind
			// 
			this.buttonAutofind.Location = new System.Drawing.Point(12, 114);
			this.buttonAutofind.Name = "buttonAutofind";
			this.buttonAutofind.Size = new System.Drawing.Size(91, 28);
			this.buttonAutofind.TabIndex = 1;
			this.buttonAutofind.Text = "Autofind";
			this.buttonAutofind.UseVisualStyleBackColor = true;
			this.buttonAutofind.Click += new System.EventHandler(this.buttonAutofind_Click);
			// 
			// textBoxROI
			// 
			this.textBoxROI.Location = new System.Drawing.Point(12, 181);
			this.textBoxROI.Name = "textBoxROI";
			this.textBoxROI.Size = new System.Drawing.Size(91, 22);
			this.textBoxROI.TabIndex = 2;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(12, 161);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(36, 17);
			this.label1.TabIndex = 3;
			this.label1.Text = "ROI:";
			// 
			// BeadSelectorDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(628, 436);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.textBoxROI);
			this.Controls.Add(this.buttonAutofind);
			this.Controls.Add(this.buttonRemoveAll);
			this.Controls.Add(this.buttonCancel);
			this.Controls.Add(this.buttonOK);
			this.Controls.Add(this.pictureBox);
			this.Name = "BeadSelectorDlg";
			this.Text = "BeadSelectorDlg";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
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
	}
}