using QTrkDotNet;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NanoBLOC
{
	class Util
	{
		public static List<Int2> CheckBeadPos(int roisize, Size imageSize, List<Int2> beadCornerPosList)
		{
			List<Int2> result = new List<Int2>();
			foreach (Int2 pos in beadCornerPosList)
				if (pos.x > roisize / 2 && pos.y > roisize / 2 && pos.x + roisize / 2 < imageSize.Width && pos.y + roisize / 2 < imageSize.Height)
					result.Add(pos);
			return result;
		}

		public static int[] IntRange(int count, int start=0)
		{
			int[] r = new int[count];
			for (int i = 0; i < count; i++) r[i] = start + i;
			return r;
		}

		public static T[] Sequence<T>(int count, Converter<int, T> conv)
		{
			return Array.ConvertAll(IntRange(count), conv);
		}

		public static float[][] ParseFloatMatrix(string filename)
		{
			LinkedList<float[]> data = new LinkedList<float[]>();

			using (StreamReader sr = new StreamReader(filename))
			{
				while (!sr.EndOfStream)
				{
					string line = sr.ReadLine();
					float[] n = Array.ConvertAll(line.Split('\t'), str =>
					{
						float v = float.NaN;
						float.TryParse(str, out v); return v;
					});
					data.AddLast(n);
				}
			}
			return data.ToArray();
		}

		public static void ForEachObjectField(object obj, Action<string, object> cb)
		{
			var tp = obj.GetType();
			foreach (var f in tp.GetFields())
				cb(f.Name, f.GetValue(obj));
		}
	}
}
