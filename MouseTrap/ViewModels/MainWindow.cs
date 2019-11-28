using System.ComponentModel;

namespace MouseTrap.ViewModels
{
	public class MainWindow : INotifyPropertyChanged
	{
		private AppToolbar _toolbarViewModel;
		private IViewModel _currentViewModel;
		private string _windowSubtitle;

		public event PropertyChangedEventHandler PropertyChanged;

		public AppToolbar ToolbarViewModel
		{
			get => _toolbarViewModel;
			set
			{
				_toolbarViewModel = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ToolbarViewModel)));
			}
		}

		public IViewModel CurrentViewModel
		{
			get => _currentViewModel;
			set
			{
				_currentViewModel = value;
				
				if (_toolbarViewModel != null)
				{
					_toolbarViewModel.CurrentView = _currentViewModel.ViewType;
				}

				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(CurrentViewModel)));
			}
		}

		public string WindowSubtitle
		{
			get => _windowSubtitle;
			set
			{
				_windowSubtitle = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WindowSubtitle)));
			}
		}
	}
}