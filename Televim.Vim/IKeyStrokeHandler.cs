namespace Televim.Vim;

public interface IKeyStrokeHandler
{
    bool Handle(KeyStroke keyStroke);
}

