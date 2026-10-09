using Microsoft.Extensions.Logging;
using Televim.Core.Events;

namespace Televim.Shell.Input;

internal class InputModeHolder(ILogger<InputModeHolder> logger, IEventService eventService) : IInputModeHolder, IEventTarget<InputModeSetRequestedEvent>
{
    public InputModes CurrentMode 
    {
        get
        {
            if (field == InputModes.INVALID)
                logger.LogError("{Property} is set to {Value}, application is not expected to work properly", nameof(CurrentMode), Enum.GetName(field));

            return field;
        }
        private set
        {
            if (value == InputModes.INVALID)
                logger.LogError("Setting {Property} to {Value}, application is not expected to work properly", nameof(CurrentMode), Enum.GetName(field));

            field = value;
        }
    } = InputModes.NORMAL;

    public async Task Handle(InputModeSetRequestedEvent @event)
    {
        CurrentMode = @event.InputMode;
        await eventService.Raise(new InputModeChangedEvent(CurrentMode));
    }
}
