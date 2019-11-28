using MouseTrap.Data;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;

namespace MouseTrap.DesignTime
{
	public class WindowList : ViewModels.WindowList
	{
		public WindowList() : base()
		{
			// Create list
			var items = new List<WindowListItem>
			{
				new WindowListItem { IsMinimized = true, Handle = new System.IntPtr(0xb1), ProcessId = 1001, ProcessPath = @"nope.exe", Title = "This is a test title", Width = 1920, Height = 1280 },
				new WindowListItem { Handle = new System.IntPtr(0xA1), ProcessId = 101, ProcessPath = @"nope.exe", Title = "This is a test title", Width = 1920, Height = 1280 },
				new WindowListItem { Handle = new System.IntPtr(0xA2), ProcessId = 102, ProcessPath = @"c:\windows\system32\notepad.exe", Title = "This is a test title", Width = 1920, Height = 1280 },
				new WindowListItem { Handle = new System.IntPtr(0xA3), ProcessId = 103, ProcessPath = @"c:\windows\system32\notepad.exe", Title = "This is a test title", Width = 1920, Height = 1280 },
				new WindowListItem { IsMinimized = true, Handle = new System.IntPtr(0xb2), ProcessId = 1002, ProcessPath = @"c:\windows\system32\notepad.exe", Title = "This is a test title", Width = 1920, Height = 1280 },
				new WindowListItem { IsMinimized = true, Handle = new System.IntPtr(0xb3), ProcessId = 1003, ProcessPath = @"c:\windows\system32\notepad.exe", Title = "This is a test title", Width = 1920, Height = 1280 },
				new WindowListItem { IsMinimized = true, Handle = new System.IntPtr(0xb1), ProcessId = 1001, ProcessPath = @"nope.exe", Title = "This is a test title", Width = 1920, Height = 1280 },
				new WindowListItem { Handle = new System.IntPtr(0xA1), ProcessId = 101, ProcessPath = @"nope.exe", Title = "This is a test title", Width = 1920, Height = 1280 },
				new WindowListItem { Handle = new System.IntPtr(0xA2), ProcessId = 102, ProcessPath = @"c:\windows\system32\notepad.exe", Title = "This is a test title", Width = 1920, Height = 1280 },
				new WindowListItem { Handle = new System.IntPtr(0xA3), ProcessId = 103, ProcessPath = @"c:\windows\system32\notepad.exe", Title = "This is a test title", Width = 1920, Height = 1280 },
				new WindowListItem { IsMinimized = true, Handle = new System.IntPtr(0xb2), ProcessId = 1002, ProcessPath = @"c:\windows\system32\notepad.exe", Title = "This is a test title", Width = 1920, Height = 1280 },
				new WindowListItem { IsMinimized = true, Handle = new System.IntPtr(0xb3), ProcessId = 1003, ProcessPath = @"c:\windows\system32\notepad.exe", Title = "This is a test title", Width = 1920, Height = 1280 },
			};

			foreach (var item in items)
			{
				item.ShortPath = Path.GetFileName(item.ProcessPath);
				item.ProcessIcon = IconGenerator.GetIcon(item.ProcessPath);
			}

			// Set model
			WindowListItems = new ObservableCollection<IWindowListItem>(items);

			// Set selected item
			SelectedWindow = WindowListItems[2];
		}
	}
}
