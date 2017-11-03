using System.Linq;
using Twenty57.Linx.Components.Database.Common;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.CodeGeneration;

namespace Twenty57.Linx.Components.Database.Mongo.Common
{
	internal static class MongoDBCodeGenerator
	{
		public static void CompileExpression(IFunctionBuilder functionBuilder, IFunctionData data, string parameterName, string expressionName)
		{
			var propertyValue = data.Properties[parameterName].Value;
			var expressionString = (propertyValue is string) ? propertyValue as string : string.Empty;

			SqlStringHandler stringHandler = SqlStringHandler.GetSqlStringHandler(expressionString);

			var parameterValues = new string[stringHandler.Expressions.Count];
			for (int i = 0; i < parameterValues.Length; i++)
				parameterValues[i] = functionBuilder.GetParamName(expressionName + i) + ".ToString()";

			if (parameterValues.Length > 0)
			{
				functionBuilder.AddCode(string.Format(@"{0} = Twenty57.Linx.Components.Database.Mongo.MongoStringHandler.GetExecutableQuery(
					{0},
					{1},
					new [] {{ {2} }});",
					functionBuilder.GetParamName(parameterName),
					CSharpUtilities.ArrayAsString(stringHandler.Expressions.Select(x => x.MatchText)),
					parameterValues.Aggregate((a, b) => a + ",\n" + b)
					));
			}
		}
	}
}
