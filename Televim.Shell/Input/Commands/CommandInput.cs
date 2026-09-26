namespace Televim.Shell.Input.Commands;

internal record CommandInput(IReadOnlyList<KeyStroke> KeyStrokes, int NumberPrefix, InputMode Mode);
