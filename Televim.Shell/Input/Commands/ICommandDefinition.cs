namespace Televim.Shell.Input.Commands;

internal interface ICommandDefinition
{
    string Text { get; }
    InputMode Mode { get; }
    bool AllowNumberPrefix { get; }
}

