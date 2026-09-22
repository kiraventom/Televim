using Televim.Core.Configuration;

namespace Televim.Core.Telegram;

internal interface ITelegramClient
{
    Task SetTdLibParameters(TDLibConfig tdLibConfig);
}

