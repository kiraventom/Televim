using Avalonia.Input;

namespace Televim.Shell.Input;

internal interface IKeyPressHandler
{
    void HandleKeyPress(KeyEventArgs input);
}

