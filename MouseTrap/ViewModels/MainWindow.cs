using System.ComponentModel;

namespace MouseTrap.ViewModels
{
	public class MainWindow : INotifyPropertyChanged
	{
		private IViewModel _toolbarViewModel;
		private IViewModel _currentViewModel;
		private string _windowSubtitle;

		public event PropertyChangedEventHandler PropertyChanged;

		public IViewModel ToolbarViewModel
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