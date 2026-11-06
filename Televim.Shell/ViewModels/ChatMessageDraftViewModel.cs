namespace Televim.Shell.ViewModels;

internal class ChatMessageDraftViewModel : ChatMessageViewModel, IChatMessageDraftViewModel
{
    internal event Action<ChatMessageDraftViewModel> TextChanged;

    internal ChatMessageDraftViewModel()
    {
        PropertyChanged += (_, e) => 
        {
            if (e.PropertyName == nameof(Text))
                TextChanged?.Invoke(this);
        };
    }
}
