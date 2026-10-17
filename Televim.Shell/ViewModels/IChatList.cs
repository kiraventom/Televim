using Avalonia.Collections;

namespace Televim.Shell.ViewModels;

public interface IChatList
{
    IChat SelectedChat { get; set; }
    AvaloniaList<IChat> Chats { get; }

    void SelectFirst();
    void SelectByIndex(int index);
    void SelectLast();
    void MoveSelectionUp();
    void MoveSelectionDown();
}

