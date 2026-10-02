namespace Televim.Shell.Input.Commands;

internal interface ICommandFactory
{
    IReadOnlyList<ICommandDefinition> Definitions { get; }
    ICommand Build(ICommandDefinition definition);
}

