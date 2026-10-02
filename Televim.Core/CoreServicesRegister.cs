using Microsoft.Extensions.DependencyInjection;
using Serilog;
using TdLib;
using Televim.Core.Configuration;
using Televim.Core.Events;
using Televim.Core.Telegram;
using Televim.Core.Telegram.Updates;
using Televim.Core.Telegram.Updates.Handlers;

namespace Televim.Core;

public static class CoreServicesRegister
{
    public const string PROJECT_NAME = "Televim";

    public static IServiceCollection AddCore(this IServiceCollection services)
    {
        services
            .AddSerilog(ConfigureLogger)
            .AddSingleton<Paths>(BuildPaths)
            .AddSingleton<IEventService, EventService>()
            .AddSingleton<IConfigService, ConfigService>()
            .AddSingleton<IUpdateRouter, UpdateRouter>()
            .AddSingleton<TdClient>(ConfigureTdClient)
            .AddSingleton<ITelegramClient, TelegramClient>();

        RegisterGenericImplementations(services, typeof(IUpdateHandler<>), (c, i, t) => { foreach (var @interface in i) c.AddTransient(@interface, t); } );

        return services;
    }

    public static IServiceCollection RegisterEventTargets(this IServiceCollection services)
    {
        RegisterGenericImplementations(services, typeof(IEventTarget<>), (collection, eventTargetInterfaces, concreteType) =>
        {
            // Let's check if this type is already registered
            var typeDescriptor = services.FirstOrDefault(d => d.ServiceType == concreteType);
            if (typeDescriptor is not null)
            {
                if (typeDescriptor.Lifetime != ServiceLifetime.Singleton)
                {
                    Log.Fatal("Can't register {Type} as IEventTarget: it's {Lifetime}, not a Singleton", concreteType, Enum.GetName<ServiceLifetime>(typeDescriptor.Lifetime));
                    throw new NotSupportedException();
                }

                foreach (var eventTargetInterface in eventTargetInterfaces)
                {
                    services.AddSingleton(eventTargetInterface, sp => sp.GetRequiredService(concreteType));
                    Log.Information("Registered {Type} as IEventTarget<{Event}>", concreteType, eventTargetInterface.GetGenericArguments().First());
                }

                return;
            }

            var interfaces = concreteType.GetInterfaces().Where(ifc => !ifc.IsGenericType || ifc.GetGenericTypeDefinition() != typeof(IEventTarget<>));
            foreach (var @interface in interfaces)
            {
                var interfaceDescriptors = services.Where(d => d.ServiceType == @interface).Where(d => !d.IsKeyedService);
                if (interfaceDescriptors.Count() > 1)
                {
                    Log.Warning("Trying to register type {Type} as IEventTarget<>: Can't use {Interface} because {Type} is not the only implementation", concreteType, @interface, concreteType);
                    continue;
                }

                var interfaceDescriptor = interfaceDescriptors.SingleOrDefault();
                if (interfaceDescriptor is not null)
                {
                    if (interfaceDescriptor.Lifetime != ServiceLifetime.Singleton)
                    {
                        Log.Fatal("Can't register {Type} as IEventTarget<>: it's {Lifetime}, not a Singleton", interfaceDescriptor.ServiceType, Enum.GetName<ServiceLifetime>(interfaceDescriptor.Lifetime));
                        throw new NotSupportedException();
                    }

                    foreach (var eventTargetInterface in eventTargetInterfaces)
                    {
                        services.AddSingleton(eventTargetInterface, sp => sp.GetRequiredService(interfaceDescriptor.ServiceType));
                        Log.Information("Registered {Type} as IEventTarget<{Event}>", interfaceDescriptor.ServiceType, eventTargetInterface.GetGenericArguments().First());
                    }
                    return;
                }
            }

            Log.Error("Type {Type} seems not to be registered. Is {Method} called after all services were registered?", concreteType, nameof(RegisterEventTargets));
        });

        return services;
    }

    private static void RegisterGenericImplementations(IServiceCollection collection, Type genericInterface, Action<IServiceCollection, IEnumerable<Type>, Type> register)
    {
        if (!genericInterface.IsGenericType)
            throw new NotSupportedException($"Type {genericInterface.FullName} is not generic");

        var classes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => t.IsClass && !t.IsAbstract)
            .Where(t => t.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericInterface));

        foreach (var @class in classes)
        {
            var implemented = @class.GetInterfaces()
                .Where(i => i.IsGenericType)
                .Where(i => i.GetGenericTypeDefinition() == genericInterface);

            register(collection, implemented, @class);
        }
    }

    private static TdClient ConfigureTdClient(IServiceProvider provider) => new TdClient();

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
            // .WriteTo.Console(outputTemplate: template, restrictedToMinimumLevel: LogEventLevel.Information);
            .WriteTo.Console(outputTemplate: template);
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
