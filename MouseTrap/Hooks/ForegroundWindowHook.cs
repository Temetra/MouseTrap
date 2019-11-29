using MouseTrap.Hooks.Events;
using MouseTrap.Interop;
using System;
using System.Diagnostics;

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
				// Capture range of events
				StartWinEventHook(WinEventConstant.EVENT_SYSTEM_FOREGROUND, WinEventConstant.EVENT_SYSTEM_MINIMIZEEND);
			}
		}

		public void StopHook()
		{
			StopWinEventHook();
		}

		protected override void WinEventCallback(WinEventConstant eventType, IntPtr handle, int objectId, int childId)
		{
			LogWinEventCallback(eventType, handle, objectId, childId);

			// Only process events related to FG capture
			if (eventType != WinEventConstant.EVENT_SYSTEM_FOREGROUND && eventType != WinEventConstant.EVENT_SYSTEM_MINIMIZEEND)
			{
				return;
			}

			// Only process events with valid parameters
			if (handle == null || objectId != 0)
			{
				return;
			}

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

		[Conditional("DEBUG")]
		private void LogWinEventCallback(WinEventConstant eventType, IntPtr handle, int objectId, int childId)
		{
			NativeMethods.GetWindowThreadProcessId(handle, out uint windowThreadProcId);
			string processName = NativeMethods.GetFullProcessName((int)windowThreadProcId);
			Logging.Logger.Write($"{eventType} {processName}");
		}
	}
}