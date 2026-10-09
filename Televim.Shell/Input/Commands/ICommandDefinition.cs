namespace Televim.Shell.Input.Commands;

internal interface ICommandDefinition
{
    string CommandName { get; }
    IReadOnlyList<KeyStroke> KeyStrokes { get; }
    string Notation { get; }
    InputModes Mode { get; }
    bool AllowNumberPrefix { get; }
}

