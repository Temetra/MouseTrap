using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace MouseTrap.Controls
{
	public class BarButtonControl : Control
	{
		static BarButtonControl()
		{
			DefaultStyleKeyProperty.OverrideMetadata(
				typeof(BarButtonControl), 
				new FrameworkPropertyMetadata(typeof(BarButtonControl))
			);
		}

		public BarButtonControl() : base()
		{
			MouseDown += BarButtonControl_MouseDown;
			MouseUp += BarButtonControl_MouseUp;
		}

		private static DependencyProperty Register<T>(string name)
		{
			return DependencyProperty.Register(name, typeof(T), typeof(BarButtonControl));
		} 

		public static readonly DependencyProperty ImageProperty = Register<ImageSource>("Image");
		public static readonly DependencyProperty ImageMarginProperty = Register<Thickness>("ImageMargin");
		public static readonly DependencyProperty TextProperty = Register<string>("Text");
		public static readonly DependencyProperty HighlightedBackgroundProperty = Register<Brush>("HighlightedBackground");
		public static readonly DependencyProperty HoverBackgroundProperty = Register<Brush>("HoverBackground");
		public static readonly DependencyProperty HoverBorderProperty = Register<Brush>("HoverBorder");
		public static readonly DependencyProperty HoverHighlightedBackgroundProperty = Register<Brush>("HoverHighlightedBackground");
		public static readonly DependencyProperty PressedForegroundProperty = Register<Brush>("PressedForeground");
		public static readonly DependencyProperty PressedBackgroundProperty = Register<Brush>("PressedBackground");
		public static readonly DependencyProperty PressedBorderProperty = Register<Brush>("PressedBorder");
		public static readonly DependencyProperty DisabledBackgroundProperty = Register<Brush>("DisabledBackground");
		public static readonly DependencyProperty DisabledBorderProperty = Register<Brush>("DisabledBorder");
		public static readonly DependencyProperty DisabledForegroundProperty = Register<Brush>("DisabledForeground");
		public static readonly DependencyProperty IsPressedProperty = Register<bool>("IsPressed");
		public static readonly DependencyProperty IsHighlightedProperty = Register<bool>("IsHighlighted");
		public static readonly DependencyProperty CommandProperty = Register<ICommand>("Command");

		public ImageSource Image
		{
			get => (ImageSource)GetValue(ImageProperty);
			set => SetValue(ImageProperty, value);
		}

		public Thickness ImageMargin
		{
			get => (Thickness)GetValue(ImageMarginProperty);
			set => SetValue(ImageMarginProperty, value);
		}

		public string Text
		{
			get => (string)GetValue(TextProperty);
			set => SetValue(TextProperty, value);
		}

		public Brush HighlightedBackground
		{
			get => (Brush)GetValue(HighlightedBackgroundProperty);
			set => SetValue(HighlightedBackgroundProperty, value);
		}

		public Brush HoverBackground
		{
			get => (Brush)GetValue(HoverBackgroundProperty);
			set => SetValue(HoverBackgroundProperty, value);
		}

		public Brush HoverBorder
		{
			get => (Brush)GetValue(HoverBorderProperty);
			set => SetValue(HoverBorderProperty, value);
		}

		public Brush HoverHighlightedBackground
		{
			get => (Brush)GetValue(HoverHighlightedBackgroundProperty);
			set => SetValue(HoverHighlightedBackgroundProperty, value);
		}

		public Brush PressedForeground
		{
			get => (Brush)GetValue(PressedForegroundProperty);
			set => SetValue(PressedForegroundProperty, value);
		}

		public Brush PressedBackground
		{
			get => (Brush)GetValue(PressedBackgroundProperty);
			set => SetValue(PressedBackgroundProperty, value);
		}

		public Brush PressedBorder
		{
			get => (Brush)GetValue(PressedBorderProperty);
			set => SetValue(PressedBorderProperty, value);
		}

		public Brush DisabledBackground
		{
			get => (Brush)GetValue(DisabledBackgroundProperty);
			set => SetValue(DisabledBackgroundProperty, value);
		}

		public Brush DisabledBorder
		{
			get => (Brush)GetValue(DisabledBorderProperty);
			set => SetValue(DisabledBorderProperty, value);
		}

		public Brush DisabledForeground
		{
			get => (Brush)GetValue(DisabledForegroundProperty);
			set => SetValue(DisabledForegroundProperty, value);
		}

		public bool IsPressed
		{
			get => (bool)GetValue(IsPressedProperty);
			set => SetValue(IsPressedProperty, value);
		}

		public bool IsHighlighted
		{
			get => (bool)GetValue(IsHighlightedProperty);
			set => SetValue(IsHighlightedProperty, value);
		}

		public ICommand Command
		{
			get => (ICommand)GetValue(CommandProperty);
			set => SetValue(CommandProperty, value);
		}

		private void BarButtonControl_MouseDown(object sender, MouseButtonEventArgs e)
		{
			// Don't capture mouse if the control is disabled
			if (!IsEnabled) return;

			// Capture the mouse to the control
			// This will allow us to tell if the user released the mouse outside the control
			UIElement el = (UIElement)sender;
			IsPressed = el.CaptureMouse();

			// Finished
			e.Handled = true;
		}

		private void BarButtonControl_MouseUp(object sender, MouseButtonEventArgs e)
		{
			// Get the capture state and release
			UIElement el = (UIElement)sender;
			el.ReleaseMouseCapture();

			// Execute the command if the mouse was pressed and is still within the control
			if (IsPressed && el.IsMouseOver)
			{
				if (Command != null && Command.CanExecute(this)) 
				{
					Command.Execute(this);
					CommandManager.InvalidateRequerySuggested();
				}
			}

			// Cancel press
			IsPressed = false;

			// Finished
			e.Handled = true;
		}
	}
}
