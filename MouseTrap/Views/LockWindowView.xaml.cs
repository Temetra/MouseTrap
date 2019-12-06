using System.Windows.Controls;
using System.Windows.Input;

namespace MouseTrap.Views
{
	/// <summary>
	/// Interaction logic for LockWindowView.xaml
	/// </summary>
	public partial class LockWindowView : UserControl
	{
		public LockWindowView()
		{
			InitializeComponent();
		}

		private void TextBox_GotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e)
		{
			var box = sender as TextBox;
			box.SelectAll();
		}
	}
}
