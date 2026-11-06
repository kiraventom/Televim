using Avalonia.Collections;
using Televim.Common.Events;
using Televim.Shell.ViewModels.Focusable;
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
        set
        {
            SetAndRaise(ref field, value);
            PreviewSelectedChat();
        }
    }

    private void PreviewSelectedChat() => openChatVm.SetPreviewChat(SelectedChat);
    private Task OpenSelectedChat() => openChatVm.SetOpenChat(SelectedChat);

    public Task Handle(SelectRequestedEvent @event) => OpenSelectedChat();
    public Task Handle(MoveToLineEvent @event) => SelectByIndex(@event.Number);
    public Task Handle(MoveUpEvent @event) => MoveSelectionUp(@event.Number);
    public Task Handle(MoveDownEvent @event) => MoveSelectionDown(@event.Number);

    public Task Handle(MoveToTopEvent @event)
    {
        SelectFirst();
        return Task.CompletedTask;
    }

    public Task Handle(MoveToBottomEvent @event)
    {
        SelectLast();
        return Task.CompletedTask;
    }

    public void SelectFirst() => SelectedChat = Chats.FirstOrDefault();
    public void SelectLast() => SelectedChat = Chats.LastOrDefault();

    public Task SelectByIndex(int index)
    {
        var hasChat = index < Chats.Count;
        if (hasChat)
            SelectedChat = Chats[index];
        
        return Task.CompletedTask;
    }

    public Task MoveSelectionUp(int number)
    {
        var index = Chats.IndexOf(SelectedChat);
        if (index < 0)
        {
            SelectLast();
        }
        else
        {
            index -= number;
            while (index < 0)
                index += Chats.Count;

            SelectedChat = Chats[index];
        }

        return Task.CompletedTask;
    }

    public Task MoveSelectionDown(int number)
    {
        var index = Chats.IndexOf(SelectedChat);
        if (index < 0)
        {
            SelectFirst();
        }
        else
        {
            index += number;
            while (index > Chats.Count - 1)
                index -= Chats.Count;

            SelectedChat = Chats[index];
        }

        return Task.CompletedTask;
    }
}

