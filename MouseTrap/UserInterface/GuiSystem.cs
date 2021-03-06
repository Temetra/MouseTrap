﻿using MouseTrap.UserInterface.Components;
using MouseTrap.ViewModels;
using System;

namespace MouseTrap.UserInterface
{
	public class GuiSystem : IGuiSystem
	{
		// Delegates
		private Action<IViewModel> SetModeViewModel { get; set; }

		// Components
		private readonly IToolbarComponent _toolbarComponent;
		private readonly IWindowListComponent _windowListComponent;
		private readonly IFindProgramComponent _findProgramComponent;
		private readonly ILockWindowComponent _lockWindowComponent;
		private readonly IAboutComponent _aboutComponent;
		private readonly ISettingsComponent _settingsComponent;
		private readonly IMainWindowComponent _mainWindowComponent;
		private readonly ILockingComponent _lockingComponent;

		// Constructor
		public GuiSystem(ILockingComponent lockingComponent,
			IToolbarComponent toolbarComponent,
			IWindowListComponent windowListComponent,
			IFindProgramComponent findProgramComponent,
			ILockWindowComponent lockWindowComponent,
			IAboutComponent aboutComponent,
			ISettingsComponent settingsComponent,
			IMainWindowComponent mainWindowComponent)
		{
			_lockingComponent = lockingComponent;
			_toolbarComponent = toolbarComponent;
			_windowListComponent = windowListComponent;
			_findProgramComponent = findProgramComponent;
			_lockWindowComponent = lockWindowComponent;
			_aboutComponent = aboutComponent;
			_settingsComponent = settingsComponent;
			_mainWindowComponent = mainWindowComponent;
		}

		// System interface
		public void Startup()
		{
			// Show window
			_mainWindowComponent.ShowWindow();

			// Initialise components
			_mainWindowComponent.Unlock = _lockingComponent.Unlock;
			_toolbarComponent.SwitchView = _lockingComponent.SwitchView;
			_toolbarComponent.RefreshWindowList = _windowListComponent.RefreshViewModel;
			_toolbarComponent.ShowAboutWindow = _aboutComponent.ShowWindow;
			_toolbarComponent.ShowSettingsWindow = _settingsComponent.ShowWindow;
			_toolbarComponent.QuitProgram = _mainWindowComponent.CloseWindow;
			_windowListComponent.SetLockableState = _toolbarComponent.WindowLockEnabled;
			_findProgramComponent.SetLockableState = _toolbarComponent.WindowLockEnabled;
			_findProgramComponent.GetPreviousView = _lockingComponent.GetPreviousView;
			_lockingComponent.GetTargetHandle = _windowListComponent.GetTargetHandle;
			_lockingComponent.GetTargetPath = _findProgramComponent.GetTargetPath;
			_lockingComponent.SetViewModel = SetViewModel;

			// Configure delegates
			SetModeViewModel += _mainWindowComponent.SetModeViewModel;
			SetModeViewModel += _toolbarComponent.SetModeViewModel;

			// Update window
			_mainWindowComponent.SetToolbarViewModel(_toolbarComponent.GetViewModel());

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
					var suggestedPath = _windowListComponent.GetTargetPath();
					_findProgramComponent.RefreshViewModel(suggestedPath);
					viewModel = _findProgramComponent.GetViewModel();
				}
				break;
			}

			if (viewModel != null)
			{
				SetModeViewModel?.Invoke(viewModel);
			}
		}
	}
}
