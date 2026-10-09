namespace Televim.Shell.Input.Commands;

[AttributeUsage(AttributeTargets.Class)]
internal class CommandDefinitionAttribute(string notation, InputModes mode) : Attribute
{
    public string Notation { get; } = notation;
    public InputModes Mode { get; } = mode;
    public bool AllowNumberPrefix { get; init; }
}
