using Avalonia.Input;

namespace Televim.Shell.Input;

internal readonly record struct KeyStroke(Key Key, KeyModifiers Modifiers, char? Char)
{
    public static KeyStroke From(KeyEventArgs e) => new KeyStroke(e.Key, e.KeyModifiers, e.KeySymbol is not null ? e.KeySymbol[0] : null);

    public bool IsDigit => Modifiers == KeyModifiers.None && Char is {} c && char.IsDigit(c);
    public bool IsModifier => Key is Key.LeftAlt or Key.RightAlt or Key.LeftCtrl or Key.RightCtrl or Key.LeftShift or Key.RightShift or Key.LWin or Key.RWin;
}
