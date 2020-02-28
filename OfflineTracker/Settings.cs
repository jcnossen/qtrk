using QTrkDotNet;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using TrackerDlgUtils;

namespace NanoBLOC
{
	public class TrackerLibraryConfig
	{
		public bool useDebug, useCUDA;
	}

	public class Settings
	{
		public TrackerLibraryConfig libraryConfig;
		public string expBaseDir;
		public string lutSubdir;
//		public string[] expDirs;
		public string[] selectedExpDirs = new string[] { };
		public float zCorrectionFactor, lutStep, pixelSize;

		public float autoFindAcceptance=0.5f, autoFindMinDist=1.0f;

		public QTrkConfig trackerConfig;
		public int ROI
		{
			get { return trackerConfig.width; }
			set { trackerConfig.width = trackerConfig.height = value; }
		}

		public Settings()
		{
			libraryConfig = new TrackerLibraryConfig() { useCUDA = false, useDebug = false };
		}


		public void Save(string fn)
		{
			XmlSerializer xml = new XmlSerializer(typeof(Settings));
			using (var f = File.Open(fn,FileMode.Create))
				xml.Serialize(f, this);
		}

		public static Settings Load(string fn)
		{
			XmlSerializer xml = new XmlSerializer(typeof(Settings));

			try
			{
				using (var f = File.OpenRead(fn))
				{
					var settings = (Settings)xml.Deserialize(f);
					settings.expBaseDir = settings.expBaseDir ?? "";
					return settings;
				}
			}catch {
				return null;
			}
		}

		public string BeadListXMLPath
		{
			get { return expBaseDir + Path.DirectorySeparatorChar + "beadlist.xml"; }
		}

		public string[] GetExperimentDataPaths()
		{
			if(Directory.Exists(expBaseDir))
				return Directory.GetDirectories(expBaseDir).Select(str => Path.GetFileName(str)).ToArray();
			return new string[] { };
		}

		public string LUTImagePath { get { return expBaseDir + Path.DirectorySeparatorChar + lutSubdir; } }
	}
}
