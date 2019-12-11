using System.Diagnostics;
using System.Windows;
using System.Windows.Navigation;

namespace MouseTrap.Views
{
	/// <summary>
	/// Interaction logic for ErrorWindow.xaml
	/// </summary>
	public partial class ErrorWindow : Window
	{
		public ErrorWindow()
		{
			InitializeComponent();
		}

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}

		private void Hyperlink_RequestNavigate(object sender, RequestNavigateEventArgs e)
		{
			Process.Start(e.Uri.OriginalString);
		}
	}
}
