using Microsoft.Extensions.Logging;
using Televim.Common.Events;

namespace Televim.Vim.Actions.InsertMode;

[ActionDefinition("<Escape>", InputModes.INSERT)]
internal class EscapeInsertModeAction(ILogger<EscapeInsertModeAction> logger, IEventService eventService) : Action
{
    public override async Task Execute()
    {
        await eventService.Raise(new InputModeRequestedEvent(InputModes.NORMAL));
        logger.LogDebug("Switched to normal mode");
    }
}
