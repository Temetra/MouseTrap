using MouseTrap.Core;
using MouseTrap.Core.Events;
using MouseTrap.Data;
using MouseTrap.UserInterface.State;
using MouseTrap.ViewModels;
using MouseTrap.Views;
using System;
using System.ComponentModel;
using System.Windows;

namespace MouseTrap.UserInterface
{
	public class InterfaceSystem : IInterfaceSystem, IInterfaceStateContext
	{
		private readonly IWindowEnumerator _windowEnumerator;
		private readonly IAppSystem _appSystem;
		private MainWindowControl _mainWindowControl;
		private MainWindow _mainWindow;
		private IInterfaceState _currentState;

		public InterfaceSystem(IWindowEnumerator windowEnumerator, IAppSystem appSystem)
		{
			_windowEnumerator = windowEnumerator;

			_appSystem = appSystem;
			appSystem.LockStateChanged += AppSystem_LockStateChanged;
			appSystem.PathChanged += AppSystem_PathChanged;
			appSystem.ForegroundChanged += AppSystem_ForegroundChanged;
			appSystem.TitleChanged += AppSystem_TitleChanged;
			appSystem.DimensionsChanged += AppSystem_DimensionsChanged;
			appSystem.ElevationCheckFailed += AppSystem_ElevationCheckFailed;

			Padding = new Dimensions(10, 10, 10, 10);
		}

		public ViewType PreviousView { get; set; }
		public IntPtr ProcessHandle { get; set; }
		public string ProcessPath { get; set; }
		public Dimensions Padding { get; set; }
		public Window AboutWindow { get; set; }

		public void Startup()
		{
			// Initial state
			_currentState = new InitialState();

			// Create and show window
			_mainWindowControl = new MainWindowControl();
			_mainWindowControl.Show();

			// Main window model
			_mainWindow = new MainWindow
			{
				WindowSubtitle = "",
				ToolbarViewModel = new AppToolbar
				{
					ChooseWindowCommand = new Binding.RelayCommand(exe => _currentState.SwitchMode(this, ViewType.WindowList)),
					FindProgramCommand = new Binding.RelayCommand(exe => _currentState.SwitchMode(this, ViewType.FindProgram)),
					ToggleLockCommand = new Binding.RelayCommand(exe => _currentState.ToggleLock(this)),
					RefreshListCommand = new Binding.RelayCommand(exe => _currentState.RefreshRequested(this)),
					ShowContextMenuCommand = new Binding.RelayCommand(sender => _currentState.ShowContextMenu(this, sender)),
					MenuAboutCommand = new Binding.RelayCommand(sender => _currentState.ShowAboutWindow(this, sender)),
					MenuQuitCommand = new Binding.RelayCommand(exe => _mainWindowControl.Close())
				}
			};

			// Update window
			_mainWindowControl.DataContext = _mainWindow;
			_mainWindowControl.Closing += MainWindowControl_Closing;

			// Switch to main state
			_currentState.SwitchMode(this, ViewType.WindowList);
		}

		public void SetCurrentState(IInterfaceState nextState)
		{
			if (nextState == null) return;
			_currentState.ExitState(this);
			_currentState = nextState;
			_currentState.EnterState(this);
		}

		public void SetAppTitlePostfix(string subtitle = null)
		{
			_mainWindow.WindowSubtitle = subtitle;
		}

		public void SetWindowLockState(bool enabled)
		{
			_mainWindow.ToolbarViewModel.WindowLockEnabled = enabled;
		}

		public void SetViewModel(IViewModel viewModel)
		{
			_mainWindow.CurrentViewModel = viewModel;
		}

		public void EnumerateWindows(Action<WindowDetails> callback)
		{
			_windowEnumerator.EnumerateWindows(callback);
		}

		public void EnableLock()
		{
			if (ProcessHandle != default) _appSystem.Lock(ProcessHandle);
			else _appSystem.Lock(ProcessPath);
		}

		public void DisableLock()
		{
			_appSystem.Unlock();
		}

		public void UpdatePadding()
		{
			_appSystem.SetPadding(Padding.Left, Padding.Top, -Padding.Right, -Padding.Bottom);
		}

		private void AppSystem_LockStateChanged(object sender, LockStateChangedEventArgs e)
		{
			if (_currentState != null) _currentState.LockStateChanged(this, e.IsLocked);
		}

		private void AppSystem_PathChanged(object sender, PathChangedEventArgs e)
		{
			if (_currentState != null) _currentState.PathChanged(this, e.Path);
		}

		private void AppSystem_ForegroundChanged(object sender, ForegroundStateChangedEventArgs e)
		{
			if (_currentState != null) _currentState.ForegroundChanged(this, e.InForeground);
		}

		private void AppSystem_TitleChanged(object sender, TitleChangedEventArgs e)
		{
			if (_currentState != null) _currentState.TitleChanged(this, e.Title);
		}

		private void AppSystem_DimensionsChanged(object sender, DimensionsChangedEventArgs e)
		{
			if (_currentState != null) _currentState.DimensionsChanged(this, e.Dimensions);
		}

		private void AppSystem_ElevationCheckFailed(object sender, EventArgs e)
		{
			if (_currentState != null) _currentState.ElevationRequired(this);
		}

		private void MainWindowControl_Closing(object sender, CancelEventArgs e)
		{
			if (_currentState != null) _currentState.MainWindowClosing(this);
		}
	}
}
