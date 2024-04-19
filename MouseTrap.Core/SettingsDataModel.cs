using System.ComponentModel;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;

namespace MouseTrap.Core;

public class SettingsDataModel : Settings, INotifyPropertyChanged
{
    public SettingsDataModel()
    {
#if DEBUG
        // Throttled logging
        Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
            h => PropertyChanged += h,
            h => PropertyChanged -= h
            )
            .Throttle(TimeSpan.FromMilliseconds(1000))
            .Subscribe(x => Log.Logger.Debug("Updated {Setting}", x.EventArgs.PropertyName));
#endif
    }

    public void Init(Settings source)
    {
        if (source != null)
        {
            base.TitlePadding = source.TitlePadding;
            base.WindowPadding = source.WindowPadding;
            base.SelectedTheme = source.SelectedTheme;
            base.UseAudioFeedback = source.UseAudioFeedback;
            base.AudioActivate = source.AudioActivate;
            base.AudioDeactivate = source.AudioDeactivate;
        }
    }

    public new int TitlePadding
    {
        get => base.TitlePadding;
        set
        {
            if (base.TitlePadding != value)
            {
                base.TitlePadding = value;
                OnPropertyChanged();
            }
        }
    }

    public new int WindowPadding
    {
        get => base.WindowPadding;
        set
        {
            if (base.WindowPadding != value)
            {
                base.WindowPadding = value;
                OnPropertyChanged();
            }
        }
    }

    public new ThemeSetting SelectedTheme
    {
        get => base.SelectedTheme;
        set
        {
            if (base.SelectedTheme != value)
            {
                base.SelectedTheme = value;
                OnPropertyChanged();
            }
        }
    }

    public new bool UseAudioFeedback
    {
        get => base.UseAudioFeedback;
        set
        {
            if (base.UseAudioFeedback != value)
            {
                base.UseAudioFeedback = value;
                OnPropertyChanged();
            }
        }
    }

    public new string AudioActivate
    {
        get => base.AudioActivate;
        set
        {
            if (base.AudioActivate != value)
            {
                base.AudioActivate = value;
                OnPropertyChanged();
            }
        }
    }

    public new string AudioDeactivate
    {
        get => base.AudioDeactivate;
        set
        {
            if (base.AudioDeactivate != value)
            {
                base.AudioDeactivate = value;
                OnPropertyChanged();
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
