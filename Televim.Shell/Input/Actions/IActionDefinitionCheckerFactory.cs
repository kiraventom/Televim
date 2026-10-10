namespace Televim.Shell.Input.Actions;

internal interface IActionDefinitionCheckerFactory
{
    IActionDefinitionChecker Build(IActionDefinition definition);
}
