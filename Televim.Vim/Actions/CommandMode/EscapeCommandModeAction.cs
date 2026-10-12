using Microsoft.Extensions.Logging;
using Televim.Core.Events;
using Televim.Vim.Commands;

namespace Televim.Vim.Actions.CommandMode;

[ActionDefinition("<Escape>", InputModes.COMMAND)]
internal class EscapeCommandModeAction(ILogger<EscapeCommandModeAction> logger, ICommandTextHolder commandTextHolder, IEventService eventService) : Action
{
    public override async Task Execute()
    {
        await eventService.Raise(new InputModeRequestedEvent(InputModes.NORMAL));
        commandTextHolder.CommandText = null;
        logger.LogDebug("Switched to normal mode");
    }
}
