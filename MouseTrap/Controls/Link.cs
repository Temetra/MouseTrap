using System;
using System.Diagnostics;
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
			Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
			e.Handled = true;
		}
	}
}
