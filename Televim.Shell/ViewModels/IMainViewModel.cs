using Avalonia.Input;
using Televim.Shell.Input;

namespace Televim.Shell.ViewModels;

public interface IMainViewModel
{
    event Action<InputModes> InputModeChanged;
    InputModes CurrentInputMode { get; }
    string Buffer { get; }
    string CommandText { get; set; }
    void OnKeyDown(KeyEventArgs e);
}
