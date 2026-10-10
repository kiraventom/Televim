using Avalonia.Input;

namespace Televim.Shell.Input;

internal class InputListener(IKeyPressHandler inputHandler) : IInputListener
{
    public void OnKeyDown(KeyEventArgs e) => inputHandler.HandleKeyPress(e);
}
