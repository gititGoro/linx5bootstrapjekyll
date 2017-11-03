using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;

namespace Twenty57.Linx.Components.Database.Common
{
	public enum ConnectionType
	{
		[Description("SQL Server")]
		SqlServer,
		Oracle,
		[Description("OLE DB")]
		OleDb,
		[Description("ODBC")]
		Odbc
	}

	public abstract class DatabaseAssistant
	{
		private const string TableNameRegexString = @"(\w+\.)?\""?(\w[\w ]*)\""?";
		private const string StoredProcedureNameRegexString = @"(\w+\.)?\""?(\w[\w ]*)\""?(;\d+)?";

		private static readonly Regex TableNameRegex;
		private static readonly Regex StoredProcedureNameRegex;

		static DatabaseAssistant()
		{
			TableNameRegex = new Regex(TableNameRegexString, RegexOptions.Compiled);
			StoredProcedureNameRegex = new Regex(StoredProcedureNameRegexString, RegexOptions.Compiled);
		}

		public abstract DatabaseModel.ConnectionStringComponent[] BasicConnectionStringComponents { get; }

		public string RefreshConnectionString(string previousConnectionString)
		{
			return ConnectionStringHelpers.GetConnectionString(
				RefreshConnectionParameters(
					previousConnectionString.GetConnectionParameterMap().ToLookup(p => p.Key, p => p.Value)
				).ToLookup(p => p.Key, p => p.Value)
			);
		}

		public Dictionary<string, string> RefreshConnectionParameters(ILookup<string, string> previousConnectionParameters, bool retainUnknownParameters = false)
		{
			Dictionary<string, string> parameterMap = new Dictionary<string, string>();
			foreach (var nextConnectionStringComponent in BasicConnectionStringComponents)
			{
				var matchingParameter = previousConnectionParameters.FirstOrDefault(p => p.Key.Equals(nextConnectionStringComponent.Name, StringComparison.InvariantCultureIgnoreCase));
				parameterMap.Add(matchingParameter == null ? nextConnectionStringComponent.Name : matchingParameter.Key, matchingParameter == null ? nextConnectionStringComponent.DefaultTextValue : matchingParameter.First());
			}

			if (retainUnknownParameters)
			{
				foreach (var nextPreviousParameter in previousConnectionParameters)
					if (!parameterMap.Keys.Any(k => k.Equals(nextPreviousParameter.Key, StringComparison.InvariantCultureIgnoreCase)))
						parameterMap.Add(nextPreviousParameter.Key, nextPreviousParameter.First());
			}

			ILookup<string, string> newConnectionParameters = parameterMap.ToLookup(p => p.Key, p => p.Value);
			List<string> parametersToRemove = new List<string>(parameterMap.Count);
			foreach (var nextParameterName in parameterMap.Keys)
			{
				var connectionStringComponent = BasicConnectionStringComponents.SingleOrDefault(c => c.Name.Equals(nextParameterName, StringComparison.CurrentCultureIgnoreCase));
				if ((connectionStringComponent != null) && (!connectionStringComponent.IsApplicable(newConnectionParameters)))
					parametersToRemove.Add(nextParameterName);
			}
			foreach (string nextParameter in parametersToRemove)
				parameterMap.Remove(nextParameter);

			return parameterMap;
		}

		public IEnumerable<DatabaseModel.Column> GetTableColumns(IDbConnection connection, string tableName)
		{
			Match m = TableNameRegex.Match(tableName);
			string domain = (m.Success) && (m.Groups[1].Length != 0) ? m.Groups[1].Value.Substring(0, m.Groups[1].Value.Length - 1) : null;
			if (m.Success)
				tableName = m.Groups[2].Value;
			return GetTableColumns(connection, domain, tableName);
		}

		public IEnumerable<DatabaseModel.ProcedureParameter> GetStoredProcedureParameters(IDbConnection connection, string storedProcedureName)
		{
			Match m = StoredProcedureNameRegex.Match(storedProcedureName);
			string domain = (m.Success) && (m.Groups[1].Length != 0) ? m.Groups[1].Value.Substring(0, m.Groups[1].Value.Length - 1) : null;
			if (m.Success)
				storedProcedureName = m.Groups[2].Value;
			return GetStoredProcedureParameters(connection, domain, storedProcedureName);
		}

		public IEnumerable<DatabaseModel.ProcedureParameter> GetStoredProcedureParameters(IDbConnection connection, string domain, string storedProcedureName)
		{
			return GetStoredProcedureParametersUnordered(connection, domain, storedProcedureName).OrderBy(p => p.Position);
		}

		protected IDbCommand PrepareDefaultStoredProcedureCommand(IDbConnection connection, string storedProcedureName)
		{
			return BuildStoredProcedureCommand(connection, storedProcedureName,
				GetStoredProcedureParameters(connection, storedProcedureName)
				.Select(p => new DatabaseModel.ProcedureParameterValue(p.Name, p.DataType.GetDbDefaultValue(), p.Direction, p.Size, p.DataType == DatabaseModel.DataType.RefCursor)).ToArray());
		}

