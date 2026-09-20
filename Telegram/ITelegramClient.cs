using Televim.Configuration;

namespace Televim.Telegram;

public interface ITelegramClient
{
    Task SetTdLibParameters(TDLibConfig tdLibConfig);
}

