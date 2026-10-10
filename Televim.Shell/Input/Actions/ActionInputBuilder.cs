using System.Text;

namespace Televim.Shell.Input.Actions;

internal readonly record struct NumberPrefixClampedEvent(int ClampedTo);

internal class ActionInputBuilder(IInputModeHolder inputModeHolder) : IActionInputBuilder
{
    public const int MAX_NUMBER_PREFIX = 100;

    public ActionInput Build(IReadOnlyList<KeyStroke> keyStrokes)
    {
        StringBuilder numberSb = new();

        int bodyStart = -1;

        for (int i = 0; i < keyStrokes.Count; ++i)
        {
            var keyStroke = keyStrokes[i];

            if (bodyStart < 0 && keyStroke.IsDigit)
            {
                numberSb.Append(keyStroke.Char);
            }
            else
            {
                bodyStart = i;
                break;
            }
        }

        var numberStr = numberSb.ToString();
        int number = 0;

        if (numberStr.Length != 0)
        {
            try
            {
                number = int.Parse(numberStr);
            }
            catch (OverflowException)
            {
                number = int.MaxValue;
            }
        }
        
        number = Math.Min(number, MAX_NUMBER_PREFIX);

        return new ActionInput(keyStrokes.Skip(bodyStart).ToList(), number, inputModeHolder.CurrentMode);
    }
}

