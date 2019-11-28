using MouseTrap.Data;
using System;

namespace MouseTrap.Hooks.Events
{
	public class DimensionsChangedEventArgs : EventArgs
	{
		public Dimensions Dimensions { get; set; }
	}
}
