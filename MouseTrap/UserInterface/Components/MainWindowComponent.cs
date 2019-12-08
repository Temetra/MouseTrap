using MouseTrap.Core;
using MouseTrap.Core.Events;
using MouseTrap.Data;
using MouseTrap.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace MouseTrap.UserInterface.Components
{
	/// <summary>
	/// Responsible for the main window and audio feedback
	/// </summary>
	public interface IMainWindowComponent : IDisposable
	{
		// System query delegates
		Action Unlock { get; set; }

		// Commands
		void ShowWindow();
		void CloseWindow();
		void SetModeViewModel(ViewModels.IViewModel viewModel);
		void SetToolbarViewModel(ViewModels.IViewModel viewModel);
	}
	
	public class MainWindowComponent : IMainWindowComponent
	{
		// Fields
		private bool _isDisposed;
		private readonly IAppSystem _appSystem;
		private readonly ViewModels.MainWindow _viewModel;
		private Views.MainWindow _mainWindow;
		private bool _inForeground;

		// Constructor
		public MainWindowComponent(IAppSystem appSystem)
		{
			_appSystem = appSystem;
			_appSystem.ForegroundChanged += AppSystem_ForegroundChanged;
			_viewModel = new ViewModels.MainWindow();
		}

		// Component interface
		public Action Unlock { get; set; }

		public void ShowWindow()
		{
			if (_mainWindow == null)
			{
				_mainWindow = new Views.MainWindow { DataContext = _viewModel };
				_mainWindow.Closing += MainWindow_Closing;
				_mainWindow.StateChanged += MainWindow_StateChanged;
				_mainWindow.Show();
			}
		}

		public void CloseWindow()
		{
			_mainWindow?.Close();
		}

		public void SetModeViewModel(ViewModels.IViewModel viewModel)
		{
			_viewModel.CurrentViewModel = viewModel;
			UpdateWindowSubtitle();
		}

		public void SetToolbarViewModel(ViewModels.IViewModel viewModel)
		{
			_viewModel.ToolbarViewModel = viewModel;
		}

		// App system event handlers
		private void AppSystem_ForegroundChanged(object sender, ForegroundStateChangedEventArgs e)
		{
			if (_inForeground = e.InForeground)
			{
				AudioFeedback.Play(Settings.Default.AudioFeedbackGainedForeground);
			}
			else
			{
				AudioFeedback.Play(Settings.Default.AudioFeedbackLostForeground);
			}

			UpdateWindowSubtitle();
		}
		
		private void UpdateWindowSubtitle()
		{
			if (_viewModel.CurrentViewModel?.ViewType != ViewModels.ViewType.LockWindow)
			{
				_viewModel.WindowSubtitle = string.Empty;
			}
			else if (_inForeground)
			{
				_viewModel.WindowSubtitle = "Locked";
			}
			else
			{
				_viewModel.WindowSubtitle = "Waiting";
			}
		}

		// Event handlers
		private void MainWindow_Closing(object sender, CancelEventArgs e)
		{
			Application.Current.Windows
				.OfType<Window>()
				.Where(window => window != _mainWindow)
				.ForEach(window => window.Close());
			
			Unlock?.Invoke();
		}

		private void MainWindow_StateChanged(object sender, EventArgs e)
		{
			var windows = Application.Current.Windows
				.OfType<Window>()
				.Where(window => window != _mainWindow);

			if (_mainWindow.WindowState == WindowState.Minimized)
			{
				windows.ForEach(window => window.Hide());
			}
			else if (_mainWindow.WindowState == WindowState.Normal)
			{
				windows.ForEach(window => window.Show());
			}
		}

		// IDisposable
		~MainWindowComponent()
		{
			Dispose(false);
		}

		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_isDisposed) return;

			if (disposing)
			{
				// Free any other managed objects here.
				_appSystem.ForegroundChanged -= AppSystem_ForegroundChanged;
			}

			// Free any unmanaged objects here.

			// Done
			_isDisposed = true;
		}
	}
}
