using Avalonia.Collections;
using Microsoft.Extensions.Logging;
using Televim.Common.Events;
using Televim.Vim;

namespace Televim.Shell.ViewModels;

internal class OpenChatViewModel(ILogger<OpenChatViewModel> logger, IEventService eventService, IInputModeHolder inputModeHolder) :
    FocusableViewModel(eventService),
    IOpenChatViewModel,
    IEventTarget<InputModeChangedEvent>
{
    public string Name 
    { 
        get => field; 
        set => SetAndRaise(ref field, value);
    }

    public AvaloniaList<IChatMessageViewModel> Messages { get; } = [];

    public InputModes CurrentInputMode => inputModeHolder.CurrentMode;

    public IChatViewModel Chat
    {
        get => field;
        set
        {
            field = value;
            Name = value?.Name;
            Messages.Clear();

            if (value is not null)
                Messages.AddRange(value.Messages);
        }
    }

    public string InputText
    { 
        get => field;
        set => SetAndRaise(ref field, value);
    }

    public bool IsEmpty => Chat is null;

    public Task SetChat(IChatViewModel chat, bool focus = false)
    {
        Chat = chat;
        return focus ? RequestFocus() : Task.CompletedTask;
    }

    protected override Task OnUnfocus()
    {
        Chat = null;
        return Task.CompletedTask;
    }

    public Task Handle(InputModeChangedEvent @event)
    {
        RaisePropertyChanged(nameof(CurrentInputMode));
        return Task.CompletedTask;
    }
}
