using NUnit.Framework;
using System;
using System.Configuration;
using System.Data;
using System.Linq;
using Twenty57.Linx.Components.Database.Common;
using Twenty57.Linx.Components.Database.Tests.Helpers;

namespace Twenty57.Linx.Components.Database.Tests
{
	[TestFixture(ConnectionType.Odbc)]
	[TestFixture(ConnectionType.OleDb)]
	[TestFixture(ConnectionType.Oracle)]
	[TestFixture(ConnectionType.SqlServer)]
	public class TestDatabaseAssistant
	{
		private ConnectionType connectionType;

		public TestDatabaseAssistant(ConnectionType connectionType)
		{
			this.connectionType = connectionType;
		}

		[OneTimeSetUp]
		public void SetUpFixture()
		{
			if (this.connectionType == ConnectionType.Odbc)
				OdbcHelpers.CreateSqlServerDSN("LinxSqlServerTest", "Test Linx Database component through ODBC connection.", ConfigurationManager.AppSettings["SqlServerDriverPath"], ConfigurationManager.AppSettings["OdbcServer"], "TestLinx", "ved");
		}

		[OneTimeTearDown]
		public void TearDownFixture()
		{
			if (this.connectionType == ConnectionType.Odbc)
				OdbcHelpers.RemoveSqlServerDSN("LinxSqlServerTest");
		}

		[Test]
		public void TestTableData()
		{
			var columns = DatabaseObjectCreation.CreateTestTable(this.connectionType);

			DatabaseAssistant databaseAssistant = DatabaseAssistant.GetDatabaseAssistant(this.connectionType);
			try
			{
				using (var connection = databaseAssistant.CreateConnection(DatabaseHelpers.GetDefaultConnectionString(this.connectionType)))
				{
					connection.Open();
					Assert.IsTrue(databaseAssistant.GetTableNames(connection).Any(t => t.IndexOf("TestTable", StringComparison.InvariantCultureIgnoreCase) != -1));
				}
			}
			finally
			{
				DatabaseObjectCreation.RemoveTestTable(this.connectionType);
			}

			Assert.AreEqual(5, columns.Count);

			var column = columns[0];
			Assert.AreEqual("INTVALUE", column.Name.ToUpper());
			Assert.AreEqual(DatabaseModel.DataType.Int32, column.DataType);
			Assert.IsFalse(column.IsNullable);
			column = columns[1];
			Assert.AreEqual("DOUBLEVALUE", column.Name.ToUpper());
			Assert.AreEqual(DatabaseModel.DataType.Double, column.DataType);
			Assert.IsTrue(column.IsNullable);
			column = columns[2];
			Assert.AreEqual("STRINGVALUE", column.Name.ToUpper());
			Assert.AreEqual(DatabaseModel.DataType.String, column.DataType);
			Assert.IsTrue(column.IsNullable);
			column = columns[3];
			Assert.AreEqual("DATEVALUE", column.Name.ToUpper());
			Assert.AreEqual(this.connectionType == ConnectionType.OleDb ? DatabaseModel.DataType.String : DatabaseModel.DataType.Date,
				column.DataType);
			Assert.IsTrue(column.IsNullable);
			column = columns[4];
			Assert.AreEqual("BYTESVALUE", column.Name.ToUpper());
			Assert.AreEqual(DatabaseModel.DataType.Binary, column.DataType);
			Assert.IsTrue(column.IsNullable);
		}

