using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Fiction.Windows
{
	public class RegexValidationRule : ValidationRule
	{
		public string Regex { get; set; }
		public override ValidationResult Validate(object value, System.Globalization.CultureInfo cultureInfo)
		{
			if (System.Text.RegularExpressions.Regex.Match((string)value, Regex).Success)
				return new ValidationResult(true, null);
			return new ValidationResult(false, Resources.WindowResources.RegexValidationError);
		}
	}
}
