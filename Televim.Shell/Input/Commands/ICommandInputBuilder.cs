namespace Televim.Shell.Input.Commands;

internal interface ICommandInputBuilder
{
    CommandInput Build(IReadOnlyList<KeyStroke> keyStrokes);
}

