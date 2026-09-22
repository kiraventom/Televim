using TdLib;
using Televim.Core.Configuration;
using static TdLib.TdApi.AuthorizationState;

namespace Televim.Core.Telegram.Updates.Handlers;

internal class AuthorizationStateHandler(ITelegramClient client, IConfigService configService) : UpdateHandler<TdApi.Update.UpdateAuthorizationState>(client)
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

