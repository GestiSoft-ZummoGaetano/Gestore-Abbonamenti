using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;
namespace Gestore_Abbonamenti.Converters
{
    public class BoolToVisibilityConverter : MarkupExtension, IValueConverter
    {
        #region Properties

        public Visibility TrueValue { get; set; }
        
        public Visibility FalseValue { get; set; }

        #endregion
        public BoolToVisibilityConverter()
        {
            TrueValue = Visibility.Visible;
            FalseValue = Visibility.Collapsed;
        }

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) => System.Convert.ToBoolean(value) ? TrueValue : FalseValue;

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture) => TrueValue.Equals(value);

        public override object ProvideValue(IServiceProvider serviceProvider) => this;
    }
}