		public abstract IDbConnection CreateConnection(string connectionString);

		public abstract IEnumerable<string> GetTableNames(IDbConnection connection);

		public abstract IEnumerable<string> GetViewNames(IDbConnection connection);

		protected abstract IEnumerable<DatabaseModel.Column> GetTableColumns(IDbConnection connection, string domain, string tableName);

		public abstract IEnumerable<string> GetStoredProcedureNames(IDbConnection connection);

		protected abstract IEnumerable<DatabaseModel.ProcedureParameter> GetStoredProcedureParametersUnordered(IDbConnection connection, string domain, string storedProcedureName);

		public abstract DatabaseModel.ResultSets GetResultSets(IDbConnection connection, string storedProcedureName);

		public virtual IDbCommand BuildStoredProcedureCommand(IDbConnection connection, string storedProcedureName, params DatabaseModel.ProcedureParameterValue[] parameters)
		{
			var command = connection.CreateCommand();
			command.CommandText = FormatStoredProcedureName(storedProcedureName);
			command.CommandType = CommandType.StoredProcedure;

			foreach (var nextParameter in parameters)
			{
				var dbParameter = command.CreateParameter();
				dbParameter.ParameterName = nextParameter.ParameterName;
				dbParameter.Value = nextParameter.Value;
				dbParameter.Direction = DatabaseModel.ToSystemDirection(nextParameter.Direction);
				if (nextParameter.Size.HasValue)
					dbParameter.Size = nextParameter.Size.Value;
				command.Parameters.Add(dbParameter);
			}

			return command;
		}

		public static DatabaseAssistant GetDatabaseAssistant(ConnectionType connectionType)
		{
			switch (connectionType)
			{
				case ConnectionType.SqlServer: return new SqlServerDatabaseAssistant();
				case ConnectionType.Oracle: return new OracleDatabaseAssistant();
				case ConnectionType.OleDb: return new OleDbDatabaseAssistant();
				case ConnectionType.Odbc: return new OdbcDatabaseAssistant();
				default: throw new ArgumentException("Unsupported ConnectionType: " + connectionType);
			}
		}

		public static ConnectionType? DetectConnectionType(string connectionString)
		{
			var connectionParameters = connectionString.GetConnectionParameterMap();

			if (connectionParameters.Any(p => (p.Key.Equals("provider", StringComparison.InvariantCultureIgnoreCase))))
				return ConnectionType.OleDb;
			if (connectionParameters.Keys.Any(n => n.Equals("initial catalog", StringComparison.InvariantCultureIgnoreCase)))
				return ConnectionType.SqlServer;
			if (connectionParameters.Keys.Any(n => n.Equals("data source", StringComparison.InvariantCultureIgnoreCase)))
				return ConnectionType.Oracle;
			if ((connectionParameters.Keys.Any(n => n.Equals("driver", StringComparison.InvariantCultureIgnoreCase))) || (connectionParameters.Keys.Any(n => n.Equals("dsn", StringComparison.InvariantCultureIgnoreCase))))
				return ConnectionType.Odbc;
			return null;
		}

		protected static string FormatStoredProcedureName(string storedProcedureName)
		{
			Match m = StoredProcedureNameRegex.Match(storedProcedureName);
			return m.Success ? $"{m.Groups[1]}\"{m.Groups[2]}\"{m.Groups[3]}" : storedProcedureName;
		}
	}

	public class SqlServerDatabaseAssistant : DatabaseAssistant
	{
		public override DatabaseModel.ConnectionStringComponent[] BasicConnectionStringComponents
		{
			get
			{
				var userIdAndPasswordApplicable = new Func<DatabaseModel.ConnectionStringComponent.ConnectionParameterLookup, bool>(lookup =>
				{
					return (!lookup.ContainsKey("Integrated Security")) || (!lookup["Integrated Security"].Equals("True", StringComparison.CurrentCultureIgnoreCase));
				});

				return new DatabaseModel.ConnectionStringComponent[] {
					new DatabaseModel.ConnectionStringComponent("Data Source", "The name of the server.", "Database server"),
					new DatabaseModel.ConnectionStringComponent("Initial Catalog", "The name of the database to connect to.", "Database"),
					new DatabaseModel.ConnectionStringComponent("Integrated Security", "Select whether Windows authentication should be used.", "Windows authentication", DatabaseModel.ConnectionStringComponentType.Flag) { FalseValue = string.Empty },
					new DatabaseModel.ConnectionStringComponent("User ID", "The username for authentication.", isApplicableFunc:userIdAndPasswordApplicable),
					new DatabaseModel.ConnectionStringComponent("Password", "The password for authentication.", isApplicableFunc:userIdAndPasswordApplicable)
				};
			}
		}

