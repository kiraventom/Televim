using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Televim.Core;
using Televim.Core.Configuration;
using Televim.Core.Events;
using Televim.Shell.Views;

namespace Televim.Shell;

public partial class App : Application,
    IEventTarget<DefaultConfigGeneratedEvent>,
    IEventTarget<DefaultConfigDetectedEvent>
{
    public override void Initialize() => AvaloniaXamlLoader.Load(this);

    public override void OnFrameworkInitializationCompleted()
    {
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            var mainWindow = Program.AppHost.Services.GetRequiredService<MainWindow>();
            desktop.MainWindow = mainWindow;
        }

        base.OnFrameworkInitializationCompleted();
    }

    public async Task Handle(DefaultConfigGeneratedEvent @event)
    {
        // TODO Possibly replace
        Log.Information($"No config file found. Default config created at {@event.ConfigFilePath}");
    }

    public async Task Handle(DefaultConfigDetectedEvent @event)
    {
        // TODO Possibly replace
        Log.Error($"Default config detected at {@event.ConfigFilePath}"); 
        Log.Information($"{CoreServicesRegister.PROJECT_NAME} requires following properties to be specified: {string.Join(", ", @event.PropsToChange)}.");
        Log.Information("Exiting...");

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            desktop.TryShutdown(1);
    }
}
