using System;
using System.Collections.Generic;
using System.Windows;

namespace MouseTrap.UserInterface.Components
{
	/// <summary>
	/// Responsible for the About window
	/// </summary>
	public interface ISettingsComponent
	{
		// Commands
		void ShowWindow();
	}

	public class SettingsComponent : ISettingsComponent
	{
		// Fields
		private Window _settingsWindow;
		private ViewModels.SettingsWindow _viewModel;

		public void ShowWindow()
		{
			if (_settingsWindow == null)
			{
				// Create view model
				_viewModel = new ViewModels.SettingsWindow
				{
					SoundSources = new List<string>(Data.AudioFeedback.GetStockSounds()),
					ForegroundSource = Properties.Settings.Default.AudioFeedbackGainedForeground,
					BackgroundSource = Properties.Settings.Default.AudioFeedbackLostForeground,
					CloseWindowCommand = new Binding.RelayCommand(CloseWindow)
				};

				// Bind model events
				_viewModel.PropertyChanged += ViewModel_PropertyChanged;

				// Create window
				_settingsWindow = new Views.SettingsWindow
				{
					DataContext = _viewModel
				};

				// Center on main window
				var mainWindow = Application.Current.MainWindow;
				_settingsWindow.Top = mainWindow.Top + (mainWindow.Height - _settingsWindow.Height) / 2;
				_settingsWindow.Left = mainWindow.Left + (mainWindow.Width - _settingsWindow.Width) / 2;

				// Bind events and show window
				_settingsWindow.Closed += SettingsWindow_Closed;
				_settingsWindow.Show();
			}
			else
			{
				// Bring window to front
				_settingsWindow.Activate();
			}
		}

		private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(ViewModels.SettingsWindow.ForegroundSource))
			{
				Properties.Settings.Default.AudioFeedbackGainedForeground = _viewModel.ForegroundSource;
				Data.AudioFeedback.Play(_viewModel.ForegroundSource);
			}
			else if (e.PropertyName == nameof(ViewModels.SettingsWindow.BackgroundSource))
			{
				Properties.Settings.Default.AudioFeedbackLostForeground = _viewModel.BackgroundSource;
				Data.AudioFeedback.Play(_viewModel.BackgroundSource);
			}
		}

		// Command handler
		private void CloseWindow(object parameter)
		{
			if (_settingsWindow != null) _settingsWindow.Close();
		}

		// Event handler
		private void SettingsWindow_Closed(object sender, EventArgs e)
		{
			_settingsWindow = null;
			Properties.Settings.Default.Save();
		}
	}
}
