using MouseTrap.Data;
using MouseTrap.Hooks;
using MouseTrap.Core.AppState;
using System;
using MouseTrap.Interop;

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
				WeAreElevated = NativeMethods.IsCurrentProcessElevated(),
				PathModeLocksToHandle = false,
				ForegroundHook = fgHook,
				WindowHook = winHook,
				MouseHook = msHook,
				SetCurrentState = (next) => SetCurrentState(next),
				SendLockStateChange = () => LockStateChanged.Invoke(this, new Events.LockStateChangedEventArgs { IsLocked = IsLocked }),
				SendPathChange = (path) => PathChanged.Invoke(this, new Events.PathChangedEventArgs { Path = path }),
				SendTitleChange = (title) => TitleChanged.Invoke(this, new Events.TitleChangedEventArgs { Title = title }),
				SendDimensionsChange = (dimensions) => DimensionsChanged.Invoke(this, new Events.DimensionsChangedEventArgs { Dimensions = dimensions }),
				SendForegroundChange = (inForeground) => ForegroundChanged.Invoke(this, new Events.ForegroundStateChangedEventArgs { InForeground = inForeground }),
				SendElevationRequired = () => ElevationCheckFailed.Invoke(this, EventArgs.Empty)
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
		public event EventHandler ElevationCheckFailed;

		// Accessor for lock state
		public bool IsLocked
		{
			get => _state.IsLocked;
		}

		// Lock to a specific window
		public void Lock(IntPtr handle)
		{
			Logging.Logger.Write($"Handle {handle.ToString()}");
			_state.Lock(_stateContext, handle);
		}

		// Lock to a specific application path
		public void Lock(string path)
		{
			Logging.Logger.Write($"Path {path}");
			_state.Lock(_stateContext, path);
		}

		// Unlock system
		public void Unlock()
		{
			Logging.Logger.Write();
			_state.Unlock(_stateContext);
		}

		// Set padding
		public void SetPadding(double left, double top, double right, double bottom)
		{
			Logging.Logger.Write($"{left} {top} {right} {bottom}");
			_state.PaddingChanged(_stateContext, new Dimensions(left, top, right, bottom));
		}

		// Sets current state
		private void SetCurrentState(IAppState nextState)
		{
			Logging.Logger.Write($"From {_state.GetType().Name} to {nextState.GetType().Name}");
			_state = nextState;
		}

		// Foreground hook event handler
		private void ForegroundHook_ForegroundWindowChanged(object sender, Hooks.Events.ForegroundWindowChangedEventArgs e)
		{
			Logging.Logger.Write($"{e.ProcessPath}");
			_state.ForegroundChanged(_stateContext, e.Handle, e.WindowThreadProcId, e.ProcessPath);
		}

		// Window hook closed event handler
		private void WindowHook_WindowClosed(object sender, EventArgs e)
		{
			Logging.Logger.Write($"{_stateContext.ProcessPath}");
			_state.WindowClosed(_stateContext);
		}

		// Window hook dimensions event handler
		private void WindowHook_DimensionsChanged(object sender, Hooks.Events.DimensionsChangedEventArgs e)
		{
			Logging.Logger.Write($"{e.Dimensions}");
			_state.WindowDimensionsChanged(_stateContext, e.Dimensions);
		}

		// Window hook title change event handler
		private void WindowHook_TitleChanged(object sender, Hooks.Events.TitleChangedEventArgs e)
		{
			Logging.Logger.Write($"{e.Title}");
			_state.WindowTitleChanged(_stateContext, e.Title);
		}
	}
}
