using Avalonia.Input;
using Televim.Core.Events;
using Televim.Core.Utils;
using Televim.Shell.Input.Commands;

namespace Televim.Shell.Input;

internal class InputHandler : ChannelHandler<ICommand>, IInputHandler
{
    private readonly List<KeyStroke> _buffer = [];
    private readonly IEventService _eventService;
    private readonly ICommandService _commandService;
    private readonly ICommandInputBuilder _inputBuilder;

    public InputHandler(IEventService eventService, ICommandService commandService, ICommandInputBuilder inputBuilder)
    {
        _eventService = eventService;
        _inputBuilder = inputBuilder;
        _commandService = commandService;
    }

    public void Handle(KeyEventArgs e)
    {
        var keyStroke = KeyStroke.From(e);
        _buffer.Add(keyStroke);

        var input = _inputBuilder.Build(_buffer);
        var result = _commandService.TryGetCommand(_buffer, out var command);

        switch (result)
        {
            case CommandCheckResult.NO_MATCH:
                _buffer.Clear();
                // Do not handle the event
                break;

            case CommandCheckResult.PARTIAL_MATCH:
                e.Handled = true;
                // Do not clear the buffer
                break;

            case CommandCheckResult.MATCH:
                _buffer.Clear();
                e.Handled = true;
                Receive(command);
                break;
        }
    }

    protected override Task Handle(ICommand command) => _eventService.Raise(command);
}

