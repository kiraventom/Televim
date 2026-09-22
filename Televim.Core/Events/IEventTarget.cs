namespace Televim.Core.Events;

internal interface IEventTarget<T>
{
    Task Handle(T @event);
}

