namespace Televim.Shell.Input.Commands;

internal interface ICommandFactory
{
    IReadOnlyCollection<ICommandDefinition> Definitions { get; }
    ICommand Build(ICommandDefinition definition);
}

