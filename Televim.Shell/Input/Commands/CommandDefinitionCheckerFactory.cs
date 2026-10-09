using Microsoft.Extensions.Logging;

namespace Televim.Shell.Input.Commands;

internal class CommandDefinitionCheckerFactory(ILoggerFactory loggerFactory) : ICommandDefinitionCheckerFactory
{
    public ICommandDefinitionChecker Build(ICommandDefinition definition)
    {
        return new CommandDefinitionChecker(loggerFactory.CreateLogger<CommandDefinitionChecker>(), definition);
    }
}
