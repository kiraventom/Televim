using Televim.Vim.Utils;
using Televim.Vim.Actions;
using Televim.Core.Events;
using Microsoft.Extensions.Logging;

namespace Televim.Vim;

internal class KeyStrokeBuffer(ILogger<KeyStrokeBuffer> logger, IEventService eventService) : IKeyStrokeBuffer
{
    private readonly List<KeyStroke> _keyStrokes = [];
    private readonly PrefixBuilder _prefixBuilder = new();

    public string Text { get; private set; }

    public int? Prefix => _prefixBuilder.Number;
    public IReadOnlyList<KeyStroke> KeyStrokes => _keyStrokes;

    public void Append(KeyStroke keyStroke)
    {
        _keyStrokes.Add(keyStroke);
        UpdateText();
    }

    public void AppendPrefix(char ch)
    {
        _prefixBuilder.Append(ch);
        UpdateText();
    }

    public void Clear()
    {
        _keyStrokes.Clear();
        _prefixBuilder.Clear();
        UpdateText();
    }

    private void UpdateText()
    {
        var prefixStr = _prefixBuilder.Number is {} n ? n.ToString() : string.Empty;
        var body = string.Join(string.Empty, _keyStrokes.Select(k => k.ToString()));
        Text = $"{prefixStr}{body}";

        logger.LogDebug("Key stroke buffer changed to '{text}'", Text);

        _ = eventService.Raise(new ActionBufferChangedEvent(Text));
    }
}


