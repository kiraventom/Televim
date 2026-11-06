using Microsoft.Extensions.Logging;

namespace Televim.Vim.Actions.NormalMode;

[ActionDefinition("gT", InputModes.NORMAL, AllowNumberPrefix = true)]
internal class PrevTabAction(ILogger<PrevTabAction> logger) : Action
{
    public override Task Execute()
    {
        if (Input.NumberPrefix is not {} number)
            logger.LogInformation("Moved to previous tab");
        else
            logger.LogInformation("Moved back {num} tabs", number);

        return Task.CompletedTask;
    }
}
