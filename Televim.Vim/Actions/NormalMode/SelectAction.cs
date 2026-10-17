using Microsoft.Extensions.Logging;
using Televim.Common.Events;

namespace Televim.Vim.Actions.NormalMode;

public readonly record struct SelectRequestedEvent();

[ActionDefinition("<Enter>", InputModes.NORMAL)]
internal class SelectAction(ILogger<SelectAction> logger, IEventService eventService) : Action
{
    public override async Task Execute()
    {
        await eventService.Raise(new SelectRequestedEvent());
    }
}


