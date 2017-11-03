using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Reflection;
using System.Text.RegularExpressions;
using Twenty57.Linx.Components.Database.Common;

namespace Twenty57.Linx.Components.Database.ExecuteSQL
{
	public static class DatabaseHelpers
	{
		private static readonly Regex invalidCharacterMatchRegex;
		private static readonly Regex sqlParamRegex;

		static DatabaseHelpers()
		{
			invalidCharacterMatchRegex = new Regex(@"^\d|[^a-z0-9_]", RegexOptions.IgnoreCase | RegexOptions.Compiled);
			sqlParamRegex = new Regex(SqlStringHandler.sqlParameterPrefix + @"\d+", RegexOptions.Compiled);
		}

		public static DataTable RetrieveSchema(ConnectionType connectionType, string connectionString, string sqlString)
		{
			DataTable dataSet = new DataTable();

			switch (connectionType)
			{
				case ConnectionType.SqlServer:
					using (SqlDataAdapter sqlDataAdapter = new SqlDataAdapter(sqlString, connectionString))
					{
						sqlDataAdapter.FillSchema(dataSet, SchemaType.Source);
					}
					break;
				case ConnectionType.Odbc:
					using (OdbcDataAdapter odbcDataAdapter = new OdbcDataAdapter(sqlString, connectionString))
					{
						odbcDataAdapter.FillSchema(dataSet, SchemaType.Source);
					}

					if (dataSet.Columns.Count == 0 && dataSet.Rows.Count == 0)
					{
						OdbcCommand myOdbcCommand = null;

						try
						{
							myOdbcCommand = new OdbcCommand(sqlString);
							myOdbcCommand.Connection = new OdbcConnection(connectionString);
							myOdbcCommand.Connection.Open();

							OdbcDataReader dataReader = myOdbcCommand.ExecuteReader();
							DataTable schema = dataReader.GetSchemaTable();

							foreach (DataRow row in schema.Rows)
							{
								Type runtimeType = row[5].GetType();
								PropertyInfo propInfo = runtimeType.GetProperty("UnderlyingSystemType");
								dataSet.Columns.Add(row[0].ToString(), (Type)propInfo.GetValue(row[5], null));
							}
						}
						finally
						{
							myOdbcCommand.Connection.Close();
						}
					}
					break;
				case ConnectionType.Oracle:
					using (OracleDataAdapter oracleDataAdapter = new OracleDataAdapter(sqlString, connectionString))
					{
						try
						{
							oracleDataAdapter.FillSchema(dataSet, SchemaType.Source);
						}
						catch (Exception exc)
						{
							if (exc.Message == "TTCExecuteSql:ReceiveExecuteResponse - Unexpected Packet received.")
								oracleDataAdapter.FillSchema(dataSet, SchemaType.Source);
							else
								throw exc;
						}
					}
					break;
				case ConnectionType.OleDb:
					using (OleDbDataAdapter oleDbDataAdapter = new OleDbDataAdapter(sqlString, connectionString))
					{
						oleDbDataAdapter.FillSchema(dataSet, SchemaType.Source);
					}
					break;
			}

			return dataSet;
		}

		public static T CheckDbValue<T>(dynamic value)
		{
			if (null == value || value is DBNull)
				return (typeof(T).Equals(typeof(String))) ? (T)Convert.ChangeType(String.Empty, typeof(T)) : default(T);

			if (value is byte[] && typeof(IEnumerable<byte>).IsAssignableFrom(typeof(T)))
				return (T)Convert.ChangeType(new List<byte>(value), typeof(T));

			return (value is T) ? (T)value :
				(typeof(T) == typeof(string)) ? value.ToString() :
					Convert.ChangeType(value, typeof(T));
		}

		public static string GetValidName(string name)
		{
			if (String.IsNullOrWhiteSpace(name))
				throw new Exception("Name cannot be blank.");

			return invalidCharacterMatchRegex.Replace(name, (match) => { return (match.Value.Length > 0 && Char.IsDigit(match.Value[0])) ? "_" + match.Value : "_"; });
		}

		internal static DbCommand CreateCommand(ConnectionType connectionType, string connectionString, string sql, object[] sqlValues, int timeout)
		{
			var assistant = DatabaseAssistant.GetDatabaseAssistant(connectionType);
			var connection = assistant.CreateConnection(connectionString);
			return CreateCommand(connectionType, connection, sql, sqlValues, timeout);
		}

		internal static DbCommand CreateCommand(ConnectionType connectionType, IDbConnection connection, string sql, object[] sqlValues, int timeout)
		{
			var commandText = sql;
			switch (connectionType)
			{
				case ConnectionType.OleDb:
				case ConnectionType.Odbc: commandText = sqlParamRegex.Replace(commandText, "?"); break;
				case ConnectionType.Oracle: commandText = sqlParamRegex.Replace(commandText, (match) => ":" + match.Value.Substring(1)); break;
			}

			DbCommand command = (DbCommand)connection.CreateCommand();
			command.CommandText = commandText;
			command.CommandType = CommandType.Text;

			for (int i = 0; i < sqlValues.Length; i++)
			{
				IDbDataParameter parameter = command.CreateParameter();
				parameter.ParameterName = SqlStringHandler.sqlParameterPrefix + i;
				object value = sqlValues[i];
				if (value == null)
					parameter.Value = DBNull.Value;
				else
					parameter.Value = (!(value is string) && value.GetType().IsEnumerableType()) ? TypeHelpers.ToArray(value) : value;
				command.Parameters.Add(parameter);
			}

			command.CommandTimeout = timeout;
			return command;
		}
	}
}