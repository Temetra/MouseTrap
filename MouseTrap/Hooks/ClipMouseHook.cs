using MouseTrap.Data;
using MouseTrap.Interop;
using System;
using System.Threading;

namespace MouseTrap.Hooks
{
	internal class ClipMouseHook : IMouseHook
	{
		private bool _isDisposed;
		private bool _isRestricted;
		private Win32Rect _region;
		private Timer _timer;

		public void StartHook(Dimensions region)
		{
			SetRegion(region);
		}

		public void StopHook()
		{
			UnrestrictMouse();
		}

		public void RestrictMouseToRegion()
		{
			_isRestricted = true;
			_timer = new Timer(TimerCallback, null, 0, 17);
		}

		private void TimerCallback(object state)
		{
			if (_isRestricted) NativeMethods.ClipCursor(ref _region);
		}

		public void SetRegion(Dimensions region)
		{
			_region = new Win32Rect
			{
				Left = (int) region.Left,
				Top = (int)region.Top,
				Right = (int)region.Right,
				Bottom = (int)region.Bottom
			};

			if (_isRestricted) RestrictMouseToRegion();
		}

		public void UnrestrictMouse()
		{
			_isRestricted = false;
			_timer.Dispose();
			NativeMethods.ClipCursor(IntPtr.Zero);
		}

		// IDisposable
		~ClipMouseHook()
		{
			Dispose(false);
		}
		// IDisposable
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		// IDisposable
		protected virtual void Dispose(bool disposing)
		{
			if (_isDisposed) return;

			if (disposing)
			{
				// Free any other managed objects here.
			}

			// Free any unmanaged objects here.
			StopHook();

			// Done
			_isDisposed = true;
		}
	}
}
