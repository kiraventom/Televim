using Avalonia.Input;
using Televim.Vim;
using Televim.Vim.Commands;

namespace Televim.Shell.ViewModels;

public interface IMainViewModel : ICommandTextHolder
{
    event Action<InputModes> InputModeChanged;
    InputModes CurrentInputMode { get; }
    string ActionBuffer { get; }
    void OnKeyDown(KeyEventArgs e);

    IChatListViewModel ChatList { get; }
    IOpenChatViewModel OpenChat { get; }

    FocusableViewModel FocusedViewModel { get; set; }
}
