using System.Globalization;
using System.Windows.Controls;
using System.Windows.Data;

namespace MouseTrap.Binding
{
	public class FindProgramFilenameRule : ValidationRule
	{
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			if (value is string path)
			{
				if (string.IsNullOrEmpty(path)) return new ValidationResult(false, "Path must have a value");
				else return new ValidationResult(true, null);
			}
			else
			{
				return new ValidationResult(false, "Value cannot be converted to a string");
			}
		}

		public override ValidationResult Validate(object value, CultureInfo cultureInfo, BindingExpressionBase owner)
		{
			// Validate
			var result = base.Validate(value, cultureInfo, owner);

			// Update model
			if (owner is BindingExpression expression && expression.DataItem is ViewModels.FindProgram model)
			{
				model.IsFilenameValid = result.IsValid;
			}

			// Return
			return result;
		}
	}
}
