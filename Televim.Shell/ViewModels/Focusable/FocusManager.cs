using Microsoft.Extensions.Logging;
using Televim.Common.Events;

namespace Televim.Shell.ViewModels.Focusable;

internal readonly record struct FocusRequestedEvent(IFocusableViewModel Sender);
internal readonly record struct UnfocusRequestedEvent();

internal class FocusManager(ILogger<FocusManager> logger, IEventService eventService) : IFocusManager,
    IEventTarget<FocusRequestedEvent>,
    IEventTarget<UnfocusRequestedEvent>
{
    private readonly Stack<IFocusableViewModel> FocusStack = [];

    public void Focus(IFocusableViewModel newFocused)
    {
        if (FocusStack.Contains(newFocused))
        {
            logger.LogError("Focus requested for object {type} that focus stack already contains", newFocused.GetType().Name);
            return;
        }

        // Can be empty only on app startup
        if (FocusStack.Count == 0)
        {
            logger.LogInformation("Focus stack is empty, this message should only appear once per session");
        }
        else
        {
            var oldFocused = FocusStack.Peek();
            eventService.Ignore(oldFocused);
            oldFocused.IsFocused = false;
            logger.LogDebug("{Type} has lost focus", oldFocused.GetType().Name);
        }

        FocusStack.Push(newFocused);
        eventService.StopIgnore(newFocused);
        newFocused.IsFocused = true;
        logger.LogDebug("{Type} has gained focus", newFocused.GetType().Name);
    }

    public bool Unfocus()
    {
        if (FocusStack.Count == 1)
            return false;

        var oldFocused = FocusStack.Pop();
        eventService.Ignore(oldFocused);
        oldFocused.IsFocused = false;
        logger.LogDebug("{Type} has lost focus", oldFocused.GetType().Name);

        var newFocused = FocusStack.Peek();
        eventService.StopIgnore(newFocused);
        newFocused.IsFocused = true;
        logger.LogDebug("{Type} has gained focus", newFocused.GetType().Name);
        return true;
    }

    public Task Handle(FocusRequestedEvent @event)
    {
        Focus(@event.Sender);
        return Task.CompletedTask;
    }

    public Task Handle(UnfocusRequestedEvent @event)
    {
        Unfocus();
        return Task.CompletedTask;
    }
}
