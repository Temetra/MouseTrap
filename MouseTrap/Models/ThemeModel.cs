using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Media;
using System.Collections.Generic;
using Windows.Foundation;

namespace MouseTrap.Models;

internal class ThemeModel : IThemeModel
{
    public string Tag { get; set; }
    public string Name { get; set; }
    public string Tooltip { get; set; }
    public Brush Fill { get; set; }

    public ElementTheme Theme => Tag switch
    {
        "system" => ElementTheme.Default,
        "light" => ElementTheme.Light,
        "dark" => ElementTheme.Dark,
        _ => ElementTheme.Default
    };

    public static readonly SolidColorBrush LightBrush = new(Colors.White);
    public static readonly SolidColorBrush DarkBrush = new(Colors.Black);
    public static readonly LinearGradientBrush SystemBrush = new()
    {
        StartPoint = new Point(0, 0),
        EndPoint = new Point(1, 1),
        GradientStops = [
            new GradientStop { Color = Colors.White, Offset = 0.0 },
            new GradientStop { Color = Colors.White, Offset = 0.5 },
            new GradientStop { Color = Colors.Black, Offset = 0.5 },
            new GradientStop { Color = Colors.Black, Offset = 1.0 }
        ]
    };

    public static List<IThemeModel> Themes =>
    [
        new ThemeModel { Tag = "system", Tooltip = "Default", Name = "Default Theme", Fill = SystemBrush },
        new ThemeModel { Tag = "light", Tooltip = "Light", Name = "Light Theme", Fill = LightBrush },
        new ThemeModel { Tag = "dark", Tooltip = "Dark", Name = "Dark Theme", Fill = DarkBrush },
    ];
}
