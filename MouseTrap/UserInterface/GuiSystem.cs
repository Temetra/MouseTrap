using MouseTrap.UserInterface.Components;
using MouseTrap.ViewModels;
using MouseTrap.Views;
using System.ComponentModel;

namespace MouseTrap.UserInterface
{
	public class GuiSystem : IGuiSystem
	{
		// Main window
		private MainWindowControl _mainWindowControl;

		// Components
		private readonly IToolbarComponent _toolbarComponent;
		private readonly IWindowListComponent _windowListComponent;
		private readonly IFindProgramComponent _findProgramComponent;
		private readonly ILockWindowComponent _lockWindowComponent;
		private readonly IAboutComponent _aboutComponent;
		private readonly IMainWindowComponent _mainWindowComponent;
		private readonly ILockingComponent _lockingComponent;

		// Constructor
		public GuiSystem(ILockingComponent lockingComponent,
			IToolbarComponent toolbarComponent,
			IWindowListComponent windowListComponent,
			IFindProgramComponent findProgramComponent,
			ILockWindowComponent lockWindowComponent,
			IAboutComponent aboutComponent,
			IMainWindowComponent mainWindowComponent)
		{
			_lockingComponent = lockingComponent;
			_toolbarComponent = toolbarComponent;
			_windowListComponent = windowListComponent;
			_findProgramComponent = findProgramComponent;
			_lockWindowComponent = lockWindowComponent;
			_aboutComponent = aboutComponent;
			_mainWindowComponent = mainWindowComponent;
		}

		// System interface
		public void Startup()
		{
			// Show window
			_mainWindowControl = new MainWindowControl();
			_mainWindowControl.Show();

			// Initialise components
			_toolbarComponent.SwitchView = _lockingComponent.SwitchView;
			_toolbarComponent.RefreshWindowList = _windowListComponent.RefreshViewModel;
			_toolbarComponent.ShowAboutWindow = _aboutComponent.ShowWindow;
			_toolbarComponent.QuitProgram = _mainWindowControl.Close;
			_windowListComponent.SetLockableState = _toolbarComponent.WindowLockEnabled;
			_findProgramComponent.SetLockableState = _toolbarComponent.WindowLockEnabled;
			_lockingComponent.GetTargetHandle = _windowListComponent.GetTargetHandle;
			_lockingComponent.GetTargetPath = _findProgramComponent.GetTargetPath;
			_lockingComponent.SetViewModel = SetViewModel;

			// Subscribe to events
			_mainWindowControl.Closing += MainWindowControl_Closing;

			// Update window
			_mainWindowComponent.SetToolbarViewModel(_toolbarComponent.GetViewModel());
			_mainWindowControl.DataContext = _mainWindowComponent.GetViewModel();

			// Switch to main state
			_lockingComponent.SwitchView(ViewType.WindowList);
		}

		private void SetViewModel(ViewType targetViewType)
		{
			IViewModel viewModel = null;

			switch (targetViewType)
			{
				case ViewType.LockWindow:
				{
					var lockType = _lockingComponent.GetLockType();
					_lockWindowComponent.RefreshViewModel(lockType);
					viewModel = _lockWindowComponent.GetViewModel();
				}
				break;
				case ViewType.WindowList:
				{
					_windowListComponent.RefreshViewModel();
					viewModel = _windowListComponent.GetViewModel();
				}
				break;
				case ViewType.FindProgram:
				{
					var previousView = _lockingComponent.GetPreviousView();
					var lastLockType = _lockingComponent.GetLockType();
					var suggestedPath = _windowListComponent.GetTargetPath();
					_findProgramComponent.RefreshViewModel(previousView, lastLockType, suggestedPath);
					viewModel = _findProgramComponent.GetViewModel();
				}
				break;
			}

			if (viewModel != null)
			{
				_mainWindowComponent.SetModeViewModel(viewModel);
				_toolbarComponent.ModeSwitched(targetViewType);
			}
		}

		// Main window events
		private void MainWindowControl_Closing(object sender, CancelEventArgs e)
		{
			_lockingComponent.Unlock();
			_aboutComponent.CloseWindow();
		}
	}
}
