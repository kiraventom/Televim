using Microsoft.Extensions.Logging;
using Televim.Core.Events;
using Televim.Shell.ViewModels;

namespace Televim.Shell.Input.Commands.CommandMode;

[CommandDefinition("<Esc>", InputModes.COMMAND)]
internal class EscapeCommandModeCommand(ILogger<EscapeCommandModeCommand> logger, IMainViewModel viewModel, IEventService eventService) : Command
{
    public override async Task Execute()
    {
        await eventService.Raise(new InputModeRequestedEvent(InputModes.NORMAL));
        viewModel.CommandText = null;
        logger.LogDebug("Switched to normal mode");
    }
}
