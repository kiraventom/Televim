namespace Televim.Shell.ViewModels;

internal class ChatMessageViewModel : BasePropertyChanged, IChatMessage
{
    public string Text
    { 
        get => field;
        set => SetAndRaise(ref field, value);
    }

    public DateTime DT
    { 
        get => field;
        set => SetAndRaise(ref field, value);
    }
}

