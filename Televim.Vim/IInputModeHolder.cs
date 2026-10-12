namespace Televim.Vim;

public interface IInputModeHolder
{
    InputModes CurrentMode { get; }
    bool CurrentModeSupportsPrefixes();
}
