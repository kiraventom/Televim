namespace Televim.Shell.Input.Commands;

internal interface ICommandService
{
    CommandCheckResult TryGetCommand(IReadOnlyList<KeyStroke> keyStrokes, out ICommand command);
}

