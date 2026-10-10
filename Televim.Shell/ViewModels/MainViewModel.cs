using Avalonia.Input;
using Microsoft.Extensions.Logging;
using Televim.Core.Events;
using Televim.Shell.Input;

namespace Televim.Shell.ViewModels;

internal class MainViewModel(ILogger<MainViewModel> logger, IInputListener listener) 
    : BasePropertyChanged(logger), 
    IMainViewModel, 
    IEventTarget<BufferChangedEvent>, 
    IEventTarget<InputModeChangedEvent>
{
    public event Action<InputModes> InputModeChanged;

    public InputModes CurrentInputMode 
    {
    	get => field;
    	private set => SetAndRaise(ref field, value);
    }

    public string Buffer 
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

    public Task Handle(BufferChangedEvent @event)
    {
        Buffer = @event.Buffer;
        return Task.CompletedTask;
    }

    public Task Handle(InputModeChangedEvent @event)
    {
        CurrentInputMode = @event.InputMode;
        InputModeChanged?.Invoke(@event.InputMode);
        return Task.CompletedTask;
    }
}
