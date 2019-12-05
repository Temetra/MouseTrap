using MouseTrap.UserInterface;
using System.ComponentModel;
using System.Windows.Input;

namespace MouseTrap.ViewModels
{
	public class FindProgram : IViewModel, INotifyPropertyChanged
	{
		private string _filename;
		private bool _isFilenameValid;

		public event PropertyChangedEventHandler PropertyChanged;

		public string Filename
		{
			get => _filename;
			set
			{
				_filename = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Filename)));
			}
		}

		public bool IsFilenameValid
		{
			get => _isFilenameValid;
			set
			{
				_isFilenameValid = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsFilenameValid)));
			}
		}

		public ICommand FindFileCommand
		{
			get;
			set;
		}
	}
}
