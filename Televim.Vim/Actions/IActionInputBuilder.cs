namespace Televim.Vim.Actions;

internal interface IActionInputBuilder
{
    ActionInput Build(IKeyStrokeBuffer buffer);
}

