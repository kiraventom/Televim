using Microsoft.Extensions.Logging;
using Televim.Vim.Actions;

namespace Televim.Vim;

internal class KeyStrokeHandler(ILogger<KeyStrokeHandler> logger, IActionInputBuilder inputBuilder, IActionInputHandler inputHandler, IKeyStrokeBuffer buffer, IInputModeHolder inputModeHolder) : IKeyStrokeHandler
{
    public bool Handle(KeyStroke keyStroke)
    {
        if (keyStroke.IsIgnorable)
            return false;

        logger.LogDebug("Parsed key stroke: {stroke}", keyStroke);

        if (keyStroke.IsDigit && buffer.KeyStrokes.Count == 0)
        {
            if (inputModeHolder.CurrentModeSupportsPrefixes())
                buffer.AppendPrefix(keyStroke.Char.Value);

            return false;
        }

        buffer.Append(keyStroke);

        var input = inputBuilder.Build(buffer);

        var (IsEventHandled, SkipBufferClear) = inputHandler.Handle(input);

        if (!SkipBufferClear)
            buffer.Clear();

        return IsEventHandled;
    }
}

