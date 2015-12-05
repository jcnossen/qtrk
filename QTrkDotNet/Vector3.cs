using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Diagnostics;

using System.ComponentModel;

namespace QTrkDotNet
{
	public struct Vector3
	{
		[XmlAttribute] public float x;
		[XmlAttribute] public float y;
		[XmlAttribute] public float z;

		// Properties to allow editing in property grid
		[XmlIgnore] public float X { get { return x; } set { x=value; }}
		[XmlIgnore] public float Y { get { return y; } set { y=value; }}
		[XmlIgnore] public float Z { get { return z; } set { z=value; }}

		public Vector3(float X,float Y,float Z)
		{ x = X; y = Y; z = Z; }

		public static Vector3 Right { get { return new Vector3(1.0f, 0.0f, 0.0f); } }
		public static Vector3 Up { get { return new Vector3(0.0f, 1.0f, 0.0f); } }
		public static Vector3 Front { get { return new Vector3(0.0f, 0.0f, 1.0f); } }

		#region Operators

		public static Vector3 operator*(Vector3 v, float f)
		{
			Vector3 r;
			r.x = v.x * f;
			r.y = v.y * f;
			r.z = v.z * f;
			return r;
		}

		public static Vector3 operator *(float f, Vector3 v)
		{
			Vector3 r;
			r.x = v.x * f;
			r.y = v.y * f;
			r.z = v.z * f;
			return r;
		}

		public static Vector3 operator /(Vector3 v, float f)
		{
			Vector3 r;
			r.x = v.x / f;
			r.y = v.y / f;
			r.z = v.z / f;
			return r;
		}

		public static Vector3 operator +(Vector3 a, Vector3 b)
		{
			Vector3 r;
			r.x = a.x + b.x;
			r.y = a.y + b.y;
			r.z = a.z + b.z;
			return r;
		}

		public static Vector3 operator -(Vector3 a, Vector3 b)
		{
			Vector3 r;
			r.x = a.x - b.x;
			r.y = a.y - b.y;
			r.z = a.z - b.z;
			return r;
		}

		public static Vector3 operator -(Vector3 a)
		{
			return new Vector3(-a.x, -a.y, -a.z);
		}

		// Dot-product
		public float Dot(Vector3 a)
		{
			return a.x * x + a.y * y + a.z * z;
		}
		public static float operator|(Vector3 a, Vector3 b)
		{
			return a.x * b.x + a.y * b.y + a.z * b.z;
		}

		// Component-wise multiplication
		public Vector3 Multiply(Vector3 v)
		{
			return new Vector3(x * v.x, y * v.y, z * v.z);
		}

		//Cross-product
		public static Vector3 operator %(Vector3 a, Vector3 b)
		{
			Vector3 r;
			r.x = (a.y * b.z) - (a.z * b.y);
			r.y = (a.z * b.x) - (a.x * b.z);
			r.z = (a.x * b.y) - (a.y * b.x);
			return r;
		}
		#endregion

		public float Length
		{
			get { return (float)System.Math.Sqrt((double)(x * x + y * y + z * z)); }
		}

		public float SqLength
		{
			get { return x * x + y * y + z * z; }
		}

		/// <summary>
		/// Normalizes the vector
		/// </summary>
		/// <returns>Original length</returns>
		public float Normalize()
		{
			float len = Length;

			if (len < 0.0001f)
				return 0.0f;

			float invLength = 1.0f / len;
			x *= invLength;
			y *= invLength;
			z *= invLength;
			return len;
		}

		/// <summary>
		/// Generates 2 vectors perpendicular to this vector
		/// </summary>
		public void OrthoSpace(out Vector3 a, out Vector3 b)
		{
			Vector3 n = this;
			n.Normalize();

			if (Math.Abs(n.y) > 0.8f)
				n = Right;
			else
				n = Up;

			a = this % n;
			b = a % this;
			a.Normalize();
			b.Normalize();
		}

		// Calculates the exact nearest point, not just one of the box'es vertices
		public static Vector3 NearestBoxPoint(Vector3 min, Vector3 max, Vector3 pos)
		{
			Vector3 ret;
			Vector3 mid = (max + min) * 0.5f;
			if(pos.x < min.x) ret.x = min.x;
			else if(pos.x > max.x) ret.x = max.x;
			else ret.x = pos.x;

			if(pos.y < min.y) ret.y = min.y;
			else if(pos.y > max.y) ret.y = max.y;
			else ret.y = pos.y;

			if(pos.z < min.z) ret.z = min.z;
			else if(pos.z > max.z) ret.z = max.z;
			else ret.z = pos.z;
			return ret;
		}

		public override string ToString()
		{
			return String.Format("X={0}, Y={1}, Z={2}", x, y, z);
		}

		public Vector3 Absolute()
		{
			return new Vector3(Math.Abs(x), Math.Abs(y), Math.Abs(z));
		}

		public Vector3 MapUnitCubeToUnitSphere()
		{
			float xx=x*x, yy = y*y, zz=z*z;
			return new Vector3(
				x * (float)Math.Sqrt(1.0f - 0.5f * yy - 0.5f * zz + (yy * zz) / 3.0f),
				y * (float)Math.Sqrt(1.0f - 0.5f * xx - 0.5f * zz + (xx * zz) / 3.0f),
				z * (float)Math.Sqrt(1.0f - 0.5f * xx - 0.5f * yy + (xx * yy) / 3.0f));
		}

		//FIXME: Untested
		public Vector3 MapSphereToUnitCube()
		{
			Vector3 abs = Absolute();

			if (abs.x > abs.y)
			{
				if (abs.x > abs.z)
					return this / abs.x;
				else
					return this / abs.z;
			}
			else
			{
				if (abs.y > abs.z)
					return this / abs.y;
				else
					return this / abs.z;
			}
		}

		public void BBMax(Vector3 v)
		{
			if (v.x > x) x = v.x;
			if (v.y > y) y = v.y;
			if (v.z > z) z = v.z;
		}

		public void BBMin(Vector3 v)
		{
			if (v.x < x) x = v.x;
			if (v.y < y) y = v.y;
			if (v.z < z) z = v.z;
		}

		public float DistanceToRay(Vector3 start, Vector3 dir)
		{
			Vector3 ortho = (this - start) % dir;
			Vector3 planeNorm = ortho % dir;
			if (planeNorm.Normalize() == 0.0f)
				return 0.0f;
			else
				return Math.Abs((planeNorm | start) - (planeNorm | this));
		}

		public float DistanceTo(Vector3 a)
		{
			return (this - a).Length;
		}

		public bool InRange(Vector3 dir, Vector3 v1, Vector3 v2)
		{
			float d1 = dir | v1;
			float d2 = dir | v2;
			float dcmp = dir | this;

			return dcmp >= Math.Min(d1, d2) && dcmp <= Math.Max(d1, d2);
		}

		public void RoundToGrid(float gridSize)
		{
			x = gridSize * (float)Math.Round(x / gridSize);
			y = gridSize * (float)Math.Round(y / gridSize);
			z = gridSize * (float)Math.Round(z / gridSize);
		}
	}
}
