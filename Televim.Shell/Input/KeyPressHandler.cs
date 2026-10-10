using Avalonia.Input;
using Microsoft.Extensions.Logging;
using Televim.Core.Events;
using Televim.Shell.Input.Actions;
using Televim.Shell.Input.Utils;

namespace Televim.Shell.Input;

internal readonly record struct ActionBufferChangedEvent(string Buffer);

internal class KeyPressHandler(ILogger<KeyPressHandler> logger, IEventService eventService, IActionInputBuilder inputBuilder, IActionInputHandler inputHandler, IInputModeHolder inputModeHolder) : IKeyPressHandler
{
    private readonly List<KeyStroke> _buffer = [];
    private readonly PrefixBuilder _prefixBuilder = new();

    public void HandleKeyPress(KeyEventArgs e)
    {
        var keyStroke = KeyStroke.From(e);

        // Don't register the modifier alone
        if (keyStroke.IsModifier)
            return;

        logger.LogDebug("Parsed key stroke: {stroke}", keyStroke);

        if (keyStroke.IsDigit && _buffer.Count == 0)
        {
            if (inputModeHolder.CurrentModeSupportsPrefixes())
            {
                _prefixBuilder.Append(keyStroke.Char.Value);
                logger.LogDebug("Number prefix changed to {prefix}", _prefixBuilder.Number);
            }

            return;
        }

        _buffer.Add(keyStroke);
        logger.LogDebug("Current buffer state: {{ {buffer} }}", string.Join(", ", _buffer.Select((k, i) => $"[{i}] = {k}")));

        var input = inputBuilder.Build(_buffer, _prefixBuilder.Number);
        logger.LogDebug("Built input: {input}, number prefix: {prefix}", input, _prefixBuilder.Number);

        var (IsEventHandled, SkipBufferClear) = inputHandler.Handle(input);

        if (IsEventHandled)
            e.Handled = true;

        if (!SkipBufferClear)
        {
            _buffer.Clear();
            _prefixBuilder.Clear();
        }

        eventService.Raise(new ActionBufferChangedEvent(string.Join(string.Empty, _buffer.Select(k => k.Char?.ToString() ?? $"<{Enum.GetName(k.Key)}>"))));

    }
}

