using Microsoft.Extensions.Logging;
using Televim.Common.Events;

namespace Televim.Vim.Actions.NormalMode;

public readonly record struct MoveDownEvent(int Number);

[ActionDefinition("j", InputModes.NORMAL, AllowNumberPrefix = true)]
internal class MoveDownAction(ILogger<MoveDownAction> logger, IEventService eventService) : Action
{
    public override Task Execute()
    {
        return eventService.Raise(new MoveDownEvent(Input.NumberPrefix.GetValueOrDefault()));
    }
}

