using Microsoft.UI.Xaml.Controls;
using MouseTrap.Models;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace MouseTrap.Controls;

internal sealed partial class ProgramList : UserControl
{
    public ProgramList()
    {
        this.InitializeComponent();
    }

    public IProgramListModel ViewModel { get; set; }
}
