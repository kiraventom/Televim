using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Threading;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Televim.Common;
using Televim.Vim;
using Televim.Telegram;
using Televim.Shell.ViewModels;
using Televim.Shell.ViewModels.Focusable;

namespace Televim.Shell;

internal static class Program
{
    public static IHost AppHost { get; private set; }

    [STAThread]
    public static int Main(string[] args)
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
                .AddCommon()
                .AddVim()
                .AddTelegram()
                .AddShell()
                .RegisterEventTargets();

            AppHost = builder.Build();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Failed to build host, terminating");
            Log.CloseAndFlush();
            return 1;
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

        Init();

        Log.Information("Host built, running...");

        try
        {
            AppHost.Start();
            return BuildAvaloniaApp().StartWithClassicDesktopLifetime(args);
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Failed to run, terminating");
            return 1;
        }
        finally
        {
            AppHost.StopAsync().GetAwaiter().GetResult();
            AppHost.Dispose();

            Log.CloseAndFlush();
        }
    }

    private static void Init()
    {
        // Stupid temp data
        var main = AppHost.Services.GetRequiredService<IMainViewModel>();
        var chatList = AppHost.Services.GetRequiredService<IChatListViewModel>();
        var focusManager = AppHost.Services.GetRequiredService<IFocusManager>();

        for (int i = 0; i < 100; ++i)
        {
            var chat = new ChatViewModel() { Name = $"Chat #{i + 1}" };
            var messageCount = Random.Shared.Next(1, 6);
            for (int j = 0; j < messageCount; ++j)
                chat.Messages.Add(new ChatMessageViewModel { Text = Path.GetRandomFileName() });

            chatList.Chats.Add(chat);
        }

        // Actually important
        chatList.SelectedChat = chatList.Chats.First();
        focusManager.Focus(chatList);
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure(() => AppHost.Services.GetRequiredService<App>())
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
