using Televim.Shell.Input.Commands;

namespace Televim.Shell.Input;

public interface ICommandInputHandler
{
    (bool IsHandled, bool ExpectFollowUp) Handle(CommandInput input);
}


