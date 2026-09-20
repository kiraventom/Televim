using TdLib;

namespace Televim.Telegram.Updates.Handlers;

public abstract class UpdateHandler<T>(ITelegramClient client) : IUpdateHandler<T> where T : TdApi.Update
{
    protected ITelegramClient Client { get; } = client;

    public abstract Task Handle(T update);
}
