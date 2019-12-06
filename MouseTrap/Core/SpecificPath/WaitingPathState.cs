using MouseTrap.Data;
using MouseTrap.Interop;
using System;

namespace MouseTrap.Core.SpecificPath
{
	public class WaitingPathState : BaseEnabledState, IAppState
	{
		public override void ForegroundChanged(IAppStateContext context, IntPtr handle, uint processId, string processPath)
		{
			if (context.ProcessPath == processPath)
			{
				// Get title
				string title = NativeMethods.GetWindowText(handle);

				// Get padded region
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
				context.ProcessId = processId;
				context.SetCurrentState(new LockedState());

				// Start hooks
				context.WindowHook.StartHook(handle);
				context.MouseHook.StartHook(context.Padding + context.WindowDimensions);
				context.MouseHook.RestrictMouseToRegion();

				// Update UI
				context.SendTitleChange(title);
				context.SendDimensionsChange(context.WindowDimensions);
				context.SendForegroundChange(true);
			}
		}

		// No op
		public override void WindowClosed(IAppStateContext context) { }
	}
}
