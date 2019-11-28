using System;

namespace MouseTrap.Core.SpecificWindow
{
	public class LockedState : BaseEnabledState, IAppState
	{
		public override void ForegroundChanged(IAppStateContext context, IntPtr handle, uint processId, string processPath)
		{
			if (context.Handle != handle ||
				context.ProcessId != processId ||
				context.ProcessPath != processPath)
			{
				context.MouseHook.UnrestrictMouse();
				context.SetCurrentState(new WaitingState());
				context.SendForegroundChange(false);
			}
		}
	}
}
