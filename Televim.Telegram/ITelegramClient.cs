using Televim.Common.Configuration;

namespace Televim.Telegram;

internal interface ITelegramClient
{
    Task SetTdLibParameters(TDLibConfig tdLibConfig);
}

