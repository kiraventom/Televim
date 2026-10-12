using Microsoft.Extensions.Logging;
using Televim.Core.Events;

namespace Televim.Vim.Actions.NormalMode;

[ActionDefinition("i", InputModes.NORMAL)]
internal class InsertModeAction(ILogger<InsertModeAction> logger, IEventService eventService) : Action
{
    public override async Task Execute()
    {
        await eventService.Raise(new InputModeRequestedEvent(InputModes.INSERT));
        logger.LogDebug("Switched to insert mode");
    }
}
