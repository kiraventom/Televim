using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Televim.Vim.Actions;

namespace Televim.Vim;

public static class VimServicesRegister
{
    public static IServiceCollection AddVim(this IServiceCollection services)
    {
        var definitions = CollectActionDefinitions();

        services
            .AddSingleton<IActionInputHandler, ActionInputHandler>()
            .AddSingleton<IKeyStrokeBuffer, KeyStrokeBuffer>()
            .AddSingleton<IActionBuffer>(s => s.GetRequiredService<IKeyStrokeBuffer>())
            .AddSingleton<IKeyStrokeHandler, KeyStrokeHandler>()
            .AddSingleton<IActionDefinitionCheckerFactory, ActionDefinitionCheckerFactory>()
            .AddSingleton<IActionService, ActionService>()
            .AddSingleton<IActionInputBuilder, ActionInputBuilder>()
            .AddSingleton<IActionFactory, ActionFactory>(sp => BuildFactory(sp, definitions.Values.ToList()))
            .AddSingleton<IInputModeHolder, InputModeHolder>();

        RegisterActions(services, definitions.Keys);

        return services;
    }

    private static Dictionary<Type, IActionDefinition> CollectActionDefinitions()
    {
        var definitions = new Dictionary<Type, IActionDefinition>();

        var actionTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract)
            .Where(t => t.IsAssignableTo(typeof(IAction)));

        foreach (var actionType in actionTypes)
        {
            var attr = actionType.GetCustomAttribute<ActionDefinitionAttribute>();
            if (attr is null)
            {
                Log.Error("Type {commandtype} implements {interface}, but is not marked with definition. Skipping", actionType.Name, nameof(IAction));
                continue;
            }

            var actionDefinition = new ActionDefinition(actionType.Name, attr.Notation, attr.Mode, attr.AllowNumberPrefix);

            definitions.Add(actionType, actionDefinition);
        }

        return definitions;
    }
    
    private static void RegisterActions(IServiceCollection services, IEnumerable<Type> types)
    {
        foreach (var type in types)
            services.AddKeyedTransient(typeof(IAction), type.Name, type);
    }

    private static ActionFactory BuildFactory(IServiceProvider provider, IReadOnlyList<IActionDefinition> definitions)
    {
        var logger = provider.GetRequiredService<ILogger<ActionFactory>>();
        return new ActionFactory(logger, definitions, provider);
    }
}