		[Test]
		public void TestGetResultSetsForStoredProcedure()
		{
			var parameters = DatabaseObjectCreation.CreateParameterAndResultSetsTestProc(this.connectionType).Where(p => (p.Direction != DatabaseModel.ParameterDirection.ReturnValue) && (p.DataType != DatabaseModel.DataType.RefCursor)).ToList();

			DatabaseModel.ResultSets resultSets;
			DatabaseAssistant databaseAssistant = DatabaseAssistant.GetDatabaseAssistant(this.connectionType);
			try
			{
				using (var connection = databaseAssistant.CreateConnection(DatabaseHelpers.GetDefaultConnectionString(this.connectionType)))
				{
					connection.Open();
					Assert.IsTrue(databaseAssistant.GetStoredProcedureNames(connection).Any(p => p.Contains("Test Proc")));
					resultSets = databaseAssistant.GetResultSets(connection, "Test Proc");
				}
			}
			finally
			{
				DatabaseObjectCreation.RemoveTestProc(this.connectionType);
			}

			Assert.AreEqual(8, parameters.Count);

			var parameter = parameters[0];
			Assert.AreEqual("@IntValue", parameter.Name);
			Assert.AreEqual(DatabaseModel.ParameterDirection.In, parameter.Direction);
			Assert.AreEqual(DatabaseModel.DataType.Int32, parameter.DataType);
			parameter = parameters[1];
			Assert.AreEqual("@DoubleValue", parameter.Name);
			Assert.AreEqual(DatabaseModel.ParameterDirection.In, parameter.Direction);
			Assert.AreEqual(DatabaseModel.DataType.Double, parameter.DataType);
			parameter = parameters[2];
			Assert.AreEqual("@StringValue", parameter.Name);
			Assert.AreEqual(DatabaseModel.ParameterDirection.In, parameter.Direction);
			Assert.AreEqual(DatabaseModel.DataType.String, parameter.DataType);
			if (this.connectionType != ConnectionType.Oracle)
				Assert.AreEqual(20, parameter.Size);
			parameter = parameters[3];
			Assert.AreEqual("@StringValueOut", parameter.Name);
			Assert.AreEqual(DatabaseModel.ParameterDirection.InOut, parameter.Direction);
			Assert.AreEqual(DatabaseModel.DataType.String, parameter.DataType);
			if (this.connectionType != ConnectionType.Oracle)
				Assert.AreEqual(20, parameter.Size);
			parameter = parameters[4];
			Assert.AreEqual("@DateValue", parameter.Name);
			Assert.AreEqual(DatabaseModel.ParameterDirection.In, parameter.Direction);
			Assert.AreEqual(DatabaseModel.DataType.Date, parameter.DataType);
			parameter = parameters[5];
			Assert.AreEqual("@DateValueOut", parameter.Name);
			Assert.AreEqual(DatabaseModel.ParameterDirection.InOut, parameter.Direction);
			Assert.AreEqual(DatabaseModel.DataType.Date, parameter.DataType);
			parameter = parameters[6];
			Assert.AreEqual("@BytesValue", parameter.Name);
			Assert.AreEqual(DatabaseModel.ParameterDirection.In, parameter.Direction);
			Assert.AreEqual(DatabaseModel.DataType.Binary, parameter.DataType);
			if (this.connectionType != ConnectionType.Oracle)
				Assert.AreEqual(50, parameter.Size);
			parameter = parameters[7];
			Assert.AreEqual("@BytesValueOut", parameter.Name);
			Assert.AreEqual(DatabaseModel.ParameterDirection.InOut, parameter.Direction);
			Assert.AreEqual(DatabaseModel.DataType.Binary, parameter.DataType);
			if (this.connectionType != ConnectionType.Oracle)
				Assert.AreEqual(50, parameter.Size);

			Assert.AreEqual(2, resultSets.Count);

			var fields = resultSets[0].Fields;
			Assert.AreEqual(1, fields.Count);
			var field = fields[0];
			Assert.AreEqual("StringValue", field.ColumnName);
			Assert.AreEqual("StringValue", field.OutputName);

			fields = resultSets[1].Fields;
			Assert.AreEqual(2, fields.Count);
			field = fields[0];
			Assert.AreEqual("Counter", field.ColumnName);
			Assert.AreEqual("Counter", field.OutputName);
			field = fields[1];
			Assert.AreEqual("StringValue", field.ColumnName);
			Assert.AreEqual("StringValue", field.OutputName);
			Assert.AreEqual(DatabaseModel.DataType.String, field.DataType);
		}

		[Test]
		public void TestGetResultSetForStoredProcedureDataDoesNotRunProcedure()
		{
			string createProcedureStatement = GetProcedureStatement(this.connectionType);

			DatabaseObjectCreation.CreateTestTable("CREATE TABLE TestTable ( IntValue INT )", this.connectionType);
			DatabaseObjectCreation.CreateTestProc(createProcedureStatement, this.connectionType);

			DatabaseModel.ResultSets resultSets;
			try
			{
				DatabaseAssistant databaseAssistant = DatabaseAssistant.GetDatabaseAssistant(this.connectionType);
				using (var connection = databaseAssistant.CreateConnection(DatabaseHelpers.GetDefaultConnectionString(this.connectionType)))
				{
					connection.Open();
					Assert.IsTrue(databaseAssistant.GetStoredProcedureNames(connection).Any(p => p.Contains("Test Proc")));
					resultSets = databaseAssistant.GetResultSets(connection, "Test Proc");
				}

				DataTable results = DatabaseHelpers.GetDataTable("SELECT * FROM TestTable", this.connectionType);
				Assert.AreEqual(0, results.Rows.Count);
			}
			finally
			{
				DatabaseObjectCreation.RemoveTestProc(this.connectionType);
				DatabaseObjectCreation.RemoveTestTable(this.connectionType);
			}

			Assert.AreEqual(1, resultSets.Count);
			var fields = resultSets[0].Fields;
			Assert.AreEqual(1, fields.Count);
			var field = fields[0];
			Assert.AreEqual("StringValue", field.ColumnName);
			Assert.AreEqual("StringValue", field.OutputName);
		}

