namespace Televim.Shell.Input.Commands;

internal interface ICommandService
{
    CommandCheckResult TryGetCommand(CommandInput input, out ICommand command);
}

