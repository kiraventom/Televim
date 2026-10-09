namespace Televim.Shell.Input;

[Flags]
public enum InputModes 
{ 
    INVALID = 0x0,
    NORMAL = 0x1,
    INSERT = 0x2,
    VISUAL = 0x4,
    COMMAND = 0x8
}
