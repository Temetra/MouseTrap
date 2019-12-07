using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Documents;

namespace MouseTrap.Controls
{
	public class Link : Hyperlink
	{
		public Link()
		{
			RequestNavigate += ExtLink_RequestNavigate;
		}

		public Uri Uri
		{
			get => NavigateUri;
			set => NavigateUri = value;
		}

		private void ExtLink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
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
	}
}
