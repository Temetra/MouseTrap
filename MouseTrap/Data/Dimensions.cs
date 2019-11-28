namespace MouseTrap.Data
{
	public struct Dimensions
	{
		public Dimensions(double left, double top, double right, double bottom)
		{
			Left = left;
			Top = top;
			Right = right;
			Bottom = bottom;
		}

		public double Left { get; set; }
		public double Top { get; set; }
		public double Right { get; set; }
		public double Bottom { get; set; }
		public double Width => Right - Left;
		public double Height => Bottom - Top;

		public static Dimensions operator +(Dimensions a) => a;

		public static Dimensions operator -(Dimensions a) => new Dimensions(-a.Left, -a.Top, -a.Right, -a.Bottom);

		public static Dimensions operator +(Dimensions a, Dimensions b)
		{
			return new Dimensions
			{
				Left = a.Left + b.Left,
				Top = a.Top + b.Top,
				Right = a.Right + b.Right,
				Bottom = a.Bottom + b.Bottom
			};
		}

		public static Dimensions operator -(Dimensions a, Dimensions b) => a + -b;

		public static bool operator ==(Dimensions left, Dimensions right)
		{
			return left.Equals(right);
		}

		public static bool operator !=(Dimensions left, Dimensions right)
		{
			return !(left == right);
		}

		public override string ToString()
		{
			return $"{Left}, {Top}, {Right}, {Bottom}";
		}

		public override bool Equals(object obj)
		{
			return obj is Dimensions dimensions &&
				   Left == dimensions.Left &&
				   Top == dimensions.Top &&
				   Right == dimensions.Right &&
				   Bottom == dimensions.Bottom;
		}

		public override int GetHashCode()
		{
			var hashCode = -1819631549;
			hashCode = hashCode * -1521134295 + Left.GetHashCode();
			hashCode = hashCode * -1521134295 + Top.GetHashCode();
			hashCode = hashCode * -1521134295 + Right.GetHashCode();
			hashCode = hashCode * -1521134295 + Bottom.GetHashCode();
			return hashCode;
		}
	}
}
