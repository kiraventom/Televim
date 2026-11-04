using Avalonia.Collections;

namespace Televim.Shell.ViewModels;

public interface IChatViewModel
{
    string Name { get; }
    AvaloniaList<IChatMessageViewModel> Messages { get; }
    IChatMessageViewModel LastMessage => Messages.LastOrDefault();

    // Picture ProfilePic { get; }
    // Is typing? Who typing?
}

