using Avalonia.Collections;

namespace Televim.Shell.ViewModels;

internal class ChatViewModel : 
    BasePropertyChanged, 
    IChatViewModel
{
    public string Name 
    { 
        get => field;
        set => SetAndRaise(ref field, value);
    }

    public AvaloniaList<IChatMessageViewModel> Messages { get; } = [];
}

