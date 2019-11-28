using MouseTrap.Data;
using System.Windows;
using System.Windows.Controls;

namespace MouseTrap.UserInterface.State
{
	public abstract class BaseInterfaceState : IInterfaceState
	{
		public virtual void EnterState(IInterfaceStateContext context) { }
		public virtual void ExitState(IInterfaceStateContext context) { }

		public virtual void SwitchMode(IInterfaceStateContext context, ViewType viewType) { }
		public virtual void ToggleLock(IInterfaceStateContext context) { }
		public virtual void RefreshRequested(IInterfaceStateContext context) { }

		public virtual void LockStateChanged(IInterfaceStateContext context, bool isLocked) { }
		public virtual void DimensionsChanged(IInterfaceStateContext context, Dimensions dimensions) { }
		public virtual void ForegroundChanged(IInterfaceStateContext context, bool isInForeground) { }
		public virtual void PathChanged(IInterfaceStateContext context, string path) { }
		public virtual void TitleChanged(IInterfaceStateContext context, string title) { }
		public virtual void ElevationRequired(IInterfaceStateContext context) { }

		public void ShowContextMenu(IInterfaceStateContext context, object sender)
		{
			var button = sender as Controls.BarButtonControl;
			ContextMenu contextMenu = button.ContextMenu;
			contextMenu.SetBinding(FrameworkElement.DataContextProperty, new System.Windows.Data.Binding { Source = button.DataContext });
			contextMenu.PlacementTarget = button;
			contextMenu.Placement = System.Windows.Controls.Primitives.PlacementMode.Left;
			contextMenu.HorizontalOffset = button.ActualWidth;
			contextMenu.VerticalOffset = button.ActualHeight;
			contextMenu.IsOpen = true;
		}

		public void ShowAboutWindow(IInterfaceStateContext context, object sender)
		{
			if (context.AboutWindow == null)
			{
				context.AboutWindow = new Views.AboutWindowControl();
				context.AboutWindow.Closed += (w, a) => context.AboutWindow = null;
				context.AboutWindow.Show();
			}
			else
			{
				context.AboutWindow.Activate();
			}
		}

		public void MainWindowClosing(IInterfaceStateContext context)
		{
			if (context.AboutWindow != null) context.AboutWindow.Close();
		}
	}
}
