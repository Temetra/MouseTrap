﻿using MouseTrap.Core.AppState;
using MouseTrap.Data;
using MouseTrap.Hooks;
using System;

namespace MouseTrap.Core
{
	public class AppSystem : IAppSystem
	{
		// App state
		private IAppState _state;
		private readonly IAppStateContext _stateContext;

		// Constructor
		public AppSystem(IForegroundWindowHook fgHook, IWindowUpdateHook winHook, IMouseHook msHook)
		{
			_state = new InitialState();

			_stateContext = new AppStateContext
			{
				PathModeLocksToHandle = false,
				ForegroundHook = fgHook,
				WindowHook = winHook,
				MouseHook = msHook,
				SetCurrentState = (next) => SetCurrentState(next),
				SendLockStateChange = () => LockStateChanged.Invoke(this, new Events.LockStateChangedEventArgs { IsLocked = IsLocked }),
				SendPathChange = (path) => PathChanged.Invoke(this, new Events.PathChangedEventArgs { Path = path }),
				SendTitleChange = (title) => TitleChanged.Invoke(this, new Events.TitleChangedEventArgs { Title = title }),
				SendDimensionsChange = (dimensions) => DimensionsChanged.Invoke(this, new Events.DimensionsChangedEventArgs { Dimensions = dimensions }),
				SendForegroundChange = (inForeground) => ForegroundChanged.Invoke(this, new Events.ForegroundStateChangedEventArgs { InForeground = inForeground })
			};

			fgHook.ForegroundWindowChanged += ForegroundHook_ForegroundWindowChanged;
			winHook.WindowClosed += WindowHook_WindowClosed;
			winHook.DimensionsChanged += WindowHook_DimensionsChanged;
			winHook.TitleChanged += WindowHook_TitleChanged;
		}

		// Event handlers for UI feedback
		public event EventHandler<Events.LockStateChangedEventArgs> LockStateChanged;
		public event EventHandler<Events.PathChangedEventArgs> PathChanged;
		public event EventHandler<Events.TitleChangedEventArgs> TitleChanged;
		public event EventHandler<Events.DimensionsChangedEventArgs> DimensionsChanged;
		public event EventHandler<Events.ForegroundStateChangedEventArgs> ForegroundChanged;

		// Accessor for lock state
		public bool IsLocked
		{
			get => _state.IsLocked;
		}

		// Lock to a specific window
		public void Lock(IntPtr handle)
		{
			_state.Lock(_stateContext, handle);
		}

		// Lock to a specific application path
		public void Lock(string path)
		{
			_state.Lock(_stateContext, path);
		}

		// Unlock system
		public void Unlock()
		{
			_state.Unlock(_stateContext);
		}

		// Set padding
		public void SetPadding(double left, double top, double right, double bottom)
		{
			_state.PaddingChanged(_stateContext, new Dimensions(left, top, right, bottom));
		}

		// Sets current state
		private void SetCurrentState(IAppState nextState)
		{
			_state = nextState;
		}

		// Foreground hook event handler
		private void ForegroundHook_ForegroundWindowChanged(object sender, Hooks.Events.ForegroundWindowChangedEventArgs e)
		{
			_state.ForegroundChanged(_stateContext, e.Handle, e.WindowThreadProcId, e.ProcessPath);
		}

		// Window hook closed event handler
		private void WindowHook_WindowClosed(object sender, EventArgs e)
		{
			_state.WindowClosed(_stateContext);
		}

		// Window hook dimensions event handler
		private void WindowHook_DimensionsChanged(object sender, Hooks.Events.DimensionsChangedEventArgs e)
		{
			_state.WindowDimensionsChanged(_stateContext, e.Dimensions);
		}

		// Window hook title change event handler
		private void WindowHook_TitleChanged(object sender, Hooks.Events.TitleChangedEventArgs e)
		{
			_state.WindowTitleChanged(_stateContext, e.Title);
		}
	}
}
