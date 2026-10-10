using Avalonia.Input;
using Microsoft.Extensions.Logging;
using Televim.Core.Events;
using Televim.Shell.Input.Actions;

namespace Televim.Shell.Input;

internal readonly record struct ActionBufferChangedEvent(string Buffer);

internal class KeyPressHandler(ILogger<KeyPressHandler> logger, IEventService eventService, IActionInputBuilder inputBuilder, IActionInputHandler inputHandler) : IKeyPressHandler
{
    private readonly List<KeyStroke> _buffer = [];

    public void HandleKeyPress(KeyEventArgs e)
    {
        var keyStroke = KeyStroke.From(e);

        // Don't register the modifier alone
        if (keyStroke.IsModifier)
            return;

        logger.LogDebug("Parsed key stroke: {stroke}", keyStroke);

        _buffer.Add(keyStroke);
        logger.LogDebug("Current buffer state: {{ {buffer} }}", string.Join(", ", _buffer.Select((k, i) => $"[{i}] = {k}")));

        // TODO: Write starting digits into separate buffer and only in normal mode
        if (_buffer.All(k => k.IsDigit))
            return;

        var input = inputBuilder.Build(_buffer);
        logger.LogDebug("Built input: {input}", input);

        var (IsEventHandled, SkipBufferClear) = inputHandler.Handle(input);

        if (IsEventHandled)
            e.Handled = true;

        if (!SkipBufferClear)
            _buffer.Clear();

        eventService.Raise(new ActionBufferChangedEvent(string.Join(string.Empty, _buffer.Select(k => k.Char?.ToString() ?? "<null>"))));

    }
}

