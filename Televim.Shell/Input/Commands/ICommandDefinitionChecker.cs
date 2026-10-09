namespace Televim.Shell.Input.Commands;

internal interface ICommandDefinitionChecker
{
    ICommandDefinition Definition { get; }
    CommandCheckResult TryMatch(CommandInput input);
}

