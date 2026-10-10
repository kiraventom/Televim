namespace Televim.Shell.Input.Actions;

internal interface IActionFactory
{
    IReadOnlyList<IActionDefinition> Definitions { get; }
    IAction Build(IActionDefinition definition);
}

