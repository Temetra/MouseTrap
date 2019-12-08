using System;
using System.Windows;

namespace MouseTrap.UserInterface.Components
{
	/// <summary>
	/// Responsible for the About window
	/// </summary>
	public interface IAboutComponent
	{
		// Commands
		void ShowWindow();
	}

	public class AboutComponent : IAboutComponent
	{
		// Fields
		private Window _aboutWindow;

		// Constructor
		public void ShowWindow()
		{
			if (_aboutWindow == null)
			{
				// Create window
				_aboutWindow = new Views.AboutWindow
				{
					DataContext = new ViewModels.AboutWindow
					{
						Version = System.Reflection.Assembly.GetEntryAssembly().GetName().Version.Major,
						CloseWindowCommand = new Binding.RelayCommand(CloseWindow)
					}
				};

				// Center on main window
				var mainWindow = Application.Current.MainWindow;
				_aboutWindow.Top = mainWindow.Top + (mainWindow.Height - _aboutWindow.Height) / 2;
				_aboutWindow.Left = mainWindow.Left + (mainWindow.Width - _aboutWindow.Width) / 2;

				// Bind events and show window
				_aboutWindow.Closed += AboutWindow_Closed;
				_aboutWindow.Show();
			}
			else
			{
				_aboutWindow.Activate();
			}
		}

		// Command handler
		private void CloseWindow(object parameter)
		{
			if (_aboutWindow != null) _aboutWindow.Close();
		}

		// Event handler
		private void AboutWindow_Closed(object sender, EventArgs e)
		{
			_aboutWindow = null;
		}
	}
}
