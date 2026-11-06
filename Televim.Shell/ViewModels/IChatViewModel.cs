using Avalonia.Collections;

namespace Televim.Shell.ViewModels;

public interface IChatViewModel
{
    string Name { get; }
    AvaloniaList<IChatMessageViewModel> Messages { get; }
    IChatMessageDraftViewModel Draft { get; }
    IChatMessageViewModel MessagePreview { get; }

    // Picture ProfilePic { get; }
    // Is typing? Who typing?
}

