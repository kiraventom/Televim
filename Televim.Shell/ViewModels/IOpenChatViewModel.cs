namespace Televim.Shell.ViewModels;

internal interface IOpenChatViewModel : IOpenChat
{
    bool IsEmpty { get; }
    void SelectChat(IChat chat);
}

