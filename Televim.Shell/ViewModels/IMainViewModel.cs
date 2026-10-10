using Avalonia.Input;
using Televim.Shell.Input;

namespace Televim.Shell.ViewModels;

public interface IMainViewModel
{
    event Action<InputModes> InputModeChanged;
    InputModes CurrentInputMode { get; }
    string ActionBuffer { get; }
    string CommandText { get; set; }
    void OnKeyDown(KeyEventArgs e);
}
