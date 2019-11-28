using MouseTrap.Data;
using System;

namespace MouseTrap.Core.AppState
{
	public abstract class BaseAppState : IAppState
	{
		public virtual bool IsLocked => false;

		public virtual void PaddingChanged(IAppStateContext context, Dimensions dimensions)
		{
			// Store new padding value
			context.Padding = dimensions;

			// Trigger an update using existing window dimensions
			WindowDimensionsChanged(context, context.WindowDimensions);
		}

		public virtual void Lock(IAppStateContext context, IntPtr handle) { }
		public virtual void Lock(IAppStateContext context, string path) { }
		public virtual void Unlock(IAppStateContext context) { }
		public virtual void WindowClosed(IAppStateContext context) { }
		public virtual void ForegroundChanged(IAppStateContext context, IntPtr handle, uint processId, string processPath) { }
		public virtual void WindowDimensionsChanged(IAppStateContext context, Dimensions dimensions) { }
		public virtual void WindowTitleChanged(IAppStateContext context, string title) { }
	}
}
