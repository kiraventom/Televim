using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Televim.Shell.ViewModels;

namespace Televim.Shell.Views;

public partial class MainWindow : Window
{
    private IMainViewModel ViewModel { get; }

    public MainWindow(IMainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        AddHandler(KeyDownEvent, OnKeyDown, RoutingStrategies.Tunnel);
        ViewModel = viewModel;
    }

    private void OnOpened(object sender, EventArgs e) => Focus();
    private void OnKeyDown(object sender, KeyEventArgs e) => ViewModel.OnKeyDown(e);
}
