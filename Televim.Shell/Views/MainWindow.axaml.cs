using Avalonia.Controls;
using Avalonia.Input;
using Avalonia.Interactivity;
using Microsoft.Extensions.Logging;
using Televim.Shell.Input;
using Televim.Shell.ViewModels;

namespace Televim.Shell.Views;

public partial class MainWindow : Window
{
    private IMainViewModel ViewModel { get; }
    private ILogger<MainWindow> Logger { get; }

    public MainWindow(ILogger<MainWindow> logger, IMainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        AddHandler(KeyDownEvent, OnKeyDown, RoutingStrategies.Tunnel);
        ViewModel = viewModel;
        Logger = logger;

        ViewModel.InputModeChanged += OnInputModeChanged;
    }

    private void OnInputModeChanged(InputModes inputMode)
    {
        if (inputMode == InputModes.INSERT) // TODO TEMP
            Dispatcher.Post(() => InputTextBox.Focus());
        else
            Dispatcher.Post(() => Focus());
    }

    private void OnOpened(object sender, EventArgs e) => Focus();
    private void OnKeyDown(object sender, KeyEventArgs e) => ViewModel.OnKeyDown(e);
}
