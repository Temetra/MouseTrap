using MouseTrap.Hooks.Events;
using System;

namespace MouseTrap.Hooks
{
	/// <summary>
	/// Interface for a system hook that raises an event
	/// when a specified window changes title, dimensions, or is closed
	/// </summary>
	public interface IWindowUpdateHook
	{
		void StartHook(IntPtr handle);
		void StopHook();
		event EventHandler WindowClosed;
		event EventHandler<TitleChangedEventArgs> TitleChanged;
		event EventHandler<DimensionsChangedEventArgs> DimensionsChanged;
	}
}
