﻿using Microsoft.Win32;
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
		Func<ViewType> GetPreviousView { get; set; }

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
		public Func<ViewType> GetPreviousView { get; set; }
		public string GetTargetPath() => _viewModel.Filename;
		public IViewModel GetViewModel() => _viewModel;

		public void RefreshViewModel(string suggestedPath)
		{
			if (GetPreviousView() != ViewType.LockWindow || string.IsNullOrEmpty(_viewModel.Filename))
			{
				_viewModel.Filename = string.IsNullOrEmpty(suggestedPath) ? _viewModel.Filename : suggestedPath;
			}
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
			if (e.PropertyName == nameof(_viewModel.IsFilenameValid))
			{
				SetLockableState?.Invoke(_viewModel.IsFilenameValid);
			}
		}
	}
}
