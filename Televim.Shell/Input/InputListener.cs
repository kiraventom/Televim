using Avalonia.Input;
using Televim.Core.Utils;

namespace Televim.Shell.Input;

internal class InputListener(IInputHandler inputHandler) : ChannelHandler<KeyEventArgs>, IInputListener
{
    public void OnKeyDown(KeyEventArgs e) => Receive(e);
    protected override Task Handle(KeyEventArgs e) => inputHandler.Handle(e);
}


