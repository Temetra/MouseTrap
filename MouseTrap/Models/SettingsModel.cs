using Microsoft.UI.Xaml;
using MouseTrap.Core;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace MouseTrap.Models;

internal sealed class SettingsModel : ISettingsModel
{
    public SettingsModel(ProgramDataModel dataModel, SettingsDataModel settingsModel)
    {
        this.dataModel = dataModel;
        this.settingsModel = settingsModel;
        SelectedTheme = (ElementTheme)(int)settingsModel.SelectedTheme;
        TitlePadding = settingsModel.TitlePadding;
        WindowPadding = settingsModel.WindowPadding;
        UseAudioFeedback = settingsModel.UseAudioFeedback;
    }

    private readonly ProgramDataModel dataModel;
    private readonly SettingsDataModel settingsModel;
    private ElementTheme selectedTheme;
    private int titlePadding;
    private int windowPadding;
    private bool useAudioFeedback;
    private string filter;

    public void Refresh() =>
        dataModel.Refresh();

    public void AddProgram(string filename) =>
        dataModel.AddProgram(filename);

    public bool HasFilter =>
        !string.IsNullOrEmpty(Filter);

    public string Filter
    {
        get => filter;
        set
        {
            var x = value.Trim();
            if (filter != x)
            {
                // Update local store
                filter = x;
                // Send data back to model
                dataModel.Filter = value;
                // Update UI
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasFilter));
            }
        }
    }

    public int TitlePadding
    {
        get => titlePadding;
        set
        {
            if (titlePadding != value)
            {
                // Update local store
                titlePadding = value;
                // Send data back to model
                settingsModel.TitlePadding = value;
                // Update UI
                OnPropertyChanged();
            }
        }
    }

    public int WindowPadding
    {
        get => windowPadding;
        set
        {
            if (windowPadding != value)
            {
                // Update local store
                windowPadding = value;
                // Send data back to model
                settingsModel.WindowPadding = value;
                // Update UI
                OnPropertyChanged();
            }
        }
    }

    public ElementTheme SelectedTheme
    {
        get => selectedTheme;
        set
        {
            if (selectedTheme != value)
            {
                // Update local store
                selectedTheme = value;
                // Send data back to model
                settingsModel.SelectedTheme = (ThemeSetting)(int)value;
                // Update UI
                OnPropertyChanged();
            }
        }
    }

    public bool UseAudioFeedback
    {
        get => useAudioFeedback;
        set
        {
            if (useAudioFeedback != value)
            {
                // Update local store
                useAudioFeedback = value;
                // Send data back to model
                settingsModel.UseAudioFeedback = value;
                // Update UI
                OnPropertyChanged();
            }
        }
    }

    public List<IThemeModel> Themes => ThemeModel.Themes;

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
