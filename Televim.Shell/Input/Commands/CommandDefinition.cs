namespace Televim.Shell.Input.Commands;

internal record CommandDefinition(string CommandName, string Notation, InputModes Mode, bool AllowNumberPrefix) : ICommandDefinition
{
    public IReadOnlyList<KeyStroke> KeyStrokes { get; } = KeyNotation.Parse(Notation);
}

