using Televim.Core.Configuration;

namespace Televim.Core.Telegram;

public interface ITelegramClient
{
    Task SetTdLibParameters(TDLibConfig tdLibConfig);
}

