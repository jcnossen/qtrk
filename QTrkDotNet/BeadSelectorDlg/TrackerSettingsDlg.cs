using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Diagnostics;
using QTrkDotNet;
using System.IO;

namespace TrackerDlgUtils
{

	public partial class TrackerSettingsDlg : Form
	{
		public QTrkConfig TrackerConfiguration
		{
			get { return (QTrkConfig) propertyGrid.SelectedObject; }
		}

		public bool UseDebugLib
		{
			get { return checkUseDebug.Checked; }
			set { checkUseDebug.Checked = value; }
		}

		public bool UseCUDA
		{
			get { return checkCUDA.Checked; }
			set { checkCUDA.Checked = value; }
		}

		public TrackerSettingsDlg(QTrkConfig config)
		{
			InitializeComponent();

			propertyGrid.SelectedObject = config;
		}

		private void buttonOK_Click(object sender, EventArgs e)
		{

		}

		private unsafe void buttonSpeedTest_Click(object sender, EventArgs e)
		{
			// get lut
			// rescale lut
//			var lut = new Bitmap("lut000.jpg");

			var dlg = new ProgressBarDlg();

			dlg.Show(new Action(delegate
			{
				Bitmap lut = TrackerDlgUtils.Properties.Resources.lut000;
				using (QTrkInstance inst = new QTrkInstance(TrackerConfiguration))
				{
					int total = Math.Max(100000, inst.GetMaxQueueLength() * 4);

                    Trace.WriteLine(string.Format("Max queue len: {1}, running benchmark with {0} images...", total, inst.GetMaxQueueLength()));

					dlg.Update(0.0f, "Generating lookup table...", "Benchmark in progress"); 

					GetTimestamp();
					// use LUT to generate images for new rescaled lut table
					var lutf = new FloatImg(lut, 0);
					using (FloatImg img = QTrkUtil.RescaleAndSetLUT(inst, lutf, lut.Height)) { }
					// make sample image

					var cfg = inst.Config;
					var sample = new FloatImg(cfg.config.width, cfg.config.height);
					QTrkUtil.GenerateImageFromLUT(sample, lutf, cfg.config.ZLUT_minradius, cfg.zlut_maxradius, new Vector3(cfg.config.width / 2, cfg.config.height / 2, lutf.h / 2), false, 1);

					dlg.Update(0.0f, "Running speed test...", "Benchmark in progress");

					// measure tracking speed
					double st = GetTimestamp();
					ImageData imgd = sample.ImageData;
					LocalizationJob job = new LocalizationJob();
					LocalizationJob* pj = &job;
					int lastUpdate = 0;

					for (uint i = 0; i < total; i++)
					{
						pj->frame=i;
						pj->zlutIndex=0;

						inst.ScheduleLocalization(ref imgd, pj);

						int rc = inst.GetResultCount();
						if (rc - lastUpdate > total / 20)
						{
							dlg.Update(rc / (float)total, "Running speed test...", "Benchmark in progress");
							lastUpdate = rc;
						}
					}
					double st0 = GetTimestamp();
					inst.Flush();

					// wait for results
					while (true)
					{
						System.Threading.Thread.Sleep(20);
						int rc = inst.GetResultCount();

						if (rc - lastUpdate > total / 20)
						{
							dlg.Update(rc / (float)total, "Running speed test...", "Benchmark in progress");
							lastUpdate = rc;
						}

						if (rc == total) break;
					}

					double end = GetTimestamp();
					double fps = total / (end - st);

					double scheduleTime = (st0 - st) * 1000;
					Trace.WriteLine(string.Format("Scheduling time: {0} ms. Per frame: {1} us. Scheduling FPS={2}. Processing FPS={3}", 
						(int)scheduleTime, (int)(scheduleTime / total * 1000), (int)( total / (st0-st) ) , (int)fps));

					Invoke(new Action(delegate { labelSpeedResults.Text = string.Format("{0} frames processed in {1} ms. \nFPS={2}", 
						total, (int)((end - st) * 1000), (int)fps); }));

					var results = new LocalizationResult[total];
					inst.GetResults(results);

					for (int i = 0; i < 10; i++)
					{
						Vector3 p = results[i].pos;
						Trace.WriteLine(string.Format("x={0}, y={1}, z={2}", p.x, p.y, p.z));
					}

					sample.Dispose();
				}
				lut.Dispose();
			}));
		}

		double GetTimestamp()
		{
			return Stopwatch.GetTimestamp() / (double)Stopwatch.Frequency;
		}

	}
}
