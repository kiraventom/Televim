using Microsoft.Extensions.DependencyInjection;
using TdLib;
using Televim.Common;
using Televim.Telegram.Updates;
using Televim.Telegram.Updates.Handlers;

namespace Televim.Telegram;

public static class TelegramServicesRegister
{
    public static IServiceCollection AddTelegram(this IServiceCollection services)
    {
        services
            .AddSingleton<TdClient>(ConfigureTdClient)
            .AddSingleton<IUpdateRouter, UpdateRouter>()
            .AddSingleton<ITelegramClient, TelegramClient>()
            .RegisterGenericImplementations(typeof(IUpdateHandler<>), (c, i, t) => { foreach (var @interface in i) c.AddTransient(@interface, t); } );

        return services;
    }

    private static TdClient ConfigureTdClient(IServiceProvider provider) => new TdClient();

}
