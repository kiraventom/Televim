using Avalonia.Collections;

namespace Televim.Shell.ViewModels;

public interface IChatListViewModel
{
    IChatViewModel SelectedChat { get; set; }
    AvaloniaList<IChatViewModel> Chats { get; }
}