		public override IDbConnection CreateConnection(string connectionString)
		{
			return new SqlConnection(connectionString);
		}

		public override IEnumerable<string> GetTableNames(IDbConnection connection)
		{
			//https://stackoverflow.com/questions/13216564/use-sqlconnection-getschema-to-get-tables-only-no-views
			using (DataTable dt = ((SqlConnection)connection).GetSchema("Tables", new string[] { null, null, null, "BASE TABLE" }))
			{
				foreach (DataRow dr in dt.Rows)
					yield return $"{dr["TABLE_SCHEMA"]}.{dr["TABLE_NAME"]}";
			}
		}

		public override IEnumerable<string> GetViewNames(IDbConnection connection)
		{
			using (DataTable dt = ((SqlConnection)connection).GetSchema("Views"))
			{
				foreach (DataRow dr in dt.Rows)
					yield return $"{dr["TABLE_SCHEMA"]}.{dr["TABLE_NAME"]}";
			}
		}

		protected override IEnumerable<DatabaseModel.Column> GetTableColumns(IDbConnection connection, string domain, string tableName)
		{
			using (DataTable dt = ((SqlConnection)connection).GetSchema("Columns", new string[] { null, domain, tableName }))
			{
				foreach (DataRow dr in dt.Rows.OfType<DataRow>().OrderBy(row => Convert.ToInt32(row["ORDINAL_POSITION"])))
				{
					int? scale = dr["NUMERIC_SCALE"] == DBNull.Value ? (int?)null : (int)dr["NUMERIC_SCALE"];
					yield return new DatabaseModel.Column
					{
						Name = dr["COLUMN_NAME"].ToString(),
						DataType = DatabaseModel.ParseDataType(dr["DATA_TYPE"].ToString(), scale ?? 0),
						Precision = dr["NUMERIC_PRECISION"] == DBNull.Value ? (int?)null : (byte)dr["NUMERIC_PRECISION"],
						Scale = scale,
						Size = dr["CHARACTER_MAXIMUM_LENGTH"] == DBNull.Value ? (int?)null : (int)dr["CHARACTER_MAXIMUM_LENGTH"],
						IsNullable = dr["IS_NULLABLE"].ToString() == "YES"
					};
				}
			}
		}

		public override IEnumerable<string> GetStoredProcedureNames(IDbConnection connection)
		{
			string[] restrictions = new string[4];
			restrictions[3] = "PROCEDURE";
			using (DataTable dt = ((SqlConnection)connection).GetSchema("Procedures", restrictions))
			{
				foreach (DataRow dr in dt.Rows)
					yield return $"{dr["ROUTINE_SCHEMA"]}.{dr["ROUTINE_NAME"]}";
			}
		}

		protected override IEnumerable<DatabaseModel.ProcedureParameter> GetStoredProcedureParametersUnordered(IDbConnection connection, string domain, string storedProcedureName)
		{
			yield return new DatabaseModel.ProcedureParameter
			{
				Name = "@RETURN_VALUE",
				Direction = DatabaseModel.ParameterDirection.ReturnValue,
				DataType = DatabaseModel.DataType.Int32
			};

			using (DataTable dt = ((SqlConnection)connection).GetSchema("ProcedureParameters", new string[] { null, domain, storedProcedureName }))
			{
				foreach (DataRow dr in dt.Rows)
				{
					int? scale = dr["NUMERIC_SCALE"] == DBNull.Value ? (int?)null : (int)dr["NUMERIC_SCALE"];
					yield return new DatabaseModel.ProcedureParameter
					{
						Name = dr["PARAMETER_NAME"].ToString(),
						Direction = (DatabaseModel.ParameterDirection)Enum.Parse(typeof(DatabaseModel.ParameterDirection), dr["PARAMETER_MODE"].ToString(), true),
						DataType = DatabaseModel.ParseDataType(dr["DATA_TYPE"].ToString(), scale ?? 0),
						Precision = dr["NUMERIC_PRECISION"] == DBNull.Value ? (int?)null : (byte)dr["NUMERIC_PRECISION"],
						Scale = scale,
						Size = dr["CHARACTER_MAXIMUM_LENGTH"] == DBNull.Value ? (int?)null : (int)dr["CHARACTER_MAXIMUM_LENGTH"],
						IsNullable = false,
						Position = (int)dr["ORDINAL_POSITION"]
					};
				}
			}
		}

		public override DatabaseModel.ResultSets GetResultSets(IDbConnection connection, string storedProcedureName)
		{
			IDbCommand command = null;

			try
			{
				command = PrepareDefaultStoredProcedureCommand(connection, storedProcedureName);
				return GetResultSets(command);
			}
			catch
			{
				if (command != null)
				{
					for (int index = 0; index < command.Parameters.Count; index++)
						(command.Parameters[index] as SqlParameter).Value = null;

					return GetResultSets(command);
				}

				throw;
			}
			finally
			{
				if (command != null)
					command.Dispose();
			}
		}

