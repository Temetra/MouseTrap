using Microsoft.UI.Xaml.Media.Imaging;
using System;
using System.ComponentModel;

namespace MouseTrap.Models;

internal interface IProgramModel : INotifyPropertyChanged, IComparable<IProgramModel>
{
    public string Key { get; }
    public string Title { get; }
    public string ProgramPath { get; }
    public string Executable { get; }
    public string Image { get; }
    public string FullPath { get; }
    public BitmapImage AppIcon { get; }
    public bool CanTrap { get; set; }
    public bool IsPinned { get; set; }
    public bool IsTrapped { get; set; }
}
