using Microsoft.Extensions.Logging;

namespace Televim.Shell.Input.Commands.NormalMode;

[CommandDefinition("gT", InputModes.NORMAL, AllowNumberPrefix = true)]
internal class PrevTabCommand(ILogger<PrevTabCommand> logger) : Command
{
    public override Task Execute()
    {
        if (Input.NumberPrefix == 0)
            logger.LogInformation("Moved to previous tab");
        else
            logger.LogInformation("Moved back {num} tabs", Input.NumberPrefix);

        return Task.CompletedTask;
    }
}



