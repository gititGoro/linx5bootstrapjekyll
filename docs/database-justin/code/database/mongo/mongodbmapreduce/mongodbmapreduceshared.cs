using System.ComponentModel;

namespace Twenty57.Linx.Components.Database.Mongo.MongoDBMapReduce
{
	public static class MongoDBMapReduceShared
	{
		public static class Names
		{
			public static string Collection = "Collection";
			public const string Query = "Query";
			public static string QueryExpressions = "QueryExpressions";
			public const string Map = "Map";
			public static string MapExpressions = "MapExpressions";
			public const string Reduce = "Reduce";
			public static string ReduceExpressions = "ReduceExpressions";
			public const string Finalize = "Finalize";
			public static string FinalizeExpressions = "FinalizeExpressions";
			public const string Sort = "Sort";
			public const string Limit = "Limit";
			public static string ExecutionPath = "ForEachRow";
			public const string ConnectionString = "Connection string";
			public const string OutputType = "OutputType";
			public const string ReturnOptionsPropertyName = "Return options";
		}
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
	}
}