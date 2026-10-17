using Microsoft.Extensions.Logging;
using Televim.Common.Events;

namespace Televim.Vim.Actions.NormalMode;

public readonly record struct DeselectRequestedEvent();

[ActionDefinition("<Escape>", InputModes.NORMAL)]
internal class DeselectAction(ILogger<DeselectAction> logger, IEventService eventService) : Action
{
    public override async Task Execute()
    {
        await eventService.Raise(new DeselectRequestedEvent());
    }
}
