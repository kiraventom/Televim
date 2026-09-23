using System.Text;
using Avalonia.Input;
using Televim.Core.Events;
using Televim.Shell.Input.Commands;

namespace Televim.Shell.Input;

internal class InputHandler : IInputHandler
{
    private readonly StringBuilder _buffer = new();
    private readonly IEventService _eventService;
    private readonly ICommandService _commandService;

    public InputMode CurrentMode { get; private set; } = InputMode.NORMAL;

    public InputHandler(IEventService eventService, ICommandService commandService)
    {
        _eventService = eventService;
        _commandService = commandService;
    }

    public async Task Handle(KeyEventArgs input)
    {
        if (input.KeyModifiers.HasFlag(KeyModifiers.Control))
            _buffer.Append(IInputHandler.CTRL_SEQUENCE);

        if (input.KeyModifiers.HasFlag(KeyModifiers.Alt))
            _buffer.Append(IInputHandler.ALT_SEQUENCE);

        if (input.KeyModifiers.HasFlag(KeyModifiers.Meta))
            _buffer.Append(IInputHandler.META_SEQUENCE);

        if (input.KeyModifiers.HasFlag(KeyModifiers.Meta) && input.KeySymbol is null)
            _buffer.Append(IInputHandler.SHIFT_SEQUENCE);

        var str = input.KeySymbol ?? Enum.GetName<Key>(input.Key);
        _buffer.Append(str);

        var fullText = _buffer.ToString();
        var result = _commandService.TryGetCommand(fullText, out var command);

        switch (result)
        {
            case CommandCheckResult.NO_MATCH:
                _buffer.Clear();
                // Do not handle the event
                break;

            case CommandCheckResult.PARTIAL_MATCH:
                input.Handled = true;
                // Do not clear the buffer
                break;

            case CommandCheckResult.MATCH:
                _buffer.Clear();
                await _eventService.Raise(new CommandDetectedEvent(command));
                input.Handled = true;
                break;
        }
    }
}

