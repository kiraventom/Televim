namespace Televim.Shell.Input.Actions;

internal abstract class Action : IInitableAction
{
    protected ActionInput Input { get; private set; }

    public abstract Task Execute();

    public void Init(ActionInput input) => Input = input;
}
