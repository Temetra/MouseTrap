using MouseTrap.Data;
using System;
using System.Windows.Media.Imaging;

namespace MouseTrap.DesignTime
{
	public class WindowListItem : IWindowListItem
	{
		public IntPtr Handle { get; set; }
		public uint ProcessId { get; set; }
		public string ProcessPath { get; set; }
		public string Title { get; set; }
		public double Width { get; set; }
		public double Height { get; set; }
		public string ShortPath { get; set; }
		public BitmapSource ProcessIcon { get; set; }
		public bool IsMinimized { get; set; }
	}
}
