namespace Televim.Core.Events;

public interface IEventService
{
    Task Raise<T> (T @event);
}

