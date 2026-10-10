using Microsoft.Extensions.Logging;
using Televim.Core.Events;
using Televim.Shell.ViewModels;

namespace Televim.Shell.Input.Actions.CommandMode;

[ActionDefinition("<Esc>", InputModes.COMMAND)]
internal class EscapeCommandModeAction(ILogger<EscapeCommandModeAction> logger, IMainViewModel viewModel, IEventService eventService) : Action
{
    public override async Task Execute()
    {
        await eventService.Raise(new InputModeRequestedEvent(InputModes.NORMAL));
        viewModel.CommandText = null;
        logger.LogDebug("Switched to normal mode");
    }
}
