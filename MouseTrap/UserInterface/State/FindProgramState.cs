using Microsoft.Win32;
using MouseTrap.Binding;
using MouseTrap.ViewModels;
using System;
using System.ComponentModel;
using System.IO;

namespace MouseTrap.UserInterface.State
{
	public class FindProgramState : BaseInterfaceState, IInterfaceState
	{
		private readonly FindProgram _model = new FindProgram();
		private PropertyChangedEventHandler _modelChangeHandler;

		public override void EnterState(IInterfaceStateContext context)
		{
			// Create event handler
			_modelChangeHandler = (sender, e) => HandleModelChange(context, e);

			// Create view model
			_model.Filename = context.ProcessPath;
			_model.IsFilenameValid = ValidateFilename(context.ProcessPath);
			_model.FindFileCommand = new RelayCommand(exe => OpenFileDialog());
			_model.PropertyChanged += _modelChangeHandler;

			// Update context
			context.SetWindowLockState(_model.IsFilenameValid);
			context.SetViewModel(_model);
		}

		public override void ExitState(IInterfaceStateContext context)
		{
			_model.PropertyChanged -= _modelChangeHandler;
		}

		public override void SwitchMode(IInterfaceStateContext context, ViewType viewType)
		{
			if (viewType == ViewType.WindowList) context.SetCurrentState(new WindowListState());
		}

		public override void ToggleLock(IInterfaceStateContext context)
		{
			context.ProcessHandle = default;
			context.ProcessPath = _model.Filename;
			context.PreviousView = ViewType.FindProgram;
			context.SetCurrentState(new LockWindowState());
		}

		private void HandleModelChange(IInterfaceStateContext context, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(_model.Filename))
			{
				_model.IsFilenameValid = ValidateFilename(_model.Filename);
				context.SetWindowLockState(_model.IsFilenameValid);
			}
		}

		private bool ValidateFilename(string filename)
		{
			return (filename != null ? CheckProcessPath(filename) : false);
		}

		private bool CheckProcessPath(string filepath)
		{
			// Basic check for string
			if (string.IsNullOrEmpty(filepath)) return false;

			// Check filename has a valid directory
			try { if (string.IsNullOrEmpty(Path.GetDirectoryName(filepath))) return false; }
			catch (Exception) { return false; }

			// Check file exists
			return File.Exists(filepath);
		}

		private void OpenFileDialog()
		{
			OpenFileDialog dialog = new OpenFileDialog
			{
				Filter = "Executable files (*.exe)|*.exe|All files (*.*)|*.*"
			};

			if (dialog.ShowDialog() == true)
			{
				_model.Filename = dialog.FileName;
			}
		}
	}
}
