using TdLib;

namespace Televim.Core.Telegram.Updates;

internal interface IUpdateRouter
{
    void Route(TdApi.Update update);
}

