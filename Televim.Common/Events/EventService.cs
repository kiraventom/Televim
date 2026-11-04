using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Televim.Common.Events;

internal class EventService(IServiceProvider sp, ILogger<EventService> logger, EventTargetRegistry registry) : IEventService
{
    private readonly HashSet<Type> _ignored = new HashSet<Type>(registry.IgnoredByDefault);

    public bool Ignore(object obj) => _ignored.Add(obj.GetType());

    public bool StopIgnore(object obj) => _ignored.Remove(obj.GetType());

    public async Task Raise<T> (T @event)
    {
        // TODO Check for re-entrance
        var targets = sp.GetServices<IEventTarget<T>>();
        foreach (var target in targets)
        {
            if (_ignored.Contains(target.GetType()))
            {
                logger.LogDebug("Target of type {type} is ignored, not firing IEventTarget<{TName}>", target.GetType().Name, @event.GetType().Name);
                continue;
            }

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
