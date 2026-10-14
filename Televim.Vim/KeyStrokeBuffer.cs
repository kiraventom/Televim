using Televim.Vim.Utils;
using Televim.Vim.Actions;
using Televim.Common.Events;
using Microsoft.Extensions.Logging;

namespace Televim.Vim;

internal class KeyStrokeBuffer(ILogger<KeyStrokeBuffer> logger, IEventService eventService, IInputModeHolder inputModeHolder) : IKeyStrokeBuffer

{
    private readonly List<KeyStroke> _keyStrokes = [];
    private readonly PrefixBuilder _prefixBuilder = new();

    public string Text { get; private set; }

    public int? Prefix => _prefixBuilder.Number;
    public IReadOnlyList<KeyStroke> KeyStrokes => _keyStrokes;

    public void Append(KeyStroke keyStroke)
    {
        if (keyStroke.IsDigit && inputModeHolder.CurrentModeSupportsPrefixes())
            _prefixBuilder.Append(keyStroke.Char.Value);
        else
            _keyStrokes.Add(keyStroke);

        UpdateText();
    }

    public bool TryDropOldest()
    {
        if (Prefix is not null && KeyStrokes.Count > 0)
        {
            _prefixBuilder.Clear();
            UpdateText();
            return true;
        }

        if (KeyStrokes.Count > 1)
        {
            _keyStrokes.RemoveAt(0);
            UpdateText();
            return true;
        }

        return false;
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


