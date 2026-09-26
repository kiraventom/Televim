namespace Televim.Shell.Input.Commands;

internal interface ICommandDefinition
{
    IReadOnlyList<KeyStroke> KeyStrokes { get; }
    string Notation { get; }
    InputMode Mode { get; }
    bool AllowNumberPrefix { get; }
}

