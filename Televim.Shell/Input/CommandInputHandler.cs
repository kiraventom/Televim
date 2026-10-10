using Microsoft.Extensions.Logging;
using Televim.Core.Utils;
using Televim.Shell.Input.Commands;

namespace Televim.Shell.Input;

// Using ChannelHandler here so we can synchonously return if input is handled
// and then run the command asynchronously
internal class CommandInputHandler(ILogger<CommandInputHandler> logger, ICommandService commandService) : ChannelHandler<ICommand>, ICommandInputHandler
{
    public (bool IsHandled, bool ExpectFollowUp) Handle(CommandInput input)
    {
        var result = commandService.TryGetCommand(input, out var command);

        switch (result)
        {
            case CommandCheckResult.NO_MATCH:
                logger.LogDebug("No match");
                return (IsHandled: false, ExpectFollowUp: false);

            case CommandCheckResult.PARTIAL_MATCH:
                logger.LogDebug("Partial match");
                return (IsHandled: true, ExpectFollowUp: true);

            case CommandCheckResult.MATCH:
                logger.LogDebug("Match");
                Receive(command);
                return (IsHandled: true, ExpectFollowUp: false);

            default:
                throw new NotSupportedException($"Unexpected {nameof(CommandCheckResult)} with value {result}");
        }
    }

    protected override Task Handle(ICommand command) => command.Execute();
}


