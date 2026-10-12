using Avalonia.Input;
using Microsoft.Extensions.Logging;

namespace Televim.Vim;

internal class InputListener(ILogger<InputListener> logger, IKeyStrokeHandler keyStrokeHandler) : IInputListener
{
    public void OnKeyDown(KeyEventArgs e)
    {
        logger.LogDebug($"KeyEventArgs: Key: {Enum.GetName(e.Key)}, KeySymbol: {e.KeySymbol}, Modifiers: {Enum.GetName(e.KeyModifiers)}");

        var keyStroke = ToKeyStroke(e);
        var didHandle = keyStrokeHandler.Handle(keyStroke); 
        if (didHandle)
            e.Handled = true;
    }

    private KeyStroke ToKeyStroke(KeyEventArgs args)
    {
        return new(
            Char: ResolveChar(args),
            Named: ResolveNamed(args.Key),
            Modifiers: ResolveModifiers(args.KeyModifiers));
    }

    private static char? ResolveChar(KeyEventArgs e)
    {
        if (e.KeySymbol is { Length: 1 } s && !char.IsControl(s[0])) 
            return s[0];

        return null;
    }

    private static NamedKey ResolveNamed(Key key) => key switch
    {
        Key.Escape => NamedKey.Escape, 
        Key.Return => NamedKey.Enter, 
        Key.Tab => NamedKey.Tab, 
        Key.Back => NamedKey.Backspace,
        Key.Delete => NamedKey.Delete,
        Key.Insert => NamedKey.Insert,
        Key.Home => NamedKey.Home,
        Key.End => NamedKey.End,
        Key.PageUp => NamedKey.PageUp,
        Key.PageDown => NamedKey.PageDown,
        Key.Up => NamedKey.ArrowUp,
        Key.Down => NamedKey.ArrowDown,
        Key.Left => NamedKey.ArrowLeft,
        Key.Right => NamedKey.ArrowRight,
        _ => NamedKey.None,
    };

    private static Modifiers ResolveModifiers(KeyModifiers m) =>
        (m.HasFlag(KeyModifiers.Shift) ? Modifiers.Shift : 0) |
        (m.HasFlag(KeyModifiers.Control) ? Modifiers.Control : 0) |
        (m.HasFlag(KeyModifiers.Alt) ? Modifiers.Alt : 0) |
        (m.HasFlag(KeyModifiers.Meta) ? Modifiers.Meta : 0);
}
