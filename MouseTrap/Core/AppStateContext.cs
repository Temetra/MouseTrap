using MouseTrap.Data;
using MouseTrap.Hooks;
using System;

namespace MouseTrap.Core
{
	public class AppStateContext : IAppStateContext
	{
		// Context state vars
		public IntPtr Handle { get; set; }
		public uint ProcessId { get; set; }
		public string ProcessPath { get; set; }
		public Dimensions WindowDimensions { get; set; }
		public Dimensions Padding { get; set; }
		public bool PathModeLocksToHandle { get; set; }

		// Hooks
		public IForegroundWindowHook ForegroundHook { get; set; }
		public IWindowUpdateHook WindowHook { get; set; }
		public IMouseHook MouseHook { get; set; }

		// State to system methods
		public Action<IAppState> SetCurrentState { get; set; }
		public Action SendLockStateChange { get; set; }
		public Action<string> SendPathChange { get; set; }
		public Action<string> SendTitleChange { get; set; }
		public Action<Dimensions> SendDimensionsChange { get; set; }
		public Action<bool> SendForegroundChange { get; set; }
	}
}
