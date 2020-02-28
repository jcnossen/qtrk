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
using System.IO;
using System.Diagnostics;
using System.Configuration;
using System.Xml.Serialization;

using TrackerDlgUtils;

namespace NanoBLOC
{
	public partial class OfflineTrackerDlg : Form
	{
        List<Int2> beadCenterPosList = new List<Int2>(); // corner-based
        Bitmap frameViewImage;
        FloatImg lut;
		Settings settings;
        int numFrames, numLUTSteps;
		Size imageSize;
		Form introDlg;
		string invalidImagesMsg;
		string[] imageFiles, lutFiles;

		const string SettingsXMLFile = "nanobloc-cfg.xml";

		class TraceData
		{
			public string expPath;

			public struct Frame
			{
				public Vector3[] xyz;
				public int index;
				public float timestamp;
			}
			public Frame[] frames;
			int nBeads;

			public TraceData(string expPath)
			{
				this.expPath = expPath;

				string traceFile = Tracker.GetTraceFile(expPath);

				float[][] mat = Util.ParseFloatMatrix(traceFile);
				nBeads = mat.Max(row => (row.Length - 2) / 3);
				frames = Array.ConvertAll(mat, row =>
				{
					Frame f = new Frame() { index = (int)row[0], timestamp = row[1], xyz = new Vector3[nBeads] };
					for (int k = 0; k < nBeads; k++) f.xyz[k] = new Vector3(row[k * 3 + 2], row[k * 3 + 3], row[k * 3 + 4]);
					return f;
				});
			}

			public Vector3[] GetBeadTrace(int beadIndex)
			{
				return Array.ConvertAll(frames, fr => fr.xyz[beadIndex]);
			}

			public int FrameCount { get { return frames.Length;  } }
			public int NumBeads { get { return nBeads; } }
		}
		TraceData traceData;

		string CurrentExpDir
		{
			get {
				if (checkedListExp.SelectedItem != null)
					return textExpBaseDir.Text + Path.DirectorySeparatorChar + checkedListExp.SelectedItem.ToString();
				else
					return "";
			}

		}

		public OfflineTrackerDlg(Form introDlg)
		{
			this.introDlg = introDlg;
			InitializeComponent();
		}

		void ConfigurationSettingChanged()
		{
			float.TryParse(textBoxZCorrection.Text, out settings.zCorrectionFactor);
			float.TryParse(textBoxLUTStep.Text, out settings.lutStep);
			float.TryParse(textBoxPixelSize.Text, out settings.pixelSize);
			int roi;
			if (int.TryParse(textBoxROI.Text, out roi))
			{
				settings.ROI = roi;
				UpdateFrameView();
			}
		}


