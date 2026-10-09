using Microsoft.Extensions.Logging;
using Televim.Core.Events;

namespace Televim.Shell.Input.Commands.NormalMode;

[CommandDefinition("i", InputModes.NORMAL)]
internal class InsertModeCommand(ILogger<InsertModeCommand> logger, IEventService eventService) : Command
{
    public override async Task Execute()
    {
        await eventService.Raise(new InputModeSetRequestedEvent(InputModes.INSERT));
        logger.LogDebug("Switched to insert mode");
    }
}
