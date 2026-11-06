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

    private Task SelectChat(IChatViewModel chat)
    {
        SelectedChat = chat;
        return PreviewSelectedChat();
    }

    private Task PreviewSelectedChat() => openChatVm.SetChat(SelectedChat);
    private Task OpenSelectedChat() => openChatVm.SetChat(SelectedChat, focus: true);

    public Task Handle(SelectRequestedEvent @event) => OpenSelectedChat();
    public Task Handle(MoveToTopEvent @event) => SelectFirst();
    public Task Handle(MoveToLineEvent @event) => SelectByIndex(@event.Number);
    public Task Handle(MoveToBottomEvent @event) => SelectLast();
    public Task Handle(MoveUpEvent @event) => MoveSelectionUp(@event.Number);
    public Task Handle(MoveDownEvent @event) => MoveSelectionDown(@event.Number);

    public Task SelectFirst() => SelectChat(Chats.FirstOrDefault());
    public Task SelectLast() => SelectChat(Chats.LastOrDefault());

    public Task SelectByIndex(int index)
    {
        var hasChat = index < Chats.Count;
        if (hasChat)
            return SelectChat(Chats[index]);
        
        return Task.CompletedTask;
    }

    public Task MoveSelectionUp(int number)
    {
        var index = Chats.IndexOf(SelectedChat);
        if (index < 0)
            return SelectLast();

        index -= number;
        while (index < 0)
            index += Chats.Count;

        return SelectChat(Chats[index]);
    }

    public Task MoveSelectionDown(int number)
    {
        var index = Chats.IndexOf(SelectedChat);
        if (index < 0)
            return SelectFirst();

        index += number;
        while (index > Chats.Count - 1)
            index -= Chats.Count;

        return SelectChat(Chats[index]);
    }
}

