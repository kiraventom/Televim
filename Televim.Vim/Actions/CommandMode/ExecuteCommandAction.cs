using Microsoft.Extensions.Logging;
using Televim.Common.Events;
using Televim.Vim.Commands;

namespace Televim.Vim.Actions.CommandMode;

[ActionDefinition("<Enter>", InputModes.COMMAND)]
internal class ExecuteCommandAction(ILogger<ExecuteCommandAction> logger, ICommandTextHolder commandTextHolder, IEventService eventService) : Action
{
    public override async Task Execute()
    {
        var commandText = commandTextHolder.CommandText;
        commandTextHolder.CommandText = null;

        var keyStrokes = KeyNotation.Parse(commandText);
        
        // TODO Command input builder + command handler here
        logger.LogDebug("Executed command {CommandText}", commandText);
        await eventService.Raise(new InputModeRequestedEvent(InputModes.NORMAL));
    }
}

