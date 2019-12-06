using MouseTrap.Interop;
using System;
using System.Diagnostics;

namespace MouseTrap.Data
{
	public class WindowEnumerator : IWindowEnumerator
	{
		// Process ID of this application
		private readonly int _mouseTrapProcessId;

		/// <summary>
		/// Constructor
		/// </summary>
		public WindowEnumerator()
		{
			_mouseTrapProcessId = Process.GetCurrentProcess().Id;
		}

		/// <summary>
		/// Enumerates visible windows
		/// </summary>
		/// <param name="callback">Action to be called when a visible window is found</param>
		public void EnumerateWindows(Action<WindowDetails> callback)
		{
			NativeMethods.EnumWindows((hWnd, lParam) =>
			{
				// Get process ID
				_ = NativeMethods.GetWindowThreadProcessId(hWnd, out uint processId);

				// Get title
				var title = NativeMethods.GetWindowText(hWnd);

				// Validate window for list inclusion
				if (ShouldIncludeWindow(hWnd, processId, title))
				{
					// Get dimensions
					NativeMethods.GetWindowRect(hWnd, out Win32Rect rect);

					// Get process path
					var processPath = NativeMethods.GetFullProcessName((int)processId);

					// Send window details to callback
					callback(new WindowDetails
					(
						hWnd,
						processId,
						processPath,
						title,
						rect,
						NativeMethods.IsIconic(hWnd)
					));
				}

				// Continue iterating
				return true;
			}, IntPtr.Zero);
		}

		/// <summary>
		/// Determines if a window should be sent to the enumeration callback
		/// </summary>
		/// <param name="handle">Window handle</param>
		/// <param name="processId">Window process ID</param>
		/// <param name="title">Window title</param>
		/// <returns>True if window should be sent to callback</returns>
		private bool ShouldIncludeWindow(IntPtr handle, uint processId, string title)
		{
			// Exclude this application
			if (processId == _mouseTrapProcessId) return false;

			// Exclude windows without the Visible style
			if (!NativeMethods.IsWindowVisible(handle)) return false;

			// Ignore windows that are cloaked
			if (NativeMethods.IsWindowCloaked(handle)) return false;

			// Exclude windows with with no title
			if (string.IsNullOrEmpty(title)) return false;

			// Include app windows even if they have tool window style
			var windowStylesEx = NativeMethods.GetWindowStyleEx(handle);
			var windowStylesExFilter = WindowStylesEx.WS_EX_APPWINDOW;
			if ((windowStylesEx & windowStylesExFilter) != 0) return true;

			// Exclude tool windows, and windows that don't become foreground window when clicked
			windowStylesExFilter = WindowStylesEx.WS_EX_TOOLWINDOW | WindowStylesEx.WS_EX_NOACTIVATE;
			if ((windowStylesEx & windowStylesExFilter) != 0) return false;

			// Window should be included
			return true;
		}
	}
}