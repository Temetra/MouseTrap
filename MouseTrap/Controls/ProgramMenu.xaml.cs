using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MouseTrap.Helpers;
using MouseTrap.Models;

namespace MouseTrap.Controls;

internal sealed partial class ProgramMenu : UserControl
{
    public ProgramMenu()
    {
        this.InitializeComponent();
    }

    public IProgramMenuModel ViewModel { get; set; }

    private void ThemeGrid_ItemClick(object sender, ItemClickEventArgs e)
    {
        if (e.ClickedItem is IThemeModel item)
        {
            SettingsFlyout.Hide();
            ViewModel.SelectedTheme = item.Theme;
        }
    }

    private void AutoSuggestBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
    {
        SettingsFlyout.Hide();
    }

    private async void AddButton_Click(object sender, RoutedEventArgs e)
    {
        var filename = await Utilities.OpenFilePicker();
        if (filename != null)
        {
            ViewModel.AddProgram(filename);
        }
    }

    private void RefreshButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.Refresh();
    }
}
