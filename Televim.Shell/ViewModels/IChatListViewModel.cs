using Avalonia.Collections;

namespace Televim.Shell.ViewModels;

public interface IChatListViewModel : IFocusableViewModel
{
    IChatViewModel SelectedChat { get; set; }
    AvaloniaList<IChatViewModel> Chats { get; }
}