		private void buttonSelectExpDir_Click(object sender, EventArgs e)
		{
			var fbd = new FolderBrowserDialog() { Description = "Select experiment folder with images", SelectedPath=textExpBaseDir.Text };
			if (fbd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				textExpBaseDir.Text = fbd.SelectedPath;
		}


		private void textExpDir_TextChanged(object sender, EventArgs e)
		{
			settings.expBaseDir = textExpBaseDir.Text;
			UpdateExpBaseDir();
		}

		private void buttonSelectBeads_Click(object sender, EventArgs e)
		{
			if (frameViewImage == null)
			{
				MessageBox.Show("First select image dataset");
				return;
			}
			using (var fimg = new FloatImg(frameViewImage, 0))
			{
				var dlg = new TrackerDlgUtils.BeadSelectorDlg(fimg, settings.ROI, beadCenterPosList.ToArray());

				dlg.AutoFindMinDist = settings.autoFindMinDist;
				dlg.AutoFindAcceptance = settings.autoFindAcceptance;

				if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
				{
                    beadCenterPosList = dlg.ROIPositions.ToList();
					settings.ROI = dlg.ROISize;
					settings.autoFindAcceptance = dlg.AutoFindAcceptance;
					settings.autoFindMinDist = dlg.AutoFindMinDist;
                    UpdateConfigurationUIComponents();
					CheckBeadPos();
					UpdateExpBaseDir();
				}
			}
		}

        public void UpdateExpBaseDir()
        {
			string cursel = null;
			if (checkedListExp.SelectedItem != null)
				cursel = checkedListExp.SelectedItem.ToString();

			// scan the experiment base path for tmp_* directories
			string[] expPaths = settings.GetExperimentDataPaths();
			checkedListExp.Items.Clear();
			checkedListExp.Items.AddRange(expPaths);

			if (cursel != null)
				checkedListExp.SelectedIndex = Array.FindIndex(expPaths, str => cursel == str);

			if (settings.selectedExpDirs != null)
			{
				foreach (string sel in settings.selectedExpDirs) {
					int i = Array.FindIndex(expPaths, str => sel == str);
					if (i>=0) checkedListExp.SetItemCheckState(i, CheckState.Checked);
				}
			}

			comboLutDir.Items.Clear();
			comboLutDir.Items.AddRange(expPaths);
			comboLutDir.SelectedItem = expPaths.FirstOrDefault(str => settings.lutSubdir == str);

			if (lut != null)
			{
				lut.Dispose();
				lut = null;
			}

			labelBeadPosFileLoc.Text = settings.BeadListXMLPath;
			UpdateFileLists();
        }

        void UpdateFileLists()
        {
            if (Directory.Exists(settings.LUTImagePath))
            {
				string[] files = null;

				try
				{
					files = Directory.GetFiles(settings.LUTImagePath).Where(Tracker.ValidImageExtension).ToArray();
				}
				catch (Exception) { }
				if (files != null)
				{
					lutFiles = files;
					numLUTSteps = lutFiles.Length;
					if (lutFiles.Length > 0)
					{
						try
						{
							using (var bmp = new Bitmap(lutFiles[0]))
								imageSize = bmp.Size;
						}
						catch (Exception) { }
					}
					else imageSize = new Size();
					Debug.WriteLine("{0} lutsteps found", numLUTSteps);
				}

			}

            if (Directory.Exists(CurrentExpDir))
            {
				Debug.WriteLine("Exploring " + CurrentExpDir + " for images");
				string[] files = null;
				try {
					files = Directory.GetFiles(CurrentExpDir);
				} catch (Exception) {}

				if (files != null)
				{
					imageFiles = files.Where(Tracker.ValidImageExtension).ToArray();

					if (imageFiles.Length > 0)
					{
						try
						{
							using (var bmp = new Bitmap(imageFiles[0]))
								imageSize = bmp.Size;
						}
						catch (Exception) { }
					}

					numFrames = imageFiles.Length;
					trackBarFrameView.Maximum = imageFiles.Length - 1;
					if (numFrames > 0)
					{
						if (trackBarFrameView.Value < 0) trackBarFrameView.Value = 0;
						if (trackBarFrameView.Value > numFrames) trackBarFrameView.Value = numFrames - 1;
					}
				}
			}
			UpdateFrameView();

			labelNumBeads.Text = string.Format("#Beads: {0}. Frame: {1}.  LUT Steps: {2}", beadCenterPosList.Count, numFrames, numLUTSteps);

			UpdateBeadIndex();
        }

		string ImageDataPath
		{
			get { return settings.expBaseDir + Path.DirectorySeparatorChar + checkedListExp.SelectedItem.ToString(); }
		}

        private void propertyGridQTrkSettings_Click(object sender, EventArgs e)
        {

        }

        private void trackBarFrameView_Scroll(object sender, EventArgs e)
        {
            UpdateFrameView();
        }

		void CheckBeadPos()
		{
			List<Int2> positions = Util.CheckBeadPos(settings.ROI, imageSize, beadCenterPosList);
			if (positions.Count != beadCenterPosList.Count)
			{
				var r = MessageBox.Show(string.Format("{0} beads will be removed because they are out of bounds",
					beadCenterPosList.Count - positions.Count), "Bead position error", MessageBoxButtons.OKCancel);
				if (r == System.Windows.Forms.DialogResult.Cancel)
					return;
			}
			beadCenterPosList = positions;
		}

        private void buttonBuildLUT_Click(object sender, EventArgs e)
        {
			if (beadCenterPosList.Count==0)
			{
				MessageBox.Show("No beads selected");
				return;
			}

			CheckBeadPos();

			var dlg = new ProgressBarDlg();
			dlg.Show(new Action(delegate
			{
				using (QTrkInstance inst = new QTrkInstance(settings.trackerConfig))
				{
					inst.SetRadialZLUTSize(beadCenterPosList.Count, numLUTSteps);
					inst.BeginLUT(true);

					int roi = settings.ROI;
					using (FloatImg tmp = new FloatImg(roi, roi * beadCenterPosList.Count))
					{
						for (int i = 0; i < numLUTSteps; i++)
						{
							string path = lutFiles[i];
							if (!File.Exists(path))
							{
								MessageBox.Show("File " + path + " missing");
								break;
							}
							dlg.Update(i / (float)numLUTSteps, path, string.Format("Processing LUT Frame {0}/{1}", i, numLUTSteps));
							using (Bitmap bmp = new Bitmap(path))
							{
								if (bmp.Size != imageSize)
								{
									MessageBox.Show(string.Format("{0} has invalid size {1},{2}", path, bmp.Width, bmp.Height));
									break;
								}
								using (var fi = new FloatImg(bmp, 0))
								{
									// build one image with all beads
									for (int b = 0; b < beadCenterPosList.Count; b++)
										fi.CopySubimage(tmp, beadCenterPosList[b].x-roi/2, beadCenterPosList[b].y-roi/2, 0, roi * b, roi, roi);
									inst.BuildLUT(tmp, i);
								}
							}
						}
					}
					inst.FinalizeLUT();
					lut = inst.GetRadialZLUT();
				}
			}));
			UpdateFileLists();
			tabControl.SelectedTab = tabPageTrack;
		}
		
        private void OfflineTrackerDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
			string path = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar  + SettingsXMLFile;
            settings.Save(path);

			Trace.WriteLine("Wrote settings to: " + path);
		}


