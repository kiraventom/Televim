using Avalonia.Collections;
using Televim.Common.Events;
using Televim.Vim;
using Televim.Vim.Actions.NormalMode;

namespace Televim.Shell.ViewModels;

internal class OpenChatViewModel(IInputModeHolder inputModeHolder) :
    BasePropertyChanged,
    IOpenChat,
    IEventTarget<DeselectRequestedEvent>,
    IEventTarget<InputModeChangedEvent>
{
    public string Name => Chat.Name;
    public AvaloniaList<IChatMessage> Messages => Chat.Messages;

    public InputModes CurrentInputMode => inputModeHolder.CurrentMode;

    public IChat Chat
    {
        get => field;
        set
        {
            field = value;
            RaisePropertyChanged(nameof(Name));
        }
    }

    public string InputText
    { 
        get => field;
        set => SetAndRaise(ref field, value);
    }

    public Task Handle(DeselectRequestedEvent @event)
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


