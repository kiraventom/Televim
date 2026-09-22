using TdLib;

namespace Televim.Core.Telegram.Updates.Handlers;

internal abstract class UpdateHandler<T>(ITelegramClient client) : IUpdateHandler<T> where T : TdApi.Update
{
    protected ITelegramClient Client { get; } = client;

    public abstract Task Handle(T update);
}
