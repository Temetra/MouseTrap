using MouseTrap.Data;
using MouseTrap.Hooks;
using System;

namespace MouseTrap.Core
{
	/// <summary>
	/// Context for state machine and proxy to system
	/// </summary>
	public interface IAppStateContext
	{
		// Context state vars
		IntPtr Handle { get; set; }
		uint ProcessId { get; set; }
		string ProcessPath { get; set; }
		Dimensions WindowDimensions { get; set; }
		Dimensions Padding { get; set; }
		bool PathModeLocksToHandle { get; set; }

		// Hooks
		IForegroundWindowHook ForegroundHook { get; }
		IWindowUpdateHook WindowHook { get; }
		IMouseHook MouseHook { get; }

		/// <summary>
		/// Change current state of system
		/// </summary>
		Action<IAppState> SetCurrentState { get; }

		/// <summary>
		/// State of lock
		/// </summary>
		Action SendLockStateChange { get; }

		/// <summary>
		/// Path of application
		/// </summary>
		Action<string> SendPathChange { get; }

		/// <summary>
		/// Window title
		/// </summary>
		Action<string> SendTitleChange { get; }

		/// <summary>
		/// Window and padding dimensions
		/// </summary>
		Action<Dimensions> SendDimensionsChange { get; }

		/// <summary>
		/// True if window is in foreground
		/// </summary>
		Action<bool> SendForegroundChange { get; }
	}
}
