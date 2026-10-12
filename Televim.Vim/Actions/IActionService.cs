namespace Televim.Vim.Actions;

internal interface IActionService
{
    ActionCheckResult TryGetAction(ActionInput input, out IAction action);
}

