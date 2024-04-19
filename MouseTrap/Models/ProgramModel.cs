using Microsoft.UI.Xaml.Media.Imaging;
using MouseTrap.Services;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;

namespace MouseTrap.Models;

internal sealed class ProgramModel : IProgramModel
{
    public ProgramModel(string key, string title, string path, string exe, string image)
    {
        Key = key;
        Title = title;
        ProgramPath = path;
        Executable = exe;
        Image = image;
        FullPath = Path.Combine(ProgramPath, Executable);
    }

    public string Key { get; }
    public string Title { get; }
    public string ProgramPath { get; }
    public string Executable { get; }
    public string Image { get; }
    public string FullPath { get; }
    public BitmapImage AppIcon { get; private set; }
    private bool canTrap;
    private bool isPinned;
    private bool isTrapped;

    public bool CanTrap
    {
        get => canTrap;
        set
        {
            if (canTrap != value)
            {
                canTrap = value;
                OnPropertyChanged();
            }
        }
    }

    public bool IsPinned
    {
        get => isPinned;
        set
        {
            if (isPinned != value)
            {
                isPinned = value;
                OnPropertyChanged();
            }
        }
    }

    public bool IsTrapped
    {
        get => isTrapped;
        set
        {
            if (isTrapped != value)
            {
                isTrapped = value;
                OnPropertyChanged();
            }
        }
    }

    public void LoadIcon(IconService iconService)
    {
        var newIcon = string.IsNullOrEmpty(Image)
            ? iconService.GetBitmapImage(FullPath, 48, 48)
            : iconService.GetBitmapImage(ProgramPath, Image);
        AppIcon = newIcon ?? IconService.DefaultImage;
    }

    public int CompareTo(IProgramModel other)
    {
        if (other == null) return 1;

        if (ReferenceEquals(this, other)) return 0;

        int result = Title.CompareTo(other.Title);
        if (result != 0) return result;

        result = ProgramPath.CompareTo(other.ProgramPath);
        if (result != 0) return result;

        return Executable.CompareTo(other.Executable);
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
