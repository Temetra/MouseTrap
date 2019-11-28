using MouseTrap.Interop;
using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace MouseTrap.Data
{
	public class WindowDetails : IWindowListItem
	{
		public WindowDetails(IntPtr handle, uint processId, string processPath, string title, Win32Rect rect, bool isMinimized)
		{
			Handle = handle;
			ProcessId = processId;
			ProcessPath = processPath;
			Title = title;
			Width = rect.Right - rect.Left;
			Height = rect.Bottom - rect.Top;
			IsMinimized = isMinimized;
			ShortPath = Path.GetFileName(processPath);
			ProcessIcon = IconGenerator.GetIcon(processPath);
		}

		public IntPtr Handle { get; }
		public uint ProcessId { get; }
		public string ProcessPath { get; }
		public string Title { get; }
		public double Width { get; }
		public double Height { get; }
		public string ShortPath { get; }
		public BitmapSource ProcessIcon { get; }
		public bool IsMinimized { get; }
	}
}
