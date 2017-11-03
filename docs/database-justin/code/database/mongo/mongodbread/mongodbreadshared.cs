using System.ComponentModel;
namespace Twenty57.Linx.Components.Database.Mongo.MongoDBRead
{
	public static class MongoDBReadShared
	{
		public static class Names
		{
			public static string Collection = "Collection";
			public static string AggregationPipelineExpressions = "AggreagationPipelineExpression";
			public static string FieldsExpressions = "FieldsExpression";
			public static string QueryExpressions = "QueryExpression";
			public static string ExecutionPath = "ForEachRow";
			public const string ConnectionString = "Connection string";
			public const string Query = "Query";
			public const string OutputType = "OutputType";
			public const string AggregationPipeline = "Pipelines";
			public const string Operation = "Operation";
			public const string Fields = "Fields";
			public const string Sort = "Sort";
			public const string Skip = "Skip";
			public const string Limit = "Limit";
			public const string ReturnOptionsPropertyName = "Return options";
		}

		public enum OperationType
		{
			Find,
			Aggregate
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