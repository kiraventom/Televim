using Avalonia.Collections;

namespace Televim.Shell.ViewModels;

internal class ChatViewModel : 
    BasePropertyChanged, 
    IChat
{
    public string Name 
    { 
        get => field;
        set => SetAndRaise(ref field, value);
    }

    public AvaloniaList<IChatMessage> Messages { get; } = [];
}

