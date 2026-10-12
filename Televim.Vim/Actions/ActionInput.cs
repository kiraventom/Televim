using Televim.Common.Utils;

namespace Televim.Vim.Actions;

public record ActionInput(IReadOnlyList<KeyStroke> KeyStrokes, int NumberPrefix, InputModes Mode)
{
    public override string ToString()
    {
        return $"{{ KeyStrokes = {KeyStrokes.ToPrettyString()}, NumberPrefix = {NumberPrefix}, Mode = {Mode} }}";
    }
}
