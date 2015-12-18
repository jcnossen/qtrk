using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

namespace QTrkDotNet
{
	public class FloatImg : IDisposable
	{
		public IntPtr pixels;
		public int w, h;

		public FloatImg(int w, int h)
		{
			Alloc(w, h);
		}

        public unsafe FloatImg(int w, int h, float* src)
        {
            Alloc(w, h);

            float* dst=(float*)pixels.ToPointer();
            for (int i = 0; i < w * h; i++)
                dst[i] = src[i];
        }

        void Alloc(int w, int h)
		{
			Dispose();
			this.w = w; this.h = h;
			pixels = Marshal.AllocHGlobal(w * h * 4);
		}

		public unsafe FloatImg(Bitmap bmp, byte chan)
		{
			Alloc(bmp.Width,bmp.Height);
			var bmpData = bmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);

			float* dst = (float*)pixels.ToPointer();
			var line = bmpData.Scan0;
			for (int y = 0; y < h; y++)
			{
				byte* lp = (byte*)line.ToPointer();

				for (int x = 0; x < w; x++)
					dst[y * w + x] = lp[x * 4 + chan] / 255.0f;

				line = IntPtr.Add(line, bmpData.Stride);
			}
			bmp.UnlockBits(bmpData);
		}

		public Bitmap ToImage()
		{
			unsafe {
				var bmp=new Bitmap(w, h, PixelFormat.Format32bppArgb);
				var bmpData = bmp.LockBits(new Rectangle(0, 0, w, h), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
				float* src = (float*) pixels.ToPointer();

				var line = bmpData.Scan0;
				for (int y = 0; y < h; y++)
				{
					byte* lp = (byte*)line.ToPointer();

					for (int x = 0; x < w; x++)
					{
						lp[3] = 255;  
						lp[0] = lp[1] = lp[2]= (byte)(src[y * w + x] * 255);
						lp += 4;
					}
					line = IntPtr.Add(line, bmpData.Stride);
				}
				bmp.UnlockBits(bmpData);

				return bmp;
			}
		}

		public void Dispose()
		{
			if (pixels != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(pixels);
				pixels = IntPtr.Zero;
			}
		}

		public unsafe ImageData ImageData
		{
			get
			{
				ImageData r = new ImageData()
				{
					data = (float*)pixels.ToPointer(),
					width = w,
					height = h
				};
				return r;
			}

		}
	}

	public static unsafe class QTrkUtil
	{
		public static void ComputeRadialProfile(float[] dst, int angularSteps, float minradius, float maxradius, Vector2 center, FloatImg src, float mean, bool normalize)
		{
		}

		public static void NormalizeRadialProfile(float[] prof)
		{
			QTrkDLL.NormalizeRadialProfile(prof, prof.Length);
		}

		public static void GenerateImageFromLUT(FloatImg image, FloatImg zlut, float minradius, float maxradius, Vector3 pos, bool useSplineInterp, int ovs)
		{
			ImageData imgData= image.ImageData;
			ImageData zlutData=zlut.ImageData;

			QTrkDLL.GenerateImageFromLUT(ref imgData, ref zlutData, minradius, maxradius,
				pos, useSplineInterp, ovs);
		}

		public static void ApplyPoissonNoise(FloatImg img, float poissonMax, float maxValue)
		{
			ImageData imgData=img.ImageData;
			QTrkDLL.ApplyPoissonNoise(ref imgData, poissonMax, maxValue);
		}

		public static void ApplyGaussianNoise(FloatImg img, float sigma)
		{
		}

        public static Int2[] FindBeads(FloatImg img, Int2 sampleCornerPos, int roi, float imgRelDist, float acceptance)
        {
//                    public static extern IntPtr QTrkFindBeads(float* image, int w, int h, int smpCornerPosX, int smpCornerPosY, int roi, float imgRelDist, float acceptance);
            int beadCount;
            IntPtr beadListPtr = QTrkDLL.QTrkFindBeads(img.ImageData.data, img.w, img.h, sampleCornerPos.x, sampleCornerPos.y, roi, imgRelDist, acceptance, out beadCount);
            Int2* beadpos = (Int2*)beadListPtr.ToPointer();

            Int2[] r = new Int2[beadCount];
            for (int i = 0; i < beadCount; i++)
                r[i] = beadpos[i];

            QTrkDLL.QTrkFreeROIPositions(beadListPtr);
            return r;
        }


	}
}
