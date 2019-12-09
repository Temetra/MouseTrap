using System;
using System.Diagnostics;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Navigation;

namespace MouseTrap.Controls
{
	public class Link : Hyperlink
	{
		public Link()
		{
			RequestNavigate += Link_RequestNavigate;
			ContextMenu = new ContextMenu();
			ContextMenu.Items.Add(new MenuItem
			{
				Header = "Copy",
				Command = new Binding.RelayCommand(CopyLinkToClipboard)
			});
		}

		public Uri Uri
		{
			get => NavigateUri;
			set => NavigateUri = value;
		}

		private void Link_RequestNavigate(object sender, RequestNavigateEventArgs e)
		{
			string filename;
			string arguments;

			// Using a custom scheme to open the Control Panel from a link
			// eg control:/name Microsoft.Sound /page Sounds
			if (e.Uri.Scheme == "control")
			{
				filename = e.Uri.Scheme;
				arguments = e.Uri.LocalPath;
			}
			else
			{
				filename = e.Uri.OriginalString;
				arguments = string.Empty;
			}

			// Get the OS to open the link
			Process.Start(filename, arguments);

			e.Handled = true;
		}

		private void CopyLinkToClipboard(object parameter)
		{
			Clipboard.SetText(NavigateUri.OriginalString);
		}
	}
}
