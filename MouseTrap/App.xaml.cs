using MouseTrap.Core;
using MouseTrap.Data;
using MouseTrap.Hooks;
using MouseTrap.UserInterface;
using MouseTrap.UserInterface.Components;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Threading;

namespace MouseTrap
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application, IDisposable
	{
		// Fields
		private bool _isDisposed;

		// System
		private IForegroundWindowHook foregroundWindowHook;
		private IWindowUpdateHook windowUpdateHook;
		private IMouseHook mouseHook;
		private IAppSystem appSystem;
		private IWindowEnumerator windowEnumerator;
		private IGuiSystem guiSystem;

		// Components
		private ILockingComponent lockingComponent;
		private IToolbarComponent toolbarComponent;
		private IWindowListComponent windowListComponent;
		private IFindProgramComponent findProgramComponent;
		private ILockWindowComponent lockWindowComponent;
		private IAboutComponent aboutComponent;
		private ISettingsComponent settingsComponent;
		private IMainWindowComponent mainWindowComponent;

		// Constructor
		public App()
		{
			Startup += App_Startup;
			Exit += App_Exit;
			DispatcherUnhandledException += App_DispatcherUnhandledException;
		}

		// Event handlers
		private void App_Startup(object sender, StartupEventArgs e)
		{
			// Create system objects
			foregroundWindowHook = new ForegroundWindowHook();
			windowUpdateHook = new WindowUpdateHook();
			mouseHook = new ClipMouseHook();
			appSystem = new AppSystem(foregroundWindowHook, windowUpdateHook, mouseHook);
			windowEnumerator = new WindowEnumerator();

			// Create components
			lockingComponent = new LockingComponent(appSystem);
			toolbarComponent = new ToolbarComponent();
			windowListComponent = new WindowListComponent(windowEnumerator);
			findProgramComponent = new FindProgramComponent();
			lockWindowComponent = new LockWindowComponent(appSystem);
			aboutComponent = new AboutComponent();
			settingsComponent = new SettingsComponent();
			mainWindowComponent = new MainWindowComponent(appSystem);

			// Create gui system
			guiSystem = new GuiSystem(lockingComponent, toolbarComponent, windowListComponent, findProgramComponent, lockWindowComponent, aboutComponent, settingsComponent, mainWindowComponent);
			guiSystem.Startup();
		}

		private void App_Exit(object sender, ExitEventArgs e)
		{
			foregroundWindowHook.StopHook();
			windowUpdateHook.StopHook();
			mouseHook.StopHook();
		}

		private void App_DispatcherUnhandledException(object sender, DispatcherUnhandledExceptionEventArgs e)
		{
			var listener = new TextWriterTraceListener("error.txt") { TraceOutputOptions = TraceOptions.Timestamp };
			Trace.AutoFlush = true;
			Trace.Listeners.Clear();
			Trace.Listeners.Add(listener);
			Trace.TraceError($"{e.Exception.Message}\n{e.Exception.StackTrace}");
			Views.ErrorWindow.ShowWindow();
		}

		// IDisposable
		~App()
		{
			Dispose(false);
		}
		
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing)
		{
			if (_isDisposed) return;

			if (disposing)
			{
				// Free any other managed objects here.
				mainWindowComponent.Dispose();
				lockWindowComponent.Dispose();
				lockingComponent.Dispose();
			}

			// Free any unmanaged objects here.
			foregroundWindowHook.Dispose();
			windowUpdateHook.Dispose();
			mouseHook.Dispose();

			// Done
			_isDisposed = true;
		}
	}
}