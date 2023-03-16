using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Fiction.Windows
{
    /// <summary>
    /// Converter that takes an array and performs a String.Join on it
    /// </summary>
    public sealed class StringJoinConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IEnumerable<string> items)
            {
                string separator = (parameter as string) ?? culture.TextInfo.ListSeparator;
                return string.Join(separator, items);
            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
