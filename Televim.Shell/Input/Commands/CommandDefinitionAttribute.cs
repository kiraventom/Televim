namespace Televim.Shell.Input.Commands;

[AttributeUsage(AttributeTargets.Class)]
internal class CommandDefinitionAttribute(string text, InputMode mode) : Attribute, ICommandDefinition
{
    public string Text { get; set; } = text;
    public InputMode Mode { get; } = mode;
    public bool AllowNumberPrefix { get; init; }

    internal bool IsValid()
    {
        var isInvalid = string.IsNullOrEmpty(Text)
            || Text.Any(c => char.IsDigit(c))
            || Text.Contains(IInputHandler.ESCAPE_CHAR) 
               && new[] 
               { 
                   IInputHandler.CTRL_SEQUENCE, 
                   IInputHandler.ALT_SEQUENCE, 
                   IInputHandler.META_SEQUENCE, 
                   IInputHandler.SHIFT_SEQUENCE 
               }
               .All(s => !Text.Contains(s));

        return !isInvalid;
    }
}

