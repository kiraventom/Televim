using Microsoft.Extensions.Logging;
using Televim.Core.Events;

namespace Televim.Shell.Input.Commands.NormalMode;

[CommandDefinition(":", InputModes.NORMAL)]
internal class CommandModeCommand(ILogger<CommandModeCommand> logger, IEventService eventService) : Command
{
    public override async Task Execute()
    {
        await eventService.Raise(new InputModeRequestedEvent(InputModes.COMMAND));
        logger.LogDebug("Switched to command mode");
    }
}