		[Test]
		public void TestGetResultSetsForStoredProcedureWithDynamicSQL()
		{
			string createProcedureStatement = GetProcedureWithDynamicSQL(this.connectionType);
			DatabaseObjectCreation.CreateTestProc(createProcedureStatement, this.connectionType);

			DatabaseModel.ResultSets resultSets = null;
			try
			{
				DatabaseAssistant databaseAssistant = DatabaseAssistant.GetDatabaseAssistant(this.connectionType);
				using (var connection = databaseAssistant.CreateConnection(DatabaseHelpers.GetDefaultConnectionString(this.connectionType)))
				{
					connection.Open();
					Assert.IsTrue(databaseAssistant.GetStoredProcedureNames(connection).Any(p => p.Contains("Test Proc")));
					resultSets = databaseAssistant.GetResultSets(connection, "Test Proc");
				}
			}
			finally
			{
				DatabaseObjectCreation.RemoveTestProc(this.connectionType);
			}

			Assert.AreEqual(1, resultSets.Count);
			var fields = resultSets[0].Fields;
			Assert.AreEqual(1, fields.Count);
			var field = fields[0];
			Assert.AreEqual("Rowcount_Returned", field.ColumnName);
			Assert.AreEqual("Rowcount_Returned", field.OutputName);
		}

		[Test]
		public void TestGetStoredProcedureDoesNotLoadFunction()
		{
			string createProcedureStatement = GetProcedureStatement(this.connectionType);
			string createFunctionStatement = GetFunctionStatement(this.connectionType);

			DatabaseObjectCreation.CreateTestTable("CREATE TABLE TestTable ( IntValue INT )", this.connectionType);
			DatabaseObjectCreation.CreateTestProc(createProcedureStatement, this.connectionType);
			DatabaseObjectCreation.CreateTestFunction(createFunctionStatement, this.connectionType);

			try
			{
				DatabaseAssistant databaseAssistant = DatabaseAssistant.GetDatabaseAssistant(this.connectionType);
				using (var connection = databaseAssistant.CreateConnection(DatabaseHelpers.GetDefaultConnectionString(this.connectionType)))
				{
					connection.Open();
					Assert.IsTrue(databaseAssistant.GetStoredProcedureNames(connection).Any(p => p.Contains("Test Proc")));
					Assert.IsFalse(databaseAssistant.GetStoredProcedureNames(connection).Any(p => p.Contains("TestFunction")));
				}
			}
			finally
			{
				DatabaseObjectCreation.RemoveTestProc(this.connectionType);
				DatabaseObjectCreation.RemoveTestFunction(this.connectionType);
				DatabaseObjectCreation.RemoveTestTable(this.connectionType);
			}
		}

		private static string GetProcedureStatement(ConnectionType connectionType)
		{
			return (connectionType == ConnectionType.Oracle) ?
				@"CREATE PROCEDURE ""Test Proc"" 
					(ResultSet OUT SYS_REFCURSOR)
					AS BEGIN 
						INSERT INTO TestTable (IntValue) VALUES (0); 
						OPEN ResultSet FOR SELECT 'qwer' AS ""StringValue"" FROM Dual;
					END;"
				 : @"CREATE PROCEDURE ""Test Proc"" 
					AS BEGIN 
						INSERT INTO TestTable (IntValue) VALUES (0); 
						SELECT 'qwer' AS StringValue;
					END;";
		}

		private static string GetFunctionStatement(ConnectionType connectionType)
		{
			return (connectionType == ConnectionType.Oracle) ?
				@"CREATE FUNCTION TESTFUNCTION RETURN NUMBER AS
					BEGIN
						RETURN 5;
					END TESTFUNCTION;"
				:
				@"CREATE FUNCTION TestFunction ()
					RETURNS int
					AS
					BEGIN
						RETURN 5;
					END";
		}

		private static string GetProcedureWithDynamicSQL(ConnectionType connectionType)
		{
			return (connectionType == ConnectionType.Oracle) ?
				@"CREATE PROCEDURE ""Test Proc""
						(SetTo5ToRun IN NUMBER, TableName IN VARCHAR2, ResultSet OUT SYS_REFCURSOR)
					AS
						SQLToExecute VARCHAR2(100);
						TYPE cur_typ IS REF CURSOR;
						c cur_typ;
					BEGIN
						SQLToExecute := '';
						
						IF SetTo5ToRun = 5 THEN
							SQLToExecute := 'SELECT FIRSTNAME FROM ' + TableName;
							
							OPEN c FOR SQLToExecute;
							CLOSE c;
							
						END IF;
						
						OPEN ResultSet FOR SELECT 1 AS ""Rowcount_Returned"" FROM Dual;
					END;"
				:
				@"CREATE PROCEDURE ""Test Proc""
						@SetTo5ToRun int,
						@TableName varchar(50)
					AS
					BEGIN
						DECLARE @SQLToExecute varchar(MAX)

						IF(@SetTo5ToRun = 5)
						BEGIN
							SET @SQLToExecute = 'SELECT * FROM ' + @TableName
							EXECUTE(@SQLToExecute)
						END
					
						SELECT @@ROWCOUNT AS Rowcount_Returned
					END";
		}
	}
}
