using System;
using System.Runtime.InteropServices;

namespace MouseTrap.Interop
{
	[StructLayout(LayoutKind.Sequential)]
	internal struct MSLLHOOKSTRUCT
	{
		public Point pt;
		public int mouseData;
		public int flags;
		public int time;
		[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2111:PointersShouldNotBeVisible")]
		public UIntPtr dwExtraInfo;
	}
}
