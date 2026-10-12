namespace Televim.Vim.Actions;

public readonly record struct ActionBufferChangedEvent(string NewBufferValue);

public interface IActionBuffer
{
    string Text { get; }
}
