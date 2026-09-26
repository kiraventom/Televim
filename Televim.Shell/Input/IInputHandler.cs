using Avalonia.Input;

namespace Televim.Shell.Input;

internal interface IInputHandler
{
    void Handle(KeyEventArgs input);
}

