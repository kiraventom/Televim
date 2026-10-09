using Microsoft.Extensions.Logging;
using Televim.Core.Events;

namespace Televim.Shell.Input.Commands.InsertMode;

[CommandDefinition("<Esc>", InputModes.INSERT | InputModes.COMMAND | InputModes.VISUAL)]
internal class NormalModeCommand(ILogger<NormalModeCommand> logger, IEventService eventService) : Command
{
    public override async Task Execute()
    {
        await eventService.Raise(new InputModeSetRequestedEvent(InputModes.NORMAL));
        logger.LogDebug("Switched to normal mode");
    }
}

