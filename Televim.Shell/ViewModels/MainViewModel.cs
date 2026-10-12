using Avalonia.Input;
using Microsoft.Extensions.Logging;
using Televim.Core.Events;
using Televim.Vim;
using Televim.Vim.Actions;

namespace Televim.Shell.ViewModels;

internal class MainViewModel(ILogger<MainViewModel> logger, IInputListener listener, IInputModeHolder inputModeHolder, IActionBuffer actionBuffer) 
    : BasePropertyChanged(logger), 
    IMainViewModel, 
    IEventTarget<InputModeChangedEvent>,
    IEventTarget<ActionBufferChangedEvent>
{
    public event Action<InputModes> InputModeChanged;

    public InputModes CurrentInputMode => inputModeHolder.CurrentMode;
    public string ActionBuffer => actionBuffer.Text;

    public string CommandText 
    { 
        get => field;
        set => SetAndRaise(ref field, value);
    }

    public void OnKeyDown(KeyEventArgs e) => listener.OnKeyDown(e);

    public Task Handle(InputModeChangedEvent @event)
    {
        RaisePropertyChanged(nameof(CurrentInputMode));
        InputModeChanged?.Invoke(@event.InputMode);
        return Task.CompletedTask;
    }

    public Task Handle(ActionBufferChangedEvent @event)
    {
        RaisePropertyChanged(nameof(ActionBuffer));
        return Task.CompletedTask;
    }
}
