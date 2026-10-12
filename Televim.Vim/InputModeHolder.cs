using Microsoft.Extensions.Logging;
using Televim.Core.Events;

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
                logger.LogError("Setting {Property} to {Value}, application is not expected to work properly", nameof(CurrentMode), Enum.GetName(field));

            field = value;
        }
    } = InputModes.NORMAL;

    public bool CurrentModeSupportsPrefixes()
    {
        return CurrentMode is InputModes.NORMAL or InputModes.VISUAL;
    }

    public async Task Handle(InputModeRequestedEvent @event)
    {
        CurrentMode = @event.InputMode;
        await eventService.Raise(new InputModeChangedEvent(CurrentMode));
    }
}
