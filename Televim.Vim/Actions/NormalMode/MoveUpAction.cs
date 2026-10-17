using Microsoft.Extensions.Logging;
using Televim.Common.Events;

namespace Televim.Vim.Actions.NormalMode;

public readonly record struct MoveUpEvent(int Number);

[ActionDefinition("k", InputModes.NORMAL, AllowNumberPrefix = true)]
internal class MoveUpAction(ILogger<MoveUpAction> logger, IEventService eventService) : Action
{
    public override Task Execute()
    {
        return eventService.Raise(new MoveUpEvent(Input.NumberPrefix.GetValueOrDefault()));
    }
}


