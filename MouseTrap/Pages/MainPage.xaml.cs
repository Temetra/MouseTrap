using Microsoft.UI.Xaml.Controls;
using MouseTrap.Models;

namespace MouseTrap.Pages;

internal sealed partial class MainPage : Page, IViewModelPage
{
    internal MainPageModel ViewModel { get; private set; }

    public MainPage()
    {
        this.InitializeComponent();
    }

    public void Load(IViewModel model)
    {
        if (ViewModel == null && model is MainPageModel pageModel)
        {
            ViewModel = pageModel;
            ViewModel.ProgramList.Refresh();
        }
    }

    public void Unload()
    {
        ViewModel?.Dispose();
        ViewModel = null;
    }
}
