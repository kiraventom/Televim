namespace Televim.Vim;

public record InputModeChangingEvent(InputModes InputMode)
{
    public bool Cancel { get; set; }
}

