using Microsoft.Extensions.Logging;
using Televim.Common.Events;

namespace Televim.Vim.Actions.NormalMode;

[ActionDefinition(":", InputModes.NORMAL)]
internal class CommandModeAction(ILogger<CommandModeAction> logger, IEventService eventService) : Action
{
    public override async Task Execute()
    {
        logger.LogDebug("Requesting to switch to command mode");
        await eventService.Raise(new InputModeRequestedEvent(InputModes.COMMAND));
    }
}
