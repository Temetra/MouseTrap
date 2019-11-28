using MouseTrap.Data;
using System;

namespace MouseTrap.Core.Events
{
	public class DimensionsChangedEventArgs : EventArgs
	{
		public Dimensions Dimensions { get; set; }
	}
}
