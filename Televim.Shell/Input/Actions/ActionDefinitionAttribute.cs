namespace Televim.Shell.Input.Actions;

[AttributeUsage(AttributeTargets.Class)]
internal class ActionDefinitionAttribute(string notation, InputModes mode) : Attribute
{
    public string Notation { get; } = notation;
    public InputModes Mode { get; } = mode;
    public bool AllowNumberPrefix { get; init; }
}
