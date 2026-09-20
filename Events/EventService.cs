using Microsoft.Extensions.DependencyInjection;

namespace Televim.Events;

public class EventService(IServiceProvider sp) : IEventService
{
    public async Task Raise<T> (T @event)
    {
        var targets = sp.GetServices<IEventTarget<T>>();
        foreach (var target in targets)
            await target.Handle(@event);
    }
}
