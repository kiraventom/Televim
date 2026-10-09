using Avalonia.Input;

namespace Televim.Shell.Input;

internal static class KeyNotation
{
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
                    keyStroke = ParseKeyStroke(ch.ToString(), KeyModifiers.None);
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
            .Split('-', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

        var modifiers = KeyModifiers.None;
        foreach (var part in parts.SkipLast(1))
        {
            var mod = ParseMod(part);
            modifiers |= mod;
        }

        var keyStroke = ParseKeyStroke(parts.Last(), modifiers);
        return keyStroke;
    }
    
    private static KeyStroke ParseKeyStroke(string input, KeyModifiers modifiers)
    {
        var key = ParseKey(input);
        var ch = ParseChar(input);
        if (ch is {} c && char.IsUpper(c))
            modifiers |= KeyModifiers.Shift;

        return new KeyStroke(key, modifiers, ch);
    }

    private static KeyModifiers ParseMod(string input)
    {
        return input switch 
        {
            "C" => KeyModifiers.Control,
            "S" => KeyModifiers.Shift,
            "A" => KeyModifiers.Alt,
            "M" => KeyModifiers.Meta,
            _ => throw new FormatException($"Unknown modifier: {input}")
        };
    }

    private static Key ParseKey(string input) => input switch
    {
        "Enter" or "CR" => Key.Return,
        "Backspace" or "BS" => Key.Back,
        "Escape" or "Esc" => Key.Escape,
        _ when Enum.TryParse<Key>(input, ignoreCase: true, out var k) => k,
        _ => throw new FormatException($"Unknown key: {input}")
    };

    private static char? ParseChar(string input)
    {
        return input.Length == 1 ? input[0] : null;
    }
}


