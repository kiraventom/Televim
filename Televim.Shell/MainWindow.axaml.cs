using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;

namespace Televim.Shell;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        AddHandler(KeyDownEvent, OnKeyDown, RoutingStrategies.Tunnel);
    }

    private void OnOpened(object sender, EventArgs e) => Focus();

    private void OnKeyDown(object sender, KeyEventArgs e)
    {

    }
}
