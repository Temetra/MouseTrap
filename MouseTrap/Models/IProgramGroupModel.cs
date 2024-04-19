using System.Collections.ObjectModel;

namespace MouseTrap.Models;

internal enum GroupType
{
    Pinned,
    Unpinned
}

internal interface IProgramGroupModel
{
    public GroupType Type { get; }
    public IProgramListModel Parent { get; }
    public ObservableCollection<IProgramModel> Items { get; }
    public void Insert(IProgramModel item);
    public void Remove(string key);
    public bool Contains(string key);
}
