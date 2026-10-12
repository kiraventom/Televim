using Microsoft.Extensions.Logging;

namespace Televim.Vim.Actions;

internal class ActionDefinitionChecker(ILogger<ActionDefinitionChecker> logger, IActionDefinition definition) : IActionDefinitionChecker
{
    public IActionDefinition Definition { get; } = definition;

    public ActionCheckResult TryMatch(ActionInput input)
    {
        if (input.NumberPrefix != 0 && !Definition.AllowNumberPrefix)
            return ActionCheckResult.NO_MATCH;

        if (!Definition.Mode.HasFlag(input.Mode))
            return ActionCheckResult.NO_MATCH;

        if (input.KeyStrokes.Count > Definition.KeyStrokes.Count)
            return ActionCheckResult.NO_MATCH;

        for (int i = 0; i < input.KeyStrokes.Count; ++i)
        {
            var inputKeyStroke = input.KeyStrokes[i];
            var definitionKeyStroke = Definition.KeyStrokes[i];

            if (!inputKeyStroke.Matches(definitionKeyStroke))
                return ActionCheckResult.NO_MATCH;
        }

        logger.LogDebug("input keystrokes count {inputCount} vs definition keystrokes count {defCount}", input.KeyStrokes.Count, Definition.KeyStrokes.Count);

        return input.KeyStrokes.Count == Definition.KeyStrokes.Count
            ? ActionCheckResult.MATCH
            : ActionCheckResult.PARTIAL_MATCH;
    }
}
