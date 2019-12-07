using System.Windows;
using System.Windows.Controls;

namespace MouseTrap.Views
{
	/// <summary>
	/// Interaction logic for SettingsWindow.xaml
	/// </summary>
	public partial class SettingsWindow : Window
	{
		public SettingsWindow()
		{
			InitializeComponent();
		}
		private void ComboBox_PreviewDragOver(object sender, DragEventArgs e)
		{
			e.Handled = true;
		}

		private void ComboBox_PreviewDrop(object sender, DragEventArgs e)
		{
			// Get text from drop
			var text = (string[])e.Data.GetData(DataFormats.FileDrop);

			// Update model
			if (text != null && sender is ComboBox box && DataContext is ViewModels.SettingsWindow model)
			{
				if (box.Name == "ForegroundBox") model.ForegroundSource = text[0];
				else if (box.Name == "BackgroundBox") model.BackgroundSource = text[0];
			}

			e.Handled = true;
		}
	}
}