using MouseTrap.UserInterface;
using System.ComponentModel;
using System.Windows.Input;

namespace MouseTrap.ViewModels
{
	public class AppToolbar : INotifyPropertyChanged
	{
		private ViewType _currentView;
		private bool _windowLockEnabled;

		public AppToolbar()
		{
			_currentView = ViewType.WindowList;
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public ViewType CurrentView
		{
			get => _currentView;
			set
			{
				_currentView = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentView)));
			}
		}

		public bool WindowLockEnabled
		{
			get => _windowLockEnabled;
			set
			{
				_windowLockEnabled = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WindowLockEnabled)));
			}
		}

		public ICommand ChooseWindowCommand { get; set; }
		public ICommand FindProgramCommand { get; set; }
		public ICommand ToggleLockCommand { get; set; }
		public ICommand RefreshListCommand { get; set; }
		public ICommand ShowContextMenuCommand { get; set; }
		public ICommand MenuAboutCommand { get; set; }
		public ICommand MenuQuitCommand { get; set; }
	}
}
