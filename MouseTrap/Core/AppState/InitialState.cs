using System;

namespace MouseTrap.Core.AppState
{
	public class InitialState : BaseAppState, IAppState
	{
		public override void Lock(IAppStateContext context, IntPtr handle)
		{
			var nextState = new SpecificWindow.UnlockedState();
			context.SetCurrentState(nextState);
			nextState.Lock(context, handle);
		}

		public override void Lock(IAppStateContext context, string path)
		{
			var nextState = new SpecificPath.UnlockedState();
			context.SetCurrentState(nextState);
			nextState.Lock(context, path);
		}
	}
}
