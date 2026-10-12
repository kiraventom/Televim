namespace Televim.Vim.Actions;

internal interface IInitableAction : IAction
{
    void Init(ActionInput input);
}

