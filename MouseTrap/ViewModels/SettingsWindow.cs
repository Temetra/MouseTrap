using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;

namespace MouseTrap.ViewModels
{
	public class SettingsWindow : INotifyPropertyChanged
	{
		private string _foregroundSource;
		private string _backgroundSource;

		public event PropertyChangedEventHandler PropertyChanged;

		public IList<string> SoundSources { get; set; }

		public ICommand CloseWindowCommand
		{
			get;
			set;
		}

		public string ForegroundSource
		{
			get => _foregroundSource;
			set
			{
				_foregroundSource = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ForegroundSource)));
			}
		}

		public string BackgroundSource
		{
			get => _backgroundSource;
			set
			{
				_backgroundSource = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BackgroundSource)));
			}
		}
	}
}
