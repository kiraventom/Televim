using Televim.Core.Events;
using Televim.Shell.Input.Commands.Normal;

namespace Televim.Shell.Input;

internal class InputModeHolder : IInputModeHolder, IEventTarget<InputModeSetRequestedEvent>
{
    public InputMode CurrentMode { get; private set; }

    public Task Handle(InputModeSetRequestedEvent @event)
    {
        CurrentMode = @event.InputMode;
        return Task.CompletedTask;
    }
}
