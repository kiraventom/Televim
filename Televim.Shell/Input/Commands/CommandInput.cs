using System.Text;

namespace Televim.Shell.Input.Commands;

internal class CommandInput
{
    public string Text { get; }
    public int NumberPrefix { get; }

    private CommandInput(string text, int numberPrefix = 0)
    {
        Text = text;
        NumberPrefix = numberPrefix;
    }

    public static bool TryParse(string rawInput, out CommandInput input)
    {
        input = null;

        StringBuilder numberSb = new();
        StringBuilder textSb = new();

        bool nonDigitFound = false;

        for (int i = 0; i < rawInput.Length; ++i)
        {
            var c = rawInput[i];

            if (nonDigitFound || !char.IsDigit(c))
            {
                textSb.Append(c);
                nonDigitFound = true;
            }
            else
            {
                numberSb.Append(c);
            }
        }

        var numberStr = numberSb.ToString();
        int number = 0;

        if (numberStr.Length != 0 && !int.TryParse(numberStr, out number))
            return false;

        input = new CommandInput(textSb.ToString(), number);
        return true;
    }
}
