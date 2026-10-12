namespace Televim.Vim;

public readonly record struct KeyStroke(char? Char, NamedKey Named, Modifiers Modifiers)
{
    public bool IsIgnorable => Char is null && Named is NamedKey.None;

    public bool IsPrintable => Char is { } c && !char.IsControl(c)
        && (Modifiers & (Modifiers.Control | Modifiers.Alt | Modifiers.Meta)) == Modifiers.None;

    public bool IsDigit => IsPrintable && char.IsDigit(Char.Value);

    public bool Matches(KeyStroke other)
    {
        // Can't use Equals() here because typing some printable chars may require holding Shift
        // E.g., typing ':' on US QWERTY produces the following key stroke:
        // { Char = ':', Named = None, Modifiers = Shift }
        //                                         ^^^^^
        // Because of that, we ignore Shift if char is printable
        if (IsPrintable && other.IsPrintable)
        {
            return Char.Value == other.Char.Value;
        }
        
        if (!IsPrintable && !other.IsPrintable)
            return Char == other.Char && Named == other.Named && Modifiers == other.Modifiers;

        return false;
    }

    public override string ToString() => KeyStrokeStringCache.GetString(this);
}
