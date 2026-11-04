using Avalonia.Input;
using Televim.Common.Events;
using Televim.Shell.Input;
using Televim.Vim;
using Televim.Vim.Actions;

namespace Televim.Shell.ViewModels;

internal class MainViewModel(IEventService eventService, IInputListener listener, IInputModeHolder inputModeHolder, IChatListViewModel chatListVm, IOpenChatViewModel openChatVm, IActionBuffer actionBuffer) 
    : BasePropertyChanged, 
    IMainViewModel, 
    IEventTarget<InputModeChangedEvent>,
    IEventTarget<ActionBufferChangedEvent>,
    IEventTarget<OpenChatRequestedEvent>,
    IEventTarget<CloseChatRequestedEvent>
{
    public event Action<InputModes> InputModeChanged;

    public InputModes CurrentInputMode => inputModeHolder.CurrentMode;
    public string ActionBuffer => actionBuffer.Text;

    public string CommandText 
    { 
        get => field;
        set => SetAndRaise(ref field, value);
    }

    public IChatListViewModel ChatList => chatListVm;
    public IOpenChatViewModel OpenChat => openChatVm;

    public FocusableViewModel FocusedViewModel
    {
        get => field;
        set
        {
            if (field is {} oldVm)
            {
                eventService.Ignore(oldVm);
                oldVm.IsFocused = false;
            }

            field = value;

            if (field is {} newVm)
            {
                eventService.StopIgnore(newVm);
                newVm.IsFocused = true;
            }
        }
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

    // TODO Rework this into something universal?
    // maybe use same events? vim's for escape/selecg
    public Task Handle(OpenChatRequestedEvent @event)
    {
        // TODO
        FocusedViewModel = (FocusableViewModel)OpenChat;
        OpenChat.SelectChat(@event.Chat);
        return Task.CompletedTask;
    }

    public Task Handle(CloseChatRequestedEvent @event)
    {
        FocusedViewModel = (FocusableViewModel)ChatList;
        return Task.CompletedTask;
    }
}
