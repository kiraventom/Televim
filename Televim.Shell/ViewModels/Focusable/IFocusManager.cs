namespace Televim.Shell.ViewModels.Focusable;

internal interface IFocusManager
{
    void Focus(IFocusableViewModel focusable);
    bool Unfocus();
}

