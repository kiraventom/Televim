using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using TdLib;
using Televim.Configuration;
using Televim.Events;
using Televim.Telegram;
using Televim.Telegram.Updates;
using Televim.Telegram.Updates.Handlers;

namespace Televim;

public record Paths(string ConfigDir, string DataDir);

internal class Program
{
    public const string PROJECT_NAME = nameof(Televim);

    private static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateBootstrapLogger();

        Log.Information("Building host...");

        try
        {
            var builder = Host.CreateApplicationBuilder();

            builder.Services.AddSerilog(ConfigureLogger)
                .AddSingleton<Paths>(BuildPaths)
                .AddSingleton<IEventService, EventService>()
                // TODO Renderer right after event service so it can receive events from other services
                .AddSingleton<IConfigService, ConfigService>()
                .AddSingleton<IUpdateRouter, UpdateRouter>()
                .AddSingleton<TdClient>(ConfigureTdClient)
                .AddSingleton<ITelegramClient, TelegramClient>();

            RegisterImplementations(builder.Services, typeof(IEventTarget<>));
            RegisterImplementations(builder.Services, typeof(IUpdateHandler<>));

            builder.Services.AddHostedService<AppService>();

            var host = builder.Build();
            await host.RunAsync();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Failed to build host, terminating");
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }

    private static void RegisterImplementations(IServiceCollection collection, Type @interface)
    {
        if (!@interface.IsGenericType)
            throw new NotSupportedException($"Type {@interface.FullName} is not generic");

        var classes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract)
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == @interface));

        foreach (var @class in classes)
        {
            var implemented = @class.GetInterfaces()
                .Where(i => i.GetGenericTypeDefinition() == @interface);

            foreach (var @implementedInterface in implemented)
            {
                collection.AddTransient(@implementedInterface, @class);
            }
        }
    }

    private static TdClient ConfigureTdClient(IServiceProvider provider)
    {
        return new TdClient();
    }

    private static void ConfigureLogger(IServiceProvider provider, LoggerConfiguration configuration)
    {
        var paths = provider.GetRequiredService<Paths>();
        var logsDirPath = Path.Combine(paths.DataDir, "logs");
        Directory.CreateDirectory(logsDirPath);
        var logFilePath = Path.Combine(logsDirPath, $"{PROJECT_NAME}.log");
        
        const string template = @"[{Timestamp:HH:mm:ss} {Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}";

        configuration.MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", Serilog.Events.LogEventLevel.Warning)
            .WriteTo.File(logFilePath, outputTemplate: template, rollingInterval: RollingInterval.Day)
            .WriteTo.Console(outputTemplate: template, restrictedToMinimumLevel: LogEventLevel.Information);
    }

    private static Paths BuildPaths(IServiceProvider sp)
    {
        var configDirPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        var dataDirPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        var appConfigDirPath = Path.Combine(configDirPath, PROJECT_NAME);
        var appDataDirPath = Path.Combine(dataDirPath, PROJECT_NAME);

        Directory.CreateDirectory(appConfigDirPath);
        Directory.CreateDirectory(appDataDirPath);

        return new Paths(appConfigDirPath, appDataDirPath);
    }
}
