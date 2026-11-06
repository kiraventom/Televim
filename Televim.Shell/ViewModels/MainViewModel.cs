using Avalonia.Input;
using Microsoft.Extensions.Logging;
using Televim.Common.Events;
using Televim.Shell.Input;
using Televim.Vim;
using Televim.Vim.Actions;

namespace Televim.Shell.ViewModels;

internal readonly record struct FocusRequestedEvent(IFocusableViewModel Sender);
internal readonly record struct UnfocusRequestedEvent();

internal class MainViewModel(ILogger<MainViewModel> logger, IEventService eventService, IInputListener listener, IInputModeHolder inputModeHolder, IChatListViewModel chatListVm, IOpenChatViewModel openChatVm, IActionBuffer actionBuffer) 
    : BasePropertyChanged, 
    IMainViewModel, 
    IEventTarget<InputModeChangedEvent>,
    IEventTarget<ActionBufferChangedEvent>,
    IEventTarget<FocusRequestedEvent>,
    IEventTarget<UnfocusRequestedEvent>
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

    private readonly Stack<IFocusableViewModel> FocusStack = [];
    private bool _inited = false;

    public void Init()
    {
        Focus(ChatList);
        _inited = true;
    }

    private void Focus(IFocusableViewModel newFocused)
    {
        if (FocusStack.Contains(newFocused))
        {
            logger.LogError("Focus requested for object {type} that focus stack already contains", newFocused.GetType().Name);
            return;
        }

        if (_inited)
        {
            var oldFocused = FocusStack.Peek();
            eventService.Ignore(oldFocused);
        }

        FocusStack.Push(newFocused);
        eventService.StopIgnore(newFocused);
    }

    private bool Unfocus()
    {
        if (FocusStack.Count == 1)
            return false;

        var oldFocused = FocusStack.Pop();
        eventService.Ignore(oldFocused);

        var newFocused = FocusStack.Peek();
        eventService.StopIgnore(newFocused);
        return true;
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

    public Task Handle(FocusRequestedEvent @event)
    {
        Focus(@event.Sender);
        return Task.CompletedTask;
    }

    public Task Handle(UnfocusRequestedEvent @event)
    {
        Unfocus();
        return Task.CompletedTask;
    }
}
