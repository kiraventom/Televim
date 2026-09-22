namespace Televim.Core.Events;

internal interface IEventService
{
    Task Raise<T> (T @event);
}

