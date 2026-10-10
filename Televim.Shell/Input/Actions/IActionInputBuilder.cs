namespace Televim.Shell.Input.Actions;

internal interface IActionInputBuilder
{
    ActionInput Build(IReadOnlyList<KeyStroke> keyStrokes, int numberPrefix);
}

