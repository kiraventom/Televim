using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Televim.Shell.Input.Commands;

internal class CommandFactory : ICommandFactory
{
    private readonly ILogger<CommandFactory> _logger;
    private readonly IServiceProvider _sp;
    private readonly IReadOnlyList<ICommandDefinition> _definitions;

    public IReadOnlyList<ICommandDefinition> Definitions => _definitions;

    public CommandFactory(ILogger<CommandFactory> logger, IReadOnlyList<ICommandDefinition> definitions, IServiceProvider sp)
    {
        _logger = logger;
        _sp = sp;
        _definitions = definitions;
    }

    public ICommand Build(ICommandDefinition definition)
    {
        return _sp.GetRequiredKeyedService<ICommand>(definition.CommandName);
    }
}
