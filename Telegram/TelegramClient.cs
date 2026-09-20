using TdLib;
using Televim.Configuration;
using Televim.Telegram.Updates;

namespace Televim.Telegram;

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

    public async Task SetTdLibParameters(TDLibConfig tdLibConfig)
    {
        await Client.SetTdlibParametersAsync(
            apiId: tdLibConfig.ApiId,
            apiHash: tdLibConfig.ApiHash,
            databaseDirectory: tdLibConfig.DatabaseDir,
            useSecretChats: tdLibConfig.UseSecretChats,
            deviceModel: tdLibConfig.DeviceModel,
            systemVersion: tdLibConfig.SystemVersion,
            systemLanguageCode: tdLibConfig.SystemLanguageCode,
            useMessageDatabase: tdLibConfig.UseMessageDatabase);
    }
}
