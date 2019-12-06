using Microsoft.Win32;
using MouseTrap.Binding;
using MouseTrap.ViewModels;
using System;
using System.IO;

namespace MouseTrap.UserInterface.Components
{
	/// <summary>
	/// Responsible for the Find Program view
	/// </summary>
	public interface IFindProgramComponent
	{
		// System query delegates
		Action<bool> SetLockableState { get; set; }

		// Queries
		string GetTargetPath();
		IViewModel GetViewModel();

		// Commands
		void RefreshViewModel(string suggestedPath);
	}

	public class FindProgramComponent : IFindProgramComponent
	{
		// Fields
		private readonly FindProgram _viewModel;

		// Constructor
		public FindProgramComponent()
		{
			_viewModel = new FindProgram
			{
				FindFileCommand = new RelayCommand(exe => OpenFileDialog())
			};

			_viewModel.PropertyChanged += ViewModel_PropertyChanged;
		}

		// Component interface
		public Action<bool> SetLockableState { get; set; }

		public string GetTargetPath() => _viewModel.Filename;

		public IViewModel GetViewModel() => _viewModel;

		public void RefreshViewModel(string suggestedPath)
		{
			_viewModel.Filename = string.IsNullOrEmpty(suggestedPath) ? _viewModel.Filename : suggestedPath;
		}

		// Command handler
		private void OpenFileDialog()
		{
			OpenFileDialog dialog = new OpenFileDialog
			{
				Filter = "Executable files (*.exe)|*.exe|All files (*.*)|*.*"
			};

			if (dialog.ShowDialog() == true)
			{
				_viewModel.Filename = dialog.FileName;
			}
		}

		// Event handler
		private void ViewModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(_viewModel.Filename)) UpdateSystemTarget();
		}

		private void UpdateSystemTarget()
		{
			_viewModel.IsFilenameValid = CheckProcessPath(_viewModel.Filename);
			SetLockableState?.Invoke(_viewModel.IsFilenameValid);
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
	}
}
