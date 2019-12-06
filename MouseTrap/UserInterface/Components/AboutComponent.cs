using MouseTrap.ViewModels;
using MouseTrap.Views;
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
		void CloseWindow();
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
				_aboutWindow = new Views.AboutWindow
				{
					DataContext = new ViewModels.AboutWindow
					{
						Version = System.Reflection.Assembly.GetEntryAssembly().GetName().Version.Major,
						CloseWindowCommand = new Binding.RelayCommand(CloseWindow)
					}
				};

				_aboutWindow.Closed += AboutWindow_Closed;
				_aboutWindow.Show();
			}
			else
			{
				_aboutWindow.Activate();
			}
		}

		// Component interface
		public void CloseWindow()
		{
			CloseWindow(null);
		}

		// Command handler
		private void CloseWindow(object parameter)
		{
			if (_aboutWindow != null) _aboutWindow.Close();
		}

		// Event handler
		private void AboutWindow_Closed(object sender, System.EventArgs e)
		{
			_aboutWindow = null;
		}
	}
}
