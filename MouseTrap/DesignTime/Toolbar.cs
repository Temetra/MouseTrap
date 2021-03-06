﻿using MouseTrap.Binding;
using MouseTrap.ViewModels;

namespace MouseTrap.DesignTime
{
	public class Toolbar : ViewModels.AppToolbar
	{
		public Toolbar() : this(ViewType.FindProgram)
		{
		}

		public Toolbar(ViewType viewType)
		{
			CurrentView = viewType;
			WindowLockEnabled = true;

			ChooseWindowCommand = new RelayCommand(
				exe => { },
				can => (CurrentView == ViewType.FindProgram)
			);

			FindProgramCommand = new RelayCommand(
				exe => { },
				can => (CurrentView == ViewType.WindowList)
			);

			ToggleLockCommand = new RelayCommand(
				exe => { },
				can => true
			);

			RefreshListCommand = new RelayCommand(
				exe => { },
				can => true
			);
		}
	}
}
