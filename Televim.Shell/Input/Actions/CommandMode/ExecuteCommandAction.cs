using Microsoft.Extensions.Logging;
using Televim.Core.Events;
using Televim.Shell.ViewModels;

namespace Televim.Shell.Input.Actions.CommandMode;

[ActionDefinition("<Enter>", InputModes.COMMAND)]
internal class ExecuteCommandAction(ILogger<ExecuteCommandAction> logger, IMainViewModel viewModel, IEventService eventService) : Action
{
    public override async Task Execute()
    {
        var commandText = viewModel.CommandText;
        viewModel.CommandText = null;

        var keyStrokes = KeyNotation.Parse(commandText);
        
        // TODO Command input builder + command handler here
        logger.LogDebug("Executed command {CommandText}", commandText);
        await eventService.Raise(new InputModeRequestedEvent(InputModes.NORMAL));
    }
}

