using Microsoft.Extensions.Logging;
using Televim.Core.Events;

namespace Televim.Shell.Input.Commands.Normal;

internal readonly record struct InputModeSetRequestedEvent(InputMode InputMode);

[CommandDefinition("i", InputMode.NORMAL)]
internal class InsertModeCommand(ILogger<InsertModeCommand> logger, IEventService eventService) : Command
{
    public override async Task Execute()
    {
        await eventService.Raise(new InputModeSetRequestedEvent(InputMode.INSERT));
    }
}
