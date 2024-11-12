using Microsoft.UI.Windowing;
using Microsoft.UI.Xaml;
using MouseTrap.Controls;
using MouseTrap.Core;
using Windows.UI;

namespace MouseTrap;

/// <summary>
/// An empty window that can be used on its own or navigated to within a Frame.
/// </summary>
internal sealed partial class MainWindow : Window
{
    private readonly SettingsDataModel settingsModel;
    private readonly CursorService cursorService;
    public CustomInfoBar Message => MessageBar;
    public ViewModelFrame Frame => RootFrame;

    public MainWindow(SettingsDataModel settingsModel, CursorService cursorService)
    {
        this.InitializeComponent();

        // Set fields
        this.settingsModel = settingsModel;
        this.cursorService = cursorService;

        // Listen for theme changes
        settingsModel.PropertyChanged += SettingsModel_PropertyChanged;

        // Handle events
        Activated += MainWindow_Activated;
        Closed += MainWindow_Closed;

        // Set window size
        var handle = WinRT.Interop.WindowNative.GetWindowHandle(this);
        Temetra.Windows.Tools.CenterAndResizeWindow(handle, 450, 508);

        // Set app window
        AppWindow.SetIcon(@"Assets\AppIcon.ico");
        AppWindow.Title = "MouseTrap";
        AppTitleBar.Loaded += AppTitleBar_Loaded;
        AppTitleBar.SizeChanged += AppTitleBar_SizeChanged;
        ExtendsContentIntoTitleBar = true;
        SetTitleBar(AppTitleBar);
        SetTitleBarForegroundColor();

        // Change visuals for some state changes
        MainContainer.ActualThemeChanged += MainContainer_ActualThemeChanged;
        MainContainer.RequestedTheme = (ElementTheme)(int)settingsModel.SelectedTheme;
    }

    // Incoming change from data model
    private void SettingsModel_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        DispatcherQueue.TryEnqueue(() =>
        {
            if (e.PropertyName == nameof(settingsModel.SelectedTheme))
            {
                MainContainer.RequestedTheme = (ElementTheme)(int)settingsModel.SelectedTheme;
            }
        });
    }

    // Deactivate cursor service if MouseTrap becomes active window
    // SetWinEventHook interferes with WinUI3 minimize/restore somehow
    // Using WINEVENT_SKIPOWNPROCESS fixes it, but the cursor hook
    // needs to be deactivated when switching directly back to MouseTrap
    private void MainWindow_Activated(object sender, WindowActivatedEventArgs args)
    {
        if (args.WindowActivationState != WindowActivationState.Deactivated)
        {
            cursorService.Deactivate();
        }
    }

    private void MainWindow_Closed(object sender, WindowEventArgs args)
    {
        Log.Logger.Information("Window closed");
    }

    private void MainContainer_ActualThemeChanged(FrameworkElement sender, object args)
    {
        SetTitleBarForegroundColor();
    }

    private void AppTitleBar_Loaded(object sender, RoutedEventArgs e)
    {
        SetTitleBarRegions();
    }

    private void AppTitleBar_SizeChanged(object sender, SizeChangedEventArgs e)
    {
        SetTitleBarRegions();
    }

    private void SetTitleBarForegroundColor()
    {
        if (!AppWindowTitleBar.IsCustomizationSupported()) return;

        if (MainContainer.ActualTheme == ElementTheme.Dark)
        {
            AppWindow.TitleBar.ButtonForegroundColor = Color.FromArgb(255, 255, 255, 255);
        }
        else
        {
            AppWindow.TitleBar.ButtonForegroundColor = Color.FromArgb(255, 0, 0, 0);
        }
    }

    private void SetTitleBarRegions()
    {
        if (ExtendsContentIntoTitleBar != true) return;
        double scaleAdjustment = AppTitleBar.XamlRoot.RasterizationScale;
        RightPaddingColumn.Width = new GridLength(AppWindow.TitleBar.RightInset / scaleAdjustment);
        LeftPaddingColumn.Width = new GridLength(AppWindow.TitleBar.LeftInset / scaleAdjustment);
    }
}
