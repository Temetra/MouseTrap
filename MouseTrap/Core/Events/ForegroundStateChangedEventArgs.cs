using System;

namespace MouseTrap.Core.Events
{
	public class ForegroundStateChangedEventArgs : EventArgs
	{
		public bool InForeground { get; set; }
	}
}
