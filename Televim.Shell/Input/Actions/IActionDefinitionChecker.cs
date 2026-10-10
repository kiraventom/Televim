namespace Televim.Shell.Input.Actions;

internal interface IActionDefinitionChecker
{
    IActionDefinition Definition { get; }
    ActionCheckResult TryMatch(ActionInput input);
}

