using Microsoft.Extensions.Logging;

namespace Televim.Shell.Input.Actions;

internal class ActionService : IActionService
{
    private readonly ILogger<ActionService> _logger;
    private readonly IActionFactory _actionFactory;

    private readonly IReadOnlyCollection<IActionDefinitionChecker> _checkers;

    public ActionService(ILogger<ActionService> logger, IActionFactory actionFactory, IActionDefinitionCheckerFactory checkerFactory)
    {
        _logger = logger;
        _actionFactory = actionFactory;

        _checkers = _actionFactory.Definitions.Select(d => checkerFactory.Build(d)).ToList();
    }

    public ActionCheckResult TryGetAction(ActionInput input, out IAction action)
    {
        action = null;

        var results = _checkers.Select(c => new 
        { 
            Checker = c, 
            MatchResult = c.TryMatch(input), 
            ActionInput = input 
        }).ToList();

        var matches = results.Count(r => r.MatchResult == ActionCheckResult.MATCH);
        var partialMatches = results.Count(r => r.MatchResult == ActionCheckResult.PARTIAL_MATCH);

        if (matches == 0)
        {
            return partialMatches == 0 
                ? ActionCheckResult.NO_MATCH 
                : ActionCheckResult.PARTIAL_MATCH;
        }

        if (matches == 1)
        {
            var result = results.Single(r => r.MatchResult == ActionCheckResult.MATCH);
            action = _actionFactory.Build(result.Checker.Definition);
            if (action is IInitableAction initable)
                initable.Init(result.ActionInput);

            return ActionCheckResult.MATCH;
        }

        var matchedActions = results
            .Where(r => r.MatchResult == ActionCheckResult.MATCH)
            .Select(r => r.Checker.Definition.Notation)
            .Select(n => $"\"{n}\"");

        _logger.LogError("Input matches to multiple actions: {actions}. Ignoring", string.Join(", ", matchedActions));

        return ActionCheckResult.NO_MATCH;
    }
}

