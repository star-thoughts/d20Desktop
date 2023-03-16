using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Fiction.GameScreen.Controls
{
    /// <summary>
    /// Converts a monetary value for display purposes
    /// </summary>
    public sealed class MonetaryConverter : IValueConverter
    {
        private const string MoneyRegex = @"(?:(?<plat>\d+?) ?pp?)? ?(?:(?<gold>\d+?) ?gp?)? ?(?:(?<silver>\d+?) ?sp?)? ?(?:(?<copper>\d+?) ?cp?)?";

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is int amountInCopper)
            {
                int copper = amountInCopper % 10;
                int silver = (amountInCopper % 100 / 10);
                int gold = (amountInCopper % 1000 / 100);
                int plat = amountInCopper / 1000;

                if (string.Equals(parameter as string, "gold", StringComparison.InvariantCultureIgnoreCase))
                {
                    gold += plat * 10;
                    plat = 0;
                }

                List<string> parts = new List<string>();
                if (plat != 0)
                    parts.Add(plat.ToString(culture) + " pp");
                if (gold != 0)
                    parts.Add(gold.ToString(culture) + " gp");
                if (silver != 0)
                    parts.Add(silver.ToString(culture) + " sp");
                if (copper != 0)
                    parts.Add(copper.ToString(culture) + " cp");

                return string.Join(" ", parts);

            }
            return value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string moneyString)
            {
                if (string.IsNullOrEmpty(moneyString))
                    return 0;

                int result = 0;
                MatchCollection matches = Regex.Matches(moneyString, MoneyRegex);
                foreach (Match match in matches)
                {
                    if (match.Success)
                    {
                        if (match.Groups["plat"].Success && Int32.TryParse(match.Groups["plat"].Value, NumberStyles.Integer, culture, out int plat))
                            result += plat * 1000;
                        if (match.Groups["gold"].Success && Int32.TryParse(match.Groups["gold"].Value, NumberStyles.Integer, culture, out int gold))
                            result += gold * 100;
                        if (match.Groups["silver"].Success && Int32.TryParse(match.Groups["silver"].Value, NumberStyles.Integer, culture, out int silver))
                            result += silver * 10;
                        if (match.Groups["copper"].Success && Int32.TryParse(match.Groups["copper"].Value, NumberStyles.Integer, culture, out int copper))
                            result += copper;
                    }
                }

                if (result == 0)
                {
                    Int32.TryParse(moneyString, NumberStyles.Integer, culture, out result);
                    if (string.Equals(parameter as string, "gold", StringComparison.InvariantCultureIgnoreCase))
                        result *= 100;
                }

                return result;
            }
            return value;
        }
    }
}
