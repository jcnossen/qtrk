namespace NanoBLOC
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
			this.components = new System.ComponentModel.Container();
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
			System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
			System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.Windows.Forms.DataVisualization.Charting.Series series3 = new System.Windows.Forms.DataVisualization.Charting.Series();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OfflineTrackerDlg));
			this.tabControl = new System.Windows.Forms.TabControl();
			this.tabPageSetup = new System.Windows.Forms.TabPage();
			this.textBoxROI = new System.Windows.Forms.TextBox();
			this.label11 = new System.Windows.Forms.Label();
			this.labelBeadPosFileLoc = new System.Windows.Forms.Label();
			this.buttonTrackingSettings = new System.Windows.Forms.Button();
			this.buttonLoadBeadlist = new System.Windows.Forms.Button();
			this.buttonSaveBeadlist = new System.Windows.Forms.Button();
			this.trackBarFrameView = new System.Windows.Forms.TrackBar();
			this.frameView = new System.Windows.Forms.PictureBox();
			this.textBoxZCorrection = new System.Windows.Forms.TextBox();
			this.textBoxLUTStep = new System.Windows.Forms.TextBox();
			this.textBoxPixelSize = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.buttonSelectBeads = new System.Windows.Forms.Button();
			this.tabPageTrack = new System.Windows.Forms.TabPage();
			this.splitContainer = new System.Windows.Forms.SplitContainer();
			this.pictureBoxLUT = new System.Windows.Forms.PictureBox();
			this.labelBeadViewInfo = new System.Windows.Forms.Label();
			this.pictureBoxBeadView = new System.Windows.Forms.PictureBox();
			this.labelTrackingInfo = new System.Windows.Forms.Label();
			this.chartXY = new System.Windows.Forms.DataVisualization.Charting.Chart();
			this.hViewScrollBar = new System.Windows.Forms.HScrollBar();
			this.label9 = new System.Windows.Forms.Label();
			this.txtNumFrames = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.buttonDecBeadIndex = new System.Windows.Forms.Button();
			this.buttonIncBeadIndex = new System.Windows.Forms.Button();
			this.textBoxBeadIndex = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.labelTimeEstim = new System.Windows.Forms.Label();
			this.textExpBaseDir = new System.Windows.Forms.TextBox();
			this.labelNumBeads = new System.Windows.Forms.Label();
			this.buttonSelectExpDir = new System.Windows.Forms.Button();
			this.buttonBuildLUT = new System.Windows.Forms.Button();
			this.label6 = new System.Windows.Forms.Label();
			this.buttonTrack = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.comboLutDir = new System.Windows.Forms.ComboBox();
			this.checkedListExp = new System.Windows.Forms.CheckedListBox();
			this.timerUpdateTrackInfo = new System.Windows.Forms.Timer(this.components);
			this.backgroundWorker = new System.ComponentModel.BackgroundWorker();
			this.menuStrip1 = new System.Windows.Forms.MenuStrip();
			this.debugToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.generateTestImagesFromLUTToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.generateGraphDataToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.buttonRefreshExpDirList = new System.Windows.Forms.Button();
			this.tabControl.SuspendLayout();
			this.tabPageSetup.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackBarFrameView)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.frameView)).BeginInit();
			this.tabPageTrack.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).BeginInit();
			this.splitContainer.Panel1.SuspendLayout();
			this.splitContainer.Panel2.SuspendLayout();
			this.splitContainer.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxLUT)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxBeadView)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.chartXY)).BeginInit();
			this.menuStrip1.SuspendLayout();
			this.SuspendLayout();
			// 
			// tabControl
			// 
			this.tabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.tabControl.Controls.Add(this.tabPageSetup);
			this.tabControl.Controls.Add(this.tabPageTrack);
			this.tabControl.Location = new System.Drawing.Point(376, 30);
			this.tabControl.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.tabControl.Name = "tabControl";
			this.tabControl.SelectedIndex = 0;
			this.tabControl.Size = new System.Drawing.Size(963, 605);
			this.tabControl.TabIndex = 0;
			// 
			// tabPageSetup
			// 
			this.tabPageSetup.Controls.Add(this.textBoxROI);
			this.tabPageSetup.Controls.Add(this.label11);
			this.tabPageSetup.Controls.Add(this.labelBeadPosFileLoc);
			this.tabPageSetup.Controls.Add(this.buttonTrackingSettings);
			this.tabPageSetup.Controls.Add(this.buttonLoadBeadlist);
			this.tabPageSetup.Controls.Add(this.buttonSaveBeadlist);
			this.tabPageSetup.Controls.Add(this.trackBarFrameView);
			this.tabPageSetup.Controls.Add(this.frameView);
			this.tabPageSetup.Controls.Add(this.textBoxZCorrection);
			this.tabPageSetup.Controls.Add(this.textBoxLUTStep);
			this.tabPageSetup.Controls.Add(this.textBoxPixelSize);
			this.tabPageSetup.Controls.Add(this.label10);
			this.tabPageSetup.Controls.Add(this.label3);
			this.tabPageSetup.Controls.Add(this.label2);
			this.tabPageSetup.Controls.Add(this.label1);
			this.tabPageSetup.Controls.Add(this.buttonSelectBeads);
			this.tabPageSetup.Location = new System.Drawing.Point(4, 25);
			this.tabPageSetup.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.tabPageSetup.Name = "tabPageSetup";
			this.tabPageSetup.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.tabPageSetup.Size = new System.Drawing.Size(955, 576);
			this.tabPageSetup.TabIndex = 0;
			this.tabPageSetup.Text = "Configure";
			this.tabPageSetup.UseVisualStyleBackColor = true;
			// 
			// textBoxROI
			// 
			this.textBoxROI.Location = new System.Drawing.Point(460, 83);
			this.textBoxROI.Name = "textBoxROI";
			this.textBoxROI.Size = new System.Drawing.Size(69, 22);
			this.textBoxROI.TabIndex = 7;
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(299, 85);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(155, 17);
			this.label11.TabIndex = 13;
			this.label11.Text = "Region-of-interest size:";
			// 
			// labelBeadPosFileLoc
			// 
			this.labelBeadPosFileLoc.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.labelBeadPosFileLoc.Location = new System.Drawing.Point(181, 113);
			this.labelBeadPosFileLoc.Name = "labelBeadPosFileLoc";
			this.labelBeadPosFileLoc.Size = new System.Drawing.Size(767, 23);
			this.labelBeadPosFileLoc.TabIndex = 12;
			// 
			// buttonTrackingSettings
			// 
			this.buttonTrackingSettings.Location = new System.Drawing.Point(279, 14);
			this.buttonTrackingSettings.Margin = new System.Windows.Forms.Padding(4);
			this.buttonTrackingSettings.Name = "buttonTrackingSettings";
			this.buttonTrackingSettings.Size = new System.Drawing.Size(144, 28);
			this.buttonTrackingSettings.TabIndex = 11;
			this.buttonTrackingSettings.Text = "Tracking settings";
			this.buttonTrackingSettings.UseVisualStyleBackColor = true;
			this.buttonTrackingSettings.Click += new System.EventHandler(this.buttonTrackingSettings_Click);
			// 
			// buttonLoadBeadlist
			// 
			this.buttonLoadBeadlist.Location = new System.Drawing.Point(430, 48);
			this.buttonLoadBeadlist.Margin = new System.Windows.Forms.Padding(4);
			this.buttonLoadBeadlist.Name = "buttonLoadBeadlist";
			this.buttonLoadBeadlist.Size = new System.Drawing.Size(119, 28);
			this.buttonLoadBeadlist.TabIndex = 11;
			this.buttonLoadBeadlist.Text = "Load beadlist";
			this.buttonLoadBeadlist.UseVisualStyleBackColor = true;
			this.buttonLoadBeadlist.Click += new System.EventHandler(this.buttonLoadBeadlist_Click);
			// 
			// buttonSaveBeadlist
			// 
			this.buttonSaveBeadlist.Location = new System.Drawing.Point(431, 14);
			this.buttonSaveBeadlist.Margin = new System.Windows.Forms.Padding(4);
			this.buttonSaveBeadlist.Name = "buttonSaveBeadlist";
			this.buttonSaveBeadlist.Size = new System.Drawing.Size(118, 28);
			this.buttonSaveBeadlist.TabIndex = 11;
			this.buttonSaveBeadlist.Text = "Save beadlist";
			this.buttonSaveBeadlist.UseVisualStyleBackColor = true;
			this.buttonSaveBeadlist.Click += new System.EventHandler(this.buttonSaveBeadlist_Click);
			// 
			// trackBarFrameView
			// 
			this.trackBarFrameView.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.trackBarFrameView.Location = new System.Drawing.Point(20, 491);
			this.trackBarFrameView.Margin = new System.Windows.Forms.Padding(4);
			this.trackBarFrameView.Name = "trackBarFrameView";
			this.trackBarFrameView.Size = new System.Drawing.Size(928, 56);
			this.trackBarFrameView.TabIndex = 10;
			this.trackBarFrameView.Scroll += new System.EventHandler(this.trackBarFrameView_Scroll);
			// 
			// frameView
			// 
			this.frameView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.frameView.Location = new System.Drawing.Point(20, 140);
			this.frameView.Margin = new System.Windows.Forms.Padding(4);
			this.frameView.Name = "frameView";
			this.frameView.Size = new System.Drawing.Size(928, 343);
			this.frameView.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.frameView.TabIndex = 9;
			this.frameView.TabStop = false;
			this.frameView.Paint += new System.Windows.Forms.PaintEventHandler(this.frameView_Paint);
			// 
			// textBoxZCorrection
			// 
			this.textBoxZCorrection.Location = new System.Drawing.Point(184, 81);
			this.textBoxZCorrection.Margin = new System.Windows.Forms.Padding(4);
			this.textBoxZCorrection.Name = "textBoxZCorrection";
			this.textBoxZCorrection.Size = new System.Drawing.Size(87, 22);
			this.textBoxZCorrection.TabIndex = 6;
			this.textBoxZCorrection.Text = "0.88";
			// 
			// textBoxLUTStep
			// 
			this.textBoxLUTStep.Location = new System.Drawing.Point(184, 50);
			this.textBoxLUTStep.Margin = new System.Windows.Forms.Padding(4);
			this.textBoxLUTStep.Name = "textBoxLUTStep";
			this.textBoxLUTStep.Size = new System.Drawing.Size(87, 22);
			this.textBoxLUTStep.TabIndex = 5;
			// 
			// textBoxPixelSize
			// 
			this.textBoxPixelSize.Location = new System.Drawing.Point(184, 17);
			this.textBoxPixelSize.Margin = new System.Windows.Forms.Padding(4);
			this.textBoxPixelSize.Name = "textBoxPixelSize";
			this.textBoxPixelSize.Size = new System.Drawing.Size(87, 22);
			this.textBoxPixelSize.TabIndex = 4;
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(18, 113);
			this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(119, 17);
			this.label10.TabIndex = 7;
			this.label10.Text = "Bead list location:";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(17, 85);
			this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(130, 17);
			this.label3.TabIndex = 7;
			this.label3.Text = "Z Correction factor:";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(18, 55);
			this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(158, 17);
			this.label2.TabIndex = 7;
			this.label2.Text = "LUT Step distance (nm)";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(17, 23);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(107, 17);
			this.label1.TabIndex = 7;
			this.label1.Text = "Pixel size:  (nm)";
			// 
			// buttonSelectBeads
			// 
			this.buttonSelectBeads.Location = new System.Drawing.Point(279, 48);
			this.buttonSelectBeads.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.buttonSelectBeads.Name = "buttonSelectBeads";
			this.buttonSelectBeads.Size = new System.Drawing.Size(144, 28);
			this.buttonSelectBeads.TabIndex = 10;
			this.buttonSelectBeads.Text = "Select beads";
			this.buttonSelectBeads.UseVisualStyleBackColor = true;
			this.buttonSelectBeads.Click += new System.EventHandler(this.buttonSelectBeads_Click);
			// 
			// tabPageTrack
			// 
			this.tabPageTrack.Controls.Add(this.splitContainer);
			this.tabPageTrack.Controls.Add(this.labelTimeEstim);
			this.tabPageTrack.Location = new System.Drawing.Point(4, 25);
			this.tabPageTrack.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.tabPageTrack.Name = "tabPageTrack";
			this.tabPageTrack.Padding = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.tabPageTrack.Size = new System.Drawing.Size(955, 576);
			this.tabPageTrack.TabIndex = 1;
			this.tabPageTrack.Text = "Tracking results";
			this.tabPageTrack.UseVisualStyleBackColor = true;
			// 
			// splitContainer
			// 
			this.splitContainer.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.splitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
			this.splitContainer.Location = new System.Drawing.Point(3, 2);
			this.splitContainer.Name = "splitContainer";
			// 
			// splitContainer.Panel1
			// 
			this.splitContainer.Panel1.Controls.Add(this.pictureBoxLUT);
			this.splitContainer.Panel1.Controls.Add(this.labelBeadViewInfo);
			this.splitContainer.Panel1.Controls.Add(this.pictureBoxBeadView);
			this.splitContainer.Panel1.Controls.Add(this.labelTrackingInfo);
			this.splitContainer.Panel1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			// 
			// splitContainer.Panel2
			// 
			this.splitContainer.Panel2.Controls.Add(this.chartXY);
			this.splitContainer.Panel2.Controls.Add(this.hViewScrollBar);
			this.splitContainer.Panel2.Controls.Add(this.label9);
			this.splitContainer.Panel2.Controls.Add(this.txtNumFrames);
			this.splitContainer.Panel2.Controls.Add(this.label8);
			this.splitContainer.Panel2.Controls.Add(this.label4);
			this.splitContainer.Panel2.Controls.Add(this.buttonDecBeadIndex);
			this.splitContainer.Panel2.Controls.Add(this.buttonIncBeadIndex);
			this.splitContainer.Panel2.Controls.Add(this.textBoxBeadIndex);
			this.splitContainer.Panel2.Controls.Add(this.label7);
			this.splitContainer.Panel2.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.splitContainer.Size = new System.Drawing.Size(949, 572);
			this.splitContainer.SplitterDistance = 315;
			this.splitContainer.TabIndex = 7;
			// 
			// pictureBoxLUT
			// 
			this.pictureBoxLUT.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureBoxLUT.Location = new System.Drawing.Point(17, 332);
			this.pictureBoxLUT.Name = "pictureBoxLUT";
			this.pictureBoxLUT.Size = new System.Drawing.Size(269, 219);
			this.pictureBoxLUT.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBoxLUT.TabIndex = 9;
			this.pictureBoxLUT.TabStop = false;
			this.pictureBoxLUT.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxLUT_Paint);
			// 
			// labelBeadViewInfo
			// 
			this.labelBeadViewInfo.AutoSize = true;
			this.labelBeadViewInfo.Location = new System.Drawing.Point(14, 120);
			this.labelBeadViewInfo.Name = "labelBeadViewInfo";
			this.labelBeadViewInfo.Size = new System.Drawing.Size(106, 17);
			this.labelBeadViewInfo.TabIndex = 8;
			this.labelBeadViewInfo.Text = "Bead X frame 0";
			// 
			// pictureBoxBeadView
			// 
			this.pictureBoxBeadView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureBoxBeadView.Location = new System.Drawing.Point(17, 139);
			this.pictureBoxBeadView.Name = "pictureBoxBeadView";
			this.pictureBoxBeadView.Size = new System.Drawing.Size(271, 187);
			this.pictureBoxBeadView.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBoxBeadView.TabIndex = 7;
			this.pictureBoxBeadView.TabStop = false;
			this.pictureBoxBeadView.Paint += new System.Windows.Forms.PaintEventHandler(this.pictureBoxBeadView_Paint);
			// 
			// labelTrackingInfo
			// 
			this.labelTrackingInfo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.labelTrackingInfo.Location = new System.Drawing.Point(14, 7);
			this.labelTrackingInfo.Name = "labelTrackingInfo";
			this.labelTrackingInfo.Size = new System.Drawing.Size(296, 87);
			this.labelTrackingInfo.TabIndex = 3;
			this.labelTrackingInfo.Text = "Estimated time left:";
			// 
			// chartXY
			// 
			this.chartXY.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.chartXY.BackColor = System.Drawing.Color.LightGray;
			chartArea1.AxisX.LineWidth = 2;
			chartArea1.AxisY.LineWidth = 2;
			chartArea1.Name = "ChartArea1";
			chartArea2.AlignWithChartArea = "ChartArea1";
			chartArea2.AxisX.LineWidth = 2;
			chartArea2.AxisY.LineWidth = 2;
			chartArea2.Name = "ChartArea2";
			this.chartXY.ChartAreas.Add(chartArea1);
			this.chartXY.ChartAreas.Add(chartArea2);
			legend1.Name = "Legend1";
			this.chartXY.Legends.Add(legend1);
			this.chartXY.Location = new System.Drawing.Point(6, 139);
			this.chartXY.Name = "chartXY";
			series1.BorderWidth = 2;
			series1.ChartArea = "ChartArea1";
			series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
			series1.IsXValueIndexed = true;
			series1.Legend = "Legend1";
			series1.Name = "X";
			series2.BorderWidth = 2;
			series2.ChartArea = "ChartArea1";
			series2.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
			series2.Color = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
			series2.IsXValueIndexed = true;
			series2.Legend = "Legend1";
			series2.Name = "Y";
			series3.ChartArea = "ChartArea2";
			series3.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.Line;
			series3.IsXValueIndexed = true;
			series3.Legend = "Legend1";
			series3.Name = "Z";
			this.chartXY.Series.Add(series1);
			this.chartXY.Series.Add(series2);
			this.chartXY.Series.Add(series3);
			this.chartXY.Size = new System.Drawing.Size(617, 425);
			this.chartXY.TabIndex = 2;
			this.chartXY.Text = "chart1";
			// 
			// hViewScrollBar
			// 
			this.hViewScrollBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.hViewScrollBar.Location = new System.Drawing.Point(31, 90);
			this.hViewScrollBar.Name = "hViewScrollBar";
			this.hViewScrollBar.Size = new System.Drawing.Size(580, 21);
			this.hViewScrollBar.TabIndex = 10;
			this.hViewScrollBar.Scroll += new System.Windows.Forms.ScrollEventHandler(this.hViewScrollBar_Scroll);
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(363, 27);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(105, 17);
			this.label9.TabIndex = 9;
			this.label9.Text = "Frames in view:";
			// 
			// txtNumFrames
			// 
			this.txtNumFrames.Location = new System.Drawing.Point(474, 22);
			this.txtNumFrames.Name = "txtNumFrames";
			this.txtNumFrames.Size = new System.Drawing.Size(100, 22);
			this.txtNumFrames.TabIndex = 8;
			this.txtNumFrames.Text = "500";
			this.txtNumFrames.TextChanged += new System.EventHandler(this.txtNumFrames_TextChanged);
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(127, 46);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(79, 17);
			this.label8.TabIndex = 7;
			this.label8.Text = "(Down key)";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(127, 17);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(62, 17);
			this.label4.TabIndex = 7;
			this.label4.Text = "(Up key)";
			// 
			// buttonDecBeadIndex
			// 
			this.buttonDecBeadIndex.Location = new System.Drawing.Point(91, 43);
			this.buttonDecBeadIndex.Name = "buttonDecBeadIndex";
			this.buttonDecBeadIndex.Size = new System.Drawing.Size(30, 23);
			this.buttonDecBeadIndex.TabIndex = 6;
			this.buttonDecBeadIndex.Text = "-";
			this.buttonDecBeadIndex.UseVisualStyleBackColor = true;
			this.buttonDecBeadIndex.Click += new System.EventHandler(this.buttonDecBeadIndex_Click);
			// 
			// buttonIncBeadIndex
			// 
			this.buttonIncBeadIndex.Location = new System.Drawing.Point(91, 14);
			this.buttonIncBeadIndex.Name = "buttonIncBeadIndex";
			this.buttonIncBeadIndex.Size = new System.Drawing.Size(30, 23);
			this.buttonIncBeadIndex.TabIndex = 6;
			this.buttonIncBeadIndex.Text = "+";
			this.buttonIncBeadIndex.UseVisualStyleBackColor = true;
			this.buttonIncBeadIndex.Click += new System.EventHandler(this.buttonIncBeadIndex_Click);
			// 
			// textBoxBeadIndex
			// 
			this.textBoxBeadIndex.Location = new System.Drawing.Point(6, 31);
			this.textBoxBeadIndex.Name = "textBoxBeadIndex";
			this.textBoxBeadIndex.Size = new System.Drawing.Size(63, 22);
			this.textBoxBeadIndex.TabIndex = 5;
			this.textBoxBeadIndex.TextChanged += new System.EventHandler(this.textBoxBeadIndex_TextChanged);
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(3, 7);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(82, 17);
			this.label7.TabIndex = 4;
			this.label7.Text = "Bead index:";
			// 
			// labelTimeEstim
			// 
			this.labelTimeEstim.AutoSize = true;
			this.labelTimeEstim.Location = new System.Drawing.Point(746, 68);
			this.labelTimeEstim.Name = "labelTimeEstim";
			this.labelTimeEstim.Size = new System.Drawing.Size(0, 17);
			this.labelTimeEstim.TabIndex = 4;
			// 
			// textExpBaseDir
			// 
			this.textExpBaseDir.Location = new System.Drawing.Point(12, 31);
			this.textExpBaseDir.Name = "textExpBaseDir";
			this.textExpBaseDir.Size = new System.Drawing.Size(358, 22);
			this.textExpBaseDir.TabIndex = 1;
			this.textExpBaseDir.TextChanged += new System.EventHandler(this.textExpDir_TextChanged);
			// 
			// labelNumBeads
			// 
			this.labelNumBeads.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.labelNumBeads.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.labelNumBeads.Location = new System.Drawing.Point(111, 447);
			this.labelNumBeads.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.labelNumBeads.Name = "labelNumBeads";
			this.labelNumBeads.Size = new System.Drawing.Size(263, 23);
			this.labelNumBeads.TabIndex = 6;
			// 
			// buttonSelectExpDir
			// 
			this.buttonSelectExpDir.Location = new System.Drawing.Point(12, 55);
			this.buttonSelectExpDir.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.buttonSelectExpDir.Name = "buttonSelectExpDir";
			this.buttonSelectExpDir.Size = new System.Drawing.Size(159, 39);
			this.buttonSelectExpDir.TabIndex = 2;
			this.buttonSelectExpDir.Text = "Select experiment dir:";
			this.buttonSelectExpDir.UseVisualStyleBackColor = true;
			this.buttonSelectExpDir.Click += new System.EventHandler(this.buttonSelectExpDir_Click);
			// 
			// buttonBuildLUT
			// 
			this.buttonBuildLUT.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonBuildLUT.Location = new System.Drawing.Point(83, 472);
			this.buttonBuildLUT.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.buttonBuildLUT.Name = "buttonBuildLUT";
			this.buttonBuildLUT.Size = new System.Drawing.Size(179, 35);
			this.buttonBuildLUT.TabIndex = 0;
			this.buttonBuildLUT.Text = "Build Z Lookup Table";
			this.buttonBuildLUT.UseVisualStyleBackColor = true;
			this.buttonBuildLUT.Click += new System.EventHandler(this.buttonBuildLUT_Click);
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(16, 178);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(89, 17);
			this.label6.TabIndex = 14;
			this.label6.Text = "Experiments:";
			// 
			// buttonTrack
			// 
			this.buttonTrack.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.buttonTrack.Location = new System.Drawing.Point(268, 472);
			this.buttonTrack.Name = "buttonTrack";
			this.buttonTrack.Size = new System.Drawing.Size(106, 35);
			this.buttonTrack.TabIndex = 6;
			this.buttonTrack.Text = "Track";
			this.buttonTrack.UseVisualStyleBackColor = true;
			this.buttonTrack.Click += new System.EventHandler(this.buttonTrack_Click);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(16, 151);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(39, 17);
			this.label5.TabIndex = 14;
			this.label5.Text = "LUT:";
			// 
			// comboLutDir
			// 
			this.comboLutDir.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
			this.comboLutDir.FormattingEnabled = true;
			this.comboLutDir.Location = new System.Drawing.Point(111, 148);
			this.comboLutDir.Name = "comboLutDir";
			this.comboLutDir.Size = new System.Drawing.Size(259, 24);
			this.comboLutDir.TabIndex = 13;
			this.comboLutDir.SelectedIndexChanged += new System.EventHandler(this.comboLutDir_SelectedIndexChanged);
			// 
			// checkedListExp
			// 
			this.checkedListExp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
			this.checkedListExp.FormattingEnabled = true;
			this.checkedListExp.Location = new System.Drawing.Point(111, 178);
			this.checkedListExp.Name = "checkedListExp";
			this.checkedListExp.Size = new System.Drawing.Size(259, 259);
			this.checkedListExp.TabIndex = 12;
			this.checkedListExp.ItemCheck += new System.Windows.Forms.ItemCheckEventHandler(this.checkedListExp_ItemCheck);
			this.checkedListExp.MouseClick += new System.Windows.Forms.MouseEventHandler(this.checkedListExp_MouseClick);
			this.checkedListExp.SelectedIndexChanged += new System.EventHandler(this.checkedListExp_SelectedIndexChanged);
			this.checkedListExp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.checkedListExp_MouseDown);
			// 
			// backgroundWorker
			// 
			this.backgroundWorker.WorkerReportsProgress = true;
			this.backgroundWorker.WorkerSupportsCancellation = true;
			this.backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.backgroundWorker_DoWork);
			this.backgroundWorker.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.backgroundWorker_ProgressChanged);
			// 
			// menuStrip1
			// 
			this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.debugToolStripMenuItem,
            this.aboutToolStripMenuItem});
			this.menuStrip1.Location = new System.Drawing.Point(0, 0);
			this.menuStrip1.Name = "menuStrip1";
			this.menuStrip1.Size = new System.Drawing.Size(1339, 28);
			this.menuStrip1.TabIndex = 1;
			this.menuStrip1.Text = "menuStrip1";
			// 
			// debugToolStripMenuItem
			// 
			this.debugToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.generateTestImagesFromLUTToolStripMenuItem,
            this.generateGraphDataToolStripMenuItem});
			this.debugToolStripMenuItem.Name = "debugToolStripMenuItem";
			this.debugToolStripMenuItem.Size = new System.Drawing.Size(66, 24);
			this.debugToolStripMenuItem.Text = "Debug";
			this.debugToolStripMenuItem.Visible = false;
			// 
			// generateTestImagesFromLUTToolStripMenuItem
			// 
			this.generateTestImagesFromLUTToolStripMenuItem.Name = "generateTestImagesFromLUTToolStripMenuItem";
			this.generateTestImagesFromLUTToolStripMenuItem.Size = new System.Drawing.Size(283, 24);
			this.generateTestImagesFromLUTToolStripMenuItem.Text = "Generate test images from LUT";
			this.generateTestImagesFromLUTToolStripMenuItem.Click += new System.EventHandler(this.generateTestImagesFromLUTToolStripMenuItem_Click);
			// 
			// generateGraphDataToolStripMenuItem
			// 
			this.generateGraphDataToolStripMenuItem.Name = "generateGraphDataToolStripMenuItem";
			this.generateGraphDataToolStripMenuItem.Size = new System.Drawing.Size(283, 24);
			this.generateGraphDataToolStripMenuItem.Text = "Generate graph data";
			this.generateGraphDataToolStripMenuItem.Click += new System.EventHandler(this.generateGraphDataToolStripMenuItem_Click);
			// 
			// aboutToolStripMenuItem
			// 
			this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
			this.aboutToolStripMenuItem.Size = new System.Drawing.Size(62, 24);
			this.aboutToolStripMenuItem.Text = "About";
			this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
			// 
			// buttonRefreshExpDirList
			// 
			this.buttonRefreshExpDirList.Location = new System.Drawing.Point(177, 55);
			this.buttonRefreshExpDirList.Name = "buttonRefreshExpDirList";
			this.buttonRefreshExpDirList.Size = new System.Drawing.Size(114, 40);
			this.buttonRefreshExpDirList.TabIndex = 15;
			this.buttonRefreshExpDirList.Text = "Refresh list";
			this.buttonRefreshExpDirList.UseVisualStyleBackColor = true;
			this.buttonRefreshExpDirList.Click += new System.EventHandler(this.buttonRefreshExpDirList_Click);
			// 
			// OfflineTrackerDlg
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1339, 640);
			this.Controls.Add(this.textExpBaseDir);
			this.Controls.Add(this.buttonRefreshExpDirList);
			this.Controls.Add(this.tabControl);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.menuStrip1);
			this.Controls.Add(this.buttonTrack);
			this.Controls.Add(this.label5);
			this.Controls.Add(this.comboLutDir);
			this.Controls.Add(this.checkedListExp);
			this.Controls.Add(this.buttonBuildLUT);
			this.Controls.Add(this.labelNumBeads);
			this.Controls.Add(this.buttonSelectExpDir);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.KeyPreview = true;
			this.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
			this.Name = "OfflineTrackerDlg";
			this.Text = "NanoBLoc - Nanometer Accurate Bead Localizer";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.OfflineTrackerDlg_FormClosing);
			this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.OfflineTrackerDlg_FormClosed);
			this.Load += new System.EventHandler(this.OfflineTrackerDlg_Load);
			this.Shown += new System.EventHandler(this.OfflineTrackerDlg_Shown);
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.OfflineTrackerDlg_KeyDown);
			this.tabControl.ResumeLayout(false);
			this.tabPageSetup.ResumeLayout(false);
			this.tabPageSetup.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.trackBarFrameView)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.frameView)).EndInit();
			this.tabPageTrack.ResumeLayout(false);
			this.tabPageTrack.PerformLayout();
			this.splitContainer.Panel1.ResumeLayout(false);
			this.splitContainer.Panel1.PerformLayout();
			this.splitContainer.Panel2.ResumeLayout(false);
			this.splitContainer.Panel2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.splitContainer)).EndInit();
			this.splitContainer.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxLUT)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBoxBeadView)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.chartXY)).EndInit();
			this.menuStrip1.ResumeLayout(false);
			this.menuStrip1.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TabControl tabControl;
		private System.Windows.Forms.TabPage tabPageSetup;
		private System.Windows.Forms.TabPage tabPageTrack;
		private System.Windows.Forms.Button buttonSelectExpDir;
		private System.Windows.Forms.Button buttonBuildLUT;
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
		private System.Windows.Forms.Label labelTimeEstim;
		private System.Windows.Forms.Label labelTrackingInfo;
		private System.Windows.Forms.DataVisualization.Charting.Chart chartXY;
		private System.Windows.Forms.Button buttonTrackingSettings;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.ComboBox comboLutDir;
		private System.Windows.Forms.CheckedListBox checkedListExp;
		private System.Windows.Forms.TextBox textExpBaseDir;
		private System.Windows.Forms.SplitContainer splitContainer;
		private System.Windows.Forms.Button buttonTrack;
		private System.Windows.Forms.Label labelBeadViewInfo;
		private System.Windows.Forms.PictureBox pictureBoxBeadView;
		private System.Windows.Forms.PictureBox pictureBoxLUT;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox textBoxBeadIndex;
		private System.Windows.Forms.Timer timerUpdateTrackInfo;
		private System.ComponentModel.BackgroundWorker backgroundWorker;
		private System.Windows.Forms.MenuStrip menuStrip1;
		private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem debugToolStripMenuItem;
		private System.Windows.Forms.ToolStripMenuItem generateTestImagesFromLUTToolStripMenuItem;
		private System.Windows.Forms.HScrollBar hViewScrollBar;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TextBox txtNumFrames;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Button buttonDecBeadIndex;
		private System.Windows.Forms.Button buttonIncBeadIndex;
		private System.Windows.Forms.ToolStripMenuItem generateGraphDataToolStripMenuItem;
		private System.Windows.Forms.Button buttonRefreshExpDirList;
		private System.Windows.Forms.Label labelBeadPosFileLoc;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox textBoxROI;
		private System.Windows.Forms.Label label11;
	}
}

