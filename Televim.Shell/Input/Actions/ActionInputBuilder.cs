namespace Televim.Shell.Input.Actions;

internal class ActionInputBuilder(IInputModeHolder inputModeHolder) : IActionInputBuilder
{
    public ActionInput Build(IReadOnlyList<KeyStroke> keyStrokes, int numberPrefix)
    {
        return new ActionInput(keyStrokes, numberPrefix, inputModeHolder.CurrentMode);
    }
}

