namespace MouseTrap.Core;

public enum ThemeSetting
{
    Default,
    Light,
    Dark
}

public class Settings
{
    public int TitlePadding { get; set; } = 32;
    public int WindowPadding { get; set; } = 24;
    public ThemeSetting SelectedTheme { get; set; } = ThemeSetting.Default;
    public string AudioActivate { get; set; } = @"Assets\LockOn.wav";
    public string AudioDeactivate { get; set; } = @"Assets\LockOff.wav";
    public double AudioVolume { get; set; } = 0.0;
}
