using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Fiction.Windows
{
	class RequiredValidationRule : ValidationRule
	{
		public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
		{
			return string.IsNullOrEmpty((string)value) ? new ValidationResult(false, Resources.WindowResources.ValueRequiredValidationError) : new ValidationResult(true, null);
		}
	}
}
