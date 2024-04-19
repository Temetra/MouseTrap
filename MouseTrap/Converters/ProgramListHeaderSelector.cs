using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MouseTrap.Models;

namespace MouseTrap.Converters;

internal class ProgramListHeaderSelector : DataTemplateSelector
{
    public DataTemplate PinnedDataTemplate { get; set; }
    public DataTemplate UnpinnedDataTemplate { get; set; }

    protected override DataTemplate SelectTemplateCore(object item, DependencyObject container)
    {
        if (item is IProgramGroupModel group)
        {
            return group.Type == GroupType.Pinned
                ? PinnedDataTemplate
                : UnpinnedDataTemplate;
        }

        return base.SelectTemplateCore(item, container);
    }
}
