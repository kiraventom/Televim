using Microsoft.Extensions.Logging;

namespace Televim.Shell.Input.Commands;

internal class CommandDefinitionChecker(ILogger<CommandDefinitionChecker> logger, ICommandDefinition definition) : ICommandDefinitionChecker
{
    public ICommandDefinition Definition { get; } = definition;

    public CommandCheckResult TryMatch(CommandInput input)
    {
        if (input.NumberPrefix != 0 && !Definition.AllowNumberPrefix)
            return CommandCheckResult.NO_MATCH;

        if (!Definition.Mode.HasFlag(input.Mode))
            return CommandCheckResult.NO_MATCH;

        for (int i = 0; i < input.KeyStrokes.Count; ++i)
        {
            var inputKeyStroke = input.KeyStrokes[i];
            var definitionKeyStroke = Definition.KeyStrokes[i];

            if (inputKeyStroke.Key != definitionKeyStroke.Key || inputKeyStroke.Modifiers != definitionKeyStroke.Modifiers)
                return CommandCheckResult.NO_MATCH;
        }

        logger.LogDebug("input keystrokes count {inputCount} vs definition keystrokes count {defCount}", input.KeyStrokes.Count, Definition.KeyStrokes.Count);
        return input.KeyStrokes.Count == Definition.KeyStrokes.Count
            ? CommandCheckResult.MATCH
            : CommandCheckResult.PARTIAL_MATCH;
    }
}
