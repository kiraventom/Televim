namespace Televim.Shell.Input.Actions;

internal interface IInitableAction : IAction
{
    void Init(ActionInput input);
}

