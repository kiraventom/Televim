using Microsoft.Extensions.Logging;
using Televim.Common.Events;

namespace Televim.Vim.Actions.NormalMode;

public readonly record struct MoveToTopEvent();
public readonly record struct MoveToLineEvent(int Number);

[ActionDefinition("gg", InputModes.NORMAL, AllowNumberPrefix = true)]
internal class MoveToTopAction(ILogger<MoveToTopAction> logger, IEventService eventService) : Action
{
    public override Task Execute()
    {
        if (Input.NumberPrefix is null)
            return eventService.Raise(new MoveToTopEvent());

        return eventService.Raise(new MoveToLineEvent(Input.NumberPrefix.Value));
    }
}


