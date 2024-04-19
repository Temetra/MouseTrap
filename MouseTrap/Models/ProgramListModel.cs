using Microsoft.UI.Dispatching;
using MouseTrap.Core;
using MouseTrap.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reactive.Linq;
using System.Runtime.CompilerServices;

namespace MouseTrap.Models;

internal sealed class ProgramListModel : IProgramListModel
{
    public ProgramListModel(DispatcherQueue dispatcherQueue, ProgramDataModel dataModel, IconService iconService, CursorService cursorService)
    {
        ProgramGroupModel pinnedGroup = new(GroupType.Pinned, this);
        ProgramGroupModel unpinnedGroup = new(GroupType.Unpinned, this);
        ModelGroups = [pinnedGroup, unpinnedGroup];

        this.dispatcherQueue = dispatcherQueue;
        this.dataModel = dataModel;
        this.iconService = iconService;
        this.cursorService = cursorService;

        dataModel.PropertyChanged += DataModel_PropertyChanged;
        dataModel.Updated += DataModel_Updated;
        cursorService.Updated += CursorService_Updated;
    }

    private readonly DispatcherQueue dispatcherQueue;
    private readonly ProgramDataModel dataModel;
    private readonly IconService iconService;
    private readonly CursorService cursorService;
    private string filter = "";
    private bool disposedValue;

    public List<IProgramGroupModel> ModelGroups { get; }

    public string Filter
    {
        get => filter;
        private set
        {
            if (filter != value)
            {
                filter = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(HasFilter));
            }
        }
    }

    public bool HasFilter => !string.IsNullOrEmpty(filter);

    public void Refresh() => dataModel.Refresh();

    private void DataModel_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        dispatcherQueue.TryEnqueue(() =>
        {
            if (sender is ProgramDataModel model)
            {
                if (e.PropertyName == nameof(model.Filter))
                {
                    Filter = model.Filter;
                    UpdateLists();
                }
            }
        });
    }

    private void DataModel_Updated(object sender, DataUpdatedArgs e)
    {
        dispatcherQueue.TryEnqueue(() =>
        {
            if (sender is ProgramDataModel model)
            {
                UpdateLists();
            }
        });
    }

    private void CursorService_Updated(object sender, ProgramItem item)
    {
        var key = item?.Key;

        dispatcherQueue.TryEnqueue(() =>
        {
            if (key != null)
            {
                foreach (var item in ModelGroups[0].Items)
                {
                    item.IsTrapped = (item.Key == key);
                }
            }
            else
            {
                foreach (var item in ModelGroups[0].Items)
                {
                    item.IsTrapped = false;
                }
            }
        });
    }

    private void UpdateLists()
    {
        RemoveMissingItems(ModelGroups[0], dataModel.FilteredPinnedPrograms);
        RemoveMissingItems(ModelGroups[1], dataModel.FilteredUnpinnedPrograms);
        AddNewItems(ModelGroups[0], dataModel.FilteredPinnedPrograms);
        AddNewItems(ModelGroups[1], dataModel.FilteredUnpinnedPrograms);
    }

    private static void RemoveMissingItems(IProgramGroupModel group, IEnumerable<ProgramItem> source)
    {
        var currentKeys = source.Select(x => x.Key).ToList();
        var missingKeys = group.Items.Select(x => x.Key).Except(currentKeys).ToList();

        foreach (var key in missingKeys)
        {
            group.Remove(key);
        }
    }

    private void AddNewItems(IProgramGroupModel group, IEnumerable<ProgramItem> source)
    {
        foreach (var data in source)
        {
            if (!group.Contains(data.Key))
            {
                ProgramModel item = new(data.Key, data.Title, data.ProgramPath, data.Executable, data.Image)
                {
                    IsPinned = data.IsPinned,
                    CanTrap = data.CanTrap
                };

                item.LoadIcon(iconService);
                item.PropertyChanged += Item_PropertyChanged;

                group.Insert(item);
            }
        }
    }

    private void Item_PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
        if (sender is ProgramModel item)
        {
            if (e.PropertyName == nameof(item.IsPinned))
            {
                dataModel.UpdateProgram(item.Key, e.PropertyName, item.IsPinned);
            }
            else if (e.PropertyName == nameof(item.CanTrap))
            {
                dataModel.UpdateProgram(item.Key, e.PropertyName, item.CanTrap);
            }
        }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    private void OnPropertyChanged([CallerMemberName] string name = "") =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

    private void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
                dataModel.PropertyChanged -= DataModel_PropertyChanged;
                dataModel.Updated -= DataModel_Updated;
                cursorService.Updated -= CursorService_Updated;
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            disposedValue = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~ProgramListModel()
    // {
    //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
    //     Dispose(disposing: false);
    // }

    public void Dispose()
    {
        // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}