using System;
using System.Windows.Media.Imaging;

namespace MouseTrap.Data
{
	public interface IWindowListItem
	{
		IntPtr Handle { get; }
		uint ProcessId { get; }
		string ProcessPath { get; }
		string Title { get; }
		double Width { get; }
		double Height { get; }
		string ShortPath { get; }
		BitmapSource ProcessIcon { get; }
		bool IsMinimized { get; }
	}
}
