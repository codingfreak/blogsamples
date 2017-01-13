namespace codingfreaks.blogsamples.MvvmSample.Logic.Ui.Converters
{
    using System;
    using System.Globalization;
    using System.Linq;
    using System.Windows.Data;

    /// <summary>
    /// Converts a boolean to its opposite.
    /// </summary>
    public class BoolInverseConverter : IValueConverter
    {
        #region explicit interfaces

        /// <inheritdoc />
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

        /// <inheritdoc />
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}