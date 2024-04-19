using Microsoft.UI.Dispatching;
using MouseTrap.Core;
using MouseTrap.Services;

namespace MouseTrap.Models;

internal class MainPageModelFactory(DispatcherQueue dispatcherQueue, ProgramDataModel dataModel, SettingsDataModel settingsModel, IconService iconService, CursorService cursorService)
{
    private readonly DispatcherQueue dispatcherQueue = dispatcherQueue;
    private readonly ProgramDataModel dataModel = dataModel;
    private readonly SettingsDataModel settingsModel = settingsModel;
    private readonly IconService iconService = iconService;
    private readonly CursorService cursorService = cursorService;

    public MainPageModel Create()
    {
        ProgramListModel pm = new(dispatcherQueue, dataModel, iconService, cursorService);
        SettingsModel sm = new(dataModel, settingsModel);
        return new MainPageModel(pm, sm);
    }
}
