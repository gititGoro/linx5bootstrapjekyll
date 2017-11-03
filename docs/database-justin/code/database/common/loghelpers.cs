using System;
using System.Collections.Generic;
using System.Data;
namespace Twenty57.Linx.Components.Database.Common
{
	public static class LogHelpers
	{
		public static string GetDisplayString(object value)
		{
			if (value == null)
				return "null";
			return (value is string) && (value.ToString().Length > 100) ? value.ToString().Substring(0, 97) + "..." : value.ToString();
		}

		public static string GetDescription(this IDbCommand command)
		{
			List<string> parameterList = new List<string>(command.Parameters.Count);
			foreach (var next in command.Parameters)
			{
				var parameter = (IDataParameter)next;
				parameterList.Add(string.Format("{0}: {1}", parameter.ParameterName, LogHelpers.GetDisplayString(parameter.Value)));
			}

			return string.Format("Command text: <{0}>{1}",
				command.CommandText,
				parameterList.Count == 0 ? string.Empty : string.Format("{0}Parameters:{0}{1}", Environment.NewLine, string.Join(Environment.NewLine, parameterList)));
		}
	}
}
