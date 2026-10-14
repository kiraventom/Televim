using Televim.Vim.Actions;

namespace Televim.Vim;

internal interface IKeyStrokeBuffer : IActionBuffer
{
    int? Prefix { get; }
    IReadOnlyList<KeyStroke> KeyStrokes { get; }
    void Append(KeyStroke keyStroke);
    bool TryDropOldest();
    void Clear();
}
