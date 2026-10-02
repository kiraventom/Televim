using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Televim.Core;

namespace Televim.Shell;

internal static class Program
{
    public static IHost AppHost { get; private set; }

    [STAThread]
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Debug()
            .WriteTo.Console()
            .CreateBootstrapLogger();

        Log.Information("Building host...");

        try
        {
            var builder = Host.CreateApplicationBuilder();
            builder.Services
                .AddCore()
                .AddShell()
                .RegisterEventTargets();

            AppHost = builder.Build();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Failed to build host, terminating");
            Log.CloseAndFlush();
            return;
        }

        var lifetime = AppHost.Services.GetRequiredService<IHostApplicationLifetime>();
        lifetime.ApplicationStopping.Register(() =>
        {
            Dispatcher.UIThread.Post(() =>
            {
                if (Application.Current?.ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktopLifetime)
                    desktopLifetime.Shutdown();
            });
        });

        Log.Information("Host built, running...");

        try
        {
            BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
            AppHost.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Failed to run, terminating");
            return;
        }
        finally
        {
            AppHost.StopAsync().GetAwaiter().GetResult();
            AppHost.Dispose();

            Log.CloseAndFlush();
        }
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
