using MouseTrap.Hooks.Events;
using MouseTrap.Interop;
using System;

namespace MouseTrap.Hooks
{
	/// <summary>
	/// Implementation of <see cref="IForegroundWindowHook"/>
	/// </summary>
	internal sealed class ForegroundWindowHook : WinEventHook, IForegroundWindowHook
	{
		public event EventHandler<ForegroundWindowChangedEventArgs> ForegroundWindowChanged;

		public void StartHook()
		{
			if (!HasEventHookInstance)
			{
				StartWinEventHook(WinEventConstant.EVENT_SYSTEM_FOREGROUND);
			}
		}

		public void StopHook()
		{
			StopWinEventHook();
		}

		protected override void WinEventCallback(WinEventConstant eventType, IntPtr handle, int objectId, int childId)
		{
			if (handle != null && objectId == 0)
			{
				// Ignore these windows
				var windowStyle = NativeMethods.GetWindowStyleEx(handle);
				if ((windowStyle & WindowStylesEx.WS_EX_NOACTIVATE) == WindowStylesEx.WS_EX_NOACTIVATE) return;

				// Get process ID and name
				NativeMethods.GetWindowThreadProcessId(handle, out uint windowThreadProcId);
				string processName = NativeMethods.GetFullProcessName((int)windowThreadProcId);

				// Send event
				ForegroundWindowChanged?.Invoke(this, new ForegroundWindowChangedEventArgs
				{
					Handle = handle,
					WindowThreadProcId = windowThreadProcId,
					ProcessPath = processName
				});
			}
		}
	}
}