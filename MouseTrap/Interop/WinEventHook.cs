using System;

namespace MouseTrap.Interop
{
	internal abstract class WinEventHook : IDisposable
	{
		private IntPtr _eventHookInstance;

		// Ensure delegate is not collected prematurely
		private readonly WinEventDelegate _winEventDelegate;

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
				uint processId = 0;
				uint threadId = 0;
				if (processHandle != IntPtr.Zero) threadId = NativeMethods.GetWindowThreadProcessId(processHandle, out processId);

				_eventHookInstance = NativeMethods.SetWinEventHook(
					(uint)winEventMin,
					(uint)(winEventMax > 0 ? winEventMax : winEventMin),
					IntPtr.Zero, _winEventDelegate, processId, threadId, (uint)WinEventConstant.WINEVENT_OUTOFCONTEXT);
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
