using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Televim.Shell.Input;
using Televim.Shell.Input.Commands;
using Televim.Shell.ViewModels;
using Televim.Shell.Views;

namespace Televim.Shell;

internal static class ShellServicesRegister
{
    public static IServiceCollection AddShell(this IServiceCollection services)
    {
        var definitions = CollectCommandDefinitions();

        services
            .AddSingleton<IInputListener, InputListener>()
            .AddSingleton<IInputHandler, InputHandler>()
            .AddSingleton<ICommandService, CommandService>()
            .AddSingleton<ICommandInputBuilder, CommandInputBuilder>()
            .AddSingleton<ICommandFactory, CommandFactory>(sp => BuildFactory(sp, definitions.Values.ToList()))
            .AddSingleton<IInputModeHolder, InputModeHolder>()
            .AddSingleton<IMainViewModel, MainViewModel>()
            .AddSingleton<MainWindow>();

        RegisterCommands(services, definitions.Keys);

        return services;
    }

    private static Dictionary<Type, ICommandDefinition> CollectCommandDefinitions()
    {
        var definitions = new Dictionary<Type, ICommandDefinition>();

        var commandTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract)
            .Where(t => t.IsAssignableTo(typeof(ICommand)));

        foreach (var commandType in commandTypes)
        {
            var attr = commandType.GetCustomAttribute<CommandDefinitionAttribute>();
            if (attr is null)
            {
                Log.Error("Type {commandtype} implements {interface}, but is not marked with definition. Skipping", commandType.Name, nameof(ICommand));
                continue;
            }

            var commandDefinition = new CommandDefinition(commandType.Name, attr.Notation, attr.Mode, attr.AllowNumberPrefix);

            definitions.Add(commandType, commandDefinition);
        }

        return definitions;
    }
    
    private static void RegisterCommands(IServiceCollection services, IEnumerable<Type> types)
    {
        foreach (var type in types)
            services.AddKeyedTransient(typeof(ICommand), type.Name, type);
    }

    private static CommandFactory BuildFactory(IServiceProvider provider, IReadOnlyList<ICommandDefinition> definitions)
    {
        var logger = provider.GetRequiredService<ILogger<CommandFactory>>();
        return new CommandFactory(logger, definitions, provider);
    }
}


