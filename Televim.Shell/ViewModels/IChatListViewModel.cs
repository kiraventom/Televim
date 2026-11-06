using Avalonia.Collections;
using Televim.Shell.ViewModels.Focusable;

namespace Televim.Shell.ViewModels;

public interface IChatListViewModel : IFocusableViewModel
{
    IChatViewModel SelectedChat { get; set; }
    AvaloniaList<IChatViewModel> Chats { get; }
}

