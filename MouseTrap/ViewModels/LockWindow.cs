using MouseTrap.Data;
using MouseTrap.UserInterface;
using System.ComponentModel;

namespace MouseTrap.ViewModels
{
	public class LockWindow : IViewModel, INotifyPropertyChanged
	{
		private string _title;
		private string _processPath;
		private bool _windowIsFocused;
		private double _windowHeight;
		private double _windowWidth;
		private double _leftOffset;
		private double _rightOffset;
		private double _topOffset;
		private double _bottomOffset;
		private bool _elevationRequired;
		private ViewType _lockType;

		public event PropertyChangedEventHandler PropertyChanged;

		public ViewType ViewType => ViewType.LockWindow;

		public string Title
		{
			get => _title;
			set
			{
				_title = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Title)));
			}
		}

		public string ProcessPath
		{
			get => _processPath;
			set
			{
				_processPath = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ProcessPath)));
			}
		}

		public bool WindowIsFocused
		{
			get => _windowIsFocused;
			set
			{
				_windowIsFocused = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WindowIsFocused)));
			}
		}

		public double WindowHeight
		{
			get => _windowHeight;
			set
			{
				_windowHeight = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WindowHeight)));
			}
		}

		public double WindowWidth
		{
			get => _windowWidth;
			set
			{
				_windowWidth = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(WindowWidth)));
			}
		}

		public double LeftOffset
		{
			get => _leftOffset;
			set
			{
				_leftOffset = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LeftOffset)));
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BoundaryOffset)));
			}
		}

		public double RightOffset
		{
			get => _rightOffset;
			set
			{
				_rightOffset = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(RightOffset)));
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BoundaryOffset)));
			}
		}

		public double TopOffset
		{
			get => _topOffset;
			set
			{
				_topOffset = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(TopOffset)));
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BoundaryOffset)));
			}
		}

		public double BottomOffset
		{
			get => _bottomOffset;
			set
			{
				_bottomOffset = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BottomOffset)));
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(BoundaryOffset)));
			}
		}

		public bool ElevationRequired
		{
			get => _elevationRequired;
			set
			{
				_elevationRequired = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(ElevationRequired)));
			}
		}

		public ViewType LockType
		{
			get => _lockType;
			set
			{
				_lockType = value;
				PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(LockType)));
			}
		}

		// Derived property used by marginBox
		public Dimensions BoundaryOffset => new Dimensions(_leftOffset, _topOffset, _rightOffset, _bottomOffset);
	}
}
