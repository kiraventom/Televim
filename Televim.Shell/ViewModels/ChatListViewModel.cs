using Avalonia.Collections;
using Televim.Common.Events;
using Televim.Vim.Actions.NormalMode;

namespace Televim.Shell.ViewModels;

internal class ChatListViewModel(IEventService eventService, IOpenChatViewModel openChatVm) : 
    FocusableViewModel(eventService),
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

    public Task Handle(SelectRequestedEvent @event) => openChatVm.SelectChat(SelectedChat);

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
        MoveSelectionUp(@event.Number);
        return Task.CompletedTask;
    }

    public Task Handle(MoveDownEvent @event)
    {
        MoveSelectionDown(@event.Number);
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

    public void MoveSelectionUp(int number)
    {
        var index = Chats.IndexOf(SelectedChat);
        if (index < 0)
        {
            SelectLast();
            return;
        }

        index -= number;
        while (index < 0)
            index += Chats.Count;

        SelectedChat = Chats[index];
    }

    public void MoveSelectionDown(int number)
    {
        var index = Chats.IndexOf(SelectedChat);
        if (index < 0)
        {
            SelectFirst();
            return;
        }

        index += number;
        while (index > Chats.Count - 1)
            index -= Chats.Count;

        SelectedChat = Chats[index];
    }
}

