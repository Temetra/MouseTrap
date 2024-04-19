using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;

namespace MouseTrap.Core;

public class DataUpdatedArgs : EventArgs
{
    public string Context = "Unknown";
}

public class ProgramDataModel : INotifyPropertyChanged
{
    private readonly Dictionary<string, ProgramItem> data = [];
    private string filter = "";

    public event EventHandler<DataUpdatedArgs> Updated;

    public string Filter
    {
        get => filter;
        set
        {
            if (filter != value)
            {
                filter = value;
                OnPropertyChanged();
            }
        }
    }

    public IEnumerable<ProgramItem> PinnedPrograms =>
        data.Values.Where(x => x.IsPinned);

    public IEnumerable<ProgramItem> UnpinnedPrograms =>
        data.Values.Where(x => !x.IsPinned);

    public IEnumerable<ProgramItem> FilteredPinnedPrograms =>
        data.Values.Where(x => x.IsPinned && ItemIncluded(x));

    public IEnumerable<ProgramItem> FilteredUnpinnedPrograms =>
        data.Values.Where(x => !x.IsPinned && ItemIncluded(x));

    // DataStore sets data without invoking ListUpdated
    internal void Init(IEnumerable<ProgramItem> items)
    {
        if (items != null)
        {
            data.Clear();
            foreach (var item in items)
            {
                item.IsPinned = true;
                data.TryAdd(item.Key, item);
            }
        }
    }

    // Supports CursorService
    internal ProgramItem GetFirstPinnedMatch(string filename)
    {
        var key = ProgramItem.GetKey(filename);

        return data.Values
            .Where(x =>
                x.IsPinned &&
                x.CanTrap &&
                x.Key.Equals(key, StringComparison.CurrentCultureIgnoreCase) &&
                ItemIncluded(x)
            )
            .FirstOrDefault();
    }


    [SupportedOSPlatform("windows8.0")]
    public void AddProgram(string filename)
    {
        // Check data is missing key
        var key = ProgramItem.GetKey(filename);
        if (!data.ContainsKey(key))
        {
            // Get program details from pinvoke etc
            var x = Temetra.Windows.ProgramDetails.GetFromFilename(filename);
            if (x != null)
            {
                // Add item to data
                ProgramItem item = new(x.Description, x.Path, x.Executable, x.Image, true, true);
                data.Add(key, item);

                // Check filter
                if (ItemIncluded(item))
                {
                    // Send update event
                    DataUpdatedArgs args = new()
                    {
                        Context = nameof(AddProgram)
                    };
                    Updated?.Invoke(this, args);
                }

                Log.Logger.Information("Added {Title}", item.Title);
                Log.Logger.Debug("{@Item}", item);
            }
            else
            {
                Log.Logger.Error("Add failed, no details for {Filename}", filename);
            }
        }
        else
        {
            Log.Logger.Error("Add failed, key for {Filename} already exists", filename);
        }
    }

    public void UpdateProgram(string key, string propertyName, object value)
    {
        // Find item in list
        if (data.TryGetValue(key, out ProgramItem item))
        {
            bool updated = false;

            // Check property exists
            if (propertyName == nameof(item.IsPinned))
            {
                // Ensure value is correct type
                if (value is bool bval)
                {
                    // Update
                    item.IsPinned = bval;

                    // If changed to true, also set CanTrap to true
                    if (bval) item.CanTrap = bval;

                    // Flag for event
                    updated = true;
                }
            }
            else if (propertyName == nameof(item.CanTrap))
            {
                if (value is bool bval)
                {
                    item.CanTrap = bval;
                    updated = true;
                }
            }

            // Send event if updated
            if (updated)
            {
                DataUpdatedArgs args = new()
                {
                    Context = nameof(UpdateProgram)
                };

                Updated?.Invoke(this, args);

                Log.Logger.Debug("Updated {@Item}", item);
            }
        }
    }

    [SupportedOSPlatform("windows8.0")]
    public void Refresh()
    {
        // Do refresh
        DoRefresh();

        // Send event
        DataUpdatedArgs args = new()
        {
            Context = nameof(Refresh)
        };

        Updated?.Invoke(this, args);

        Log.Logger.Information("Items refreshed");
    }

    [SupportedOSPlatform("windows8.0")]
    private void DoRefresh()
    {
        // Save pinned programs
        List<string> validKeys = data.Where(x => x.Value.IsPinned).Select(x => x.Key).ToList();

        // Get fresh list of available programs
        Temetra.Windows.FilteredWindowsEnumerator.EnumWindows(x =>
        {
            var key = ProgramItem.GetKey(x.Path, x.Executable);
            validKeys.Add(key);

            if (!data.ContainsKey(key))
            {
                ProgramItem item = new(x.Description, x.Path, x.Executable, x.Image, false, false);
                data.Add(key, item);
            }

            return true;
        });

        // Remove any missing from items
        foreach (var key in data.Keys.Except(validKeys))
        {
            data.Remove(key);
        }
    }

    private bool ItemIncluded(ProgramItem item)
    {
        var filter = Filter ?? "";
        return item.Title.Contains(filter, StringComparison.CurrentCultureIgnoreCase)
            || item.FullPath.Contains(filter, StringComparison.CurrentCultureIgnoreCase);
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
