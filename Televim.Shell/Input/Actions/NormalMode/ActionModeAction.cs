using Microsoft.Extensions.Logging;
using Televim.Core.Events;

namespace Televim.Shell.Input.Actions.NormalMode;

[ActionDefinition(":", InputModes.NORMAL)]
internal class CommandModeAction(ILogger<CommandModeAction> logger, IEventService eventService) : Action
{
    public override async Task Execute()
    {
        await eventService.Raise(new InputModeRequestedEvent(InputModes.COMMAND));
        logger.LogDebug("Switched to command mode");
    }
}
