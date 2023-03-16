using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Give an <see cref="ICampaignObject"/>, converts the name into just the first letter
    /// </summary>
    public sealed class NameToFirstLetterConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is ICampaignObject namedObject && !string.IsNullOrWhiteSpace(namedObject.Name))
                return char.ToUpper(namedObject.Name[0]);
            else if (value is string s && !string.IsNullOrWhiteSpace(s))
                return char.ToUpper(s[0]);
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