        private void buttonLoadBeadlist_Click(object sender, EventArgs e)
        {
			if (settings.BeadListXMLPath.Length == 0)
				return;

			try
			{

				using (Stream stream = File.OpenRead(settings.BeadListXMLPath))
				{
					XmlSerializer s = new XmlSerializer(typeof(Int2[]));
					var result = (Int2[])s.Deserialize(stream);
					beadCenterPosList = result.ToList();
					UpdateFileLists();
				}
				Debug.WriteLine("Loaded beadlist from: " + settings.BeadListXMLPath);
			}
			catch (Exception)
			{
				MessageBox.Show("Error reading " + settings.BeadListXMLPath);
			}
        }

        private void buttonSaveBeadlist_Click(object sender, EventArgs e)
        {
			if (settings.BeadListXMLPath.Length == 0)
				return;

			XmlSerializer s = new XmlSerializer(typeof(Int2[]));
			using (Stream stream = File.Open(settings.BeadListXMLPath, FileMode.Create))
            {
                s.Serialize(stream, beadCenterPosList.ToArray());
            }

			Debug.WriteLine("Saved beadlist to: " + settings.BeadListXMLPath);
        }

		private void buttonTrackingSettings_Click(object sender, EventArgs e)
		{
			var dlg = new TrackerDlgUtils.TrackerSettingsDlg(settings.trackerConfig);
			dlg.UseCUDA = settings.libraryConfig.useCUDA;
			dlg.UseDebugLib = settings.libraryConfig.useDebug;
			if (dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				settings.trackerConfig = dlg.TrackerConfiguration;
				settings.libraryConfig.useDebug = dlg.UseDebugLib;
				settings.libraryConfig.useCUDA = dlg.UseCUDA;
			}
		}

		private void checkedListExp_SelectedIndexChanged(object sender, EventArgs e)
		{
			UpdateFileLists();
		}

		private void comboLutDir_SelectedIndexChanged(object sender, EventArgs e)
		{
			settings.lutSubdir = (string)comboLutDir.SelectedItem;
		}

