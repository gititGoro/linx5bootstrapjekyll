using System;
using System.ComponentModel;
using Twenty57.Linx.Plugin.Common;

namespace Twenty57.Linx.Components.Database.DbBulkCopy
{
	public enum ConnectionType
	{
		[Description("SQL Server")]
		SqlServer,
		Oracle
	}

	public static class DbBulkCopyShared
	{
		public const string ConnectionTypePropertyName = "Connection type\u200B";
		public const string TimeoutPropertyName = "Timeout";
		public const string TableNamePropertyName = "Table name";
		public const string ColumnsPropertyName = "Columns";
		public const string BatchSizePropertyName = "Batch size";

		public const string ExecutionPathName = "Loader";

		public static string GetPropertyName(string columnName)
		{
			columnName = Names.GetValidName(columnName.Replace(" ", string.Empty));
			return Char.ToUpper(columnName[0]) + columnName.Substring(1);
		}
	}
}
