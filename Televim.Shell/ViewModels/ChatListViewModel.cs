using Avalonia.Collections;

namespace Televim.Shell.ViewModels;

internal class ChatListViewModel : 
    BasePropertyChanged,
    IChatList
{
    public AvaloniaList<IChat> Chats { get; } = [];

    public IChat SelectedChat 
    { 
        get => field;
        set => SetAndRaise(ref field, value);
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
        if (index > 0)
            SelectedChat = Chats[index - 1];
    }

    public void MoveSelectionDown()
    {
        var index = Chats.IndexOf(SelectedChat);
        if (index >= 0 && index < Chats.Count - 1)
            SelectedChat = Chats[index + 1];
    }
}

