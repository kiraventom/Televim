using Microsoft.Extensions.Logging;

namespace Televim.Vim.Actions.NormalMode;

[ActionDefinition("gt", InputModes.NORMAL, AllowNumberPrefix = true)]
internal class NextTabAction(ILogger<NextTabAction> logger) : Action
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
