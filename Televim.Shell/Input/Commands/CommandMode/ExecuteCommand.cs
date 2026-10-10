using Microsoft.Extensions.Logging;
using Televim.Core.Events;
using Televim.Shell.ViewModels;

namespace Televim.Shell.Input.Commands.CommandMode;

[CommandDefinition("<Enter>", InputModes.COMMAND)]
internal class ExecuteCommand(ILogger<ExecuteCommand> logger, IMainViewModel viewModel, IEventService eventService, ICommandInputBuilder inputBuilder, ICommandInputHandler inputHandler) : Command
{
    public override async Task Execute()
    {
        var commandText = viewModel.CommandText;
        viewModel.CommandText = null;

        var keyStrokes = KeyNotation.Parse(commandText);
        var commandInput = inputBuilder.Build(keyStrokes);
        
        // TODO Use special command input handler and different commands here
        logger.LogDebug("Executed command {CommandText}", commandText);
        await eventService.Raise(new InputModeRequestedEvent(InputModes.NORMAL));
    }
}