		private void checkedListExp_ItemCheck(object sender, ItemCheckEventArgs e)
		{
			this.BeginInvoke(new Action(delegate
			{
				settings.selectedExpDirs = checkedListExp.CheckedItems.Cast<string>().ToArray();
			}));
		}

		private void OfflineTrackerDlg_Load(object sender, EventArgs e)
		{
			settings = Settings.Load(SettingsXMLFile);

			if (settings != null)
				QTrkInstance.SelectDLL(settings.libraryConfig.useDebug, settings.libraryConfig.useCUDA);

			if (settings == null)
			{
				QTrkInstance.SelectDLL(false, false);
				settings = new Settings();
				settings.trackerConfig = QTrkConfig.Default;
			}

			UpdateExpBaseDir();
			UpdateFileLists();

		    UpdateConfigurationUIComponents();

			EventHandler update = delegate { ConfigurationSettingChanged(); };
			textBoxPixelSize.TextChanged += update;
			textBoxLUTStep.TextChanged += update;
			textBoxZCorrection.TextChanged += update;
			textBoxROI.TextChanged += update;

//			introDlg.Close();
		}

        private void UpdateConfigurationUIComponents()
        {
            textExpBaseDir.Text = settings.expBaseDir;
            textBoxLUTStep.Text = settings.lutStep.ToString();
            textBoxZCorrection.Text = settings.zCorrectionFactor.ToString();
            textBoxPixelSize.Text = settings.pixelSize.ToString();
            textBoxROI.Text = settings.ROI.ToString();

        }

