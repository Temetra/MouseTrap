using MouseTrap.ViewModels;
using System;
using System.Windows;
using System.Windows.Controls;

namespace MouseTrap.UserInterface.Components
{
	/// <summary>
	/// Handles main window toolbar interaction
	/// </summary>
	public interface IToolbarComponent
	{
		// System query delegates
		Action<ViewType> SwitchView { get; set; }
		Action RefreshWindowList { get; set; }
		Action ShowAboutWindow { get; set; }
		Action QuitProgram { get; set; }

		// Queries
		IViewModel GetViewModel();

		// Commands
		void SetModeViewModel(IViewModel viewModel);
		void WindowLockEnabled(bool state);
	}

	public class ToolbarComponent : IToolbarComponent
	{
		// Fields
		private readonly AppToolbar _viewModel;

		// Constructor
		public ToolbarComponent()
		{
			_viewModel = new AppToolbar
			{
				ChooseWindowCommand = new Binding.RelayCommand(p => SwitchView?.Invoke(ViewType.WindowList)),
				FindProgramCommand = new Binding.RelayCommand(p => SwitchView?.Invoke(ViewType.FindProgram)),
				ToggleLockCommand = new Binding.RelayCommand(p => SwitchView?.Invoke(ViewType.LockWindow)),
				RefreshListCommand = new Binding.RelayCommand(p => RefreshWindowList?.Invoke()),
				MenuAboutCommand = new Binding.RelayCommand(p => ShowAboutWindow?.Invoke()),
				MenuQuitCommand = new Binding.RelayCommand(p => QuitProgram?.Invoke()),
				ShowContextMenuCommand = new Binding.RelayCommand(ShowContextMenu)
			};
		}

		// Component interface
		public Action<ViewType> SwitchView { get; set; }
		public Action RefreshWindowList { get; set; }
		public Action ShowAboutWindow { get; set; }
		public Action QuitProgram { get; set; }

		public IViewModel GetViewModel() => _viewModel;

		public void SetModeViewModel(IViewModel viewModel)
		{
			_viewModel.CurrentView = viewModel.ViewType;
		}

		public void WindowLockEnabled(bool state)
		{
			_viewModel.WindowLockEnabled = state;
		}

		// Command handler
		private void ShowContextMenu(object parameter)
		{
			var button = parameter as Controls.BarButtonControl;
			ContextMenu contextMenu = button.ContextMenu;
			contextMenu.SetBinding(FrameworkElement.DataContextProperty, new System.Windows.Data.Binding { Source = button.DataContext });
			contextMenu.PlacementTarget = button;
			contextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Left;
			contextMenu.HorizontalOffset = button.ActualWidth;
			contextMenu.VerticalOffset = button.ActualHeight;
			contextMenu.IsOpen = true;
		}
	}
}
