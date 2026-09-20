using TdLib;
using Televim.Telegram.Updates;

namespace Televim.Telegram;

public interface ITelegramClient
{

}

public class TelegramClient : ITelegramClient
{
    private TdClient Client { get; }
    private IUpdateRouter UpdateRouter { get; }

    public TelegramClient(TdClient tdClient, IUpdateRouter updateRouter)
    {
        Client = tdClient;
        Client.UpdateReceived += OnTdUpdate;

        UpdateRouter = updateRouter;
    }

    private void OnTdUpdate(object sender, TdApi.Update e)
    {
        UpdateRouter.Route(e);
    }
}
