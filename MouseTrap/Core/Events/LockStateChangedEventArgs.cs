using System;

namespace MouseTrap.Core.Events
{
	public class LockStateChangedEventArgs : EventArgs
	{
		public bool IsLocked { get; set; }
	}
	
}
