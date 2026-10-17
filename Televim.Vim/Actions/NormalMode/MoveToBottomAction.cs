using Microsoft.Extensions.Logging;
using Televim.Common.Events;

namespace Televim.Vim.Actions.NormalMode;

public readonly record struct MoveToBottomEvent();

[ActionDefinition("G", InputModes.NORMAL, AllowNumberPrefix = true)]
internal class MoveToBottomAction(ILogger<MoveToBottomAction> logger, IEventService eventService) : Action
{
    public override Task Execute()
    {
        if (Input.NumberPrefix is null)
            return eventService.Raise(new MoveToBottomEvent());

        return eventService.Raise(new MoveToLineEvent(Input.NumberPrefix.Value));
    }
}

