﻿using MouseTrap.Hooks.Events;
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
		private string _lastTitle;
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

			// Get process ID
			_ = NativeMethods.GetWindowThreadProcessId(handle, out uint windowThreadProcId);

			// Ignore these windows
			var windowStyle = NativeMethods.GetWindowStyleEx(handle);
			if ((windowStyle & WindowStylesEx.WS_EX_NOACTIVATE) == WindowStylesEx.WS_EX_NOACTIVATE) return;

			// Ignore ghost window when target is unresponsive
			// Ghost windows take foreground but target window doesn't trigger EVENT_SYSTEM_FOREGROUND
			// when responsive again.
			var className = NativeMethods.GetClassName(handle);
			var currentTitle = NativeMethods.GetWindowText(handle);
			if (className == "Ghost" && _lastTitle == currentTitle)
			{
				return;
			}

			// Get process path
			string processName = NativeMethods.GetFullProcessName((int)windowThreadProcId);

			// Send event
			ForegroundWindowChanged?.Invoke(this, new ForegroundWindowChangedEventArgs
			{
				Handle = handle,
				WindowThreadProcId = windowThreadProcId,
				ProcessPath = processName
			});

			// Store title
			_lastTitle = currentTitle;
		}
	}
}