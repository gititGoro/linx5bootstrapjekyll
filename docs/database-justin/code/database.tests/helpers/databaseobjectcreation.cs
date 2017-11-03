using Oracle.ManagedDataAccess.Client;
using System.Data.Common;
using System.Data.Odbc;
using System.Data.OleDb;
using System.Data.SqlClient;
using Twenty57.Linx.Components.Database.Common;

namespace Twenty57.Linx.Components.Database.Tests.Helpers
{
	public static class DatabaseObjectCreation
	{
		public static DatabaseModel.Columns CreateTestTable(ConnectionType connectionType)
		{
			string createTableStatement = connectionType == ConnectionType.Oracle
				? @"CREATE TABLE TestTable (
					IntValue INT NOT NULL,
					DoubleValue REAL,
					StringValue NVARCHAR2(20),
					DateValue DATE,
					BytesValue BLOB
					)"
				: @"CREATE TABLE TestTable (
					IntValue INT NOT NULL,
					DoubleValue NUMERIC(10,4),
					StringValue NVARCHAR(20),
					DateValue DATE,
					BytesValue VARBINARY(50)
					)";
			return CreateTestTable(createTableStatement, connectionType);
		}

		public static DatabaseModel.Columns CreateTestTable(string createTableStatement, ConnectionType connectionType)
		{
			try
			{
				RemoveTestTable(connectionType);
			}
			catch { }

			try
			{
				DatabaseHelpers.ExecuteSqlStatement(createTableStatement, connectionType);

				DatabaseAssistant databaseAssistant = DatabaseAssistant.GetDatabaseAssistant(connectionType);
				using (var connection = databaseAssistant.CreateConnection(DatabaseHelpers.GetDefaultConnectionString(connectionType)))
				{
					connection.Open();
					return new DatabaseModel.Columns(databaseAssistant.GetTableColumns(connection, "TESTTABLE"));
				}
			}
			catch
			{
				try
				{
					RemoveTestTable(connectionType);
				}
				catch { }
				throw;
			}
		}

		public static void RemoveTestTable(ConnectionType connectionType)
		{
			DatabaseHelpers.ExecuteSqlStatement(@"DROP TABLE TESTTABLE", connectionType);
		}

		public static DatabaseModel.ProcedureParameters CreateParameterTestProc(ConnectionType connectionType)
		{
			string createProcedureStatement = connectionType == ConnectionType.Oracle
				? @"CREATE PROCEDURE ""Test Proc"" (
					""@IntValue"" INT,
					""@DoubleValue"" REAL,
					""@StringValue"" NVARCHAR2,
					""@StringValueOut"" IN OUT NVARCHAR2,
					""@DateValue"" DATE,
					""@DateValueOut"" IN OUT DATE,
					""@BytesValue"" BLOB,
					""@BytesValueOut"" IN OUT BLOB
				)
				AS BEGIN
					""@StringValueOut"" := ""@StringValue"";
					""@DateValueOut"" := ""@DateValue"";
					""@BytesValueOut"" := ""@BytesValue"";
				END;"
				: @"CREATE PROCEDURE ""Test Proc"" (
					@IntValue INT,
					@DoubleValue NUMERIC(10,4),
					@StringValue NVARCHAR(20),
					@StringValueOut NVARCHAR(20) OUTPUT,
					@DateValue DATE,
					@DateValueOut DATE OUTPUT,
					@BytesValue VARBINARY(50),
					@BytesValueOut VARBINARY(50) OUTPUT
				)
				AS BEGIN
					SET @StringValueOut = @StringValue;
					SET @DateValueOut = @DateValue;
					SET @BytesValueOut = @BytesValue;
					RETURN 1;
				END;";
			return CreateTestProc(createProcedureStatement, connectionType);
		}

		public static DatabaseModel.ProcedureParameters CreateResultSetTestProc(ConnectionType connectionType)
		{
			string createProcedureStatement = connectionType == ConnectionType.Oracle
				? @"CREATE PROCEDURE ""Test Proc"" (""@IntValue"" INT, ResultSet1 OUT SYS_REFCURSOR)
				AS BEGIN
					OPEN ResultSet1 FOR SELECT 1 AS ""Counter"", 'one' AS ""StringValue"" FROM Dual WHERE 1 <= ""@IntValue""
						UNION SELECT 2 AS ""Counter"", 'two' AS ""StringValue"" FROM Dual WHERE 2 <= ""@IntValue"";
				END;"
				: @"CREATE PROCEDURE ""Test Proc"" (@IntValue INT)
				AS BEGIN
					SELECT 1 AS Counter, 'one' AS StringValue WHERE 1 <= @IntValue
						UNION SELECT 2 AS Counter, 'two' AS StringValue WHERE 2 <= @IntValue;
				END;";
			return CreateTestProc(createProcedureStatement, connectionType);
		}

		public static DatabaseModel.ProcedureParameters CreateResultSetsTestProc(ConnectionType connectionType)
		{
			string createProcedureStatement = connectionType == ConnectionType.Oracle
				? @"CREATE PROCEDURE ""Test Proc"" (ResultSet1 OUT SYS_REFCURSOR, ResultSet2 OUT SYS_REFCURSOR)
				AS BEGIN
					OPEN ResultSet1 FOR SELECT 1 AS ""Counter"", 'one' AS ""StringValue"" FROM Dual
						UNION SELECT 2 AS ""Counter"", 'two' AS ""StringValue"" FROM Dual;
					OPEN ResultSet2 FOR SELECT 'qwer' AS ""StringValue"" FROM Dual;
				END;"
				: @"CREATE PROCEDURE ""Test Proc"" 
				AS BEGIN
					SELECT 1 AS Counter, 'one' AS StringValue
						UNION SELECT 2 AS Counter, 'two' AS StringValue;
					SELECT 'qwer' AS StringValue;
				END;";
			return CreateTestProc(createProcedureStatement, connectionType);
		}

		public static DatabaseModel.ProcedureParameters CreateParameterAndResultSetsTestProc(ConnectionType connectionType)
		{
			string createProcedureStatement = connectionType == ConnectionType.Oracle
				? @"CREATE PROCEDURE ""Test Proc"" (
					""@IntValue"" INT,
					""@DoubleValue"" REAL,
					""@StringValue"" NVARCHAR2,
					""@StringValueOut"" IN OUT NVARCHAR2,
					""@DateValue"" DATE,
					""@DateValueOut"" IN OUT DATE,
					""@BytesValue"" BLOB,
					""@BytesValueOut"" IN OUT BLOB,
					ResultSet1 OUT SYS_REFCURSOR,
					ResultSet2 OUT SYS_REFCURSOR
				)
				AS BEGIN
					""@StringValueOut"" := ""@StringValue"";
					""@DateValueOut"" := ""@DateValue"";
					""@BytesValueOut"" := ""@BytesValue"";
					OPEN ResultSet1 FOR SELECT 'qwer' AS ""StringValue"" FROM Dual;
					OPEN ResultSet2 FOR SELECT 1 AS ""Counter"", ""@StringValue"" AS ""StringValue"" FROM Dual WHERE 1 <= ""@IntValue""
						UNION ALL SELECT 2 AS ""Counter"", ""@StringValue"" AS ""StringValue"" FROM Dual WHERE 2 <= ""@IntValue"";
				END;"
				: @"CREATE PROCEDURE ""Test Proc"" (
					@IntValue INT,
					@DoubleValue NUMERIC(10,4),
					@StringValue NVARCHAR(20),
					@StringValueOut NVARCHAR(20) OUTPUT,
					@DateValue DATE,
					@DateValueOut DATE OUTPUT,
					@BytesValue VARBINARY(50),
					@BytesValueOut VARBINARY(50) OUTPUT
				)
				AS BEGIN
					SET @StringValueOut = @StringValue;
					SET @DateValueOut = @DateValue;
					SET @BytesValueOut = @BytesValue;
					SELECT 'qwer' AS StringValue;
					SELECT 1 AS Counter, @StringValue AS StringValue WHERE 1 <= @IntValue
						UNION SELECT 2 AS Counter, @StringValue AS StringValue WHERE 2 <= @IntValue;
					RETURN 1;
				END;";
			return CreateTestProc(createProcedureStatement, connectionType);
		}

		public static DatabaseModel.ProcedureParameters CreateTestProc(string createProcedureStatement, ConnectionType connectionType)
		{
			try
			{
				RemoveTestProc(connectionType);
			}
			catch { }

			try
			{
				DatabaseHelpers.ExecuteSqlStatement(createProcedureStatement, connectionType);

				DatabaseAssistant databaseAssistant = DatabaseAssistant.GetDatabaseAssistant(connectionType);
				using (var connection = databaseAssistant.CreateConnection(DatabaseHelpers.GetDefaultConnectionString(connectionType)))
				{
					connection.Open();
					return new DatabaseModel.ProcedureParameters(databaseAssistant.GetStoredProcedureParameters(connection, "Test Proc"));
				}
			}
			catch
			{
				try
				{
					RemoveTestProc(connectionType);
				}
				catch { }
				throw;
			}
		}

		public static void RemoveTestProc(ConnectionType connectionType)
		{
			DatabaseHelpers.ExecuteSqlStatement("DROP PROCEDURE \"Test Proc\"", connectionType);
		}

		internal static void CreateTestFunction(string createFunctionStatement, ConnectionType connectionType)
		{
			try
			{
				RemoveTestFunction(connectionType);
			}
			catch { }

			try
			{
				DatabaseHelpers.ExecuteSqlStatement(createFunctionStatement, connectionType);
			}
			catch
			{
				try
				{
					RemoveTestFunction(connectionType);
				}
				catch { }
				throw;
			}
		}

		internal static void RemoveTestFunction(ConnectionType connectionType)
		{
			DatabaseHelpers.ExecuteSqlStatement("DROP Function TestFunction", connectionType);
		}
	}
}
