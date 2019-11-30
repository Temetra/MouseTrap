using System.Windows;

namespace MouseTrap.Views
{
	/// <summary>
	/// Interaction logic for AboutWindowControl.xaml
	/// </summary>
	public partial class AboutWindowControl : Window
	{
		public AboutWindowControl()
		{
			InitializeComponent();
			Closed += AboutWindowControl_Closed;
		}

		private void AboutWindowControl_Closed(object sender, System.EventArgs e)
		{
			(DataContext as ViewModels.AboutWindow)?.HasClosedCommand.Execute(sender);
		}
	}
}
