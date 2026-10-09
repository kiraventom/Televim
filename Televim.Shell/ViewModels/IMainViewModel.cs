using Avalonia.Input;
using Televim.Shell.Input;

namespace Televim.Shell.ViewModels;

public interface IMainViewModel
{
    event Action<InputModes> InputModeChanged;
    bool IsInsertMode { get; } // TODO Temp
    string Buffer { get; }
    void OnKeyDown(KeyEventArgs e);
}
