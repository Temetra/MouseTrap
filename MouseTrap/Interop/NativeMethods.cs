using System;
using System.Runtime.InteropServices;
using System.Text;

namespace MouseTrap.Interop
{
	internal delegate bool WindowEnumCallback(IntPtr hWnd, int lParam);
	internal delegate void WinEventDelegate(IntPtr hWinEventHook, uint eventType, IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime);
	internal delegate IntPtr HookProc(int code, IntPtr wParam, IntPtr lParam);

	internal static class NativeMethods
	{
		[DllImport("user32.dll")]
		internal static extern bool EnumWindows(WindowEnumCallback lpEnumFunc, IntPtr lParam);

		[DllImport("user32.dll", SetLastError = true)]
		internal static extern uint GetWindowThreadProcessId(IntPtr hWnd, out uint processId);

		[DllImport("user32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
		internal static extern int GetWindowText(IntPtr hWnd, StringBuilder lpString, int nMaxCount);

		internal static string GetWindowText(IntPtr hWnd)
		{
			// Get a stringbuilder
			var sb = new StringBuilder(1024);

			// Get window text
			_ = GetWindowText(hWnd, sb, sb.Capacity);
			var result = sb.ToString();

			// Return result
			return result;
		}

		[DllImport("user32.dll")]
		internal static extern bool IsWindowVisible(IntPtr hWnd);

		[DllImport("dwmapi.dll")]
		internal static extern int DwmGetWindowAttribute(IntPtr hWnd, DWMWINDOWATTRIBUTE dwAttribute, out int pvAttribute, int cbAttribute);

		// Check if window is cloaked - this returns false if the call fails
		internal static bool IsWindowCloaked(IntPtr hWnd)
		{
			var result = DwmGetWindowAttribute(hWnd, DWMWINDOWATTRIBUTE.Cloaked, out int isCloaked, sizeof(int));
			return (result == 0 && isCloaked != 0);
		}

		[DllImport("user32.dll", EntryPoint = "GetWindowLong")]
		private static extern int GetWindowLongPtr32(IntPtr hWnd, int nIndex);

		[DllImport("user32.dll", EntryPoint = "GetWindowLongPtr")]
		private static extern IntPtr GetWindowLongPtr64(IntPtr hWnd, int nIndex);

		// This static method is required because Win32 does not support
		// GetWindowLongPtr directly
		internal static IntPtr GetWindowLong(IntPtr hWnd, int nIndex)
		{
			if (IntPtr.Size == 4) return new IntPtr(GetWindowLongPtr32(hWnd, nIndex));
			return GetWindowLongPtr64(hWnd, nIndex);
		}

		internal static WindowStylesEx GetWindowStyleEx(IntPtr hWnd)
		{
			return (WindowStylesEx)GetWindowLong(hWnd, (int)GWLFlags.GWL_EXSTYLE).ToInt32();
		}

		[DllImport("user32.dll", SetLastError = true)]
		internal static extern bool GetWindowRect(IntPtr hWnd, out Win32Rect lpRect);

		[DllImport("user32.dll")]
		internal static extern bool IsIconic(IntPtr hWnd);

		[DllImport("kernel32.dll")]
		internal static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, bool bInheritHandle, int dwProcessId);

		[DllImport("Kernel32.dll", CharSet = CharSet.Unicode)]
		internal static extern bool QueryFullProcessImageName([In] IntPtr hProcess, [In] uint dwFlags, [Out] StringBuilder lpExeName, [In, Out] ref uint lpdwSize);

		[DllImport("kernel32.dll", SetLastError = true)]
		internal static extern bool CloseHandle(IntPtr hHandle);

		internal static string GetFullProcessName(int processId)
		{
			// Initialise method result
			string result = string.Empty;

			// Get a stringbuilder from the pool
			var sb = new StringBuilder(1024);

			// Handle for process query
			IntPtr limitedHandle = IntPtr.Zero;

			// Size of buffer for process executable name
			uint len = (uint)sb.Capacity + 1;

			try
			{
				// Open process with limited permissions
				limitedHandle = OpenProcess(ProcessAccessFlags.QueryLimitedInformation, false, processId);

				// Get process executable name
				if (QueryFullProcessImageName(limitedHandle, 0, sb, ref len))
				{
					// Store result
					result = sb.ToString();
				}
				else
				{
					result = "[QueryFullProcessImageName failed]";
				}
			}
			catch (Exception)
			{
			}
			finally
			{
				// Close handle
				if (limitedHandle != IntPtr.Zero) CloseHandle(limitedHandle);
			}

			// Return result
			return result;
		}

		[DllImport("user32.dll")]
		internal static extern IntPtr SetWinEventHook(uint eventMin, uint eventMax,
			IntPtr hmodWinEventProc, WinEventDelegate lpfnWinEventProc, uint idProcess, uint idThread, uint dwFlags);

		[DllImport("user32.dll")]
		internal static extern bool UnhookWinEvent(IntPtr hWinEventHook);

		[DllImport("user32.dll")]
		public static extern bool IsWindow(IntPtr hWnd);

		[DllImport("user32.dll")]
		internal static extern bool ClipCursor(ref Win32Rect lpRect);

		[DllImport("user32.dll")]
		internal static extern bool ClipCursor(IntPtr lpRect);

		[DllImport("user32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
		internal static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);

		internal static string GetClassName(IntPtr handle)
		{
			string result = null;
			var sb = new StringBuilder(256);
			
			if (GetClassName(handle, sb, sb.Capacity) != 0)
			{
				result = sb.ToString();
			}
			
			return result;
		}
	}
}