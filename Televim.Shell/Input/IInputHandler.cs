using Avalonia.Input;

namespace Televim.Shell.Input;

internal interface IInputHandler
{
    const string ESCAPE_CHAR = "^";
    const string CTRL_SEQUENCE = ESCAPE_CHAR + "C";
    const string ALT_SEQUENCE = ESCAPE_CHAR + "A";
    const string META_SEQUENCE = ESCAPE_CHAR + "M";
    const string SHIFT_SEQUENCE = ESCAPE_CHAR + "S";

    void Handle(KeyEventArgs input);
}

