namespace Televim.Shell.Input.Actions;

internal interface IActionService
{
    ActionCheckResult TryGetAction(ActionInput input, out IAction action);
}

