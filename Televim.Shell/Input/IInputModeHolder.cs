namespace Televim.Shell.Input;

internal interface IInputModeHolder
{
    InputModes CurrentMode { get; }
    bool CurrentModeSupportsPrefixes();
}
