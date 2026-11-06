using Televim.Common.Events;
using Televim.Vim;
using Televim.Vim.Actions.NormalMode;

namespace Televim.Shell.ViewModels.Focusable;

[IgnoreEventsByDefault]
public abstract class FocusableViewModel(IEventService eventService) : 
    BasePropertyChanged, 
    IFocusableViewModel,
    IEventTarget<InputModeChangingEvent>,
    IEventTarget<DeselectRequestedEvent>
{
    public bool IsFocused
    {
        get => field;
        set => SetAndRaise(ref field, value);
    }

    protected InputModes AllowedInputModes { get; set; } = InputModes.NORMAL | InputModes.COMMAND | InputModes.VISUAL;

    public async Task Handle(DeselectRequestedEvent @event)
    {
        await OnUnfocus();
        await eventService.Raise(new UnfocusRequestedEvent());
    }

    protected Task RequestFocus() => eventService.Raise(new FocusRequestedEvent(this));

    protected virtual Task OnUnfocus() => Task.CompletedTask;

    public Task Handle(InputModeChangingEvent @event)
    {
        if (!AllowedInputModes.HasFlag(@event.InputMode))
            @event.Cancel = true;

        return Task.CompletedTask;
    }
}

