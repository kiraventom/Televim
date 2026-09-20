public interface IEventService
{
    Task Raise<T> (T @event);
}

