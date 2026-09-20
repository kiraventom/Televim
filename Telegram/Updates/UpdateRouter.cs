using System.Threading.Channels;
using Microsoft.Extensions.Logging;
using TdLib;
using Televim.Telegram.Updates.Handlers;

namespace Televim.Telegram.Updates;

public class UpdateRouter : IAsyncDisposable, IUpdateRouter
{
    private readonly Channel<TdApi.Update> _channel;
    private readonly Task _loop;
    private readonly CancellationTokenSource _cts = new();
    private readonly IServiceProvider _sp;

    private ILogger<UpdateRouter> Logger { get; }

    public UpdateRouter(ILogger<UpdateRouter> logger, IServiceProvider sp)
    {
        Logger = logger;

        _sp = sp;
        _channel = Channel.CreateUnbounded<TdApi.Update>(new() { SingleReader = true });
        _loop = Task.Run(() => Run(_cts.Token));
    }

    public void Route(TdApi.Update update)
    {
        _channel.Writer.TryWrite(update);
    }

    public async ValueTask DisposeAsync()
    {
        _cts.Cancel();
        _channel.Writer.TryComplete();

        try
        {
            await _loop;
        }
        catch (OperationCanceledException)
        {
        }

        _cts.Dispose();
    }

    private async Task Run(CancellationToken ct)
    {
        await foreach (var update in _channel.Reader.ReadAllAsync(ct))
        {
            var updateType = update.GetType();

            try
            {
                var updateHandlerType = typeof(IUpdateHandler<>).MakeGenericType(update.GetType());
                var service = _sp.GetService(updateHandlerType);
                if (service is not IUpdateHandler handler)
                {
                    Logger.LogTrace("No handler registered for update type \"{type}\"", updateType.FullName);
                    continue;
                }

                await handler.Handle(update);
            }
            catch (Exception ex)
            {
                Logger.LogError("Exception thrown during handling update of type \"{type}\": {ex}", updateType.FullName, ex);
            }
        }
    }
}
