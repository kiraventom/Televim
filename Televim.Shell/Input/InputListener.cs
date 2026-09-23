using Avalonia.Input;

namespace Televim.Shell.Input;

internal class InputListener(IInputHandler inputHandler) : IInputListener
{
    public void OnKeyDown(KeyEventArgs e) => inputHandler.Handle(e);
}
