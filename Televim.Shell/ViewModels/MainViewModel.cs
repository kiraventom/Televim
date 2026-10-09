using System.ComponentModel;
using Avalonia.Input;
using Televim.Core.Events;
using Televim.Shell.Input;

namespace Televim.Shell.ViewModels;

internal class MainViewModel(IInputListener listener) : IMainViewModel, INotifyPropertyChanged, IEventTarget<BufferChangedEvent>, IEventTarget<InputModeChangedEvent>
{
    public event PropertyChangedEventHandler PropertyChanged;
    public event Action<InputModes> InputModeChanged;

    public bool IsInsertMode { get; private set; }
    public string Buffer { get; private set; }

    public void OnKeyDown(KeyEventArgs e) => listener.OnKeyDown(e);

    public Task Handle(BufferChangedEvent @event)
    {
        Buffer = @event.Buffer;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Buffer)));
        return Task.CompletedTask;
    }

    public Task Handle(InputModeChangedEvent @event)
    {
        IsInsertMode = @event.InputMode == InputModes.INSERT;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(IsInsertMode)));
        InputModeChanged?.Invoke(@event.InputMode);
        return Task.CompletedTask;
    }
}
