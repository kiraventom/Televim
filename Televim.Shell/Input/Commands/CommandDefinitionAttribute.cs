namespace Televim.Shell.Input.Commands;

[AttributeUsage(AttributeTargets.Class)]
internal class CommandDefinitionAttribute(string notation, InputMode mode) : Attribute
{
    public string Notation { get; } = notation;
    public InputMode Mode { get; } = mode;
    public bool AllowNumberPrefix { get; init; }
}
