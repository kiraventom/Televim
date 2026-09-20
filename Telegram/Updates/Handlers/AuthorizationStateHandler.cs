using TdLib;
using Televim.Configuration;
using static TdLib.TdApi.AuthorizationState;

namespace Televim.Telegram.Updates.Handlers;

public class AuthorizationStateHandler(ITelegramClient client, IConfigService configService) : UpdateHandler<TdApi.Update.UpdateAuthorizationState>(client)
{
    public override async Task Handle(TdApi.Update.UpdateAuthorizationState update)
    {
        // TODO Polymorph this
        switch (update.AuthorizationState)
        {
            case AuthorizationStateWaitTdlibParameters:
                var config = await configService.GetCurrent();
                await Client.SetTdLibParameters(config.TDLib);
                break;
        }
    }
}

