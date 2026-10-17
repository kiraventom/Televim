using Avalonia.Input;
using Televim.Common.Events;
using Televim.Shell.Input;
using Televim.Vim;
using Televim.Vim.Actions;
using Televim.Vim.Actions.NormalMode;

namespace Televim.Shell.ViewModels;

internal class MainViewModel(IInputListener listener, IInputModeHolder inputModeHolder, IActionBuffer actionBuffer) 
    : BasePropertyChanged, 
    IMainViewModel, 
    IEventTarget<InputModeChangedEvent>,
    IEventTarget<ActionBufferChangedEvent>,
    IEventTarget<SelectRequestedEvent>,
    IEventTarget<MoveToTopEvent>,
    IEventTarget<MoveToLineEvent>,
    IEventTarget<MoveToBottomEvent>,
    IEventTarget<MoveUpEvent>,
    IEventTarget<MoveDownEvent>
{
    private readonly OpenChatViewModel _openChat = new OpenChatViewModel(inputModeHolder);

    public event Action<InputModes> InputModeChanged;

    public InputModes CurrentInputMode => inputModeHolder.CurrentMode;
    public string ActionBuffer => actionBuffer.Text;

    public string CommandText 
    { 
        get => field;
        set => SetAndRaise(ref field, value);
    }

    public IChatList ChatList { get; } = new ChatListViewModel();
    public IOpenChat OpenChat => _openChat;

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

    public Task Handle(SelectRequestedEvent @event)
    {
        if (_openChat.Chat is null)
            _openChat.Chat = ChatList.SelectedChat;

        return Task.CompletedTask;
    }

    public Task Handle(MoveToTopEvent @event)
    {
        if (OpenChat is null)
            ChatList.SelectFirst();

        return Task.CompletedTask;
    }

    public Task Handle(MoveToLineEvent @event)
    {
        if (OpenChat is null)
            ChatList.SelectByIndex(@event.Number);

        return Task.CompletedTask;
    }

    public Task Handle(MoveToBottomEvent @event)
    {
        if (OpenChat is null)
            ChatList.SelectLast();

        return Task.CompletedTask;
    }

    public Task Handle(MoveUpEvent @event)
    {
        if (OpenChat is null)
            ChatList.MoveSelectionUp();

        return Task.CompletedTask;
    }

    public Task Handle(MoveDownEvent @event)
    {
        if (OpenChat is null)
            ChatList.MoveSelectionDown();

        return Task.CompletedTask;
    }
}
