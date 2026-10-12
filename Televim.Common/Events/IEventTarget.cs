namespace Televim.Common.Events;

public interface IEventTarget<T>
{
    Task Handle(T @event);
}

