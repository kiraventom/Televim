namespace Televim.Vim.Actions;

internal interface IActionDefinition
{
    string ActionName { get; }
    IReadOnlyList<KeyStroke> KeyStrokes { get; }
    string Notation { get; }
    InputModes Mode { get; }
    bool AllowNumberPrefix { get; }
}

