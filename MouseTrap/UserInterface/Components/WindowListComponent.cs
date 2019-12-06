using MouseTrap.Data;
using MouseTrap.ViewModels;
using System;
using System.Linq;

namespace MouseTrap.UserInterface.Components
{
	/// <summary>
	/// Responsible for the Window List view
	/// </summary>
	public interface IWindowListComponent
	{
		// System query delegates
		Action<bool> SetLockableState { get; set; }

		// Queries
		IntPtr GetTargetHandle();
		string GetTargetPath();
		IViewModel GetViewModel();

		// Commands
		void RefreshViewModel();
	}
	
	public class WindowListComponent : IWindowListComponent
	{
		// Fields
		private readonly IWindowEnumerator _windowEnumerator;
		private readonly WindowList _viewModel;

		// Constructor
		public WindowListComponent(IWindowEnumerator windowEnumerator)
		{
			_windowEnumerator = windowEnumerator;
			_viewModel = new WindowList();
			_viewModel.PropertyChanged += ViewModel_PropertyChanged;
		}

		// Component interface
		public Action<bool> SetLockableState { get; set; }

		public IntPtr GetTargetHandle() => _viewModel.SelectedWindow == null ? IntPtr.Zero : _viewModel.SelectedWindow.Handle;

		public string GetTargetPath() => _viewModel.SelectedWindow?.ProcessPath;

		public IViewModel GetViewModel() => _viewModel;

		public void RefreshViewModel()
		{
			// Store selected value
			// Context values will be cleared when WindowListItems is cleared
			IntPtr processHandle = default;
			string processPath = default;
			if (_viewModel.SelectedWindow != null)
			{
				processHandle = _viewModel.SelectedWindow.Handle;
				processPath = _viewModel.SelectedWindow.ProcessPath;
			}

			// Clear list
			_viewModel.WindowListItems.Clear();

			// Get windows
			_windowEnumerator.EnumerateWindows(details => _viewModel.WindowListItems.Add(details));

			// Reselect item
			_viewModel.SelectedWindow = _viewModel.WindowListItems.FirstOrDefault(item =>
			{
				return item.Handle == processHandle && item.ProcessPath == processPath;
			});
		}

		// Event handler
		private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(_viewModel.SelectedWindow))
			{
				SetLockableState?.Invoke(_viewModel.SelectedWindow != null);
			}
		}
	}
}
