using MouseTrap.Data;
using MouseTrap.Interop;
using System;
using System.Runtime.InteropServices;

namespace MouseTrap.Hooks
{
	/// <summary>
	/// Implemention of <see cref="IMouseHook"/>
	/// </summary>
	internal class MouseHook : IMouseHook
	{
		private bool _isDisposed;
		private readonly HookProc _mouseHookCallback;
		private IntPtr _mouseHookPtr;
		private Dimensions _boundaries;
		private bool _isRestricted;

		public MouseHook()
		{
			_mouseHookCallback = new HookProc(MouseHookCallbackFunction);
			_mouseHookPtr = IntPtr.Zero;
			_boundaries = new Dimensions();
			_isDisposed = false;
		}

		public void StartHook(Dimensions region)
		{
			if (_mouseHookPtr == IntPtr.Zero)
			{
				// Set state
				_isRestricted = false;

				// Set hook, using field to avoid issues with GetLastWin32Error
				// GetLastWin32Error called immediately after SetWindowsHookEx
				_mouseHookPtr = NativeMethods.SetWindowsHookEx(HookType.WH_MOUSE_LL, _mouseHookCallback, IntPtr.Zero, 0);
				//int errorCode = Marshal.GetLastWin32Error();

				// TODO improve error handling
				if (_mouseHookPtr == IntPtr.Zero)
				{
					//string errorMessage = new Win32Exception(errorCode).Message;
					//Logging.Logger.Write("Hook", $"Mouse error {errorCode} {errorMessage}");
				}
			}
		}

		public void StopHook()
		{
			if (_mouseHookPtr != IntPtr.Zero) NativeMethods.UnhookWindowsHookEx(_mouseHookPtr);
			_mouseHookPtr = IntPtr.Zero;
		}

		public void SetRegion(Dimensions region)
		{
			_boundaries = new Dimensions(region.Left, region.Top, region.Right, region.Bottom);
		}

		public void RestrictMouseToRegion()
		{
			_isRestricted = true;
		}

		public void UnrestrictMouse()
		{
			_isRestricted = false;
		}

		private bool BoundaryIsValid
		{
			get
			{
				var width = _boundaries.Right - _boundaries.Left;
				var height = _boundaries.Bottom - _boundaries.Top;
				return (width > 0) && (height > 0);
			}
		}

		private IntPtr MouseHookCallbackFunction(int code, IntPtr wParam, IntPtr lParam)
		{
			// Only handle WM_MOUSEMOVE messages
			var isMouseMoveMsg = (wParam.ToInt32() == 0x0200); // #define WM_MOUSEMOVE 0x0200

			// Check if message should be handled
			if (_isRestricted && isMouseMoveMsg && code >= 0 && BoundaryIsValid)
			{
				// Get pointer data
				var mouseInfo = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
				var point = mouseInfo.pt;

				// Limit X
				if (point.X < _boundaries.Left) point.X = (int)_boundaries.Left;
				else if (point.X > _boundaries.Right) point.X = (int)_boundaries.Right;

				// Limit Y
				if (point.Y < _boundaries.Top) point.Y = (int)_boundaries.Top;
				else if (point.Y > _boundaries.Bottom) point.Y = (int)_boundaries.Bottom;

				// Move cursor
				NativeMethods.SetCursorPos(point.X, point.Y);

				// Done
				return new IntPtr(1);
			}

			// Skip
			return NativeMethods.CallNextHookEx(IntPtr.Zero, code, wParam, lParam);
		}

		// IDisposable
		~MouseHook()
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
