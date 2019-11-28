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
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
