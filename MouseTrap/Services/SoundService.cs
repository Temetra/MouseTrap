using MouseTrap.Core;
using System.IO;
using System.Media;

namespace MouseTrap.Services;

internal class SoundService(CursorService cursorService, SettingsDataModel settingsModel)
{
    private readonly CursorService cursorService = cursorService;
    private readonly SettingsDataModel settingsModel = settingsModel;
    private SoundPlayer activatedSound;
    private SoundPlayer deactivatedSound;

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
        if (settingsModel.UseAudioFeedback)
        {
            if (item != null)
            {
                activatedSound?.Play();
            }
            else
            {
                deactivatedSound?.Play();
            }
        }
    }

    private static SoundPlayer GetPlayer(string path)
    {
        if (File.Exists(path))
        {
            return new(path);
        }
        else
        {
            return null;
        }
    }
}
