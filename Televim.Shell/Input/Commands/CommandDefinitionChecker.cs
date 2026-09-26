namespace Televim.Shell.Input.Commands;

internal class CommandDefinitionChecker(ICommandDefinition definition)
{
    public ICommandDefinition Definition { get; } = definition;

    public CommandCheckResult TryMatch(CommandInput input)
    {
        if (input.NumberPrefix != 0 && !Definition.AllowNumberPrefix)
            return CommandCheckResult.NO_MATCH;

        if (input.Mode != Definition.Mode)
            return CommandCheckResult.NO_MATCH;

        for (int i = 0; i < input.KeyStrokes.Count; ++i)
        {
            var inputKeyStroke = input.KeyStrokes[i];
            var definitionKeyStroke = Definition.KeyStrokes[i];

            if (inputKeyStroke != definitionKeyStroke)
                return CommandCheckResult.NO_MATCH;
        }

        return input.KeyStrokes.Count == Definition.KeyStrokes.Count
            ? CommandCheckResult.MATCH
            : CommandCheckResult.PARTIAL_MATCH;
    }
}


