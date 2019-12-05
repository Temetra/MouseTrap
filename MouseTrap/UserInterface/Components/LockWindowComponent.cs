using MouseTrap.Core;
using MouseTrap.Core.Events;
using MouseTrap.ViewModels;
using System;

namespace MouseTrap.UserInterface.Components
{
	public interface ILockWindowComponent : IDisposable
	{
		// Queries
		IViewModel GetViewModel();

		// Commmands
		void RefreshViewModel(ViewType lockType);
	}
	
	public class LockWindowComponent : ILockWindowComponent
	{
		// Fields
		private bool _isDisposed;
		private readonly IAppSystem _appSystem;
		private readonly LockWindow _viewModel;

		// Constructor
		public LockWindowComponent(IAppSystem appSystem)
		{
			_appSystem = appSystem;
			_appSystem.PathChanged += AppSystem_PathChanged;
			_appSystem.ForegroundChanged += AppSystem_ForegroundChanged;
			_appSystem.TitleChanged += AppSystem_TitleChanged;
			_appSystem.DimensionsChanged += AppSystem_DimensionsChanged;
			_appSystem.ElevationCheckFailed += AppSystem_ElevationCheckFailed;

			// Create view model
			_viewModel = new LockWindow
			{
				LeftOffset = 10,
				TopOffset = 10,
				RightOffset = 10,
				BottomOffset = 10
			};

			// Subscribe to events
			_viewModel.PropertyChanged += ViewModel_PropertyChanged;

			// Send padding values to system
			_appSystem.SetPadding(_viewModel.LeftOffset, _viewModel.TopOffset, -_viewModel.RightOffset, -_viewModel.BottomOffset);
		}

		// Component interface
		public IViewModel GetViewModel() => _viewModel;

		public void RefreshViewModel(ViewType lockType)
		{
			_viewModel.Title = "<Loading>";
			_viewModel.ProcessPath = "-";
			_viewModel.WindowHeight = 0;
			_viewModel.WindowWidth = 0;
			_viewModel.WindowIsFocused = false;
			_viewModel.ElevationRequired = false;
			_viewModel.LockType = lockType;
		}

		// Event handler
		private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case nameof(_viewModel.LeftOffset):
				case nameof(_viewModel.TopOffset):
				case nameof(_viewModel.RightOffset):
				case nameof(_viewModel.BottomOffset):
					_appSystem.SetPadding(_viewModel.LeftOffset, _viewModel.TopOffset, -_viewModel.RightOffset, -_viewModel.BottomOffset);
					break;
				default:
					break;
			}
		}

		// App system event handlers
		private void AppSystem_PathChanged(object sender, PathChangedEventArgs e)
		{
			_viewModel.ProcessPath = e.Path;
		}

		private void AppSystem_ForegroundChanged(object sender, ForegroundStateChangedEventArgs e)
		{
			_viewModel.WindowIsFocused = e.InForeground;
		}

		private void AppSystem_TitleChanged(object sender, TitleChangedEventArgs e)
		{
			_viewModel.Title = e.Title;
		}

		private void AppSystem_DimensionsChanged(object sender, DimensionsChangedEventArgs e)
		{
			_viewModel.WindowHeight = e.Dimensions.Height;
			_viewModel.WindowWidth = e.Dimensions.Width;
		}

		private void AppSystem_ElevationCheckFailed(object sender, EventArgs e)
		{
			_viewModel.ElevationRequired = true;
		}

		// IDisposable
		~LockWindowComponent()
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
				_appSystem.PathChanged -= AppSystem_PathChanged;
				_appSystem.ForegroundChanged -= AppSystem_ForegroundChanged;
				_appSystem.TitleChanged -= AppSystem_TitleChanged;
				_appSystem.DimensionsChanged -= AppSystem_DimensionsChanged;
				_appSystem.ElevationCheckFailed -= AppSystem_ElevationCheckFailed;
			}

			// Free any unmanaged objects here.

			// Done
			_isDisposed = true;
		}
	}
}
