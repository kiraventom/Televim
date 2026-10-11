using System.Text;

namespace Televim.Shell.Input.Utils;

internal class PrefixBuilder
{
    public const int MAX_PREFIX_VALUE = 100;

    private readonly StringBuilder _sb = new();
    public int Number { get; private set; }

    public int Append(char ch)
    {
        if (!char.IsDigit(ch))
            throw new ArgumentException($"Char '{ch}' is not a digit");

        if (Number == MAX_PREFIX_VALUE)
            return Number;

        _sb.Append(ch);

        Number = int.Parse(_sb.ToString());

        if (Number > MAX_PREFIX_VALUE)
        {
            Number = MAX_PREFIX_VALUE;
            _sb.Clear();
            _sb.Append(Number.ToString());
        }

        return Number;
    }

    public void Clear()
    {
        _sb.Clear();
        Number = 0;
    }
}
