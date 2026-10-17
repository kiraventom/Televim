using Televim.Vim;

namespace Televim.Shell.ViewModels;

public interface IOpenChat : IChat
{
    string InputText { get; set; }
    InputModes CurrentInputMode { get; }
}
