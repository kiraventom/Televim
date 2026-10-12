namespace Televim.Vim.Actions;

internal interface IActionDefinitionChecker
{
    IActionDefinition Definition { get; }
    ActionCheckResult TryMatch(ActionInput input);
}