		private DatabaseModel.ResultSets GetResultSets(IDbCommand command)
		{
			using (SqlDataAdapter adapter = new SqlDataAdapter())
			{
				adapter.SelectCommand = (SqlCommand)command;
				DataSet dataSet = new DataSet();
				adapter.FillSchema(dataSet, SchemaType.Source);
				return new DatabaseModel.ResultSets(dataSet);
			}
		}
	}

	public class OracleDatabaseAssistant : DatabaseAssistant
	{
		public override DatabaseModel.ConnectionStringComponent[] BasicConnectionStringComponents
		{
			get
			{
				return new DatabaseModel.ConnectionStringComponent[] {
					new DatabaseModel.ConnectionStringComponent("Data Source", "The name of the tns_names entry, or the full server-description string.", "tns_names entry"),
					new DatabaseModel.ConnectionStringComponent("User ID", "The username for authentication."),
					new DatabaseModel.ConnectionStringComponent("Password", "The password for authentication.")
				};
			}
		}

		public override IDbConnection CreateConnection(string connectionString)
		{
			return new OracleConnection(connectionString);
		}

		public override IEnumerable<string> GetTableNames(IDbConnection connection)
		{
			using (DataTable dt = ((OracleConnection)connection).GetSchema("Tables"))
			{
				foreach (DataRow dr in dt.Rows)
					yield return $"{dr["OWNER"]}.{dr["TABLE_NAME"]}";
			}
		}

		public override IEnumerable<string> GetViewNames(IDbConnection connection)
		{
			using (DataTable dt = ((OracleConnection)connection).GetSchema("Views"))
			{
				foreach (DataRow dr in dt.Rows)
					yield return $"{dr["OWNER"]}.{dr["VIEW_NAME"]}";
			}
		}

		protected override IEnumerable<DatabaseModel.Column> GetTableColumns(IDbConnection connection, string domain, string tableName)
		{
			using (DataTable dt = ((OracleConnection)connection).GetSchema("Columns", new string[] { domain, tableName }))
			{
				foreach (DataRow dr in dt.Rows)
				{
					int? scale = dr["SCALE"] == DBNull.Value ? (int?)null : (int)(decimal)dr["SCALE"];
					yield return new DatabaseModel.Column
					{
						Name = dr["COLUMN_NAME"].ToString(),
						DataType = DatabaseModel.ParseDataType(dr["DATATYPE"].ToString(), scale ?? 0),
						Precision = dr["PRECISION"] == DBNull.Value ? (int?)null : (int)(decimal)dr["PRECISION"],
						Scale = scale,
						Size = dr["LENGTH"] == DBNull.Value ? (int?)null : (int)(decimal)dr["LENGTH"],
						IsNullable = dr["NULLABLE"].ToString() == "Y"
					};
				}
			}
		}

		public override IEnumerable<string> GetStoredProcedureNames(IDbConnection connection)
		{
			using (DataTable dt = ((OracleConnection)connection).GetSchema("Procedures"))
			{
				foreach (DataRow dr in dt.Rows)
					yield return $"{dr["OWNER"]}.{dr["OBJECT_NAME"]}";
			}
		}

		protected override IEnumerable<DatabaseModel.ProcedureParameter> GetStoredProcedureParametersUnordered(IDbConnection connection, string owner, string storedProcedureName)
		{
			using (DataTable dt = ((OracleConnection)connection).GetSchema("ProcedureParameters", new string[] { owner, storedProcedureName }))
			{
				foreach (DataRow dr in dt.Rows)
				{
					int? scale = dr["DATA_SCALE"] == DBNull.Value ? (int?)null : (int)(decimal)dr["DATA_SCALE"];
					yield return new DatabaseModel.ProcedureParameter
					{
						Name = dr["ARGUMENT_NAME"].ToString(),
						Direction = (DatabaseModel.ParameterDirection)Enum.Parse(typeof(DatabaseModel.ParameterDirection), dr["IN_OUT"].ToString().Replace("/", string.Empty), true),
						DataType = DatabaseModel.ParseDataType(dr["DATA_TYPE"].ToString(), scale ?? 0),
						Precision = dr["DATA_PRECISION"] == DBNull.Value ? (int?)null : (int)(decimal)dr["DATA_PRECISION"],
						Scale = scale,
						Size = dr["DATA_LENGTH"] == DBNull.Value ? (int?)null : (int)(decimal)dr["DATA_LENGTH"],
						IsNullable = false,
						Position = (int)(decimal)dr["POSITION"]
					};
				}
			}
		}

