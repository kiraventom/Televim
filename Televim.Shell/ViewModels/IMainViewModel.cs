using Avalonia.Input;

namespace Televim.Shell.ViewModels;

public interface IMainViewModel
{
    string Buffer { get; }
    void OnKeyDown(KeyEventArgs e);
}
