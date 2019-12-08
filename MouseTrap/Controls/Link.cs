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
			try
			{
				if (e.Uri.Scheme == "control")
				{
					Process.Start(e.Uri.Scheme, e.Uri.LocalPath);
				}
				else
				{
					Process.Start(e.Uri.OriginalString);
				}
			}
			catch (InvalidOperationException ex)
			{
				Logging.Logger.Write($"{ex.Message}");
			}
			catch (FileNotFoundException ex)
			{
				Logging.Logger.Write($"{ex.Message}");
			}
			catch (System.ComponentModel.Win32Exception ex)
			{
				Logging.Logger.Write($"{ex.Message}");
			}

			e.Handled = true;
		}

		private void CopyLinkToClipboard(object parameter)
		{
			Clipboard.SetText(NavigateUri.OriginalString);
		}
	}
}
