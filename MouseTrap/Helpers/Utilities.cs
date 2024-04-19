using Microsoft.UI.Xaml.Controls;
using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using Windows.ApplicationModel.DataTransfer;
using Windows.Storage.Pickers;

namespace MouseTrap.Helpers;

internal static class Utilities
{
    public static async Task<string> OpenFilePicker()
    {
        var hWnd = WinRT.Interop.WindowNative.GetWindowHandle(App.MainWindow);
        var filePicker = new FileOpenPicker();
        WinRT.Interop.InitializeWithWindow.Initialize(filePicker, hWnd);
        filePicker.ViewMode = PickerViewMode.Thumbnail;
        filePicker.FileTypeFilter.Add(".exe");
        var file = await filePicker.PickSingleFileAsync();
        return file?.Path;
    }

    public static void CopyToClipboard(string text)
    {
        var package = new DataPackage();
        package.SetText(text);
        Clipboard.SetContent(package);
    }

    public static void ExploreFolder(string filename)
    {
        try
        {
            Process.Start("explorer.exe", Path.GetDirectoryName(filename));
        }
        catch (Win32Exception wex)
        {
            Core.Log.Logger.Error(wex, "ExploreFolder exception");
            App.MainWindow.Message.ShowMessage($"Error ({wex.NativeErrorCode})", wex.Message, InfoBarSeverity.Error);
        }
        catch (Exception ex)
        {
            Core.Log.Logger.Error(ex, "ExploreFolder exception");
            App.MainWindow.Message.ShowMessage("Error", ex.Message, InfoBarSeverity.Error);
        }
    }

    public static void RunProgram(string name, string filename)
    {
        try
        {
            ProcessStartInfo info = new()
            {
                FileName = filename,
                WorkingDirectory = Path.GetDirectoryName(filename)
            };

            Process.Start(info);
        }
        catch (Win32Exception wex)
        {
            Core.Log.Logger.Error(wex, "RunProgram exception");

            if (wex.NativeErrorCode == 740)
            {
                App.MainWindow.Message.ShowMessage($"Error", $"{name} requires elevation to run.", InfoBarSeverity.Error);
            }
            else
            {
                App.MainWindow.Message.ShowMessage($"Error ({wex.NativeErrorCode})", wex.Message, InfoBarSeverity.Error);
            }
        }
        catch (Exception ex)
        {
            Core.Log.Logger.Error(ex, "RunProgram exception");
            App.MainWindow.Message.ShowMessage("Error", ex.Message, InfoBarSeverity.Error);
        }
    }
}
