using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using System;

namespace MouseTrap.Controls;

public sealed partial class SpinnyButton : UserControl
{
    public SpinnyButton()
    {
        this.InitializeComponent();
    }

    public event RoutedEventHandler Click;

    private void HandleButtonClick(RoutedEventArgs e)
    {
        var time = SpinnyAnim.GetCurrentTime();
        if (time == TimeSpan.Zero || time == Duration.TimeSpan)
        {
            SpinnyAnim.Begin();
        }
        Click?.Invoke(this, e);
    }

    private void Button_Click(object sender, RoutedEventArgs e) => HandleButtonClick(e);

    protected override void OnKeyboardAcceleratorInvoked(KeyboardAcceleratorInvokedEventArgs args)
    {
        base.OnKeyboardAcceleratorInvoked(args);
        HandleButtonClick(new RoutedEventArgs());
    }

    protected override void OnPointerEntered(PointerRoutedEventArgs e)
    {
        base.OnPointerEntered(e);
        VisualStateManager.GoToState(this, nameof(PointerOver), true);
    }

    protected override void OnPointerExited(PointerRoutedEventArgs e)
    {
        base.OnPointerExited(e);
        VisualStateManager.GoToState(this, nameof(Normal), true);
    }

    public double WindAngle
    {
        get => (double)GetValue(WindAngleProperty);
        set => SetValue(WindAngleProperty, value);
    }

    public double SpinAngle
    {
        get => (double)GetValue(SpinAngleProperty);
        set => SetValue(SpinAngleProperty, value);
    }

    public Duration Duration
    {
        get => (Duration)GetValue(DurationProperty);
        set => SetValue(DurationProperty, value);
    }

    public string Glyph
    {
        get => (string)GetValue(GlyphProperty);
        set => SetValue(GlyphProperty, value);
    }

    public FlyoutBase Flyout
    {
        get => (FlyoutBase)GetValue(FlyoutProperty);
        set => SetValue(FlyoutProperty, value);
    }

    public static readonly DependencyProperty WindAngleProperty =
        DependencyProperty.Register(nameof(WindAngle), typeof(double), typeof(SpinnyButton), new PropertyMetadata(0d));

    public static readonly DependencyProperty SpinAngleProperty =
        DependencyProperty.Register(nameof(SpinAngle), typeof(double), typeof(SpinnyButton), new PropertyMetadata(360d));

    public static readonly DependencyProperty DurationProperty =
        DependencyProperty.Register(nameof(Duration), typeof(Duration), typeof(SpinnyButton), new PropertyMetadata(default(Duration)));

    public static readonly DependencyProperty GlyphProperty =
        DependencyProperty.Register(nameof(Glyph), typeof(string), typeof(SpinnyButton), new PropertyMetadata(""));

    public static readonly DependencyProperty FlyoutProperty =
        DependencyProperty.Register(nameof(Flyout), typeof(FlyoutBase), typeof(SpinnyButton), new PropertyMetadata(default(FlyoutBase)));
}
