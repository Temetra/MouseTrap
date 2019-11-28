using System;
using System.ComponentModel;
using System.Reflection;
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
			Version = Assembly.GetEntryAssembly().GetName().Version.Major;
			DataContext = this;
		}

		public int Version { get; }

		private void Button_Click(object sender, RoutedEventArgs e)
		{
			Close();
		}
	}
}
