using MouseTrap.Core;
using MouseTrap.Core.Events;
using MouseTrap.Data;
using MouseTrap.ViewModels;
using System;

namespace MouseTrap.UserInterface.Components
{
	public interface IMainWindowComponent : IDisposable
	{
		// Queries
		IViewModel GetViewModel();

		// Commmands
		void SetModeViewModel(IViewModel viewModel);
		void SetToolbarViewModel(IViewModel viewModel);
	}
	
	public class MainWindowComponent : IMainWindowComponent
	{
		// Fields
		private bool _isDisposed;
		private readonly IAppSystem _appSystem;
		private readonly MainWindow _viewModel;
		private bool _elevationRequired;

		// Constructor
		public MainWindowComponent(IAppSystem appSystem)
		{
			_appSystem = appSystem;
			_appSystem.ForegroundChanged += AppSystem_ForegroundChanged;
			_appSystem.ElevationCheckFailed += AppSystem_ElevationCheckFailed;
			_viewModel = new MainWindow();
		}

		// Component interface
		public IViewModel GetViewModel() => _viewModel;

		public void SetModeViewModel(IViewModel viewModel)
		{
			_elevationRequired = false;
			_viewModel.CurrentViewModel = viewModel;
		}

		public void SetToolbarViewModel(IViewModel viewModel)
		{
			_viewModel.ToolbarViewModel = viewModel;
		}

		// App system event handlers
		private void AppSystem_ForegroundChanged(object sender, ForegroundStateChangedEventArgs e)
		{
			if (_elevationRequired == false)
			{
				if (e.InForeground)
				{
					_viewModel.WindowSubtitle = "Locked";
					AudioFeedbackGainedForeground();
				}
				else
				{
					_viewModel.WindowSubtitle = "Waiting";
					AudioFeedbackLostForeground();
				}
			}
		}

		private void AppSystem_ElevationCheckFailed(object sender, EventArgs e)
		{
			_elevationRequired = true;
			_viewModel.WindowSubtitle = "Run as admin required";
		}

		private void AudioFeedbackGainedForeground() => AudioFeedback.Play(Properties.Settings.Default.AudioFeedbackGainedForeground);

		private void AudioFeedbackLostForeground() => AudioFeedback.Play(Properties.Settings.Default.AudioFeedbackLostForeground);

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
				_appSystem.ElevationCheckFailed -= AppSystem_ElevationCheckFailed;
			}

			// Free any unmanaged objects here.

			// Done
			_isDisposed = true;
		}
	}
}
