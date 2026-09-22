using Microsoft.Extensions.Hosting;
using Televim.Core.Configuration;
using Televim.Core.Events;

namespace Televim.Core;

internal class AppService() : BackgroundService, 
    IEventTarget<DefaultConfigGeneratedEvent>,
    IEventTarget<DefaultConfigDetectedEvent>
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await Task.Delay(Timeout.Infinite, stoppingToken);
    }

    public async Task Handle(DefaultConfigGeneratedEvent @event)
    {
        // TODO Possibly replace
        Console.WriteLine($"No config file found. Default config created at {@event.ConfigFilePath}");
    }

    public async Task Handle(DefaultConfigDetectedEvent @event)
    {
        // TODO Possibly replace
        Console.Error.WriteLine($"Default config detected at {@event.ConfigFilePath}"); 
        Console.WriteLine($"{Program.PROJECT_NAME} requires following properties to be specified: {string.Join(", ", @event.PropsToChange)}.");
        Console.WriteLine("Exiting...");

        using var cts = new CancellationTokenSource();
        cts.CancelAfter(TimeSpan.FromSeconds(5));
        await StopAsync(cts.Token);
    }
}

