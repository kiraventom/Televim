using Microsoft.Extensions.Logging;
using Televim.Core.Events;

namespace Televim.Shell.Input.Actions.InsertMode;

[ActionDefinition("<Esc>", InputModes.INSERT)]
internal class EscapeInsertModeAction(ILogger<EscapeInsertModeAction> logger, IEventService eventService) : Action
{
    public override async Task Execute()
    {
        await eventService.Raise(new InputModeRequestedEvent(InputModes.NORMAL));
        logger.LogDebug("Switched to normal mode");
    }
}
