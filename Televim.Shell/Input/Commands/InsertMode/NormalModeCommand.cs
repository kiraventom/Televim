using Microsoft.Extensions.Logging;
using Televim.Core.Events;

namespace Televim.Shell.Input.Commands.InsertMode;

[CommandDefinition("<Esc>", InputModes.INSERT)]
internal class EscapeInsertModeCommand(ILogger<EscapeInsertModeCommand> logger, IEventService eventService) : Command
{
    public override async Task Execute()
    {
        await eventService.Raise(new InputModeRequestedEvent(InputModes.NORMAL));
        logger.LogDebug("Switched to normal mode");
    }
}
