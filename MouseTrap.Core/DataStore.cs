using System.ComponentModel;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Text.Json;

namespace MouseTrap.Core;

public class DataStore(ProgramDataModel dataModel, SettingsDataModel settingsModel)
{
    private readonly ProgramDataModel dataModel = dataModel;
    private readonly SettingsDataModel settingsModel = settingsModel;
    private readonly CancellationTokenSource cts = new();

    private static readonly string JsonPath = Path.Combine(GetBasePath(), "settings.json");
    private static readonly JsonSerializerOptions JsonOptions = new() { WriteIndented = true };

    public Task Start()
    {
        Log.Logger.Debug("Starting DataStore");

        // Merge different datamodel events into a single saving event

        var obs1 = Observable.FromEventPattern<DataUpdatedArgs>(
            h => dataModel.Updated += h,
            h => dataModel.Updated -= h
            )
            .Where(x => x.EventArgs.Context != nameof(ProgramDataModel.Refresh))
            .Select(x => "Items");

        var obs2 = Observable.FromEventPattern<PropertyChangedEventHandler, PropertyChangedEventArgs>(
            h => settingsModel.PropertyChanged += h,
            h => settingsModel.PropertyChanged -= h
            )
            .Select(x => "Settings");

        Observable.Merge([obs1, obs2])
            .Throttle(TimeSpan.FromMilliseconds(5000))
            .Subscribe(async x => await Save(), cts.Token);

        return Load();
    }

    public Task Stop()
    {
        Log.Logger.Debug("Stopping DataStore");
        cts.Cancel();
        return Save();
    }

    public async Task Load()
    {
        // Check file exists first
        if (File.Exists(JsonPath))
        {
            Log.Logger.Information("Loading");
            try
            {
                // Read file
                await using FileStream stream = File.OpenRead(JsonPath);
                DataStoreModel model = await JsonSerializer.DeserializeAsync<DataStoreModel>(stream);

                // Update data models
                settingsModel.Init(model.Settings);
                dataModel.Init(model.PinnedPrograms);
            }
            catch (Exception ex)
            {
                Log.Logger.Error(ex, "Exception while loading");
            }
        }
        else
        {
            Log.Logger.Information("No settings to load");
        }
    }

    private async Task Save()
    {
        Log.Logger.Information("Saving");

        try
        {
            // Create settings object
            DataStoreModel model = new()
            {
                Settings = settingsModel,
                PinnedPrograms = dataModel.PinnedPrograms,
            };

            // Save to file
            await using FileStream stream = File.Create(JsonPath);
            await JsonSerializer.SerializeAsync(stream, model, JsonOptions);
        }
        catch (Exception ex)
        {
            Log.Logger.Error(ex, "Exception while saving");
        }
    }

    private static string GetBasePath()
    {
        using var processModule = Process.GetCurrentProcess().MainModule;
        return Path.GetDirectoryName(processModule?.FileName);
    }
}
