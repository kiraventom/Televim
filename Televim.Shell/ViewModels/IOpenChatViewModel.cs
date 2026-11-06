using Televim.Shell.ViewModels.Focusable;

namespace Televim.Shell.ViewModels;

public interface IOpenChatViewModel : IFocusableViewModel
{
    IChatViewModel Chat { get; }

    bool IsEmpty { get; }
    void SetPreviewChat(IChatViewModel chat);
    Task SetOpenChat(IChatViewModel chat);
}

