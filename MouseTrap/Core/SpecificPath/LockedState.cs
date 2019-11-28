using System;

namespace MouseTrap.Core.SpecificPath
{
	public class LockedState : BaseEnabledState, IAppState
	{
		public override void ForegroundChanged(IAppStateContext context, IntPtr handle, uint processId, string processPath)
		{
			if (context.Handle != handle ||
				context.ProcessId != processId ||
				context.ProcessPath != processPath)
			{
				// Release mouse
				context.MouseHook.UnrestrictMouse();

				// If locks to handle, retain state and wait for handle
				if (context.PathModeLocksToHandle)
				{
					// Update state
					context.SetCurrentState(new WaitingHandleState());

					// Update UI
					context.SendForegroundChange(false);
				}
				else
				{
					// Otherwise same result as closed window
					WindowClosed(context);
				}
			}
		}
	}
}
