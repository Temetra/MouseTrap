using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MouseTrap.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MouseTrap.Controls;

internal sealed partial class FilterPrompt : UserControl
{
    public FilterPrompt()
    {
        this.InitializeComponent();
    }

    public IFilterPromptModel ViewModel { get; set; }

    private void ClearFilterButton_Click(object sender, RoutedEventArgs e)
    {
        ViewModel.Filter = string.Empty;
    }
}
