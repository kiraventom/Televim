using Microsoft.Extensions.Logging;

namespace Televim.Shell.Input.Commands.Normal;

[CommandDefinition("G", InputMode.NORMAL, AllowNumberPrefix = true)]
internal class MoveToBottomCommand(ILogger<MoveToBottomCommand> logger) : Command
{
    public override Task Execute()
    {
        // TEMP
        if (Input.NumberPrefix == 0)
            logger.LogInformation("Move to bottom received");
        else
            logger.LogInformation("Move to line {number} received", Input.NumberPrefix);

        return Task.CompletedTask;
    }
}

