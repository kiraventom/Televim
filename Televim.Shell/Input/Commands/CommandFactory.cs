using System.Reflection;
using Microsoft.Extensions.Logging;
using Televim.Core.Configuration;

namespace Televim.Shell.Input.Commands;

internal class CommandFactory : ICommandFactory
{
    private readonly ILogger<CommandFactory> _logger;
    private readonly IServiceProvider _sp;

    private readonly Dictionary<ICommandDefinition, Type> _typesByDefs;

    public IReadOnlyCollection<ICommandDefinition> Definitions => _typesByDefs.Keys;

    public CommandFactory(ILogger<CommandFactory> logger, Config config, IServiceProvider sp)
    {
        _logger = logger;
        _sp = sp;

        var commandTypes = Assembly.GetExecutingAssembly().GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract)
            .Where(t => t.IsAssignableTo(typeof(ICommand)));

        foreach (var commandType in commandTypes)
        {
            var attr = commandType.GetCustomAttribute<CommandDefinitionAttribute>();
            if (attr is null)
            {
                _logger.LogError("Type {commandtype} implements {interface}, but is not marked with definition. Skipping", commandType.Name, nameof(ICommand));
                continue;
            }

            // TODO Here we can load custom keybinds from config
            // if (config.Input.Keybinds.ContainsKey(commandType.Name))
            //     attr.Text = config.Input.Keybinds[commandType.Name];

            if (!attr.IsValid())
            {
                _logger.LogError("Type {commandtype} has incorrect definition. Skipping", commandType.Name);
                continue;
            }

            var definition = attr;
            _typesByDefs.Add(definition, commandType);
        }
    }

    public ICommand Build(ICommandDefinition definition)
    {
        var type = _typesByDefs[definition];
        return (ICommand)_sp.GetService(type);
    }
}
