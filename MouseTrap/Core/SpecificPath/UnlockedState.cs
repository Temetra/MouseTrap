using MouseTrap.Core.AppState;
using System;

namespace MouseTrap.Core.SpecificPath
{
	public class UnlockedState : BaseAppState, IAppState
	{
		public override void Lock(IAppStateContext context, string path)
		{
			// Basic input checking
			if (string.IsNullOrWhiteSpace(path)) return;

			// Set state
			context.Handle = default;
			context.ProcessId = default;
			context.ProcessPath = path;
			context.SetCurrentState(new WaitingPathState());

			// Start hooks
			context.ForegroundHook.StartHook();

			// Update UI
			context.SendLockStateChange();
			context.SendPathChange(path);
			context.SendTitleChange("Waiting for application");
		}

		public override void Lock(IAppStateContext context, IntPtr handle)
		{
			var nextState = new SpecificWindow.UnlockedState();
			context.SetCurrentState(nextState);
			nextState.Lock(context, handle);
		}
	}
}
