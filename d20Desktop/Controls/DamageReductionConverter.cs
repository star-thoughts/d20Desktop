using Fiction.GameScreen.Combat;
using Fiction.GameScreen.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Fiction.GameScreen.Controls
{
    public sealed class DamageReductionConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is IDamageReduction vm)
            {
                return ConvertDamageReduction(vm);
            }
            else if (value is IEnumerable<DamageReduction> list)
            {
                string separator = "; ";
                string[] parts = list.Select(p => ConvertDamageReduction(p))
                    .ToArray();

                return string.Join(separator, parts);
            }
            return value?.ToString();
        }

        private static string ConvertDamageReduction(IDamageReduction vm)
        {
            if (vm.Types.Any())
            {
                string separator = vm.RequiresAllTypes ? " and " : " or ";
                return string.Format("{0}/{1}", vm.Amount, string.Join(separator, vm.Types.ToArray()));
            }
            else
            {
                return string.Format("{0}/-", vm.Amount);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
