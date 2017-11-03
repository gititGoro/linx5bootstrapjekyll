using System;
using System.Collections.Generic;
using System.Linq;

namespace Twenty57.Linx.Components.Database.Common
{
	public static class ConnectionStringHelpers
	{
		private const char ConnectionParameterDelimiter = ';', ConnectionParameterValueDelimiter = '=';

		public static Dictionary<string, string> GetConnectionParameterMap(this string connectionString)
		{
			string[] parameters = connectionString.Split(new char[] { ConnectionParameterDelimiter }, StringSplitOptions.RemoveEmptyEntries);
			Dictionary<string, string> parameterMap = new Dictionary<string, string>(parameters.Length);
			foreach (string nextParameter in parameters)
			{
				int equalIdx = nextParameter.IndexOf(ConnectionParameterValueDelimiter);
				if (equalIdx > 0)
					parameterMap[nextParameter.Substring(0, equalIdx).Trim()] = nextParameter.Substring(equalIdx + 1).Trim();
			}
			return parameterMap;
		}

		public static string GetConnectionString(ILookup<string, string> connectionParameterMap)
		{
			return string.Join(ConnectionParameterDelimiter.ToString(), connectionParameterMap.Select(p => string.Format("{0}{1}{2}", p.Key, ConnectionParameterValueDelimiter, p.First()))) + ConnectionParameterDelimiter;
		}

		public static bool IsEmptyConnectionString(this string connectionString)
		{
			if (string.IsNullOrEmpty(connectionString))
				return true;
			foreach (string nextValue in connectionString.GetConnectionParameterMap().Values)
				if (!string.IsNullOrEmpty(nextValue))
					return false;
			return true;
		}
	}
}
