using Avalonia.Input;
using Microsoft.Extensions.Logging;
using Televim.Core.Events;
using Televim.Shell.Input;

namespace Televim.Shell.ViewModels;

internal class MainViewModel(ILogger<MainViewModel> logger, IInputListener listener, IInputModeHolder inputModeHolder) 
    : BasePropertyChanged(logger), 
    IMainViewModel, 
    IEventTarget<ActionBufferChangedEvent>, 
    IEventTarget<InputModeChangedEvent>
{
    public event Action<InputModes> InputModeChanged;

    public InputModes CurrentInputMode => inputModeHolder.CurrentMode;

    public string ActionBuffer 
    { 
        get => field;
        private set => SetAndRaise(ref field, value);
    }

    public string CommandText 
    { 
        get => field;
        set => SetAndRaise(ref field, value);
    }

    public void OnKeyDown(KeyEventArgs e) => listener.OnKeyDown(e);

    public Task Handle(ActionBufferChangedEvent @event)
    {
        ActionBuffer = @event.Buffer;
        return Task.CompletedTask;
    }

    public Task Handle(InputModeChangedEvent @event)
    {
        RaisePropertyChanged(nameof(CurrentInputMode));
        InputModeChanged?.Invoke(@event.InputMode);
        return Task.CompletedTask;
    }
}
