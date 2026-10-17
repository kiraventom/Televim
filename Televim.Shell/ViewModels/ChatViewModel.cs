using Avalonia.Collections;
using Televim.Common.Events;
using Televim.Vim;

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

