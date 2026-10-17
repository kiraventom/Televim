using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace Televim.Shell.ViewModels;

internal class BasePropertyChanged : INotifyPropertyChanged
{
    public event PropertyChangedEventHandler PropertyChanged;

    protected void RaisePropertyChanged([CallerMemberName] string propertyName = null)
    {
        if (!IsPropertyNameValid(propertyName))
            return;

        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

    protected bool SetAndRaise<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
    {
        if (!IsPropertyNameValid(propertyName))
            return false;

        if (field is not null && field.Equals(value))
            return false;

        if (field is null && value is null)
            return false;

        field = value;
        RaisePropertyChanged(propertyName);
        return true;
    }

    private bool IsPropertyNameValid(string propertyName)
    {
        if (string.IsNullOrWhiteSpace(propertyName))
            return false;

        return true;
    }
}
