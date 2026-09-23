namespace Televim.Shell.Input.Commands;

internal class CommandDefinitionChecker(ICommandDefinition definition)
{
    public ICommandDefinition Definition { get; } = definition;

    public CommandCheckResult TryMatch(string rawInput, out CommandInput input)
    {
        if (!CommandInput.TryParse(rawInput, out input))
            return CommandCheckResult.NO_MATCH;

        if (input.NumberPrefix != 0 && !Definition.AllowNumberPrefix)
            return CommandCheckResult.NO_MATCH;

        // if (CurrentMode != Definition.Mode)
            // return DefinitionCheckResult.NO_MATCH;

        if (input.Text == Definition.Text)
            return CommandCheckResult.MATCH;

        if (Definition.Text.StartsWith(input.Text))
            return CommandCheckResult.PARTIAL_MATCH;

        return CommandCheckResult.NO_MATCH;
    }
}


