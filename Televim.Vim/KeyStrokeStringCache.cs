namespace Televim.Vim;

internal static class KeyStrokeStringCache
{
    private static readonly Dictionary<KeyStroke, string> _cache = [];

    public static string GetString(KeyStroke keyStroke)
    {
        if (!_cache.TryGetValue(keyStroke, out var str))
        {
            var shifted = keyStroke.Modifiers.HasFlag(Modifiers.Shift);
            if (keyStroke.IsPrintable)
            {
                str = keyStroke.Char.Value.ToString();
                if (shifted)
                    str = str.ToUpper();
            }
            else
            {
                var mod = shifted ? "S-" : string.Empty;
                str = $"<{mod}{Enum.GetName(keyStroke.Named)}>";
            }

            _cache[keyStroke] = str;
        }

        return str;
    }
}

