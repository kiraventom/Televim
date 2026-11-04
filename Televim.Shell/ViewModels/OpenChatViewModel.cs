using Avalonia.Collections;
using Microsoft.Extensions.Logging;
using Televim.Common.Events;
using Televim.Vim;
using Televim.Vim.Actions.NormalMode;

namespace Televim.Shell.ViewModels;

internal class OpenChatViewModel(ILogger<OpenChatViewModel> logger, IInputModeHolder inputModeHolder) :
    BasePropertyChanged,
    IOpenChatViewModel,
    IOpenChat,
    IEventTarget<DeselectRequestedEvent>,
    IEventTarget<InputModeChangedEvent>
{
    public string Name 
    { 
        get => field; 
        set => SetAndRaise(ref field, value);
    }

    public AvaloniaList<IChatMessage> Messages { get; } = [];

    public InputModes CurrentInputMode => inputModeHolder.CurrentMode;

    public IChat Chat
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

    public void SelectChat(IChat chat)
    {
        Chat = chat;
    }

    public Task Handle(DeselectRequestedEvent @event)
    {
        Chat = null;
        logger.LogDebug("Deselect executed");
        return Task.CompletedTask;
    }

    public Task Handle(InputModeChangedEvent @event)
    {
        RaisePropertyChanged(nameof(CurrentInputMode));
        return Task.CompletedTask;
    }
}
