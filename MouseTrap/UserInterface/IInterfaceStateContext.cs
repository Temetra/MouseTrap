using MouseTrap.Data;
using MouseTrap.ViewModels;
using System;
using System.Windows;

namespace MouseTrap.UserInterface
{
	public interface IInterfaceStateContext
	{
		ViewType PreviousView { get; set; }
		IntPtr ProcessHandle { get; set; }
		string ProcessPath { get; set; }
		Dimensions Padding { get; set; }
		Window AboutWindow { get; set; }

		void SetCurrentState(IInterfaceState nextState);
		void SetAppTitlePostfix(string postfix = null);
		void SetWindowLockState(bool enabled);
		void SetViewModel(IViewModel viewModel);
		void EnumerateWindows(Action<WindowDetails> callback);
		void EnableLock();
		void DisableLock();
		void UpdatePadding();
	}
}
