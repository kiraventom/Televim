namespace Televim.Vim.Actions;

internal class ActionInputBuilder(IInputModeHolder inputModeHolder) : IActionInputBuilder
{
    public ActionInput Build(IKeyStrokeBuffer buffer)
    {
        return new ActionInput(buffer.KeyStrokes, buffer.Prefix, inputModeHolder.CurrentMode);
    }
}

