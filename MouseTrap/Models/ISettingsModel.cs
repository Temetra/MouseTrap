using Microsoft.UI.Xaml;
using System.Collections.Generic;
using System.ComponentModel;

namespace MouseTrap.Models;

internal interface IProgramMenuModel : INotifyPropertyChanged
{
    public string Filter { get; set; }
    public int TitlePadding { get; set; }
    public int WindowPadding { get; set; }
    public ElementTheme SelectedTheme { get; set; }
    public double AudioVolume { get; set; }
    public List<IThemeModel> Themes { get; }
    public void AddProgram(string filename);
    public void Refresh();
}

internal interface IFilterPromptModel : INotifyPropertyChanged
{
    public bool HasFilter { get; }
    public string Filter { get; set; }
}

internal interface ISettingsModel : IProgramMenuModel, IFilterPromptModel
{
}
