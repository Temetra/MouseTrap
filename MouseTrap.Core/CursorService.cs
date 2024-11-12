using System.Runtime.Versioning;
using Temetra.Windows;

namespace MouseTrap.Core;

[SupportedOSPlatform("windows8.0")]
public class CursorService(ProgramDataModel dataModel, SettingsDataModel settingsModel)
{
    private readonly ProgramDataModel dataModel = dataModel;
    private readonly SettingsDataModel settingsModel = settingsModel;
    private ForegroundWindowHook windowHook;
    private ClipCursorHook cursorHook;
    private bool isHooking;

    public event EventHandler<ProgramItem> Updated;

    public void Start()
    {
        Log.Logger.Debug("Starting CursorService");
        cursorHook = new ClipCursorHook();
        windowHook = new ForegroundWindowHook(skipOwnProcess: true);
        windowHook.ForegroundWindowChanged += WindowHook_ForegroundWindowChanged;
        windowHook.StartHook();
    }

    public void Stop()
    {
        Log.Logger.Debug("Stopping CursorService");
        windowHook.ForegroundWindowChanged -= WindowHook_ForegroundWindowChanged;
        windowHook.Dispose();
        cursorHook.Dispose();
    }

    public void Deactivate()
    {
        // It doesn't matter if cursorHook is repeatedly stopped
        cursorHook?.StopHook();

        // But avoiding event spam when alt-tabbing is good
        if (isHooking)
        {
            Updated?.Invoke(this, null);
            Log.Logger.Information("Stop cursor hook");
        }

        isHooking = false;
    }

    private void WindowHook_ForegroundWindowChanged(object sender, ForegroundWindowChangedEventArgs e)
    {
        Log.Logger.Debug("Foreground changed {Source} {Elapsed}", e.FromTimer ? "Timer" : "Event", e.Elapsed);

        // Query app data for matching pinned entry
        var item = dataModel.GetFirstPinnedMatch(e.FileName);

        // Set hook state
        if (item != null)
        {
            ClipCursorPadding padding = new(
                settingsModel.WindowPadding,
                settingsModel.TitlePadding,
                settingsModel.WindowPadding,
                settingsModel.WindowPadding);

            cursorHook?.StartHook(e.Handle, padding);
            Updated?.Invoke(this, item);
            isHooking = true;

            Log.Logger.Information("Start cursor hook for {Title}", item.Title);
        }
        else
        {
            Deactivate();
        }
    }
}
