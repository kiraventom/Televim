using Microsoft.Extensions.Logging;

namespace Televim.Shell.Input.Commands.NormalMode;

[CommandDefinition("G", InputModes.NORMAL, AllowNumberPrefix = true)]
internal class MoveToBottomCommand(ILogger<MoveToBottomCommand> logger) : Command
{
    public override Task Execute()
    {
        // TEMP
        if (Input.NumberPrefix == 0)
            logger.LogInformation("Moved to bottom");
        else
            logger.LogInformation("Moved to line {number}", Input.NumberPrefix);

        return Task.CompletedTask;
    }
}

