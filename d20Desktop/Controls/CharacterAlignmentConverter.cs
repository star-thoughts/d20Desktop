using Fiction.GameScreen.Monsters;
using System;
using System.Globalization;
using System.Windows.Data;

namespace Fiction.GameScreen.Controls
{
    public sealed class CharacterAlignmentConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Alignment alignment)
                return AlignmentExtensions.ToDisplayString(alignment);
            return AlignmentExtensions.ToDisplayString(Alignment.Unknown);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
