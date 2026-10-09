using Microsoft.Extensions.Logging;

namespace Televim.Shell.Input.Commands.NormalMode;

[CommandDefinition("gt", InputModes.NORMAL, AllowNumberPrefix = true)]
internal class NextTabCommand(ILogger<NextTabCommand> logger) : Command
{
    public override Task Execute()
    {
        if (Input.NumberPrefix == 0)
            logger.LogInformation("Moved to next tab");
        else
            logger.LogInformation("Move to tab number {num}", Input.NumberPrefix);

        return Task.CompletedTask;
    }
}
