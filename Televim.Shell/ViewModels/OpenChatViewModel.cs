using Avalonia.Collections;
using Microsoft.Extensions.Logging;
using Televim.Common.Events;
using Televim.Vim;
using Televim.Vim.Actions.NormalMode;

namespace Televim.Shell.ViewModels;

public readonly record struct CloseChatRequestedEvent;

internal class OpenChatViewModel(ILogger<OpenChatViewModel> logger, IEventService eventService, IInputModeHolder inputModeHolder) :
    FocusableViewModel,
    IOpenChatViewModel,
    IEventTarget<DeselectRequestedEvent>,
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

    public void SelectChat(IChatViewModel chat)
    {
        Chat = chat;
    }

    public Task Handle(DeselectRequestedEvent @event)
    {
        Chat = null;
        return eventService.Raise(new CloseChatRequestedEvent());
    }

    public Task Handle(InputModeChangedEvent @event)
    {
        RaisePropertyChanged(nameof(CurrentInputMode));
        return Task.CompletedTask;
    }
}
