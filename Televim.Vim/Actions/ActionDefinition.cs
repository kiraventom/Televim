namespace Televim.Vim.Actions;

internal record ActionDefinition(string ActionName, string Notation, InputModes Mode, bool AllowNumberPrefix) : IActionDefinition
{
    public IReadOnlyList<KeyStroke> KeyStrokes { get; } = KeyNotation.Parse(Notation);
}

