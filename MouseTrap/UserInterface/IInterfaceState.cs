using MouseTrap.Data;

namespace MouseTrap.UserInterface
{
	public interface IInterfaceState
	{
		void EnterState(IInterfaceStateContext context);
		void ExitState(IInterfaceStateContext context);

		void SwitchMode(IInterfaceStateContext context, ViewType viewType);
		void ToggleLock(IInterfaceStateContext context);
		void RefreshRequested(IInterfaceStateContext context);

		void ShowContextMenu(IInterfaceStateContext context, object sender);
		void ShowAboutWindow(IInterfaceStateContext context, object sender);
		void MainWindowClosing(IInterfaceStateContext context);

		void LockStateChanged(IInterfaceStateContext context, bool isLocked);
		void PathChanged(IInterfaceStateContext context, string path);
		void ForegroundChanged(IInterfaceStateContext context, bool isInForeground);
		void TitleChanged(IInterfaceStateContext context, string title);
		void DimensionsChanged(IInterfaceStateContext context, Dimensions dimensions);
		void ElevationRequired(IInterfaceStateContext context);
	}
}
