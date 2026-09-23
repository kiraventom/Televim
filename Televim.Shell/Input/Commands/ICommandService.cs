namespace Televim.Shell.Input.Commands;

internal interface ICommandService
{
    CommandCheckResult TryGetCommand(string input, out ICommand command);
}

