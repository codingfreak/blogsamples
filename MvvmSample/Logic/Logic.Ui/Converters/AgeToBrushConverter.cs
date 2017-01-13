using System;
using System.Linq;

namespace codingfreaks.blogsamples.MvvmSample.Logic.Ui.Converters
{
    using System.Globalization;
    using System.Windows.Data;
    using System.Windows.Media;

    /// <summary>
    /// Converts a person age to the desired background color.
    /// </summary>
    public class AgeToBrushConverter : IValueConverter
    {
        #region explicit interfaces

        /// <inheritdoc />
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                return new SolidColorBrush(Colors.Transparent);
            }
            var transformed = 0;
            if (int.TryParse(value.ToString(), out transformed))
            {
                return transformed < 18 ? new SolidColorBrush(Color.FromRgb(255, 0, 0)) : new SolidColorBrush(Color.FromRgb(0, 255, 0));
            }
            throw new ArgumentException("Value is not valid.");
        }

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}