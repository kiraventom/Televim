using Televim.Common.Events;
using Televim.Vim.Actions.NormalMode;

namespace Televim.Shell.ViewModels;

[IgnoreEventsByDefault]
public abstract class FocusableViewModel(IEventService eventService) : 
    BasePropertyChanged, 
    IFocusableViewModel,
    IEventTarget<DeselectRequestedEvent>
{
    public async Task Handle(DeselectRequestedEvent @event)
    {
        await OnUnfocus();
        await eventService.Raise(new UnfocusRequestedEvent());
    }

    protected Task RequestFocus()
    {
        return eventService.Raise(new FocusRequestedEvent(this));
    }

    protected virtual Task OnUnfocus()
    {
        return Task.CompletedTask;
    }
}

