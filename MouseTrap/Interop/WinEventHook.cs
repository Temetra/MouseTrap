using System;

namespace MouseTrap.Interop
{
	internal abstract class WinEventHook : IDisposable
	{
		private uint _processId;
		private uint _threadId;
		private IntPtr _eventHookInstance;
		private WinEventConstant _winEventMin;
		private WinEventConstant _winEventMax;

		// Ensure delegate is not collected prematurely
		private WinEventDelegate _winEventDelegate;

		protected bool HasEventHookInstance { get => (_eventHookInstance != IntPtr.Zero); }

		// Constructor
		public WinEventHook()
		{
			_eventHookInstance = IntPtr.Zero;
			_winEventDelegate = new WinEventDelegate(WinEventCallback);
		}

		protected void StartWinEventHook(WinEventConstant winEventMin, WinEventConstant winEventMax = 0, IntPtr processHandle = default)
		{
			if (!HasEventHookInstance)
			{
				// If the idProcess parameter is nonzero and idThread is zero, the hook function receives 
				// the specified events from all threads in that process. If the idProcess parameter is 
				// zero and idThread is nonzero, the hook function receives the specified events only from 
				// the thread specified by idThread. If both are zero, the hook function receives the 
				// specified events from all threads and processes.
				_processId = 0;
				_threadId = 0;
				if (processHandle != IntPtr.Zero) _threadId = NativeMethods.GetWindowThreadProcessId(processHandle, out _processId);

				_winEventMin = winEventMin;
				_winEventMax = winEventMax > 0 ? winEventMax : winEventMin;

				_eventHookInstance = NativeMethods.SetWinEventHook(
					(uint)_winEventMin,
					(uint)_winEventMax,
					IntPtr.Zero, _winEventDelegate, _processId, _threadId, (uint)WinEventConstant.WINEVENT_OUTOFCONTEXT);
			}
		}

		protected void StopWinEventHook()
		{
			if (HasEventHookInstance)
			{
				NativeMethods.UnhookWinEvent(_eventHookInstance);
				_eventHookInstance = IntPtr.Zero;
			}
		}

		// Callback triggered when hook triggers
		private void WinEventCallback(IntPtr hWinEventHook, uint eventType,
			IntPtr hwnd, int idObject, int idChild, uint dwEventThread, uint dwmsEventTime)
		{
			if (hwnd != IntPtr.Zero)
			{
				WinEventCallback((WinEventConstant)eventType, hwnd, idObject, idChild);
			}
		}

		// Callback implemented by child class
		protected abstract void WinEventCallback(WinEventConstant eventType, IntPtr handle, int objectId, int childId);

		// IDisposable
		bool disposed = false;

		~WinEventHook()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (disposed) return;

			if (disposing)
			{
				// Free any other managed objects here.
			}

			// Free any unmanaged objects here.
			StopWinEventHook();

			// Done
			disposed = true;
		}
	}
}
