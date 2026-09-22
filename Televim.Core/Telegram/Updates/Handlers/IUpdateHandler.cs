using TdLib;

namespace Televim.Core.Telegram.Updates.Handlers;

internal interface IUpdateHandler
{
    Task Handle(TdApi.Update update);
}

internal interface IUpdateHandler<T> : IUpdateHandler where T : TdApi.Update
{
    Task IUpdateHandler.Handle(TdApi.Update update) => this.Handle((T)update);
    Task Handle(T update);
}
