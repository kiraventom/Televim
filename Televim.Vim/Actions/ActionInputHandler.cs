using Microsoft.Extensions.Logging;
using Televim.Common.Utils;
using Televim.Vim.Actions;

namespace Televim.Vim;

// Using ChannelHandler here so we can synchonously return if input is handled
// and then run the action asynchronously
internal class ActionInputHandler(ILogger<ActionInputHandler> logger, IActionService actionService) : ChannelHandler<IAction>(logger), IActionInputHandler
{
    public (bool IsHandled, bool ExpectFollowUp) Handle(ActionInput input)
    {
        var result = actionService.TryGetAction(input, out var action);

        switch (result)
        {
            case ActionCheckResult.NO_MATCH:
                logger.LogDebug("No match");
                return (IsHandled: false, ExpectFollowUp: false);

            case ActionCheckResult.PARTIAL_MATCH:
                logger.LogDebug("Partial match");
                return (IsHandled: true, ExpectFollowUp: true);

            case ActionCheckResult.MATCH:
                logger.LogDebug("Match");
                Receive(action);
                return (IsHandled: true, ExpectFollowUp: false);

            default:
                throw new NotSupportedException($"Unexpected {nameof(ActionCheckResult)} with value {result}");
        }
    }

    protected override async Task Handle(IAction action)
    {
        try
        {
            await action.Execute();
        }
        catch (Exception ex)
        {
            logger.LogError("Exception thrown during executing action \"{type}\": {ex}", action.GetType().FullName, ex);
        }
    }
}


