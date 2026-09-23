namespace Televim.Core.Events;

public interface IEventTarget<T>
{
    Task Handle(T @event);
}

