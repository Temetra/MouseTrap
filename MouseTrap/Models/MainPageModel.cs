using System;

namespace MouseTrap.Models;

internal sealed class MainPageModel(IProgramListModel programListModel, ISettingsModel settingsModel) : IViewModel, IDisposable
{
    public IFilterPromptModel FilterPrompt { get; set; } = settingsModel;
    public IProgramMenuModel ProgramMenu { get; set; } = settingsModel;
    public IProgramListModel ProgramList { get; set; } = programListModel;
    private bool disposedValue;

    private void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                // TODO: dispose managed state (managed objects)
                ProgramList?.Dispose();
                ProgramList = null;
            }

            // TODO: free unmanaged resources (unmanaged objects) and override finalizer
            // TODO: set large fields to null
            disposedValue = true;
        }
    }

    // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
    // ~MainPageModel()
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
