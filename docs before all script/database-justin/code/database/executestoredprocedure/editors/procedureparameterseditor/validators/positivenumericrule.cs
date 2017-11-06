using System.Globalization;
using System.Windows.Controls;

namespace Twenty57.Linx.Components.Database.ExecuteStoredProcedure.Editors.ProcedureParametersEditor.Validators
{
	public class ParameterSizeValidator : ValidationRule
	{
		public override ValidationResult Validate(object value, CultureInfo cultureInfo)
		{
			string stringValue = value as string;
			if (string.IsNullOrEmpty(stringValue))
				return new ValidationResult(false, "This procedure parameter requires a size value.");
			int intValue;
			if ((!int.TryParse(stringValue, out intValue)) || (intValue <= 0))
				return new ValidationResult(false, "Size must be a positive integer.");
			return new ValidationResult(true, null);
		}
	}
}
