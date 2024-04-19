using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Input;
using MouseTrap.Helpers;
using MouseTrap.Models;
using Windows.Foundation;

namespace MouseTrap.Controls;

internal sealed partial class ProgramCard : UserControl
{
    public ProgramCard()
    {
        this.InitializeComponent();
    }

    public IProgramModel ViewModel
    {
        get => (IProgramModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    public bool IsTrapped
    {
        get => (bool)GetValue(IsTrappedProperty);
        set => SetValue(IsTrappedProperty, value);
    }

    public static readonly DependencyProperty ViewModelProperty =
        DependencyProperty.Register(nameof(ViewModel), typeof(IProgramModel), typeof(ProgramCard), new PropertyMetadata(null));

    public static readonly DependencyProperty IsTrappedProperty =
        DependencyProperty.Register(nameof(IsTrapped), typeof(bool), typeof(ProgramCard), new PropertyMetadata(false, new PropertyChangedCallback(IsTrappedChanged)));

    public static void IsTrappedChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is ProgramCard card)
        {
            var name = card.IsTrapped ? "Trapped" : "Normal";
            VisualStateManager.GoToState(card.TheButton, name, true);
        }
    }

    private void Button_ContextRequested(UIElement sender, ContextRequestedEventArgs args)
    {
        args.TryGetPosition(null, out Point point);

        var options = new FlyoutShowOptions()
        {
            Position = point
        };

        ContextMenu.ShowAt(null, options);
    }

    private void AppBarButton_Click(object sender, RoutedEventArgs e)
    {
        if (sender is AppBarButton button)
        {
            ContextMenu.Hide();

            var tag = button.Tag.ToString();

            switch (tag)
            {
                case "Copy":
                    Utilities.CopyToClipboard(ViewModel.FullPath);
                    break;
                case "Explore":
                    Utilities.ExploreFolder(ViewModel.FullPath);
                    break;
                case "Run":
                    Utilities.RunProgram(ViewModel.Title, ViewModel.FullPath);
                    break;
                default:
                    break;
            }
        }
    }
}
