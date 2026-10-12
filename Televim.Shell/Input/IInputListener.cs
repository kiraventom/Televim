using Avalonia.Input;

namespace Televim.Vim;

internal interface IInputListener
{
    void OnKeyDown(KeyEventArgs e);
}
