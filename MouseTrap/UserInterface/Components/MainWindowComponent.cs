using MouseTrap.Core;
using MouseTrap.Core.Events;
using MouseTrap.Data;
using MouseTrap.ViewModels;
using MouseTrap.Properties;
using System;

namespace MouseTrap.UserInterface.Components
{
	/// <summary>
	/// Responsible for the main window and audio feedback
	/// </summary>
	public interface IMainWindowComponent : IDisposable
	{
		// Queries
		MainWindow GetViewModel();

		// Commands
		void SetModeViewModel(IViewModel viewModel);
		void SetToolbarViewModel(IViewModel viewModel);
	}
	
	public class MainWindowComponent : IMainWindowComponent
	{
		// Fields
		private bool _isDisposed;
		private readonly IAppSystem _appSystem;
		private readonly MainWindow _viewModel;
		private bool _inForeground;

		// Constructor
		public MainWindowComponent(IAppSystem appSystem)
		{
			_appSystem = appSystem;
			_appSystem.ForegroundChanged += AppSystem_ForegroundChanged;
			_viewModel = new MainWindow();
		}

		// Component interface
		public MainWindow GetViewModel() => _viewModel;

		public void SetModeViewModel(IViewModel viewModel)
		{
			_viewModel.CurrentViewModel = viewModel;
			UpdateWindowSubtitle();
		}

		public void SetToolbarViewModel(IViewModel viewModel)
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
			if (_viewModel.CurrentViewModel?.ViewType != ViewType.LockWindow)
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
