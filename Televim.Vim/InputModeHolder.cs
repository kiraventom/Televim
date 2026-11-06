using Microsoft.Extensions.Logging;
using Televim.Common.Events;

namespace Televim.Vim;

internal class InputModeHolder(ILogger<InputModeHolder> logger, IEventService eventService) : IInputModeHolder, IEventTarget<InputModeRequestedEvent>
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
                logger.LogError("Setting {Property} to {Value}, application is not expected to work properly", nameof(CurrentMode), Enum.GetName(value));

            field = value;
        }
    } = InputModes.NORMAL;

    public bool CurrentModeSupportsPrefixes()
    {
        return CurrentMode is InputModes.NORMAL or InputModes.VISUAL;
    }

    public async Task Handle(InputModeRequestedEvent @event)
    {
        var changing = new InputModeChangingEvent(@event.InputMode);
        await eventService.Raise(changing);
        if (changing.Cancel)
        {
            logger.LogDebug("Request to change InputMode to {inputMode} was cancelled, staying at {oldInputMode}", @event.InputMode, CurrentMode);
            return;
        }

        CurrentMode = @event.InputMode;
        await eventService.Raise(new InputModeChangedEvent(CurrentMode));
    }
}
