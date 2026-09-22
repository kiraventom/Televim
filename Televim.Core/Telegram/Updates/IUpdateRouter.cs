using TdLib;

namespace Televim.Core.Telegram.Updates;

public interface IUpdateRouter
{
    void Route(TdApi.Update update);
}

