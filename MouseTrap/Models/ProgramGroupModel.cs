using System.Collections.ObjectModel;
using System.Linq;

namespace MouseTrap.Models;

internal sealed class ProgramGroupModel(GroupType groupType, IProgramListModel parent) : IProgramGroupModel
{
    public GroupType Type { get; } = groupType;
    public IProgramListModel Parent { get; } = parent;
    public ObservableCollection<IProgramModel> Items { get; } = [];

    public void Insert(IProgramModel item)
    {
        if (!Items.Contains(item))
        {
            for (var i = 0; i < Items.Count; i++)
            {
                if (item.CompareTo(Items[i]) < 0)
                {
                    Items.Insert(i, item);
                    return;
                }
            }

            Items.Add(item);
        }
    }

    public void Remove(string key)
    {
        var item = Items.Where(x => x.Key == key).FirstOrDefault();
        if (item != null)
        {
            Items.Remove(item);
        }
    }

    public bool Contains(string key)
    {
        return Items.Where(x => x.Key == key).FirstOrDefault() != null;
    }
}
