using MouseTrap.ViewModels;

namespace MouseTrap.DesignTime
{
	public class MainWindow : ViewModels.MainWindow
	{
		public MainWindow()
		{
			ToolbarViewModel = new Toolbar(ViewType.WindowList);
			CurrentViewModel = new WindowList();
			WindowSubtitle = null;
		}
	}
}
