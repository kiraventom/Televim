using Microsoft.Extensions.Logging;

namespace Televim.Shell.Input.Commands;

internal class CommandService : ICommandService
{
    private readonly ILogger<CommandService> _logger;
    private readonly ICommandFactory _factory;
    private readonly IServiceProvider _sp;

    private readonly IReadOnlyCollection<CommandDefinitionChecker> _checkers;

    public CommandService(ILogger<CommandService> logger, ICommandFactory factory)
    {
        _logger = logger;
        _factory = factory;

        _checkers = _factory.Definitions.Select(d => new CommandDefinitionChecker(d)).ToList();
    }

    public CommandCheckResult TryGetCommand(CommandInput input, out ICommand command)
    {
        command = null;

        var results = _checkers.Select(c => new 
        { 
            Checker = c, 
            MatchResult = c.TryMatch(input), 
            CommandInput = input 
        }).ToList();

        var matches = results.Count(r => r.MatchResult == CommandCheckResult.MATCH);
        var partialMatches = results.Count(r => r.MatchResult == CommandCheckResult.PARTIAL_MATCH);

        if (matches == 0)
        {
            return partialMatches == 0 
                ? CommandCheckResult.NO_MATCH 
                : CommandCheckResult.PARTIAL_MATCH;
        }

        if (matches == 1)
        {
            var result = results.Single();
            command = _factory.Build(result.Checker.Definition);
            if (command is IInitableCommand initable)
                initable.Init(result.CommandInput);

            return CommandCheckResult.MATCH;
        }

        var matchedCommands = results
            .Where(r => r.MatchResult == CommandCheckResult.MATCH)
            .Select(r => r.Checker.Definition.Notation)
            .Select(n => $"\"{n}\"");

        _logger.LogError("Input matches to multiple commands: {commands}. Ignoring", string.Join(", ", matchedCommands));

        return CommandCheckResult.NO_MATCH;
    }
}

