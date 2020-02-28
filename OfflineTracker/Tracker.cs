using QTrkDotNet;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NanoBLOC
{
	class Tracker
	{
		public static string[] ExtensionList = new string[] { ".jpg",".jpeg", ".png", ".tif", ".tiff" };

        public static bool ValidImageExtension(string str)
        {
            string ext = Path.GetExtension(str);
            if (ext == null) return false;
            return ExtensionList.Contains(ext.ToLower());
        }
        public static string[] GetImageFiles(string path)
        {
            return Directory.GetFiles(path).Where(ValidImageExtension).ToArray();
        }

		public class Config
		{
			public Settings settings;
			public FloatImg lut;
			public int lutplanes;
			public Int2[] beadCornerPos;
		}

		int totalFiles;

		class Experiment
		{
			public string path;
			public string[] imageFiles;
			public string outputTraceFile, outputFrameInfoFile;
		}
		List<Experiment> experiments = new List<Experiment>();
		Config config;

		public Tracker(Config config)
		{
			this.config = config;
			foreach (string dir in config.settings.selectedExpDirs)
			{
				string path= config.settings.expBaseDir + Path.DirectorySeparatorChar + dir;
				var exp = new Experiment()
				{
					path = dir,
					imageFiles = GetImageFiles(path),
					outputTraceFile = GetTraceFile(path),
					outputFrameInfoFile = GetFrameInfoFile(path)
				};

				totalFiles += exp.imageFiles.Length;
				experiments.Add(exp);
			}
		}

		public struct ProgressState
		{
			public float done; //fraction
			public string currentExp;
		}

		public void Run(Action<ProgressState, string> progressReporter)
		{
			ProgressState ps = new ProgressState();
			int filesDone = 0;

			Debug.WriteLine("Tracking settings:");
			Util.ForEachObjectField(config.settings.trackerConfig, 
				(name, value) => Debug.WriteLine("\t{0}={1}", name, value));

			Vector3 scale = new Vector3(config.settings.pixelSize / 1000.0f, config.settings.pixelSize / 1000.0f, 
				config.settings.zCorrectionFactor * config.settings.lutStep / 1000.0f);

			using (QTrkInstance trk = new QTrkInstance(config.settings.trackerConfig))
			{
				trk.SetRadialZLUT(config.lut, config.beadCornerPos.Length, config.lutplanes);

				for (int expIndex = 0; expIndex < experiments.Count; expIndex++)
				{
					var exp = experiments[expIndex];
					using (ResultManager rm = new ResultManager(exp.outputTraceFile, null, new ResultManagerConfig()
					{
						binaryOutput = 0,
						maxFramesInMemory = 100000,
						numBeads = config.beadCornerPos.Length,
						numFrameInfoColumns = 0,
						offset = new Vector3(),
						scaling = scale,
						writeInterval = 50
					}, null))
					{
						rm.SetTracker(trk);

						trk.SetLocalizationMode(LocalizeModeEnum.LocalizeZ | LocalizeModeEnum.QI);

						ps.currentExp = exp.path;
						for (int fr = 0; fr < exp.imageFiles.Length; fr++)
						{
							if (fr % 50 == 0)
							{
								ps.done = filesDone / (float)totalFiles;
								progressReporter(ps, string.Format("Processing experiment {0} ({1}/{2}).\nFrame {3}/{4}\n",
									exp.path, expIndex + 1, experiments.Count, fr + 1, exp.imageFiles.Length));
							}
							using (var bmp = new Bitmap(exp.imageFiles[fr]))
							{
								LocalizationJob[] jobs = Util.Sequence(config.beadCornerPos.Length, (i) => new LocalizationJob() { zlutIndex = i, frame = (uint)fr });
								trk.ScheduleFrameBitmap(bmp, config.beadCornerPos, jobs);
								//	rm.StoreFrameInfo(fr, fr, new float[] { 0 });
							}

							filesDone++;
						}
						trk.Flush();
						while (!trk.IsIdle())
							Thread.Sleep(10);
						rm.Flush();
					}
					Debug.WriteLine(string.Format("Experiment {0} done", exp.path));
					progressReporter(ps, "All done! ");
				}
			}
		}


		public static string GetTraceFile(string expPath)
		{
			return expPath + "-xyz.txt";
		}

		public static string GetFrameInfoFile(string expPath)
		{
			return expPath + "-frameinfo.txt";
		}

    }
}
