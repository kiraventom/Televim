using Microsoft.Extensions.Logging;

namespace Televim.Shell.Input.Commands.Normal;

[CommandDefinition("gg", InputMode.NORMAL, AllowNumberPrefix = true)]
internal class MoveToTopCommand(ILogger<MoveToTopCommand> logger) : Command
{
    public override Task Execute()
    {
        // TEMP
        if (Input.NumberPrefix == 0)
            logger.LogInformation("Move to top received");
        else
            logger.LogInformation("Move to line {number} received", Input.NumberPrefix);

        return Task.CompletedTask;
    }
}

