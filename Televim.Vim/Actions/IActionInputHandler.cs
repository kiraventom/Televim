using Televim.Vim.Actions;

namespace Televim.Vim;

public interface IActionInputHandler
{
    (bool IsHandled, bool ExpectFollowUp) Handle(ActionInput input);
}


