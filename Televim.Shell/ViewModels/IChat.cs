using Avalonia.Collections;

namespace Televim.Shell.ViewModels;

public interface IChat
{
    string Name { get; }
    AvaloniaList<IChatMessage> Messages { get; }
    IChatMessage LastMessage => Messages.LastOrDefault();

    // Picture ProfilePic { get; }
    // Is typing? Who typing?
}

