using Microsoft.UI.Xaml;

namespace MouseTrap.Converters;

internal static class Binding
{
    public static string BoolToString(bool state, string left, string right) => state ? left : right;
    public static Visibility ZeroToVisibility(int state) => state == 0 ? Visibility.Visible : Visibility.Collapsed;
    public static Visibility TrueToVisibility(bool state) => state ? Visibility.Visible : Visibility.Collapsed;
    public static Visibility FalseToVisibility(bool state) => !state ? Visibility.Visible : Visibility.Collapsed;
    public static int ThemeToInt(ElementTheme theme) => (int)theme;
}
