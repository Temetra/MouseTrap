using MouseTrap.Data;
using MouseTrap.Interop;
using System;
using System.Diagnostics;
using MouseTrap.Hooks.Events;

namespace MouseTrap.Hooks
{
	/// <summary>
	/// Implementation of <see cref="IWindowUpdateHook"/>
	/// </summary>
	internal sealed class WindowUpdateHook : WinEventHook, IWindowUpdateHook
	{
		private IntPtr _targetHandle = IntPtr.Zero;

		public void StartHook(IntPtr handle)
		{
			if (handle != IntPtr.Zero && _targetHandle == IntPtr.Zero)
			{
				_targetHandle = handle;
				StartWinEventHook(WinEventConstant.EVENT_OBJECT_DESTROY, WinEventConstant.EVENT_OBJECT_NAMECHANGE, handle);
				SendWindowTitle(handle);
				SendWindowDimensions(handle);
			}
		}

		public void StopHook()
		{
			StopWinEventHook();
			_targetHandle = IntPtr.Zero;
		}

		protected override void WinEventCallback(WinEventConstant eventType, IntPtr handle, int objectId, int childId)
		{
			if (handle == _targetHandle && objectId == 0)
			{
				if (eventType == WinEventConstant.EVENT_OBJECT_DESTROY)
				{
					// Target window was closed
					WindowClosed?.Invoke(this, EventArgs.Empty);
				}
				else if (eventType == WinEventConstant.EVENT_OBJECT_LOCATIONCHANGE)
				{
					// Target window size has changed
					SendWindowDimensions(handle);
				}
				else if (eventType == WinEventConstant.EVENT_OBJECT_NAMECHANGE)
				{
					// Target window title has changed
					SendWindowTitle(handle);
				}
			}
		}

		private void SendWindowTitle(IntPtr handle)
		{
			var title = NativeMethods.GetWindowText(handle);
			TitleChanged?.Invoke(this, new TitleChangedEventArgs { Title = title });
		}

		private void SendWindowDimensions(IntPtr handle)
		{
			NativeMethods.GetWindowRect(handle, out Win32Rect rect);
			DimensionsChanged?.Invoke(this, new DimensionsChangedEventArgs
			{
				Dimensions = new Dimensions
				{
					Left = rect.Left,
					Top = rect.Top,
					Right = rect.Right,
					Bottom = rect.Bottom
				}
			});
		}

		public event EventHandler WindowClosed;
		public event EventHandler<TitleChangedEventArgs> TitleChanged;
		public event EventHandler<DimensionsChangedEventArgs> DimensionsChanged;
	}
}
