using Microsoft.Extensions.DependencyInjection;
using Televim.Shell.Input;
using Televim.Shell.ViewModels;
using Televim.Shell.Views;
using Televim.Vim.Commands;

namespace Televim.Shell;

internal static class ShellServicesRegister
{
    public static IServiceCollection AddShell(this IServiceCollection services)
    {
        services
            .AddSingleton<IInputListener, InputListener>()
            .AddSingleton<IMainViewModel, MainViewModel>()
            .AddSingleton<ICommandTextHolder>(s => s.GetRequiredService<IMainViewModel>())
            .AddSingleton<MainWindow>()
            .AddSingleton<App>();

        return services;
    }

}


