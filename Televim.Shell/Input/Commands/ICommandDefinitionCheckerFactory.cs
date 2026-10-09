namespace Televim.Shell.Input.Commands;

internal interface ICommandDefinitionCheckerFactory
{
    ICommandDefinitionChecker Build(ICommandDefinition definition);
}
