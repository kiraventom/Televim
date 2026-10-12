namespace Televim.Vim;

internal static class KeyNotation
{
    public const char MODIFIER_SEPARATOR = '-';

    public static string GetModifierStr(Modifiers mod) => Enum.GetName(mod)[0..0];

    public static IReadOnlyList<KeyStroke> Parse(string notation)
    {
        List<KeyStroke> keyStrokes = [];

        for (int i = 0; i < notation.Length; ++i)
        {
            KeyStroke keyStroke;

            var ch = notation[i];
            switch (ch)
            {
                case '<':
                    var end = notation.IndexOf('>', i);
                    var blob = notation[i..(end + 1)];
                    keyStroke = ParseBlob(blob);
                    i = end;
                    break;

                default:
                    keyStroke = ParseChar(ch);
                    break;
            }

            keyStrokes.Add(keyStroke);
        }

        return keyStrokes;
    }

    private static KeyStroke ParseBlob(string blob)
    {
        var parts = blob
            .Trim('<', '>')
            .Split(MODIFIER_SEPARATOR, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var modifiers = Modifiers.None;
        foreach (var part in parts.SkipLast(1))
        {
            var mod = ParseMod(part);
            modifiers |= mod;
        }

        var keyStroke = ParseString(parts.Last(), modifiers);
        return keyStroke;
    }

    private static KeyStroke ParseString(string input, Modifiers modifiers)
    {
        return input switch
        {
            { Length: 1 } s => ParseChar(s[0], modifiers),
            _ when Enum.TryParse<NamedKey>(input, ignoreCase: true, out var n) && Enum.IsDefined(n) => new KeyStroke(null, n, modifiers),
            _ => throw new FormatException($"Unknown key: {input}")
        };
    }

    private static KeyStroke ParseChar(char ch, Modifiers modifiers = Modifiers.None) => new KeyStroke(ch, NamedKey.None, modifiers);

    private static Modifiers ParseMod(string input)
    {
        return input switch 
        {
            "C" => Modifiers.Control,
            "S" => Modifiers.Shift,
            "A" => Modifiers.Alt,
            "M" => Modifiers.Meta,
            _ => throw new FormatException($"Unknown modifier: {input}")
        };
    }
}
