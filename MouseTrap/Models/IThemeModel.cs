using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;

namespace MouseTrap.Models;

internal interface IThemeModel
{
    public string Tag { get; }
    public string Name { get; }
    public string Tooltip { get; }
    public Brush Fill { get; }
    public ElementTheme Theme { get; }
}
