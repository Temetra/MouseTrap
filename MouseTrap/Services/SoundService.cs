using Microsoft.UI.Dispatching;
using MouseTrap.Core;
using System.IO;
using System.Windows.Media;

namespace MouseTrap.Services;

internal class SoundService(CursorService cursorService, SettingsDataModel settingsModel, DispatcherQueue dispatcherQueue)
{
    private readonly CursorService cursorService = cursorService;
    private readonly SettingsDataModel settingsModel = settingsModel;
    private readonly DispatcherQueue dispatcherQueue = dispatcherQueue;
    private MediaPlayer activatedSound;
    private MediaPlayer deactivatedSound;

    public void Start()
    {
        cursorService.Updated += CursorService_Updated;
        activatedSound = GetPlayer(settingsModel.AudioActivate);
        deactivatedSound = GetPlayer(settingsModel.AudioDeactivate);
    }

    public void Stop()
    {
        cursorService.Updated -= CursorService_Updated;
    }

    private void CursorService_Updated(object sender, ProgramItem item)
    {
        if (settingsModel.AudioVolume > 0)
        {
            if (item != null)
            {
                Play(activatedSound, settingsModel.AudioVolume);
            }
            else
            {
                Play(deactivatedSound, settingsModel.AudioVolume);
            }
        }
    }

    private static MediaPlayer GetPlayer(string path)
    {
        if (File.Exists(path))
        {
            MediaPlayer player = new();
            player.Open(new System.Uri(Path.GetFullPath(path)));
            return player;
        }
        else
        {
            return null;
        }
    }

    private void Play(MediaPlayer player, double volume)
    {
        dispatcherQueue.TryEnqueue(() =>
        {
            if (player != null)
            {
                player.Position = System.TimeSpan.Zero;
                player.Volume = volume / 100;
                player.Play();
            }
        });
    }
}
