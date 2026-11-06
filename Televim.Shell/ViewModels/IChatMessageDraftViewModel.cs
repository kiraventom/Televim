namespace Televim.Shell.ViewModels;

public interface IChatMessageDraftViewModel : IChatMessageViewModel
{
    new string Text { get; set; }
}

