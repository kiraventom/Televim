using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Televim.Configuration;

namespace Televim;

public record Paths(string ConfigDir, string DataDir);

internal class Program
{
    private const string PROJECT_NAME = nameof(Televim);

    private static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateBootstrapLogger();

        Log.Information("Building host...");

        try
        {
            var paths = BuildPaths();

            var builder = Host.CreateApplicationBuilder();

            builder.Services.AddSerilog(ConfigureLogger)
                .AddSingleton<Paths>(paths)
                .AddSingleton<IEventService, EventService>()
                .AddSingleton<IConfigService, ConfigService>();

            RegisterEventTargets(builder.Services);

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

    private static void RegisterEventTargets(IServiceCollection collection)
    {
        var targetInterfaceType = typeof(IEventTarget<>);
        var eventTargetTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract)
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == targetInterfaceType));

        foreach (var type in eventTargetTypes)
        {
            var implementedInterfaces = type.GetInterfaces()
                .Where(i => i.IsGenericType && i.GetGenericTypeDefinition() == targetInterfaceType);

            foreach (var @interface in implementedInterfaces)
            {
                collection.AddTransient(@interface, type);
            }
        }
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

        private static Config BuildConfig(IServiceProvider provider)
    {
        var paths = provider.GetRequiredService<Paths>();
        var configFilePath = Path.Combine(paths.ConfigDir, CONFIG_FILENAME);

        if (!File.Exists(configFilePath))
        {
            Config.Default.Save(configFilePath); 
            throw new InvalidOperationException($"Default config created at {configFilePath}. Fill it out and restart.");
        }

        return Config.Load(configFilePath);
    }

    private static Paths BuildPaths()
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
