using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Twenty57.Linx.Domain.Helpers;

namespace Twenty57.Linx.Domain.Helpers
{
	public static class ExpressionParserAddition
	{
		public const string settingsResourcesId = "$.Settings";

		public static string RefValue(this string val)
		{
			if (! val.IsExpression())
				return null;
			string trimmedVal = val.Substring(ExpressionParser.expressionPrefix.Length,val.Length - ExpressionParser.expressionPrefix.Length - ExpressionParser.expressionSufix.Length).Trim();
			Match match = Regex.Match(trimmedVal, ExpressionParser.variableMatchRegex);
			return (match.Success) && (match.Index == 0) && (match.Length == trimmedVal.Length) ? trimmedVal : null;
		}

		public static bool IsSettingVariable(this string val)
		{
			string refValue = val.RefValue();
			return refValue==null ? false : refValue.StartsWith(settingsResourcesId+'.');
		}
	}
}
