using Oracle.ManagedDataAccess.Client;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using Twenty57.Linx.Components.Database.Common;

namespace Twenty57.Linx.Components.Database.Tests.Helpers
{
	public static class DatabaseHelpers
	{
		public static void ExecuteSqlStatement(string sql, ConnectionType connectionType)
		{
			ExecuteSqlStatement(sql, connectionType, GetDefaultConnectionString(connectionType));
		}

		public static void ExecuteSqlStatement(string sql, ConnectionType connectionType, string connectionString)
		{
			var connection = CreateConnection(connectionType, connectionString);
			try
			{
				connection.Open();
				var command = connection.CreateCommand();
				command.CommandText = sql;
				command.ExecuteNonQuery();
			}
			finally
			{
				connection.Close();
				connection.Dispose();
			}
		}

		public static object ExecuteSqlScalar(string sql, ConnectionType connectionType)
		{
			return ExecuteSqlScalar(sql, connectionType, GetDefaultConnectionString(connectionType));
		}

		public static object ExecuteSqlScalar(string sql, ConnectionType connectionType, string connectionString)
		{
			var connection = CreateConnection(connectionType, connectionString);
			try
			{
				connection.Open();
				var command = connection.CreateCommand();
				command.CommandText = sql;
				return command.ExecuteScalar();
			}
			finally
			{
				connection.Close();
				connection.Dispose();
			}
		}

		public static DataTable GetDataTable(string sql, ConnectionType connectionType)
		{
			return GetDataTable(sql, connectionType, GetDefaultConnectionString(connectionType));
		}

		public static DataTable GetDataTable(string sql, ConnectionType connectionType, string connectionString)
		{
			DbDataAdapter adapter = null;
			switch (connectionType)
			{
				case ConnectionType.Odbc: adapter = new OdbcDataAdapter(sql, connectionString); break;
				case ConnectionType.OleDb: adapter = new OleDbDataAdapter(sql, connectionString); break;
				case ConnectionType.Oracle: adapter = new OracleDataAdapter(sql, connectionString); break;
				case ConnectionType.SqlServer: adapter = new SqlDataAdapter(sql, connectionString); break;
			}

			try
			{
				DataSet dataSet = new DataSet();
				adapter.Fill(dataSet);
				return dataSet.Tables[0];
			}
			finally
			{
				adapter.Dispose();
			}
		}

		public static string GetDefaultConnectionString(ConnectionType connectionType)
		{
			switch (connectionType)
			{
				case ConnectionType.Odbc: return Utilities.OdbcConnectionString;
				case ConnectionType.OleDb: return Utilities.OleDbConnectionString;
				case ConnectionType.Oracle: return Utilities.OracleConnectionString;
				case ConnectionType.SqlServer: return Utilities.SqlConnectionString;
			}
			return null;
		}

		private static IDbConnection CreateConnection(ConnectionType connectionType, string connectionString)
		{
			switch (connectionType)
			{
				case ConnectionType.Odbc: return new OdbcConnection(connectionString);
				case ConnectionType.OleDb: return new OleDbConnection(connectionString);
				case ConnectionType.Oracle: return new OracleConnection(connectionString);
				case ConnectionType.SqlServer: return new SqlConnection(connectionString);
			}
			return null;
		}
	}
}
