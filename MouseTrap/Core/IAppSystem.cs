using MouseTrap.Core.Events;
using System;

namespace MouseTrap.Core
{
	/// <summary>
	/// Front-end to mouse-locking system
	/// </summary>
	public interface IAppSystem
	{
		// Lock to a specific window
		void Lock(IntPtr handle);

		// Lock to a specific application path
		void Lock(string path);

		// Unlock system
		void Unlock();

		// Set padding
		void SetPadding(double left, double top, double right, double bottom);

		// Accessor for lock state
		bool IsLocked { get; }

		// Event handlers for UI feedback
		event EventHandler<LockStateChangedEventArgs> LockStateChanged;
		event EventHandler<PathChangedEventArgs> PathChanged;
		event EventHandler<TitleChangedEventArgs> TitleChanged;
		event EventHandler<DimensionsChangedEventArgs> DimensionsChanged;
		event EventHandler<ForegroundStateChangedEventArgs> ForegroundChanged;
	}
}
