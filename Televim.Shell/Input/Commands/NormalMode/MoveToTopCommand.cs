using Microsoft.Extensions.Logging;

namespace Televim.Shell.Input.Commands.NormalMode;

[CommandDefinition("gg", InputModes.NORMAL, AllowNumberPrefix = true)]
internal class MoveToTopCommand(ILogger<MoveToTopCommand> logger) : Command
{
    public override Task Execute()
    {
        // TEMP
        if (Input.NumberPrefix == 0)
            logger.LogInformation("Moved to top");
        else
            logger.LogInformation("Moved to line {number}", Input.NumberPrefix);

        return Task.CompletedTask;
    }
}

