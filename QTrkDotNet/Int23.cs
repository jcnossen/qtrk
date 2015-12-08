
	public struct Int2
	{
		public Int2(int x, int y)
		{
			this.x = x; this.y = y;
		}
		public int x, y;

		public void Clamp(int w, int h)
		{
			if (x < 0) x = 0; if (x > w - 1) x = w - 1;
			if (y < 0) y = 0; if (y > h - 1) y = h - 1;
		}
		public static Int2 operator +(Int2 a, Int2 b)
		{
			Int2 r;
			r.x = a.x + b.x;
			r.y = a.y + b.y;
			return r;
		}

		public static Int2 operator -(Int2 a, Int2 b)
		{
			Int2 r;
			r.x = a.x - b.x;
			r.y = a.y - b.y;
			return r;
		}

		public static Int2 operator -(Int2 a, int b)
		{
			Int2 r;
			r.x = a.x - b;
			r.y = a.y - b;
			return r;
		}

		public static Int2 operator -(int a, Int2 b)
		{
			Int2 r;
			r.x = a - b.x;
			r.y = a - b.y;
			return r;
		}

		public static Int2 operator +(Int2 a, int b)
		{
			Int2 r;
			r.x = a.x + b;
			r.y = a.y + b;
			return r;
		}

		public static Int2 operator +(int a, Int2 b)
		{
			Int2 r;
			r.x = a + b.x;
			r.y = a + b.y;
			return r;
		}

		public static Int2 operator -(Int2 a)
		{
			return new Int2(-a.x, -a.y);
		}

		public float Length
		{
			get { return (float)Math.Sqrt(x * x + y * y); }
		}
	}
	public struct Int3 : IEquatable<Int3>
	{
		public Int3(Int2 p)
		{
			this.x = p.x; this.y = p.y; this.z = 0;
		}
		public Int3(int x, int y, int z=0)
		{
			this.x = x; this.y = y;
			this.z = z;
		}
		public int x, y,z;

		public void Clamp(int w, int h, int d)
		{
			if (x < 0) x = 0; if (x > w - 1) x = w - 1;
			if (y < 0) y = 0; if (y > h - 1) y = h - 1;
			if (z < 0) z = 0; if (z > d - 1) z = d - 1;
		}

		public static bool operator ==(Int3 a, Int3 b)
		{
			return a.x == b.x && a.y == b.y && a.z == b.z;
		}
		public static bool operator !=(Int3 a, Int3 b)
		{
			return a.x != b.x || a.y != b.y || a.z != b.z;
		}

		public static Int3 operator +(Int3 a, Int3 b)
		{
			Int3 r;
			r.x = a.x + b.x;
			r.y = a.y + b.y;
			r.z = a.z + b.z;
			return r;
		}

		public static Int3 operator -(Int3 a, Int3 b)
		{
			Int3 r;
			r.x = a.x - b.x;
			r.y = a.y - b.y;
			r.z = a.z - b.z;
			return r;
		}

		public static Int3 operator -(Int3 a, int b)
		{
			Int3 r;
			r.x = a.x - b;
			r.y = a.y - b;
			r.z = a.z - b;
			return r;
		}

		public static Int3 operator -(int a, Int3 b)
		{
			Int3 r;
			r.x = a - b.x;
			r.y = a - b.y;
			r.z = a - b.z;
			return r;
		}

		public static Int3 operator +(Int3 a, int b)
		{
			Int3 r;
			r.x = a.x + b;
			r.y = a.y + b;
			r.z = a.z + b;
			return r;
		}

		public static Int3 operator +(int a, Int3 b)
		{
			Int3 r;
			r.x = a + b.x;
			r.y = a + b.y;
			r.z = a + b.z;
			return r;
		}

		public static Int3 operator -(Int3 a)
		{
			return new Int3(-a.x, -a.y, -a.z);
		}

		public float Length
		{
			get { return (float)Math.Sqrt(x * x + y * y + z * z); }
		}

		public override string ToString()
		{
			return string.Format("Int3({0},{1},{2})", x, y, z);
		}

		public Int2 XY {
			get { return new Int2(x, y); }
			set { x = value.x; y = value.y; } 
		}

		public bool Equals(Int3 other)
		{
			return other == this;
		}

		bool IEquatable<Int3>.Equals(Int3 other)
		{
			return other == this;
		}
	}