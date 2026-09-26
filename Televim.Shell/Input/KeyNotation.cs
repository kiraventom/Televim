using System.Text;
using Avalonia.Input;

namespace Televim.Shell.Input;

internal static class KeyNotation
{
    public static IReadOnlyList<KeyStroke> Parse(string notation)
    {
        var result = new List<KeyStroke>();
        for (int i = 0; i < notation.Length;)
        {
            var ch = notation[i].ToString();
            if (ch != "<")
            {
                result.Add(new KeyStroke(ParseKey(ch), KeyModifiers.None, ch));
                i++;
                continue;
            }

            int end = notation.IndexOf('>', i);
            var body = notation[(i + 1)..end];

            var mods = KeyModifiers.None;

            while (true)
            {
                var dash = body.IndexOf('-');
                if (dash < 0)
                    break;
                
                var modStr = body[0..dash];
                body = body[(dash + 1)..];
                
                var mod = ParseMod(modStr);
                mods |= mod;
            }

            result.Add(new KeyStroke(ParseKey(body), mods, null));
            i = end + 1;
        }

        return result;
    }

    private static KeyModifiers ParseMod(string modStr)
    {
        return modStr switch 
        {
            "C" => KeyModifiers.Control,
            "S" => KeyModifiers.Shift,
            "A" => KeyModifiers.Alt,
            "M" => KeyModifiers.Meta,
            _ => throw new FormatException($"Unknown modifier: {modStr}")
        };
    }

    private static Key ParseKey(string name) => name switch
    {
        "Esc" => Key.Escape,
        "Enter" => Key.Return,
        "Backspace" or "BS" => Key.Back,
        _ when Enum.TryParse<Key>(name, ignoreCase: true, out var k) => k,
        _ => throw new FormatException($"Unknown key: {name}")
    };
}