		public override DatabaseModel.ResultSets GetResultSets(IDbConnection connection, string storedProcedureName)
		{
			var transaction = connection.BeginTransaction();
			try
			{
				var command = PrepareDefaultStoredProcedureCommand(connection, storedProcedureName);
				command.Transaction = transaction;
				using (OracleDataAdapter adapter = new OracleDataAdapter())
				{
					adapter.SelectCommand = (OracleCommand)command;
					DataSet dataSet = new DataSet();
					try
					{
						adapter.FillSchema(dataSet, SchemaType.Source);
					}
					catch (Exception exc)
					{
						if (exc.Message == "TTCExecuteSql:ReceiveExecuteResponse - Unexpected Packet received.")
							adapter.FillSchema(dataSet, SchemaType.Source);
						else
							throw exc;
					}
					return new DatabaseModel.ResultSets(dataSet);
				}
			}
			finally
			{
				transaction.Rollback();
			}
		}

		public override IDbCommand BuildStoredProcedureCommand(IDbConnection connection, string storedProcedureName, params DatabaseModel.ProcedureParameterValue[] parameters)
		{
			var command = connection.CreateCommand();
			command.CommandText = FormatStoredProcedureName(storedProcedureName);
			command.CommandType = CommandType.StoredProcedure;

			foreach (var nextParameter in parameters)
			{
				var dbParameter = nextParameter.IsRefCursor ? new OracleParameter(nextParameter.ParameterName, OracleDbType.RefCursor) : new OracleParameter(nextParameter.ParameterName, nextParameter.Value);
				dbParameter.Direction = DatabaseModel.ToSystemDirection(nextParameter.Direction);
				if (nextParameter.Size.HasValue)
					dbParameter.Size = nextParameter.Size.Value;
				command.Parameters.Add(dbParameter);
			}

			return command;
		}
	}

	public class OleDbDatabaseAssistant : DatabaseAssistant
	{
		public override DatabaseModel.ConnectionStringComponent[] BasicConnectionStringComponents
		{
			get
			{
				var userIdAndPasswordApplicable = new Func<DatabaseModel.ConnectionStringComponent.ConnectionParameterLookup, bool>(lookup =>
				{
					return (!lookup.ContainsKey("Integrated Security")) || (!lookup["Integrated Security"].Equals("SSPI", StringComparison.CurrentCultureIgnoreCase));
				});

				return new DatabaseModel.ConnectionStringComponent[] {
					new DatabaseModel.ConnectionStringComponent("Provider", "The name of the OLE DB provider.", "OLE DB provider") { DefaultTextValue = "sqloledb" },
					new DatabaseModel.ConnectionStringComponent("Data Source", "The name of the server.", "Database server"),
					new DatabaseModel.ConnectionStringComponent("Initial Catalog", "The name of the database to connect to.", "Database"),
					new DatabaseModel.ConnectionStringComponent("Integrated Security", "Select whether Windows authentication should be used.", "Windows authentication", DatabaseModel.ConnectionStringComponentType.Flag) { TrueValue = "SSPI", FalseValue = string.Empty },
					new DatabaseModel.ConnectionStringComponent("User ID", "The username for authentication.", isApplicableFunc:userIdAndPasswordApplicable),
					new DatabaseModel.ConnectionStringComponent("Password", "The password for authentication.", isApplicableFunc:userIdAndPasswordApplicable)
				};
			}
		}

		public override IDbConnection CreateConnection(string connectionString)
		{
			return new OleDbConnection(connectionString);
		}

		public override IEnumerable<string> GetTableNames(IDbConnection connection)
		{
			using (DataTable dt = ((OleDbConnection)connection).GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[0]))
			{
				foreach (DataRow dr in dt.Rows)
					yield return $"{dr["TABLE_SCHEMA"]}.{dr["TABLE_NAME"]}";
			}
		}

		public override IEnumerable<string> GetViewNames(IDbConnection connection)
		{
			throw new NotImplementedException();
		}

		protected override IEnumerable<DatabaseModel.Column> GetTableColumns(IDbConnection connection, string domain, string tableName)
		{
			var dataTypeMap = new Dictionary<int, string>();
			using (DataTable dt = ((OleDbConnection)connection).GetOleDbSchemaTable(OleDbSchemaGuid.Provider_Types, null))
			{
				foreach (DataRow dr in dt.Rows)
				{
					int dataType = (int)dr["DATA_TYPE"];
					string typeName = dr["TYPE_NAME"].ToString();
					if (dataTypeMap.ContainsKey(dataType))
						dataTypeMap[dataType] = typeName;
					else
						dataTypeMap.Add(dataType, typeName);
				}
			}

			using (DataTable dt = ((OleDbConnection)connection).GetOleDbSchemaTable(OleDbSchemaGuid.Columns, new object[] { null, domain, tableName }))
			{
				foreach (DataRow dr in dt.Rows)
				{
					yield return new DatabaseModel.Column
					{
						Name = dr["COLUMN_NAME"].ToString(),
						DataType = DatabaseModel.ParseDataType(dataTypeMap[(int)dr["DATA_TYPE"]]),
						Precision = dr["NUMERIC_PRECISION"] == DBNull.Value ? (int?)null : (int)dr["NUMERIC_PRECISION"],
						Scale = dr["NUMERIC_SCALE"] == DBNull.Value ? (int?)null : (short)dr["NUMERIC_SCALE"],
						Size = dr["CHARACTER_MAXIMUM_LENGTH"] == DBNull.Value ? (int?)null : dr["CHARACTER_MAXIMUM_LENGTH"] is decimal ? (int)(decimal)dr["CHARACTER_MAXIMUM_LENGTH"] : (int)(long)dr["CHARACTER_MAXIMUM_LENGTH"],
						IsNullable = (bool)dr["IS_NULLABLE"]
					};
				}
			}
		}

