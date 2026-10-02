namespace Televim.Shell.Input.Commands;

internal interface ICommandDefinition
{
    string CommandName { get; }
    IReadOnlyList<KeyStroke> KeyStrokes { get; }
    string Notation { get; }
    InputMode Mode { get; }
    bool AllowNumberPrefix { get; }
}

