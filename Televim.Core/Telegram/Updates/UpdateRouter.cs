using Microsoft.Extensions.Logging;
using TdLib;
using Televim.Core.Telegram.Updates.Handlers;
using Televim.Core.Utils;

namespace Televim.Core.Telegram.Updates;

internal class UpdateRouter(ILogger<UpdateRouter> logger, IServiceProvider sp) : ChannelHandler<TdApi.Update>, IUpdateRouter
{
    public void Route(TdApi.Update update) => Receive(update);

    protected override async Task Handle(TdApi.Update update)
    {
        var updateType = update.GetType();

        try
        {
            var updateHandlerType = typeof(IUpdateHandler<>).MakeGenericType(update.GetType());
            var service = sp.GetService(updateHandlerType);
            if (service is not IUpdateHandler handler)
            {
                logger.LogTrace("No handler registered for update type \"{type}\"", updateType.FullName);
                return;
            }

            await handler.Handle(update);
        }
        catch (Exception ex)
        {
            logger.LogError("Exception thrown during handling update of type \"{type}\": {ex}", updateType.FullName, ex);
        }
    }
}
