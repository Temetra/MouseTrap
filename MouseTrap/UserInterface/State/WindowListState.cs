using MouseTrap.ViewModels;
using System.ComponentModel;
using System.Linq;

namespace MouseTrap.UserInterface.State
{
	public class WindowListState : BaseInterfaceState, IInterfaceState
	{
		private readonly WindowList _model = new WindowList();
		private PropertyChangedEventHandler _modelChangeHandler;

		public override void EnterState(IInterfaceStateContext context)
		{
			// Create event handler
			_modelChangeHandler = (sender, e) => HandleModelChange(context, e);

			// Create view model
			_model.PropertyChanged += _modelChangeHandler;

			// Update context
			context.SetWindowLockState(_model.SelectedWindow != null);
			context.SetViewModel(_model);

			// Refresh list
			RefreshRequested(context);
		}

		public override void ExitState(IInterfaceStateContext context)
		{
			_model.PropertyChanged -= _modelChangeHandler;
		}

		public override void SwitchMode(IInterfaceStateContext context, ViewType viewType)
		{
			if (viewType == ViewType.FindProgram) context.SetCurrentState(new FindProgramState());
		}

		public override void ToggleLock(IInterfaceStateContext context)
		{
			// Skip if no window selected
			if (_model.SelectedWindow == null) return;

			// Lock to window
			context.PreviousView = ViewType.WindowList;
			context.SetCurrentState(new LockWindowState());
		}

		public override void RefreshRequested(IInterfaceStateContext context)
		{
			// Store selected value
			// Context values will be cleared when WindowListItems is cleared
			var processHandle = context.ProcessHandle;
			var processPath = context.ProcessPath;

			// Clear list
			_model.WindowListItems.Clear();

			// Get windows
			context.EnumerateWindows(details => _model.WindowListItems.Add(details));

			// Reselect item
			_model.SelectedWindow = _model.WindowListItems.FirstOrDefault(item => {
				return item.Handle == processHandle && 
					item.ProcessPath == processPath;
			});
		}

		private void HandleModelChange(IInterfaceStateContext context, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(_model.SelectedWindow))
			{
				// Store selected window
				if (_model.SelectedWindow != null)
				{
					context.ProcessHandle = _model.SelectedWindow.Handle;
					context.ProcessPath = _model.SelectedWindow.ProcessPath;
					context.SetWindowLockState(true);
				}
				else
				{
					context.ProcessHandle = default;
					context.ProcessPath = default;
					context.SetWindowLockState(false);
				}
			}
		}
	}
}
