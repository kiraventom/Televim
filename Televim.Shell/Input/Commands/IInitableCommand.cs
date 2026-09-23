namespace Televim.Shell.Input.Commands;

internal interface IInitableCommand : ICommand
{
    void Init(CommandInput input);
}

