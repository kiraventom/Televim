using Microsoft.Extensions.Logging;

namespace Televim.Shell.Input.Actions.NormalMode;

[ActionDefinition("G", InputModes.NORMAL, AllowNumberPrefix = true)]
internal class MoveToBottomAction(ILogger<MoveToBottomAction> logger) : Action
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

