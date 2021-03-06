﻿using MouseTrap.Data;
using MouseTrap.Core.AppState;

namespace MouseTrap.Core.SpecificWindow
{
	public class BaseEnabledState : BaseAppState, IAppState
	{
		public override void Unlock(IAppStateContext context)
		{
			// Stop hooks
			context.ForegroundHook.StopHook();
			context.WindowHook.StopHook();
			context.MouseHook.StopHook();

			// Update state
			context.Handle = default;
			context.ProcessId = default;
			context.ProcessPath = default;
			context.SetCurrentState(new UnlockedState());

			// Update UI
			context.SendLockStateChange();
		}

		public override void WindowClosed(IAppStateContext context)
		{
			Unlock(context);
		}

		public override void WindowDimensionsChanged(IAppStateContext context, Dimensions dimensions)
		{
			// Store dimensions
			context.WindowDimensions = dimensions;

			// Create padded dimensions
			var paddedDimensions = context.Padding + context.WindowDimensions;

			// Update system and UI
			context.MouseHook.SetRegion(paddedDimensions);
			context.SendDimensionsChange(context.WindowDimensions);
		}

		public override void WindowTitleChanged(IAppStateContext context, string title)
		{
			// Update UI
			context.SendTitleChange(title);
		}

		public override bool IsLocked { get => true; }
	}
}
