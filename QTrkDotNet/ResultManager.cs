using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace QTrkDotNet
{
	public class ResultManager : IDisposable
	{
		IntPtr inst;
		QTrkInstance tracker;
		ResultManagerConfig rmcfg;

		public ResultManager(string file, string frameInfoFile, ResultManagerConfig rmcfg, string[] colnames)
		{
			string scsColNames = colnames != null ? string.Join(";", colnames) : "";
			if (frameInfoFile == null) frameInfoFile = "";
			rmcfg.numFrameInfoColumns = colnames == null ? 0 : colnames.Length;
			inst = QTrkDLL.RMCreate(file, frameInfoFile, ref rmcfg, scsColNames);
			this.rmcfg = rmcfg;
		}

		public void SetTracker(QTrkInstance trkInst)
		{
			tracker = trkInst;
			QTrkDLL.RMSetTracker(inst, trkInst.InstancePtr);
		}

		public void StoreFrameInfo(int frame, double timestamp, float[] cols)
		{
			QTrkDLL.RMStoreFrameInfo(inst, frame, timestamp, cols);
		}

		public LocalizationResult[] GetBeadResults(int start, int numFrames, int bead)
		{
			LocalizationResult[] results=new LocalizationResult[numFrames];
			int count = QTrkDLL.RMGetBeadResults(inst, start, numFrames, bead, out results);
			Array.Resize(ref results, count);
			return results;
		}

		public RMFrameCounters GetFrameCounters()
		{
			RMFrameCounters dst;
			QTrkDLL.RMGetFrameCounters(inst, out dst);
			return dst;
		}

		public void Flush()
		{
			QTrkDLL.RMFlush(inst);
		}

		public LocalizationResult[] GetResults(int startFrame, int numFrames)
		{
			LocalizationResult[] results = new LocalizationResult[numFrames * rmcfg.numBeads];
			int count = QTrkDLL.RMGetResults(inst, startFrame, numFrames, out results);
			Array.Resize(ref results, count * rmcfg.numBeads);
			return results;
		}

		public void RemoveBead(int bead)
		{
			QTrkDLL.RMRemoveBead(inst, bead);
		}

		public ResultManagerConfig GetConfig()
		{
			 // no native call required as config stays same
			return rmcfg;
		}

		public void Dispose()
		{
			if (inst != IntPtr.Zero)
			{
				QTrkDLL.RMDestroy(inst);
				inst = IntPtr.Zero;
			}
		}
	}
}
