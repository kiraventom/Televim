namespace Televim.Common.Events;

public interface IEventService
{
    Task Raise<T>(T @event);
    bool Ignore(object target);
    bool StopIgnore(object target);
}

