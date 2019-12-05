using System.Windows.Input;

namespace MouseTrap.ViewModels
{
	public class AboutWindow
	{
		public int Version
		{
			get;
			set;
		}

		public ICommand CloseWindowCommand
		{
			get;
			set;
		}
	}
}
