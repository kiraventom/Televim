namespace Televim.Vim.Actions;

internal interface IActionDefinitionCheckerFactory
{
    IActionDefinitionChecker Build(IActionDefinition definition);
}
