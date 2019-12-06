using MouseTrap.Data;
using MouseTrap.Interop;
using MouseTrap.Core.AppState;
using System;

namespace MouseTrap.Core.SpecificWindow
{
	public class UnlockedState : BaseAppState, IAppState
	{
		public override void Lock(IAppStateContext context, IntPtr handle)
		{
			// Notify client the lock request failed
			if (!NativeMethods.IsWindow(handle))
			{
				context.SendLockStateChange();
				return;
			}

			// Get process id
			_ = NativeMethods.GetWindowThreadProcessId(handle, out uint procId);

			// Get process path
			string procPath = NativeMethods.GetFullProcessName((int)procId);

			// Get title
			string title = NativeMethods.GetWindowText(handle);

			// Get padded region
			// Upper-left and lower-right corners of the window
			NativeMethods.GetWindowRect(handle, out Win32Rect rect);
			context.WindowDimensions = new Dimensions
			{
				Left = rect.Left,
				Top = rect.Top,
				Right = rect.Right,
				Bottom = rect.Bottom
			};

			// Update state
			context.Handle = handle;
			context.ProcessId = procId;
			context.ProcessPath = procPath;
			context.SetCurrentState(new WaitingState());

			// Start hooks
			context.ForegroundHook.StartHook();
			context.WindowHook.StartHook(handle);
			context.MouseHook.StartHook(context.Padding + context.WindowDimensions);

			// Update UI
			context.SendLockStateChange();
			context.SendTitleChange(title);
			context.SendPathChange(procPath);
			context.SendDimensionsChange(context.WindowDimensions);
		}

		public override void Lock(IAppStateContext context, string path)
		{
			var nextState = new SpecificPath.UnlockedState();
			context.SetCurrentState(nextState);
			nextState.Lock(context, path);
		}
	}
}
