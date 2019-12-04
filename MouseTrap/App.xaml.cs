using MouseTrap.Core;
using MouseTrap.Data;
using MouseTrap.Hooks;
using MouseTrap.UserInterface;
using System.Windows;

namespace MouseTrap
{
	/// <summary>
	/// Interaction logic for App.xaml
	/// </summary>
	public partial class App : Application
	{
		private readonly IForegroundWindowHook foregroundWindowHook;
		private readonly IWindowUpdateHook windowUpdateHook;
		private readonly IMouseHook mouseHook;
		private readonly IAppSystem appSystem;

		private readonly IWindowEnumerator windowEnumerator;
		private readonly IInterfaceSystem interfaceSystem;

		public App()
		{
			foregroundWindowHook = new ForegroundWindowHook();
			windowUpdateHook = new WindowUpdateHook();
			mouseHook = new MouseHook();
			windowEnumerator = new WindowEnumerator();

			appSystem = new AppSystem(foregroundWindowHook, windowUpdateHook, mouseHook);
			interfaceSystem = new InterfaceSystem(windowEnumerator, appSystem);

			Startup += App_Startup;
			Exit += App_Exit;
		}

		private void App_Startup(object sender, StartupEventArgs e)
		{
			interfaceSystem.Startup();
		}

		private void App_Exit(object sender, ExitEventArgs e)
		{
			Logging.Logger.Write();
			foregroundWindowHook.StopHook();
			windowUpdateHook.StopHook();
			mouseHook.StopHook();
		}
	}
}