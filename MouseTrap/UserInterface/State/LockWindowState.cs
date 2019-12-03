using MouseTrap.Data;
using MouseTrap.ViewModels;
using System.ComponentModel;
using System.Diagnostics;

namespace MouseTrap.UserInterface.State
{
	public class LockWindowState : BaseInterfaceState, IInterfaceState
	{
		private readonly LockWindow _model = new LockWindow();
		private PropertyChangedEventHandler _modelChangeHandler;
		private bool _isUnlocking = false;

		public override void EnterState(IInterfaceStateContext context)
		{
			// Create event handler
			_modelChangeHandler = (sender, e) => HandleModelChange(context, e);

			// Create view model
			_isUnlocking = false;
			_model.Title = "<Loading>";
			_model.ProcessPath = (context.ProcessPath != default ? context.ProcessPath : "-");
			_model.LeftOffset = context.Padding.Left;
			_model.TopOffset = context.Padding.Top;
			_model.RightOffset = context.Padding.Right;
			_model.BottomOffset = context.Padding.Bottom;
			_model.PropertyChanged += _modelChangeHandler;
			_model.ElevationRequired = false;
			_model.LockType = context.PreviousView;

			// Update context
			context.SetWindowLockState(true);
			context.SetAppTitlePostfix("Waiting");
			context.SetViewModel(_model);

			// Update system
			context.UpdatePadding();
			context.EnableLock();
		}

		public override void ExitState(IInterfaceStateContext context)
		{
			_model.PropertyChanged -= _modelChangeHandler;
			context.SetAppTitlePostfix();
		}

		public override void ToggleLock(IInterfaceStateContext context)
		{
			DisableLock(context, context.PreviousView);
		}

		public override void SwitchMode(IInterfaceStateContext context, ViewType viewType)
		{
			DisableLock(context, viewType);
		}

		private void DisableLock(IInterfaceStateContext context, ViewType viewType)
		{
			if (_isUnlocking == false)
			{
				_isUnlocking = true;
				context.DisableLock();
				if (viewType == ViewType.WindowList) context.SetCurrentState(new WindowListState());
				else context.SetCurrentState(new FindProgramState());
			}
		}

		public override void LockStateChanged(IInterfaceStateContext context, bool isLocked)
		{
			if (_isUnlocking == false && isLocked == false)
			{
				ToggleLock(context);
			}
		}

		public override void PathChanged(IInterfaceStateContext context, string path)
		{
			_model.ProcessPath = path;
		}

		public override void TitleChanged(IInterfaceStateContext context, string title)
		{
			_model.Title = title;
		}

		public override void DimensionsChanged(IInterfaceStateContext context, Dimensions dimensions)
		{
			_model.WindowHeight = dimensions.Height;
			_model.WindowWidth = dimensions.Width;
		}

		public override void ForegroundChanged(IInterfaceStateContext context, bool isInForeground)
		{
			_model.WindowIsFocused = isInForeground;
			
			if (_model.ElevationRequired == false)
			{
				if (isInForeground)
				{
					context.SetAppTitlePostfix("Locked");
					AudioFeedbackGainedForeground();
				}
				else
				{
					context.SetAppTitlePostfix("Waiting");
					AudioFeedbackLostForeground();
				}
			}
		}

		public override void ElevationRequired(IInterfaceStateContext context)
		{
			_model.ElevationRequired = true;
			context.SetAppTitlePostfix("Run as admin required");
			Logging.Logger.Write($"{System.IO.Path.GetFileName(context.ProcessPath)}");
		}

		private void HandleModelChange(IInterfaceStateContext context, PropertyChangedEventArgs e)
		{
			switch (e.PropertyName)
			{
				case nameof(_model.LeftOffset):
				case nameof(_model.TopOffset):
				case nameof(_model.RightOffset):
				case nameof(_model.BottomOffset):
					context.Padding = new Dimensions(_model.LeftOffset, _model.TopOffset, _model.RightOffset, _model.BottomOffset);
					context.UpdatePadding();
					break;
				default:
					break;
			}
		}

		[Conditional("DEBUG")]
		private void AudioFeedbackGainedForeground() => System.Media.SystemSounds.Beep.Play();

		[Conditional("DEBUG")]
		private void AudioFeedbackLostForeground() => System.Media.SystemSounds.Asterisk.Play();
	}
}
