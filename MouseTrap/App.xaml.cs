using Microsoft.UI.Dispatching;
using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using MouseTrap.Core;
using MouseTrap.Models;
using MouseTrap.Pages;
using MouseTrap.Services;
using System;
using System.Diagnostics;

namespace MouseTrap;

/// <summary>
/// Provides application-specific behavior to supplement the default Application class.
/// </summary>
public partial class App : Application
{
    internal static MainWindow MainWindow { get; private set; }
    private static ProgramDataModel Data;
    private static SettingsDataModel Settings;
    private static DataStore DataStore;
    private static CursorService CursorService;
    private static SoundService SoundService;
    private static IconService IconService;
    private static MainPageModelFactory MainPageModelFactory;

    /// <summary>
    /// Initializes the singleton application object.  This is the first line of authored code
    /// executed, and as such is the logical equivalent of main() or WinMain().
    /// </summary>
    public App()
    {
        Log.Logger.Information("App opened");
        UnhandledException += App_UnhandledException;
        InitializeComponent();
    }

    /// <summary>
    /// Invoked when the application is launched.
    /// </summary>
    /// <param name="args">Details about the launch request and process.</param>
    protected override async void OnLaunched(LaunchActivatedEventArgs args)
    {
        // Start
        Log.Logger.Information("App launched");
        var dispatcherQueue = DispatcherQueue.GetForCurrentThread();

        // Single instance activation
        var appArgs = Microsoft.Windows.AppLifecycle.AppInstance.GetCurrent().GetActivatedEventArgs();
        var mainInstance = Microsoft.Windows.AppLifecycle.AppInstance.FindOrRegisterForKey("MouseTrap");
        if (!mainInstance.IsCurrent)
        {
            await mainInstance.RedirectActivationToAsync(appArgs);
            Process.GetCurrentProcess().Kill();
            return;
        }
        else
        {
            Microsoft.Windows.AppLifecycle.AppInstance.GetCurrent().Activated += App_Activated;
        }

        // Create services
        Data = new();
        Settings = new();
        DataStore = new(Data, Settings);
        CursorService = new(Data, Settings);
        SoundService = new(CursorService, Settings, dispatcherQueue);
        IconService = new();

        // Create factories
        MainPageModelFactory = new(dispatcherQueue, Data, Settings, IconService, CursorService);

        // Start services
        await DataStore.Start();
        CursorService.Start();
        SoundService.Start();

        // Create main window
        MainWindow = new(Settings, CursorService);
        MainWindow.AppWindow.Closing += AppWindow_Closing;
        MainWindow.Activate();
        MainWindow.Frame.AddViewModelFactory(typeof(MainPage), MainPageModelFactory.Create);
        MainWindow.Frame.Navigate(typeof(MainPage));
    }

    private void App_Activated(object sender, Microsoft.Windows.AppLifecycle.AppActivationArguments e)
    {
        MainWindow.DispatcherQueue.TryEnqueue(() =>
        {
            var handle = WinRT.Interop.WindowNative.GetWindowHandle(MainWindow);
            Temetra.Windows.Tools.BringWindowToFront(handle);
        });
    }

    private void App_UnhandledException(object sender, Microsoft.UI.Xaml.UnhandledExceptionEventArgs e)
    {
        Log.Logger.Error(e.Exception, "Unhandled exception");
    }

    private async void AppWindow_Closing(AppWindow sender, AppWindowClosingEventArgs args)
    {
        Log.Logger.Debug("App exiting");

        // Stop window from closing
        args.Cancel = true;

        // Navigate away from page to trigger tear-down
        MainWindow.Frame.Navigate(typeof(Page));

        // Shut down services
        CursorService.Stop();
        SoundService.Stop();
        await DataStore.Stop();

        // Unsub this handler
        MainWindow.AppWindow.Closing -= AppWindow_Closing;

        // Close window for real
        MainWindow.Close();
    }
}
