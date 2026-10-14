using Microsoft.Extensions.Logging;
using Televim.Vim.Actions;

namespace Televim.Vim;

internal class KeyStrokeHandler(ILogger<KeyStrokeHandler> logger, IActionInputBuilder inputBuilder, IActionInputHandler inputHandler, IKeyStrokeBuffer buffer) : IKeyStrokeHandler
{
    public bool Handle(KeyStroke keyStroke)
    {
        if (keyStroke.IsIgnorable)
            return false;

        logger.LogDebug("Parsed key stroke: {stroke}", keyStroke);

        buffer.Append(keyStroke);

        while (true)
        {
            var input = inputBuilder.Build(buffer);
            var (IsHandled, ExpectFollowUp) = inputHandler.Handle(input);

            if (IsHandled)
            {
                if (!ExpectFollowUp)
                    buffer.Clear();

                return true;
            }

            if (!buffer.TryDropOldest())
            {
                buffer.Clear();
                return false;
            }
        }
    }
}

