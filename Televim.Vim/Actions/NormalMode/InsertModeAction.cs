using Microsoft.Extensions.Logging;
using Televim.Common.Events;

namespace Televim.Vim.Actions.NormalMode;

[ActionDefinition("i", InputModes.NORMAL)]
internal class InsertModeAction(ILogger<InsertModeAction> logger, IEventService eventService) : Action
{
    public override async Task Execute()
    {
        logger.LogDebug("Requesting to switch to insert mode");
        await eventService.Raise(new InputModeRequestedEvent(InputModes.INSERT));
    }
}
