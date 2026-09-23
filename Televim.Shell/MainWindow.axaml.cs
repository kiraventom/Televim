using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Televim.Shell;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();

        // Avalonia has no PreviewKeyDown; this is the tunneling ("preview") equivalent.
        // It fires before the bubbled KeyDown, even when a child control has focus.
        AddHandler(KeyDownEvent, OnPreviewKeyDown, RoutingStrategies.Tunnel);
    }

    private void OnOpened(object sender, EventArgs e)
    {
        Focus(); // the window itself must own focus to receive keys
    }

    // Bubbling handler (like WPF's normal KeyDown)
    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        Log.Text = $"bubble: {e.Key} + {e.KeyModifiers}";
        e.Handled = true;
    }

    private void OnPreviewKeyDown(object sender, KeyEventArgs e)
    {
        // Log.Text = $"tunnel: {e.Key} + {e.KeyModifiers}";
    }
}
