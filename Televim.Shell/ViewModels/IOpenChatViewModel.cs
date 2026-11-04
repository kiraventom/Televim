using Avalonia.Collections;
using Televim.Vim;

namespace Televim.Shell.ViewModels;

public interface IOpenChatViewModel
{
    string Name { get; }
    AvaloniaList<IChatMessageViewModel> Messages { get; }

    string InputText { get; set; }
    InputModes CurrentInputMode { get; }

    bool IsEmpty { get; }
    void SelectChat(IChatViewModel chat);
}