		public override IEnumerable<string> GetStoredProcedureNames(IDbConnection connection)
		{

			using (DataTable dt = ((OleDbConnection)connection).GetOleDbSchemaTable(OleDbSchemaGuid.Procedures, new Object[0]))
			{
				foreach (DataRow dr in dt.Rows)
				{
					string[] ProcedureName = dr["PROCEDURE_NAME"].ToString().Split(';');
					if (ProcedureName.Length > 1)
					{
						if (ProcedureName[1] == "1")
							yield return $"{dr["PROCEDURE_SCHEMA"]}.{dr["PROCEDURE_NAME"]}";
					}
					else
						yield return $"{dr["PROCEDURE_SCHEMA"]}.{dr["PROCEDURE_NAME"]}";
				}
			}
		}

		protected override IEnumerable<DatabaseModel.ProcedureParameter> GetStoredProcedureParametersUnordered(IDbConnection connection, string domain, string storedProcedureName)
		{
			using (DataTable dt = ((OleDbConnection)connection).GetOleDbSchemaTable(OleDbSchemaGuid.Procedure_Parameters, new object[] { null, domain, storedProcedureName }))
			{
				foreach (DataRow dr in dt.Rows)
				{
					var parameter = new DatabaseModel.ProcedureParameter
					{
						Name = dr["PARAMETER_NAME"].ToString(),
						DataType = DatabaseModel.ParseDataType(dr["TYPE_NAME"].ToString()),
						Precision = dr["NUMERIC_PRECISION"] == DBNull.Value ? (int?)null : (int)dr["NUMERIC_PRECISION"],
						Scale = dr["NUMERIC_SCALE"] == DBNull.Value ? (int?)null : (short)dr["NUMERIC_SCALE"],
						Size = dr["CHARACTER_MAXIMUM_LENGTH"] == DBNull.Value ? (int?)null : dr["CHARACTER_MAXIMUM_LENGTH"] is decimal ? (int)(decimal)dr["CHARACTER_MAXIMUM_LENGTH"] : (int)(long)dr["CHARACTER_MAXIMUM_LENGTH"],
						IsNullable = (bool)dr["IS_NULLABLE"],
						Position = (int)dr["ORDINAL_POSITION"]
					};
					switch ((int)dr["PARAMETER_TYPE"])
					{
						case 1: parameter.Direction = DatabaseModel.ParameterDirection.In; break;
						case 2: parameter.Direction = DatabaseModel.ParameterDirection.InOut; break;
						case 3: parameter.Direction = DatabaseModel.ParameterDirection.Out; break;
						case 4: parameter.Direction = DatabaseModel.ParameterDirection.ReturnValue; break;
					}
					yield return parameter;
				}
			}
		}

