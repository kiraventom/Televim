namespace Televim.Common.Events;

public interface IEventService
{
    Task Raise<T> (T @event);
}

