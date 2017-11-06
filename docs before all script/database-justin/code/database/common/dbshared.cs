using System;
using System.ComponentModel;

namespace Twenty57.Linx.Components.Database.Common
{
	public static class DbShared
	{
		public const string ConnectionTypePropertyName = "Connection type";
		public const string ConnectionStringPropertyName = "Connection string";
		public const string DesignTimeConnectionTypePropertyName = "Design-time connection type";
		public const string DesignTimeConnectionStringPropertyName = "Design-time connection string";
		public const string TransactionPropertyName = "Transaction";

		public static ConnectionType? ToConnectionType(this ConnectionTypeSelection connectionTypeSelection)
		{
			ConnectionType connectionType;
			if (Enum.TryParse<ConnectionType>(connectionTypeSelection.ToString(), out connectionType))
				return connectionType;
			return null;
		}

		public static ConnectionTypeSelection ToConnectionTypeSelection(this ConnectionType connectionType)
		{
			return (ConnectionTypeSelection)Enum.Parse(typeof(ConnectionTypeSelection), connectionType.ToString());
		}
	}


	public enum ConnectionTypeSelection
	{
		[Description("SQL Server")]
		SqlServer,
		Oracle,
		[Description("OLE DB")]
		OleDb,
		[Description("ODBC")]
		Odbc,
		[Description("Use transaction")]
		UseTransaction
	}
}
