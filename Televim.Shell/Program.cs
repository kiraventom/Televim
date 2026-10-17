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

        // TEST DATA
        {
            var viewModel = AppHost.Services.GetRequiredService<IMainViewModel>();

            var chat = new ChatViewModel() { Name = "Chat #1" };
            chat.Messages.Add(new ChatMessageViewModel { Text = "Text message" });
            chat.Messages.Add(new ChatMessageViewModel { Text = "Response" });

            var otherChat = new ChatViewModel() { Name = "Чат №2" };
            otherChat.Messages.Add(new ChatMessageViewModel { Text = "Сообщение" });
            otherChat.Messages.Add(new ChatMessageViewModel { Text = "Ответ" });

            viewModel.ChatList.Chats.Add(chat);
            viewModel.ChatList.Chats.Add(otherChat);
        }
        // TEST DATA

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

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure(() => AppHost.Services.GetRequiredService<App>())
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}
