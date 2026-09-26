namespace Televim.Shell.Input.Commands;

[AttributeUsage(AttributeTargets.Class)]
internal class CommandDefinitionAttribute(string notation, InputMode mode) : Attribute, ICommandDefinition
{
    public string Notation
    {
        get => field;
        set
        {
            field = value;
            KeyStrokes = KeyNotation.Parse(field);
        }
    } = notation;

    public InputMode Mode { get; } = mode;
    public bool AllowNumberPrefix { get; init; }

    public IReadOnlyList<KeyStroke> KeyStrokes { get; private set; }
}

