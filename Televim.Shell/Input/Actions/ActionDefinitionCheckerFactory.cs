using Microsoft.Extensions.Logging;

namespace Televim.Shell.Input.Actions;

internal class ActionDefinitionCheckerFactory(ILoggerFactory loggerFactory) : IActionDefinitionCheckerFactory
{
    public IActionDefinitionChecker Build(IActionDefinition definition)
    {
        return new ActionDefinitionChecker(loggerFactory.CreateLogger<ActionDefinitionChecker>(), definition);
    }
}
