using System.Runtime.InteropServices;

namespace MouseTrap.Interop
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct Point
	{
		public int X;
		public int Y;

		public override string ToString()
		{
			return string.Format("{0}, {1}", X, Y);
		}
	}
}
