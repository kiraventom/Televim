using Televim.Core.Utils;

namespace Televim.Shell.Input.Commands;

internal record CommandInput(IReadOnlyList<KeyStroke> KeyStrokes, int NumberPrefix, InputModes Mode)
{
    public override string ToString()
    {
        return $"{{ CommandInput = {KeyStrokes.ToPrettyString()}, NumberPrefix = {NumberPrefix}, Mode = {Mode} }}";
    }
}
