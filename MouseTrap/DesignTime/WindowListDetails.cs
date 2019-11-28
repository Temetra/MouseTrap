using System.IO;

namespace MouseTrap.DesignTime
{
	public class WindowListDetails : WindowListItem
	{
		public WindowListDetails()
		{
			Handle = new System.IntPtr(0xA2);
			ProcessId = 102;
			ProcessPath = @"c:\windows\system32\notepad.exe";
			Title = "This is a test title";
			Width = 1920;
			Height = 1280;
			ShortPath = Path.GetFileName(ProcessPath);
		}
	}
}
