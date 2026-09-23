using System.Threading.Channels;

namespace Televim.Core.Utils;

public abstract class ChannelHandler<T> : IAsyncDisposable
{
    private readonly Channel<T> _channel;
    private readonly Task _loop;
    private readonly CancellationTokenSource _cts = new();

    protected ChannelHandler()
    {
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
            await Handle(item);
        }
    }
}
