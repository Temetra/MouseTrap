using MouseTrap.Data;
using System;

namespace MouseTrap.Core
{
	/// <summary>
	/// Events to be handled by each FSM state
	/// </summary>
	public interface IAppState
	{
		void Lock(IAppStateContext context, IntPtr handle);
		void Lock(IAppStateContext context, string path);
		void ForegroundChanged(IAppStateContext context, IntPtr handle, uint processId, string processPath);
		void WindowDimensionsChanged(IAppStateContext context, Dimensions dimensions);
		void WindowTitleChanged(IAppStateContext context, string title);
		void PaddingChanged(IAppStateContext context, Dimensions dimensions);
		void Unlock(IAppStateContext context);
		void WindowClosed(IAppStateContext context);
		bool IsLocked { get; }
	}
}
