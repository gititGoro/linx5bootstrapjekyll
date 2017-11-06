using System;

namespace Twenty57.Linx.Components.Database.Mongo
{
	public static class MongoStringHandler
	{
		public static string GetExecutableQuery(string query, string[] expressionIdentifiers, string[] expressionValues)
		{
			if (expressionIdentifiers.Length != expressionValues.Length)
				throw new Exception("The number of identifiers must match the number of values.");

			for (int index = 0; index < expressionIdentifiers.Length; index++)
				query = query.Replace(expressionIdentifiers[index], expressionValues[index].Replace("'", "''"));

			return query;
		}
	}
}
