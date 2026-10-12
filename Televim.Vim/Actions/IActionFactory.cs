namespace Televim.Vim.Actions;

internal interface IActionFactory
{
    IReadOnlyList<IActionDefinition> Definitions { get; }
    IAction Build(IActionDefinition definition);
}

