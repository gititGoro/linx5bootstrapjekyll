using System.ComponentModel;

namespace Twenty57.Linx.Components.MongoDB
{
	public static class MongoDBWriteShared
	{
		public enum ReturnModeType
		{
			[Description("First row")]
			FirstRow,
			[Description("First row, else empty row")]
			FirstRowElseEmptyRow,
			[Description("List of rows")]
			ListOfRows,
			[Description("Row by row")]
			RowByRow
		};

		public static class Names
		{
			public static string Criteria = "Criteria";
			public static string Operation = "Operation";
			public static string InsertIfNotFound = "Insert if not found";
			public static string UpdateOperationExpressions = "UpdateOperationExpression";
			public static string UpdateOperation = "Update operation";
			public static string Data = "Data";
			public static string Collection = "Collection";
			public static string ConnectionString = "Connection string";
			public static string CriteriaExpressions = "CriteriaExpression";
		}
	}
}