using System.Globalization;
using Avalonia.Data.Converters;
using Avalonia.Markup.Xaml;
using Televim.Shell.Input;

namespace Televim.Shell.Views.Converters;

internal class InputModeConverter(InputModes mode) : MarkupExtension, IValueConverter
{
    public override object ProvideValue(IServiceProvider serviceProvider) => this;

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => value is InputModes m && m == mode;

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => throw new NotSupportedException();
}