		public override DatabaseModel.ResultSets GetResultSets(IDbConnection connection, string storedProcedureName)
		{
			try
			{
				// FillSchema() finds only the first result set, so try Fill() first.
				var transaction = connection.BeginTransaction();
				try
				{
					var command = PrepareDefaultStoredProcedureCommand(connection, storedProcedureName);
					command.Transaction = transaction;
					using (OleDbDataAdapter adapter = new OleDbDataAdapter())
					{
						adapter.SelectCommand = (OleDbCommand)command;
						DataSet dataSet = new DataSet();
						adapter.Fill(dataSet);
						foreach (DataTable table in dataSet.Tables)
							table.Rows.Clear();
						return new DatabaseModel.ResultSets(dataSet);
					}
				}
				finally
				{
					transaction.Rollback();
				}
			}
			catch
			{
				var parameters = GetStoredProcedureParameters(connection, storedProcedureName);
				IDbCommand command = BuildStoredProcedureCommand(connection, storedProcedureName,
					parameters.Select(p => new DatabaseModel.ProcedureParameterValue(p.Name, p.DataType.GetDbDefaultValue(), p.Direction, p.Size)).ToArray());
				foreach (var nextParameter in parameters)
				{
					var dbParameter = (OleDbParameter)command.Parameters[nextParameter.Name];
					switch (nextParameter.DataType)
					{
						case DatabaseModel.DataType.Boolean: dbParameter.OleDbType = OleDbType.Boolean; break;
						case DatabaseModel.DataType.Byte: dbParameter.OleDbType = OleDbType.TinyInt; break;
						case DatabaseModel.DataType.Date: dbParameter.OleDbType = OleDbType.Date; break;
						case DatabaseModel.DataType.Time: dbParameter.OleDbType = OleDbType.DBTime; break;
						case DatabaseModel.DataType.DateTime:
						case DatabaseModel.DataType.TimeStamp: dbParameter.OleDbType = OleDbType.DBTimeStamp; break;
						case DatabaseModel.DataType.Currency: dbParameter.OleDbType = OleDbType.Currency; break;
						case DatabaseModel.DataType.Decimal: dbParameter.OleDbType = OleDbType.Decimal; break;
						case DatabaseModel.DataType.Double: dbParameter.OleDbType = OleDbType.Double; break;
						case DatabaseModel.DataType.Int16: dbParameter.OleDbType = OleDbType.SmallInt; break;
						case DatabaseModel.DataType.Int32: dbParameter.OleDbType = OleDbType.Integer; break;
						case DatabaseModel.DataType.Int64: dbParameter.OleDbType = OleDbType.BigInt; break;
						case DatabaseModel.DataType.AnsiString: dbParameter.OleDbType = OleDbType.VarChar; break;
						case DatabaseModel.DataType.AnsiStringFixedLength: dbParameter.OleDbType = OleDbType.Char; break;
						case DatabaseModel.DataType.Xml:
						case DatabaseModel.DataType.String: dbParameter.OleDbType = OleDbType.VarWChar; break;
						case DatabaseModel.DataType.StringFixedLength: dbParameter.OleDbType = OleDbType.WChar; break;
						case DatabaseModel.DataType.Guid: dbParameter.OleDbType = OleDbType.Guid; break;
						case DatabaseModel.DataType.Binary: dbParameter.OleDbType = OleDbType.Binary; break;
						default: throw new Exception($"Data type not mapped: {nextParameter.DataType}.");
					}
				}

				using (OleDbDataAdapter adapter = new OleDbDataAdapter())
				{
					adapter.SelectCommand = (OleDbCommand)command;
					DataSet dataSet = new DataSet();
					adapter.FillSchema(dataSet, SchemaType.Source);
					return new DatabaseModel.ResultSets(dataSet);
				}
			}
		}
	}

	public class OdbcDatabaseAssistant : DatabaseAssistant
	{
		public override DatabaseModel.ConnectionStringComponent[] BasicConnectionStringComponents
		{
			get
			{
				return new DatabaseModel.ConnectionStringComponent[] {
					new DatabaseModel.ConnectionStringComponent("DSN", "The Data Source Name of the of the ODBC data source", "Data source name"),
					new DatabaseModel.ConnectionStringComponent("Uid", "The username for authentication.", "Username"),
					new DatabaseModel.ConnectionStringComponent("Pwd", "The password for authentication.", "Password")
				};
			}
		}

		public override IDbConnection CreateConnection(string connectionString)
		{
			return new OdbcConnection(connectionString);
		}

		public override IEnumerable<string> GetTableNames(IDbConnection connection)
		{
			using (DataTable dt = ((OdbcConnection)connection).GetSchema("Tables"))
			{
				foreach (DataRow dr in dt.Rows)
					yield return $"{dr["TABLE_SCHEM"]}.{dr["TABLE_NAME"]}";
			}
		}

		public override IEnumerable<string> GetViewNames(IDbConnection connection)
		{
			throw new NotImplementedException();
		}

		protected override IEnumerable<DatabaseModel.Column> GetTableColumns(IDbConnection connection, string domain, string tableName)
		{
			using (DataTable dt = ((OdbcConnection)connection).GetSchema("Columns", new string[] { null, domain, tableName }))
			{
				foreach (DataRow dr in dt.Rows)
				{
					yield return new DatabaseModel.Column
					{
						Name = dr["COLUMN_NAME"].ToString(),
						DataType = DatabaseModel.ParseDataType(dr["TYPE_NAME"].ToString()),
						Precision = dr["DECIMAL_DIGITS"] == DBNull.Value ? (int?)null : (short)dr["DECIMAL_DIGITS"],
						Scale = dr["NUM_PREC_RADIX"] == DBNull.Value ? (int?)null : (short)dr["NUM_PREC_RADIX"],
						Size = dr["COLUMN_SIZE"] == DBNull.Value ? (int?)null : (int)dr["COLUMN_SIZE"],
						IsNullable = dr["IS_NULLABLE"].ToString() == "YES"
					};
				}
			}
		}

		public override IEnumerable<string> GetStoredProcedureNames(IDbConnection connection)
		{
			using (DataTable dt = ((OdbcConnection)connection).GetSchema("Procedures"))
			{
				foreach (DataRow dr in dt.Rows)
				{
					string[] ProcedureName = dr["PROCEDURE_NAME"].ToString().Split(';');
					if (ProcedureName.Length > 1)
					{
						if (ProcedureName[1] == "1")
							yield return $"{dr["PROCEDURE_SCHEM"]}.{dr["PROCEDURE_NAME"]}";
					}
					else
						yield return $"{dr["PROCEDURE_SCHEM"]}.{dr["PROCEDURE_NAME"]}";
				}
			}
		}

		protected override IEnumerable<DatabaseModel.ProcedureParameter> GetStoredProcedureParametersUnordered(IDbConnection connection, string domain, string storedProcedureName)
		{
			using (DataTable dt = ((OdbcConnection)connection).GetSchema("ProcedureParameters", new string[] { null, domain, storedProcedureName }))
			{
				foreach (DataRow dr in dt.Rows)
				{
					var parameter = new DatabaseModel.ProcedureParameter
					{
						Name = dr["COLUMN_NAME"].ToString(),
						DataType = DatabaseModel.ParseDataType(dr["TYPE_NAME"].ToString()),
						Precision = dr["DECIMAL_DIGITS"] == DBNull.Value ? (int?)null : (short)dr["DECIMAL_DIGITS"],
						Scale = dr["NUM_PREC_RADIX"] == DBNull.Value ? (int?)null : (short)dr["NUM_PREC_RADIX"],
						Size = dr["COLUMN_SIZE"] == DBNull.Value ? (int?)null : (int)dr["COLUMN_SIZE"],
						IsNullable = dr["IS_NULLABLE"].ToString() == "YES",
						Position = (int)dr["ORDINAL_POSITION"]
					};
					switch ((short)dr["COLUMN_TYPE"])
					{
						case 1: parameter.Direction = DatabaseModel.ParameterDirection.In; break;
						case 2: parameter.Direction = DatabaseModel.ParameterDirection.InOut; break;
						case 3: parameter.Direction = DatabaseModel.ParameterDirection.Out; break;
						case 5: parameter.Direction = DatabaseModel.ParameterDirection.ReturnValue; break;
					}
					yield return parameter;
				}
			}
		}

		public override DatabaseModel.ResultSets GetResultSets(IDbConnection connection, string storedProcedureName)
		{
			using (OdbcDataAdapter adapter = new OdbcDataAdapter())
			{
				DataSet dataSet = new DataSet();
				try
				{
					// FillSchema() finds only the first result set, so try Fill() first.
					var transaction = connection.BeginTransaction();
					try
					{
						var command = PrepareDefaultStoredProcedureCommand(connection, storedProcedureName);
						command.Transaction = transaction;
						adapter.SelectCommand = (OdbcCommand)command;
						adapter.Fill(dataSet);
						foreach (DataTable table in dataSet.Tables)
							table.Rows.Clear();
					}
					finally
					{
						transaction.Rollback();
					}
				}
				catch
				{
					adapter.FillSchema(dataSet, SchemaType.Source);
				}
				return new DatabaseModel.ResultSets(dataSet);
			}
		}

		public override IDbCommand BuildStoredProcedureCommand(IDbConnection connection, string storedProcedureName, params DatabaseModel.ProcedureParameterValue[] parameters)
		{
			var returnValueParameter = parameters.SingleOrDefault(p => p.Direction == DatabaseModel.ParameterDirection.ReturnValue);
			var parametersExcludingReturnValue = parameters.Where(p => p != returnValueParameter);

			var command = connection.CreateCommand();
			command.CommandText = parameters.Length == 0
				? FormatStoredProcedureName(storedProcedureName)
				: string.Format("{{{0}call {1}{2}}}",
					returnValueParameter == null ? string.Empty : "? = ",
					FormatStoredProcedureName(storedProcedureName),
					parametersExcludingReturnValue.Any() ? string.Format(" ({0})", string.Join(", ", parametersExcludingReturnValue.Select(p => "?"))) : string.Empty);
			command.CommandType = CommandType.StoredProcedure;

			if (returnValueParameter != null)
			{
				var dbParameter = new OdbcParameter(returnValueParameter.ParameterName, returnValueParameter.Value) { Direction = ParameterDirection.ReturnValue };
				if (returnValueParameter.Size.HasValue)
					dbParameter.Size = returnValueParameter.Size.Value;
				command.Parameters.Add(dbParameter);
			}
			foreach (var nextParameter in parametersExcludingReturnValue)
			{
				var dbParameter = new OdbcParameter(nextParameter.ParameterName, nextParameter.Value) { Direction = DatabaseModel.ToSystemDirection(nextParameter.Direction) };
				if (nextParameter.Size.HasValue)
					dbParameter.Size = nextParameter.Size.Value;
				command.Parameters.Add(dbParameter);
			}

			return command;
		}
	}
}
