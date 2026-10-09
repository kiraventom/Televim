using Avalonia.Input;
using Microsoft.Extensions.Logging;
using Televim.Core.Events;
using Televim.Core.Utils;
using Televim.Shell.Input.Commands;

namespace Televim.Shell.Input;

internal readonly record struct BufferChangedEvent(string Buffer);

internal class InputHandler : ChannelHandler<ICommand>, IInputHandler
{
    private readonly List<KeyStroke> _buffer = [];

    private readonly ILogger<InputHandler> _logger;
    private readonly IEventService _eventService;
    private readonly ICommandService _commandService;
    private readonly ICommandInputBuilder _inputBuilder;

    public InputHandler(ILogger<InputHandler> logger, IEventService eventService, ICommandService commandService, ICommandInputBuilder inputBuilder)
    {
        _logger = logger;
        _eventService = eventService;
        _inputBuilder = inputBuilder;
        _commandService = commandService;
    }

    public void Handle(KeyEventArgs e)
    {
        var keyStroke = KeyStroke.From(e);

        // Don't register the modifier alone
        if (keyStroke.IsModifier)
            return;

        _logger.LogDebug("Parsed key stroke: {stroke}", keyStroke);

        _buffer.Add(keyStroke);
        _logger.LogDebug("Current buffer state: {{ {buffer} }}", string.Join(", ", _buffer.Select((k, i) => $"[{i}] = {k}")));

        // Don't try the commands if we're entering the prefix
        if (_buffer.All(k => k.IsDigit))
            return;

        var input = _inputBuilder.Build(_buffer);
        _logger.LogDebug("Built input: {input}", input);

        var result = _commandService.TryGetCommand(input, out var command);

        switch (result)
        {
            case CommandCheckResult.NO_MATCH:
                _buffer.Clear();
                _logger.LogDebug("No match");
                // Do not handle the event
                break;

            case CommandCheckResult.PARTIAL_MATCH:
                e.Handled = true;
                _logger.LogDebug("Partial match");
                // Do not clear the buffer
                break;

            case CommandCheckResult.MATCH:
                _buffer.Clear();
                e.Handled = true;
                _logger.LogDebug("Match");
                Receive(command);
                break;
        }

        _eventService.Raise(new BufferChangedEvent(string.Join(string.Empty, _buffer.Select(k => k.Char?.ToString() ?? "<null>"))));
    }

    protected override Task Handle(ICommand command) => command.Execute();
}

