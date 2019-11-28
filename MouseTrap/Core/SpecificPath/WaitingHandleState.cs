using System;

namespace MouseTrap.Core.SpecificPath
{
	public class WaitingHandleState : BaseEnabledState, IAppState
	{
		public override void ForegroundChanged(IAppStateContext context, IntPtr handle, uint processId, string processPath)
		{
			if (context.Handle == handle &&
				context.ProcessId == processId &&
				context.ProcessPath == processPath)
			{
				context.MouseHook.RestrictMouseToRegion();
				context.SetCurrentState(new LockedState());
				context.SendForegroundChange(true);
			}
		}
	}
}
