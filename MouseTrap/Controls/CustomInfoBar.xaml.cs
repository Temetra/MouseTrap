using Microsoft.UI.Xaml.Controls;

namespace MouseTrap.Controls
{
    public sealed partial class CustomInfoBar : UserControl
    {
        public CustomInfoBar()
        {
            this.InitializeComponent();
            InfoBar.CloseButtonClick += InfoBar_CloseButtonClick;
        }

        public void ShowMessage(string title, string msg, InfoBarSeverity severity)
        {
            InfoBar.Title = title;
            InfoBar.Message = msg;
            InfoBar.Severity = severity;
            InfoBar.IsOpen = true;
            Visibility = Microsoft.UI.Xaml.Visibility.Visible;
        }

        public void HideMessage()
        {
            InfoBar.IsOpen = false;
            Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
        }

        private void InfoBar_CloseButtonClick(InfoBar sender, object args)
        {
            HideMessage();
        }
    }
}
