using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace MouseTrap.Models;

internal interface IProgramListModel : INotifyPropertyChanged, IDisposable
{
    public bool HasFilter { get; }
    public string Filter { get; }
    public List<IProgramGroupModel> ModelGroups { get; }
    public void Refresh();
}
