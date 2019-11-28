using MouseTrap.Hooks.Events;
using System;

namespace MouseTrap.Hooks
{
	/// <summary>
	/// Interface for a system hook that raises an event when the foreground window changes
	/// </summary>
	public interface IForegroundWindowHook
	{
		void StartHook();
		void StopHook();
		event EventHandler<ForegroundWindowChangedEventArgs> ForegroundWindowChanged;
	}
}