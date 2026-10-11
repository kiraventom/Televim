using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Televim.Core.Events;

internal class EventService(IServiceProvider sp, ILogger<EventService> logger) : IEventService
{
    public async Task Raise<T> (T @event)
    {
        // TODO Check for re-entrance
        var targets = sp.GetServices<IEventTarget<T>>();
        foreach (var target in targets)
        {
            try
            {
                await target.Handle(@event);
            }
            catch (Exception ex)
            {
                logger.LogError("Target {Target} failed to handle event {Event}: {Ex}", target.GetType().FullName, @event.GetType().FullName, ex.ToString());
                continue;
            }
        }
    }
}
