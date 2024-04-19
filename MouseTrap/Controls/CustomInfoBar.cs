using Microsoft.UI.Xaml.Controls;

namespace MouseTrap.Controls;

internal class CustomInfoBar : InfoBar
{
    public CustomInfoBar()
    {
        CloseButtonClick += CustomInfoBar_CloseButtonClick;
    }

    public void ShowMessage(string title, string msg, InfoBarSeverity severity)
    {
        Title = title;
        Message = msg;
        Severity = severity;
        IsOpen = true;
        Visibility = Microsoft.UI.Xaml.Visibility.Visible;
    }

    public void HideMessage()
    {
        IsOpen = false;
        Visibility = Microsoft.UI.Xaml.Visibility.Collapsed;
    }

    private void CustomInfoBar_CloseButtonClick(InfoBar sender, object args)
    {
        HideMessage();
    }
}
