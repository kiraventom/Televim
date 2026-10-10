using Televim.Shell.Input.Actions;

namespace Televim.Shell.Input;

public interface IActionInputHandler
{
    (bool IsHandled, bool ExpectFollowUp) Handle(ActionInput input);
}


