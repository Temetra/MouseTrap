namespace MouseTrap.DesignTime
{
	public class LockWindow : ViewModels.LockWindow
	{
		public LockWindow()
		{
			Title = "Title of window currently targetted";
			ProcessPath = @"C:\Windows\Explorer.exe";
			WindowHeight = 250;
			WindowWidth = 375;
			LeftOffset = 2;
			RightOffset = 4;
			TopOffset = 6;
			BottomOffset = 8;
			WindowIsFocused = false;
			ElevationRequired = true;
		}
	}
}
