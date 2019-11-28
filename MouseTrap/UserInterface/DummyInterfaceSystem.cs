using MouseTrap.Binding;
using MouseTrap.Core;
using MouseTrap.Data;
using MouseTrap.ViewModels;
using MouseTrap.Views;

namespace MouseTrap.UserInterface
{
	public class DummyInterfaceSystem : IInterfaceSystem
	{
		private readonly IWindowEnumerator _windowEnumerator;
		private readonly IAppSystem _appSystem;
		private MainWindowControl _mainWindowControl;
		private MainWindow _mainWindow;
		private ViewType _currentView = ViewType.WindowList;
		private ViewType _previousView = ViewType.WindowList;
		private bool _lockEnabled = true;

		private readonly WindowList windowList = new WindowList();
		private readonly FindProgram findProgram = new FindProgram();
		private readonly LockWindow lockWindow = new LockWindow();

		public DummyInterfaceSystem(IWindowEnumerator windowEnumerator, IAppSystem appSystem)
		{
			_windowEnumerator = windowEnumerator;
			_appSystem = appSystem;
		}

		public void Startup()
		{
			// Create and show window
			_mainWindowControl = new MainWindowControl();
			_mainWindowControl.Show();

			// Main window model
			_mainWindow = new MainWindow
			{
				ToolbarViewModel = new AppToolbar
				{
					ChooseWindowCommand = new RelayCommand(
						exe => SwitchMode(ViewType.WindowList),
						can => _currentView != ViewType.WindowList
						),
					FindProgramCommand = new RelayCommand(
						exe => SwitchMode(ViewType.FindProgram),
						can => _currentView != ViewType.FindProgram
						),
					ToggleLockCommand = new RelayCommand(
						exe => ToggleLock(),
						can => _lockEnabled
						),
					RefreshListCommand = new RelayCommand(
						exe => { },
						can => true
						)
				}
			};

			// Update window
			_mainWindowControl.DataContext = _mainWindow;
			ShowWindowList();
		}

		private void ShowWindowList()
		{
			windowList.WindowListItems.Clear();
			_windowEnumerator.EnumerateWindows(details => windowList.WindowListItems.Add(details));
			windowList.SelectedWindow = windowList.WindowListItems[0];
			_currentView = ViewType.WindowList;
			_mainWindow.CurrentViewModel = windowList;
		}

		private void ShowFindProgram()
		{
			_currentView = ViewType.FindProgram;
			_mainWindow.CurrentViewModel = findProgram;
		}

		private void ShowLockWindow()
		{
			_currentView = ViewType.LockWindow;
			_mainWindow.CurrentViewModel = lockWindow;
		}

		private void SwitchMode(ViewType viewType)
		{
			if (viewType == ViewType.WindowList)
			{
				ShowWindowList();
			}
			else if (viewType == ViewType.FindProgram)
			{
				ShowFindProgram();
			}
		}

		private void ToggleLock()
		{
			if (_currentView != ViewType.LockWindow)
			{
				_previousView = _currentView;
				ShowLockWindow();
			}
			else if (_previousView == ViewType.WindowList)
			{
				ShowWindowList();
			}
			else
			{
				ShowFindProgram();
			}
		}
	}
}