		/// <summary>
		/// update LUT
		/// update bead view
		/// update trace
		/// </summary>
		void UpdateBeadIndex()
		{
			int i = CurrentBeadIndex;

			if (i < 0)
				return;

			if (lut != null)
			{
				using (var img = lut.ExtractSubsection(numLUTSteps * i, numLUTSteps))
				{
					img.Normalize();
					pictureBoxLUT.Image = img.ToImage();
				}
			}

			if (imageFiles != null && imageFiles.Length > 0 && beadCenterPosList.Count > 0)
			{
				string path = imageFiles[0];
				if (File.Exists(path))
				{
					using (var bmp = new Bitmap(path))
					{
						int roi = settings.ROI;
						var dst = new Bitmap(roi,roi, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
						Graphics g = Graphics.FromImage(dst);
						var pos = beadCenterPosList[i];
						g.DrawImage(bmp, new Rectangle(0, 0, roi,roi), new Rectangle(pos.x-roi/2, pos.y-roi/2, settings.ROI, settings.ROI), GraphicsUnit.Pixel);
						pictureBoxBeadView.Image = dst;
					}
				}
			}
			labelBeadViewInfo.Text = string.Format("Bead {0} frame 0", i);

			UpdateTraceView();
		}

		int CurrentBeadIndex
		{
			get {
				int r = 0;
				if (int.TryParse(textBoxBeadIndex.Text, out r))
					return r;
				textBoxBeadIndex.Text = "0";
				return 0;
			}
			set {
				textBoxBeadIndex.Text = value.ToString();
			}
		}

		private void textBoxBeadIndex_TextChanged(object sender, EventArgs e)
		{
			var c = int.Parse(textBoxBeadIndex.Text);
			var i = c;
			if (i >= beadCenterPosList.Count) i = beadCenterPosList.Count - 1;
			if (i < 0) i = 0;
			if (c != i) textBoxBeadIndex.Text = i.ToString();
			UpdateBeadIndex();
		}

		private void buttonTrack_Click(object sender, EventArgs e)
		{
			if (backgroundWorker.IsBusy)
				return;

			if(lut == null)
			{
				MessageBox.Show("Build Lookup Table (LUT) first");
				return;
			}

			if (settings.selectedExpDirs == null || settings.selectedExpDirs.Length == 0)
			{
				MessageBox.Show("First select one or more experiments to process");
				return;
			}
			CheckBeadPos();

			tabControl.SelectedTab = tabPageTrack;
			backgroundWorker.RunWorkerAsync();
		}

		private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
		{
			var beadCornerPositions = beadCenterPosList.ConvertAll( (center) => new Int2(center.x - settings.ROI / 2, center.y - settings.ROI / 2));

			var tracker = new Tracker(new Tracker.Config()
			{
				beadCornerPos = beadCornerPositions.ToArray(),
				lut = lut,
				lutplanes = numLUTSteps,
				settings = settings
			});

			tracker.Run((Tracker.ProgressState s, string info) =>
			{
				Invoke(new Action(delegate
				{
					labelTrackingInfo.Text = info;
				}));
			});

			Invoke(new Action(delegate
			{
				traceData = null;
				UpdateTraceView();
			}));

		}

		private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
		{}

		private void generateTestImagesFromLUTToolStripMenuItem_Click(object sender, EventArgs e)
		{
			var ofd = new OpenFileDialog();
			if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
			{
				string basePath = settings.LUTImagePath;
				if (basePath.Length > 0)
					basePath += Path.DirectorySeparatorChar;

				using (var lut = new Bitmap(ofd.FileName))
				{
					var cfg = settings.trackerConfig;
					var cc = QTrkComputedConfig.FromConfig(cfg);

					FloatImg lutf = new FloatImg(lut, 0);
					FloatImg img = new FloatImg(cfg.width,cfg.height);
					for (int i = 0; i < lut.Height; i++) {
						QTrkUtil.GenerateImageFromLUT(img, lutf, cfg.ZLUT_minradius, cc.zlut_maxradius, new Vector3(cfg.width / 2, cfg.height / 2, i), false, 1);
						string fn = basePath + string.Format("lut{0:000}.png", i);
						Trace.WriteLine("Writing " + fn);
						using (var planeimg = img.ToImage())
							planeimg.Save(fn);
					}

					lutf.Dispose();
					img.Dispose();
				}
			}
		
		}

		private void hViewScrollBar_Scroll(object sender, ScrollEventArgs e)
		{
			UpdateTraceView();
		}

		private void txtNumFrames_TextChanged(object sender, EventArgs e)
		{
			UpdateTraceView();
		}

		private void buttonIncBeadIndex_Click(object sender, EventArgs e)
		{
			CurrentBeadIndex++;
		}

		private void buttonDecBeadIndex_Click(object sender, EventArgs e)
		{
			CurrentBeadIndex--;
		}

		private void OfflineTrackerDlg_KeyDown(object sender, KeyEventArgs e)
		{
			if (tabControl.SelectedTab == tabPageTrack)
			{
				if (e.KeyCode == Keys.Down)
				{
					CurrentBeadIndex--;
					e.Handled = true;
				}
				if (e.KeyCode == Keys.Up)
				{
					CurrentBeadIndex++;
					e.Handled = true;
				}
			}
		}

		void UpdateTraceView()
		{
			int bead = CurrentBeadIndex;
			// read frames in tracked data

			int start = hViewScrollBar.Value;
			int nfr = 0;
			int.TryParse(txtNumFrames.Text, out nfr);

			var xs = chartXY.Series[0];
			var ys = chartXY.Series[1];
			var zs = chartXY.Series[2];
			
			chartXY.SuspendLayout();

			var xpts = xs.Points;
			var ypts = ys.Points;
			var zpts = zs.Points;
			xpts.Clear();
			ypts.Clear();
			zpts.Clear();

			if (traceData == null)
			{
				var traceFile = Tracker.GetTraceFile(CurrentExpDir);
				if(File.Exists(traceFile))
					traceData = new TraceData(CurrentExpDir);
			}

			if (traceData != null && bead >= 0 && bead < traceData.NumBeads)
			{
				if (start + nfr >= traceData.FrameCount)
					start = traceData.FrameCount - nfr;
				if (start < 0) start = 0;
				if (start + nfr >= traceData.FrameCount)
					nfr = traceData.FrameCount - start;

				for (int x = 0; x < nfr; x++)
				{
					var pt = traceData.frames[x + start].xyz[bead];
					xpts.AddY(pt.x);
					ypts.AddY(pt.y);
					zpts.AddY(pt.z);
				}

				//			chartXY.ser

			}
			chartXY.ResumeLayout();
		}

		private void generateGraphDataToolStripMenuItem_Click(object sender, EventArgs e)
		{

		}

		private void buttonRefreshExpDirList_Click(object sender, EventArgs e)
		{
			UpdateExpBaseDir();
		}

		void UpdateFrameView()
		{
			if (imageFiles == null || imageFiles.Length==0)
			{ 
				invalidImagesMsg = "Select an experiment folder with image files in it";
				frameView.Image = null;
				if (frameViewImage != null)
					frameViewImage.Dispose();
				frameViewImage = null;
				return;
			}

			if (trackBarFrameView.Value < 0 || trackBarFrameView.Value >= imageFiles.Length)
				return;

			string path = imageFiles[trackBarFrameView.Value];
			if (File.Exists(path))
			{
				var bmp = new Bitmap(path);
				frameView.Image = bmp;
				frameView.Invalidate();
				if (frameViewImage != null) frameViewImage.Dispose();
				frameViewImage = bmp;
			}
			else frameViewImage = null;

			if (traceData != null && traceData.expPath != path)
			{
				traceData = null;
				UpdateTraceView();
			}
		}


		private void frameView_Paint(object sender, PaintEventArgs e)
		{
			if (frameViewImage == null)
			{
				if (invalidImagesMsg != null)
				{
					var sf = new StringFormat() { Alignment = StringAlignment.Center, LineAlignment = StringAlignment.Center };
					e.Graphics.DrawString(invalidImagesMsg, SystemFonts.DefaultFont, Brushes.Black, frameView.Size.Width / 2, frameView.Size.Height / 2, sf);
				}
				return;
			}

			int roi = settings.ROI;
			int w = frameViewImage.Width;
			int h = frameViewImage.Height;
			float sx = frameView.Width / (float)w;
			float sy = frameView.Height / (float)h;
			float roix = roi * sx, roiy = roi * sy;
			foreach (var pos in beadCenterPosList)
				e.Graphics.DrawRectangle(Pens.Blue, (pos.x - roi / 2) * sx, (pos.y - roi / 2) * sy, roix, roiy);
		}

		private void OfflineTrackerDlg_FormClosed(object sender, FormClosedEventArgs e)
		{
			Application.Exit();
		}

		private void OfflineTrackerDlg_Shown(object sender, EventArgs e)
		{
			if (introDlg != null)
			{
				var tmr = new Timer(components) { Interval = 400 };
				tmr.Tick += delegate {
					introDlg.Close();
					introDlg = null;
					tmr.Stop();
					components.Remove(tmr);
				};
				tmr.Start();
			}
		}

		private void buttonDiscardBead_Click(object sender, EventArgs e)
		{

		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
		{
			(new AboutDlg() { StartPosition = FormStartPosition.CenterParent }).ShowDialog();
		}

		private void pictureBoxLUT_Paint(object sender, PaintEventArgs e)
		{

		}

		private void pictureBoxBeadView_Paint(object sender, PaintEventArgs e)
		{

		}

		private void checkedListExp_MouseClick(object sender, MouseEventArgs e)
		{
		}

		private void checkedListExp_MouseDown(object sender, MouseEventArgs e)
		{
			if (e.Button == System.Windows.Forms.MouseButtons.Right)
			{
				if (checkedListExp.SelectedItem == null)
					return;

				string name =checkedListExp.SelectedItem.ToString();

				var menu = new ContextMenu(new MenuItem[] {
					new MenuItem("Open " + name, new EventHandler((o,e_) => {
						if (checkedListExp.SelectedItem == null)
							return;

						textExpBaseDir.Text =  textExpBaseDir.Text + "\\" + checkedListExp.SelectedItem.ToString();
						UpdateExpBaseDir();
					}))});

				menu.Show(checkedListExp, e.Location);
			}
		}


	}
}

