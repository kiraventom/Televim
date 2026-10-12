using TdLib;

namespace Televim.Telegram.Updates;

internal interface IUpdateRouter
{
    void Route(TdApi.Update update);
}

