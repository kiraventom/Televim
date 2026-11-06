using Microsoft.Extensions.Logging;
using Televim.Common.Events;
using Televim.Shell.ViewModels.Focusable;
using Televim.Vim;

namespace Televim.Shell.ViewModels;

internal class OpenChatViewModel(ILogger<OpenChatViewModel> logger, IEventService eventService) :
    FocusableViewModel(eventService),
    IOpenChatViewModel
{
    public IChatViewModel Chat
    {
        get => field;
        set
        {
            SetAndRaise(ref field, value);
            RaisePropertyChanged(nameof(IsEmpty));

            if (Chat is null)
                AllowedInputModes &= ~InputModes.INSERT;
            else
                AllowedInputModes |= InputModes.INSERT;
        }
    }

    public string InputText
    { 
        get => field;
        set => SetAndRaise(ref field, value);
    }

    public bool IsEmpty => Chat is null;
    
    public void SetPreviewChat(IChatViewModel chat)
    {
        Chat = chat;
    }

    public Task SetOpenChat(IChatViewModel chat)
    {
        Chat = chat;
        return RequestFocus();
    }
}
