using Avalonia.Collections;

namespace Televim.Shell.ViewModels;

internal class ChatViewModel : 
    BasePropertyChanged, 
    IChatViewModel
{
    private readonly ChatMessageDraftViewModel _draft = new();

    public string Name 
    { 
        get => field;
        set => SetAndRaise(ref field, value);
    }

    public IChatMessageDraftViewModel Draft => _draft;

    public AvaloniaList<IChatMessageViewModel> Messages { get; } = [];

    public IChatMessageViewModel MessagePreview => string.IsNullOrEmpty(Draft.Text) ? Messages.LastOrDefault() : Draft;

    internal ChatViewModel()
    {
        _draft.TextChanged += _ => RaisePropertyChanged(nameof(MessagePreview));
    }
}

