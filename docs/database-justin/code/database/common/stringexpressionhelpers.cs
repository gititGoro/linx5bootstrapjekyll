using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.Validation.Custom;

namespace Twenty57.Linx.Components.Database.Common
{
	internal static class StringExpressionHelpers
	{
		public static void Validate(Property property, IDesignerContext context, CustomValidationResults validations, ValidateDynamicValue validationMethod)
		{
			if (!property.IsVisible)
				return;

			var propertyValue = property.Value as string ?? string.Empty;
			if (string.IsNullOrEmpty(propertyValue))
				return;

			var handler = SqlStringHandler.GetSqlStringHandler(propertyValue);
			var validationErrors = new StringBuilder();
			foreach (var expression in handler.DistinctExpressionTexts)
			{
				string validationMessage;
				if (!validationMethod(context.CreateExpression(expression), out validationMessage))
					validationErrors.AppendLine(validationMessage);
			}

			if (validationErrors.Length > 0)
				validations.AddValidationResult(property.Name, validationErrors.ToString().TrimEnd(Environment.NewLine.ToCharArray()), CustomValidationType.Error);
		}

		public static void UpdateOnLoad(Property propertyToUpdate, IReadOnlyDictionary<string, IPropertyData> allDataProperties, string expressionValueKey, bool expressionValueIsZeroBased)
		{
			var propertyValue = propertyToUpdate.Value as string ?? string.Empty;
			if (string.IsNullOrEmpty(propertyValue))
				return;

			var handler = SqlStringHandler.GetSqlStringHandler(propertyValue);
			if (!handler.Expressions.Any())
				return;

			handler.Expressions.Reverse();

			var updatedSQL = handler.SqlString;
			int expressionIndex = (expressionValueIsZeroBased) ? handler.Expressions.Count - 1 : handler.Expressions.Count;
			foreach (var expression in handler.Expressions)
			{
				var expressionPropertyKey = expressionValueKey + expressionIndex;

				if (!allDataProperties.ContainsKey(expressionPropertyKey))
					continue;

				var linxExpression = allDataProperties[expressionPropertyKey].Value as IExpression;
				if (expression.ExpressionText != linxExpression.GetExpression())
					updatedSQL = updatedSQL
						.Remove(expression.StartIndex, expression.EndIndex - expression.StartIndex + 1)
						.Insert(expression.StartIndex, SqlStringHandler.CreateSqlExpression(linxExpression.GetExpression()));

				expressionIndex--;
			}

			if (updatedSQL != handler.SqlString)
				propertyToUpdate.Value = updatedSQL;
		}
	}
}
