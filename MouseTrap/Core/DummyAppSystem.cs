using System;
using MouseTrap.Core.Events;

namespace MouseTrap.Core
{
	public class DummyAppSystem : IAppSystem
	{
		public bool IsLocked => false;

		public event EventHandler<LockStateChangedEventArgs> LockStateChanged;
		public event EventHandler<PathChangedEventArgs> PathChanged;
		public event EventHandler<TitleChangedEventArgs> TitleChanged;
		public event EventHandler<DimensionsChangedEventArgs> DimensionsChanged;
		public event EventHandler<ForegroundStateChangedEventArgs> ForegroundChanged;
		public event EventHandler ElevationCheckFailed;

		public void Lock(IntPtr handle)
		{
		}

		public void Lock(string path)
		{
		}

		public void SetPadding(double left, double top, double right, double bottom)
		{
		}

		public void Unlock()
		{
		}
	}
}
