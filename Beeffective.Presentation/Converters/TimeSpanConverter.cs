using System;
using System.Globalization;
using System.Windows.Data;
using Beeffective.Presentation.Extensions;

namespace Beeffective.Presentation.Converters
{
    public class TimeSpanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture) => 
            value is TimeSpan timeSpan ? timeSpan.ToFormattedString() : null;

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture) => 
            throw new NotImplementedException();
    }
}