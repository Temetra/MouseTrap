using MouseTrap.Data;
using MouseTrap.Interop;
using System;
using System.ComponentModel;
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
		private int _xMin;
		private int _xMax;
		private int _yMin;
		private int _yMax;
		private bool _isRestricted;

		public MouseHook()
		{
			_mouseHookCallback = new HookProc(MouseHookCallbackFunction);
			_mouseHookPtr = IntPtr.Zero;
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
				int errorCode = Marshal.GetLastWin32Error();

				// TODO improve error handling
				if (_mouseHookPtr == IntPtr.Zero)
				{
					string errorMessage = new Win32Exception(errorCode).Message;
					Logging.Logger.Write($"Error {errorCode} {errorMessage}");
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
			_xMin = (int)region.Left;
			_xMax = (int)region.Right;
			_yMin = (int)region.Top;
			_yMax = (int)region.Bottom;
		}

		public void RestrictMouseToRegion()
		{
			// Restrict to region if boundary is valid
			var boundaryIsValid = (_xMax - _xMin > 0) && (_yMax - _yMin > 0);
			if (boundaryIsValid == false) Logging.Logger.Write($"Boundary is not valid - x {_xMin}:{_xMax} y {_yMin}:{_yMax}");
			_isRestricted = boundaryIsValid;
		}

		public void UnrestrictMouse()
		{
			_isRestricted = false;
		}

		private IntPtr MouseHookCallbackFunction(int code, IntPtr wParam, IntPtr lParam)
		{
			// Only handle WM_MOUSEMOVE messages
			var isMouseMoveMsg = (wParam.ToInt32() == 0x0200); // #define WM_MOUSEMOVE 0x0200

			// Check if message should be handled
			if (_isRestricted && isMouseMoveMsg && code >= 0)
			{
				// Get pointer data
				var mouseInfo = (MSLLHOOKSTRUCT)Marshal.PtrToStructure(lParam, typeof(MSLLHOOKSTRUCT));
				var posX = mouseInfo.pt.X;
				var posY = mouseInfo.pt.Y;

				// Limit X
				if (posX < _xMin) posX = _xMin;
				else if (posX > _xMax) posX = _xMax;

				// Limit Y
				if (posY < _yMin) posY = _yMin;
				else if (posY > _yMax) posY = _yMax;

				// Move cursor
				if (posX != mouseInfo.pt.X || posY != mouseInfo.pt.Y)
				{
					NativeMethods.SetCursorPos(posX, posY);
					return new IntPtr(1);
				}
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
