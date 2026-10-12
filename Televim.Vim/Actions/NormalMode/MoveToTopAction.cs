using Microsoft.Extensions.Logging;

namespace Televim.Vim.Actions.NormalMode;

[ActionDefinition("gg", InputModes.NORMAL, AllowNumberPrefix = true)]
internal class MoveToTopAction(ILogger<MoveToTopAction> logger) : Action
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

