using TdLib;

namespace Televim.Telegram.Updates;

public interface IUpdateRouter
{
    void Route(TdApi.Update update);
}

