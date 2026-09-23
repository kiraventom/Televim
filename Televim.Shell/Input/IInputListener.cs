using Avalonia.Input;

namespace Televim.Shell.Input;

internal interface IInputListener
{
    void OnKeyDown(KeyEventArgs e);
}


