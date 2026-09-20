public interface IEventTarget<T>
{
    Task Handle(T @event);
}

