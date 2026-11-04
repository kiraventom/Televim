using Televim.Common.Events;

namespace Televim.Shell.ViewModels;

[IgnoreEventsByDefault]
public abstract class FocusableViewModel : BasePropertyChanged
{
    public bool IsFocused { get; set; }
}

