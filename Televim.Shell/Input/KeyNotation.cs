using Avalonia.Input;

namespace Televim.Shell.Input;

internal static class KeyNotation
{
    private readonly record struct ShiftedKey(Key Key, bool Shifted = false);

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
        var shiftedKey = ParseKey(input);
        if (shiftedKey.Shifted)
            modifiers |= KeyModifiers.Shift;

        var ch = ParseChar(input);
        if (ch is {} c && char.IsUpper(c))
            modifiers |= KeyModifiers.Shift;

        return new KeyStroke(shiftedKey.Key, modifiers, ch);
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

    private static ShiftedKey ParseKey(string input) => input switch
    {
        "Enter" or "CR" => new ShiftedKey(Key.Return),
        "Backspace" or "BS" => new ShiftedKey(Key.Back),
        "Escape" or "Esc" => new ShiftedKey(Key.Escape),
        "!"  => new ShiftedKey(Key.D1, Shifted: true),
        "@"  => new ShiftedKey(Key.D2, Shifted: true),
        "#"  => new ShiftedKey(Key.D3, Shifted: true),
        "$"  => new ShiftedKey(Key.D4, Shifted: true),
        "%"  => new ShiftedKey(Key.D5, Shifted: true),
        "^"  => new ShiftedKey(Key.D6, Shifted: true),
        "&"  => new ShiftedKey(Key.D7, Shifted: true),
        "*"  => new ShiftedKey(Key.D8, Shifted: true),
        "("  => new ShiftedKey(Key.D9, Shifted: true),
        ")"  => new ShiftedKey(Key.D0, Shifted: true),
        "?"  =>  new ShiftedKey(Key.OemQuestion, true),
        ":"  =>  new ShiftedKey(Key.OemSemicolon, true),
        "\"" => new ShiftedKey(Key.OemQuotes, true),
        "+"  =>  new ShiftedKey(Key.OemPlus, true),
        "|"  =>  new ShiftedKey(Key.OemPipe, true),
        "~"  =>  new ShiftedKey(Key.OemTilde, true),
        "_"  =>  new ShiftedKey(Key.OemMinus, true),
        "<"  =>  new ShiftedKey(Key.OemComma, true),
        ">"  =>  new ShiftedKey(Key.OemPeriod, true),
        "{"  =>  new ShiftedKey(Key.OemOpenBrackets, true),
        "}"  =>  new ShiftedKey(Key.OemCloseBrackets, true),
        ","  =>  new ShiftedKey(Key.OemComma, false),
        "."  =>  new ShiftedKey(Key.OemPeriod, false),
        "/"  =>  new ShiftedKey(Key.OemQuestion, false),
        ";"  =>  new ShiftedKey(Key.OemSemicolon, false),
        "'" => new ShiftedKey(Key.OemQuotes, false),
        "-"  => new ShiftedKey(Key.OemMinus, false),
        "="  => new ShiftedKey(Key.OemPlus, false),
        "\\" => new ShiftedKey(Key.OemPipe, false),
        "`"  => new ShiftedKey(Key.OemTilde, false),
        "["  => new ShiftedKey(Key.OemOpenBrackets, false),
        "]"  => new ShiftedKey(Key.OemCloseBrackets, false),
        {} s when s.Length == 1 && char.IsDigit(s[0]) => new ShiftedKey((Key)((int)Key.D0 + int.Parse(s))),
        _ when Enum.TryParse<Key>(input, ignoreCase: true, out var k) && Enum.IsDefined(k) => new ShiftedKey(k),
        _ => throw new FormatException($"Unknown key: {input}")
    };

    private static char? ParseChar(string input)
    {
        return input.Length == 1 ? input[0] : null;
    }
}
