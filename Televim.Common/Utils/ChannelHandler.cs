using System.Threading.Channels;
using Microsoft.Extensions.Logging;

namespace Televim.Common.Utils;

public abstract class ChannelHandler<T> : IAsyncDisposable
{
    private readonly ILogger<ChannelHandler<T>> _logger;
    private readonly Channel<T> _channel;
    private readonly Task _loop;
    private readonly CancellationTokenSource _cts = new();

    protected ChannelHandler(ILogger<ChannelHandler<T>> logger)
    {
        _logger = logger;
        _channel = Channel.CreateUnbounded<T>(new() { SingleReader = true });
        _loop = Task.Run(() => Run(_cts.Token));
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

    protected abstract Task Handle(T item);

    protected void Receive(T item) => _channel.Writer.TryWrite(item);

    private async Task Run(CancellationToken ct)
    {
        await foreach (var item in _channel.Reader.ReadAllAsync(ct))
        {
            try
            {
                await Handle(item);
            }
            catch (Exception ex)
            {
                _logger.LogError("Exception was thrown when handling item {Item} in {Type}: {ex}", item.ToString(), this.GetType().FullName, ex.ToString());
                continue;
            }
        }
    }
}
