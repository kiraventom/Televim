using TdLib;

namespace Televim.Telegram.Updates.Handlers;

public interface IUpdateHandler
{
    Task Handle(TdApi.Update update);
}

public interface IUpdateHandler<T> : IUpdateHandler where T : TdApi.Update
{
    Task IUpdateHandler.Handle(TdApi.Update update) => this.Handle((T)update);
    Task Handle(T update);
}

public abstract class UpdateHandler<T> : IUpdateHandler<T> where T : TdApi.Update
{
    public abstract Task Handle(T update);
}
