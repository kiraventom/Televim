namespace Televim.Vim;

internal static class KeyStrokeStringCache
{
    private static readonly Dictionary<KeyStroke, string> _cache = [];

    public static string GetString(KeyStroke keyStroke)
    {
        if (!_cache.TryGetValue(keyStroke, out var str))
        {
            if (keyStroke.IsPrintable)
            {
                str = keyStroke.Char.Value.ToString();
            }
            else
            {
                var mods = Enum.GetValues<Modifiers>()
                    .Where(m => m != Modifiers.None)
                    .Where(m => keyStroke.Modifiers.HasFlag(m));

                var modsStrs = mods.Select(m => KeyNotation.GetModifierStr(m));

                var body = keyStroke.Char?.ToString() ?? Enum.GetName(keyStroke.Named);
                var parts = modsStrs.Append(body);
                var blobStr = string.Join(KeyNotation.MODIFIER_SEPARATOR, parts);

                str = $"<{blobStr}>";
            }

            _cache[keyStroke] = str;
        }

        return str;
    }
}

