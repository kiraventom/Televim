namespace Televim.Shell.Input.Commands;

internal abstract class Command : IInitableCommand
{
    protected CommandInput Input { get; private set; }

    public abstract Task Execute();

    public void Init(CommandInput input) => Input = input;
}
