using Avalonia.Collections;
using Televim.Common.Events;
using Televim.Vim.Actions.NormalMode;

namespace Televim.Shell.ViewModels;

public readonly record struct OpenChatRequestedEvent(IChatViewModel Chat);

internal class ChatListViewModel(IEventService eventService) : 
    FocusableViewModel,
    IChatListViewModel,
    IEventTarget<SelectRequestedEvent>,
    IEventTarget<MoveToTopEvent>,
    IEventTarget<MoveToLineEvent>,
    IEventTarget<MoveToBottomEvent>,
    IEventTarget<MoveUpEvent>,
    IEventTarget<MoveDownEvent>
{
    public AvaloniaList<IChatViewModel> Chats { get; } = [];

    public IChatViewModel SelectedChat 
    { 
        get => field;
        set => SetAndRaise(ref field, value);
    }

    public Task Handle(SelectRequestedEvent @event)
    {
        return eventService.Raise(new OpenChatRequestedEvent(SelectedChat));
    }

    public Task Handle(MoveToTopEvent @event)
    {
        SelectFirst();
        return Task.CompletedTask;
    }

    public Task Handle(MoveToLineEvent @event)
    {
        SelectByIndex(@event.Number);
        return Task.CompletedTask;
    }

    public Task Handle(MoveToBottomEvent @event)
    {
        SelectLast();
        return Task.CompletedTask;
    }

    public Task Handle(MoveUpEvent @event)
    {
        MoveSelectionUp();
        return Task.CompletedTask;
    }

    public Task Handle(MoveDownEvent @event)
    {
        MoveSelectionDown();
        return Task.CompletedTask;
    }

    public void SelectFirst()
    {
        SelectedChat = Chats.FirstOrDefault();
    }

    public void SelectByIndex(int index)
    {
        var hasChat = index < Chats.Count;
        if (hasChat)
            SelectedChat = Chats[index];
    }

    public void SelectLast()
    {
        SelectedChat = Chats.LastOrDefault();
    }

    public void MoveSelectionUp()
    {
        var index = Chats.IndexOf(SelectedChat);
        if (index < 0)
            SelectLast();
        else if (index > 0)
            SelectedChat = Chats[index - 1];
    }

    public void MoveSelectionDown()
    {
        var index = Chats.IndexOf(SelectedChat);
        if (index < 0)
            SelectFirst();
        else if (index >= 0 && index < Chats.Count - 1)
            SelectedChat = Chats[index + 1];
    }
}

