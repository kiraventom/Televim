using Microsoft.Extensions.Logging;
using Televim.Common.Events;

namespace Televim.Vim.Actions.InsertMode;

[ActionDefinition("<Escape>", InputModes.INSERT)]
internal class EscapeInsertModeAction(ILogger<EscapeInsertModeAction> logger, IEventService eventService) : Action
{
    public override async Task Execute()
    {
        logger.LogDebug("Requesting to switch to normal mode");
        await eventService.Raise(new InputModeRequestedEvent(InputModes.NORMAL));
    }
}
