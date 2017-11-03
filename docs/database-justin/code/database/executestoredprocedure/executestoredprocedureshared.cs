using System;
using System.ComponentModel;
using Twenty57.Linx.Plugin.Common;

namespace Twenty57.Linx.Components.Database.ExecuteStoredProcedure
{
	public enum OutputOption
	{
		[Description("First row")]
		FirstRow,
		[Description("First row, else empty row")]
		FirstRowElseEmptyRow,
		[Description("List of rows")]
		ListOfRows,
		[Description("Row by row")]
		RowByRow
	}

	public static class ExecuteStoredProcedureShared
	{
		public const string StoredProcedurePropertyName = "Stored procedure";
		public const string DesignTimeStoredProcedurePropertyName = "Design-time stored procedure";
		public const string ParametersPropertyName = "Parameters";
		public const string ResultSetCountPropertyName = "Number of result sets";
		public const string ResultSetPropertyNameFormat = "Result set {0} fields";
		public const string OutputOptionPropertyName = "Output option";

		public const string ParametersCategoryName = "Parameter values";
		public const string ResultSetsCategoryName = "Result sets";

		public const string OutParametersOutputPropertyName = "ResultParameters";
		public const string ResultSetRowOutputPropertyNameFormat = "Result{0}";
		public const string ResultSetRowsOutputPropertyNameFormat = "Result{0}Rows";

		public const string ResultSetExecutionPathNameFormat = "Result{0}";
	}
}
