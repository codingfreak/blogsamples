using System;
using System.Linq;

namespace codingfreaks.blogsamples.MvvmSample.Logic.Ui
{
    using System.Globalization;
    using System.Windows.Data;

    public class BoolInverseConverter : IValueConverter
    {
        #region explicit interfaces

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            var transformed = false;
            if (bool.TryParse(value.ToString(), out transformed))
            {
                return !transformed;
            }
            throw new ArgumentException("Value is not a bool.");
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}