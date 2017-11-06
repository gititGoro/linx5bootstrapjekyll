using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Twenty57.Linx.Components.Database.ExecuteSQL
{
	public static class ExecuteSQLShared
	{
		public const string SqlStatementPropertyName = "SQL";
		public const string TimeoutPropertyName = "Timeout";
		public const string ReturnOptionsPropertyName = "Return options";
		public const string ResultTypePropertyName = "Result type";

		public const string ExecutionPathName = "ForEachRow";

		public const string SqlValuePropertyPrefix = "Expression";

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
