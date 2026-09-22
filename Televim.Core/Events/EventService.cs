using Microsoft.Extensions.DependencyInjection;

namespace Televim.Core.Events;

internal class EventService(IServiceProvider sp) : IEventService
{
    public async Task Raise<T> (T @event)
    {
        // TODO Check for re-entrance
        var targets = sp.GetServices<IEventTarget<T>>();
        foreach (var target in targets)
            await target.Handle(@event);
    }
}
