using System.ComponentModel;
using Avalonia.Input;
using Televim.Core.Events;
using Televim.Shell.Input;

namespace Televim.Shell.ViewModels;

internal class MainViewModel(IInputListener listener) : IMainViewModel, INotifyPropertyChanged, IEventTarget<BufferChangedEvent>
{
    public event PropertyChangedEventHandler PropertyChanged;

    public string Buffer { get; private set; }

    public void OnKeyDown(KeyEventArgs e) => listener.OnKeyDown(e);

    public Task Handle(BufferChangedEvent @event)
    {
        Buffer = @event.Buffer;
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Buffer)));
        return Task.CompletedTask;
    }
}
