using NUnit.Framework;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using Twenty57.Linx.Components.Database.Common;
using Twenty57.Linx.Components.Database.ExecuteStoredProcedure;
using Twenty57.Linx.Components.Database.Tests.Helpers;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.Types;
using Twenty57.Linx.Plugin.TestKit;

namespace Twenty57.Linx.Components.Database.Tests
{
	[TestFixture(ConnectionType.Odbc)]
	[TestFixture(ConnectionType.OleDb)]
	[TestFixture(ConnectionType.Oracle)]
	[TestFixture(ConnectionType.SqlServer)]
	public class TestExecuteStoredProcedure
	{
		private ConnectionType connectionType;

		public TestExecuteStoredProcedure(ConnectionType connectionType)
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

		[SetUp]
		public void SetUp()
		{
			if (this.connectionType == ConnectionType.Oracle)
				OracleConnection.ClearAllPools();
		}

		[Test]
		public void TestExecuteWhereInputParametersMatchFunctionPropertyNames()
		{
			DatabaseObjectCreation.CreateTestTable("CREATE TABLE TESTTABLE ( INTVALUE1 INT, INTVALUE2 INT)", this.connectionType);

			string createProcedureStatement = (this.connectionType == ConnectionType.Oracle) ?
				@"CREATE PROCEDURE ""Test Proc"" (
					""@{0}"" INT,
					""@{1}"" INT
				)
				AS BEGIN
					INSERT INTO TESTTABLE (""INTVALUE1"",""INTVALUE2"") VALUES (""@{0}"",""@{1}"");
				END;"
			: @"CREATE PROCEDURE ""Test Proc"" (
					@{0} INT,
					@{1} INT
				)
				AS BEGIN
					INSERT INTO TestTable (IntValue1, IntValue2) VALUES(@{0}, @{1})
				END;";

			var parameters = new DatabaseModel.ProcedureParameters(
				DatabaseObjectCreation.CreateTestProc(string.Format(createProcedureStatement, ExecuteStoredProcedureShared.ParametersPropertyName, DbShared.TransactionPropertyName), connectionType)
				.Where(p => p.Direction != DatabaseModel.ParameterDirection.ReturnValue));
			try
			{
				Execute(this.connectionType, "Test Proc", parameters, 1, 2);

				DataTable results = DatabaseHelpers.GetDataTable("SELECT * FROM TESTTABLE", this.connectionType);
				Assert.AreEqual(1, results.Rows.Count);
				Assert.AreEqual(1, results.Rows[0][0]);
				Assert.AreEqual(2, results.Rows[0][1]);
			}
			finally
			{
				DatabaseObjectCreation.RemoveTestProc(this.connectionType);
				DatabaseObjectCreation.RemoveTestTable(this.connectionType);
			}
		}

		[Test]
		public void TestExecuteNoParametersAndNoReturnValue()
		{
			DatabaseObjectCreation.CreateTestTable("CREATE TABLE TestTable ( IntValue INT )", this.connectionType);
			DatabaseObjectCreation.CreateTestProc(@"CREATE PROCEDURE ""Test Proc"" AS BEGIN INSERT INTO TestTable (IntValue) VALUES (0); END;", this.connectionType);
			try
			{
				Execute(this.connectionType, "Test Proc");

				DataTable results = DatabaseHelpers.GetDataTable("SELECT * FROM TestTable", this.connectionType);
				Assert.AreEqual(1, results.Rows.Count);
			}
			finally
			{
				DatabaseObjectCreation.RemoveTestProc(this.connectionType);
				DatabaseObjectCreation.RemoveTestTable(this.connectionType);
			}
		}

		[Test]
		public void TestExecuteParametersAndNoReturnValue()
		{
			var parameters = new DatabaseModel.ProcedureParameters(DatabaseObjectCreation.CreateParameterTestProc(this.connectionType).Where(p => p.Direction != DatabaseModel.ParameterDirection.ReturnValue));
			try
			{
				List<byte> bytes = new List<byte>(new byte[] { 1, 2, 3, 4, 5 });
				dynamic result = Execute(this.connectionType, "Test Proc", parameters, 1, 1.0, "Qwer", string.Empty, new DateTime(1987, 1, 24), DatabaseModel.DefaultDateTime, bytes, new List<byte>()).Value;

				Assert.AreEqual("Qwer", result.ResultParameters.StringValueOut);
				Assert.AreEqual(new DateTime(1987, 1, 24), result.ResultParameters.DateValueOut);
				Assert.AreEqual(bytes, result.ResultParameters.BytesValueOut);
			}
			finally
			{
				DatabaseObjectCreation.RemoveTestProc(this.connectionType);
			}
		}

		[Test]
		public void TestExecuteNoParametersAndReturnValue()
		{
			if (this.connectionType == ConnectionType.Oracle)
			{
				// Cannot return a value in an Oracle stored procedure - have to use OUT parameters
				return;
			}

			var parameters = DatabaseObjectCreation.CreateTestProc(@"CREATE PROCEDURE ""Test Proc"" AS BEGIN RETURN 1 END;", this.connectionType);
			try
			{
				dynamic result = Execute(this.connectionType, "Test Proc", parameters).Value;

				Assert.AreEqual(1, result.ResultParameters.RETURN_VALUE);
			}
			finally
			{
				DatabaseObjectCreation.RemoveTestProc(this.connectionType);
			}
		}

		[Test]
		public void TestExecuteParametersAndReturnValue()
		{
			if (this.connectionType == ConnectionType.Oracle)
			{
				// Cannot return a value in an Oracle stored procedure - have to use OUT parameters
				return;
			}

			var parameters = new DatabaseModel.ProcedureParameters(DatabaseObjectCreation.CreateParameterTestProc(this.connectionType));
			try
			{
				List<byte> bytes = new List<byte>(new byte[] { 1, 2, 3, 4, 5 });
				dynamic result = Execute(this.connectionType, "Test Proc", parameters, 1, 1.0, "Qwer", string.Empty, new DateTime(1987, 1, 24), DatabaseModel.DefaultDateTime, bytes, new List<byte>()).Value;

				Assert.AreEqual(1, result.ResultParameters.RETURN_VALUE);
				Assert.AreEqual("Qwer", result.ResultParameters.StringValueOut);
				Assert.AreEqual(new DateTime(1987, 1, 24), result.ResultParameters.DateValueOut);
				Assert.AreEqual(bytes, result.ResultParameters.BytesValueOut);
			}
			finally
			{
				DatabaseObjectCreation.RemoveTestProc(this.connectionType);
			}
		}

		[Test]
		public void TestExecuteResultSetRowByRow()
		{
			var parameters = DatabaseObjectCreation.CreateResultSetTestProc(this.connectionType);
			try
			{
				FunctionResult result = Execute(this.connectionType, DatabaseHelpers.GetDefaultConnectionString(this.connectionType), "Test Proc", parameters, new object[] { 2 }, true, OutputOption.RowByRow);

				var executionPathResults = result.ExecutionPathResult.ToList();
				Assert.AreEqual(2, executionPathResults.Count());
				Assert.AreEqual("Result", executionPathResults[0].Name);
				var row = executionPathResults[0].Value;
				Assert.AreEqual(1, row.Counter);
				Assert.AreEqual("one", row.StringValue);

				row = executionPathResults[1].Value;
				Assert.AreEqual(2, row.Counter);
				Assert.AreEqual("two", row.StringValue);
			}
			finally
			{
				DatabaseObjectCreation.RemoveTestProc(this.connectionType);
			}
		}

		[Test]
		public void TestExecuteResultSetRowByRowWithInvalidNumberOfResultSets()
		{
			var parameters = DatabaseObjectCreation.CreateResultSetTestProc(this.connectionType);
			try
			{
				FunctionResult result = Execute(this.connectionType.ToConnectionTypeSelection(), null, DatabaseHelpers.GetDefaultConnectionString(this.connectionType), "Test Proc",
					parameters, new object[] { 2 }, FetchResultSets(this.connectionType), 1000, OutputOption.RowByRow);

				var executionPathResults = result.ExecutionPathResult.ToList();
				Assert.AreEqual(2, executionPathResults.Count());
				Assert.AreEqual("Result1", executionPathResults[0].Name);
				var row = executionPathResults[0].Value;
				Assert.AreEqual(1, row.Counter);
				Assert.AreEqual("one", row.StringValue);

				row = executionPathResults[1].Value;
				Assert.AreEqual(2, row.Counter);
				Assert.AreEqual("two", row.StringValue);
			}
			finally
			{
				DatabaseObjectCreation.RemoveTestProc(this.connectionType);
			}
		}

		[Test]
		public void TestExecuteResultSetListOfRows()
		{
			var parameters = DatabaseObjectCreation.CreateResultSetTestProc(this.connectionType);
			try
			{
				var result = Execute(this.connectionType, DatabaseHelpers.GetDefaultConnectionString(this.connectionType), "Test Proc", parameters, new object[] { 2 }, true, OutputOption.ListOfRows).Value;

				Assert.AreEqual(2, result.ResultRows.Count);
				var row = result.ResultRows[0];
				Assert.AreEqual(1, row.Counter);
				Assert.AreEqual("one", row.StringValue);

				row = result.ResultRows[1];
				Assert.AreEqual(2, row.Counter);
				Assert.AreEqual("two", row.StringValue);
			}
			finally
			{
				DatabaseObjectCreation.RemoveTestProc(this.connectionType);
			}
		}

		[Test]
		public void TestExecuteResultSetFirstRow(
			[Values(OutputOption.FirstRow, OutputOption.FirstRowElseEmptyRow)] OutputOption outputOption)
		{
			var parameters = DatabaseObjectCreation.CreateResultSetTestProc(this.connectionType);
			try
			{
				var result = Execute(this.connectionType, DatabaseHelpers.GetDefaultConnectionString(this.connectionType), "Test Proc", parameters, new object[] { 2 }, true, outputOption).Value;

				Assert.AreEqual(1, result.Result.Counter);
				Assert.AreEqual("one", result.Result.StringValue);
			}
			finally
			{
				DatabaseObjectCreation.RemoveTestProc(this.connectionType);
			}
		}

		[Test]
		public void TestExecuteResultSetFirstRowNoRows()
		{
			var parameters = DatabaseObjectCreation.CreateResultSetTestProc(this.connectionType);

			Assert.That(() => Execute(this.connectionType, DatabaseHelpers.GetDefaultConnectionString(this.connectionType), "Test Proc", parameters, new object[] { 0 }, true, OutputOption.FirstRow),
				Throws.Exception.TypeOf<ExecuteException>()
				.With.Property("Message").EqualTo("No rows returned for Result.\r\nSee Code and Parameter properties for more information."));

			DatabaseObjectCreation.RemoveTestProc(this.connectionType);
		}

		[Test]
		public void TestExecuteResultSetFirstRowElseEmptyRowNoRows()
		{
			var parameters = DatabaseObjectCreation.CreateResultSetTestProc(this.connectionType);
			try
			{
				var result = Execute(this.connectionType, DatabaseHelpers.GetDefaultConnectionString(this.connectionType), "Test Proc", parameters, new object[] { 0 }, true, OutputOption.FirstRowElseEmptyRow).Value;

				Assert.AreEqual(0, result.Result.Counter);
				Assert.AreEqual(string.Empty, result.Result.StringValue);
			}
			finally
			{
				DatabaseObjectCreation.RemoveTestProc(this.connectionType);
			}
		}

		[Test]
		public void TestExecuteResultSetRowByRowWithCustomType()
		{
			var parameters = DatabaseObjectCreation.CreateResultSetTestProc(this.connectionType);
			try
			{
				var customType = TypeReference.CreateGeneratedType(new TypeProperty("Name", typeof(string)));
				var resultSet = new DatabaseModel.ResultSet { CustomType = customType };
				resultSet.Fields.Add(new DatabaseModel.ResultSetField("Counter", DatabaseModel.DataType.Int32, string.Empty));
				resultSet.Fields.Add(new DatabaseModel.ResultSetField("StringValue", DatabaseModel.DataType.String, "Name"));
				FunctionResult result = Execute(this.connectionType, DatabaseHelpers.GetDefaultConnectionString(this.connectionType), "Test Proc", parameters, new object[] { 2 }, new DatabaseModel.ResultSets { resultSet }, OutputOption.RowByRow);

				var executionPathResults = result.ExecutionPathResult.ToList();
				Assert.AreEqual(2, executionPathResults.Count());
				Assert.AreEqual("Result", executionPathResults[0].Name);
				Assert.AreEqual(Names.GetValidName(customType.Name), executionPathResults[0].Value.GetType().Name);
				Assert.AreEqual("one", executionPathResults[0].Value.Name);
				Assert.AreEqual("two", executionPathResults[1].Value.Name);
			}
			finally
			{
				DatabaseObjectCreation.RemoveTestProc(this.connectionType);
			}
		}

		[Test]
		public void TestExecuteResultSetsRowByRow()
		{
			var parameters = DatabaseObjectCreation.CreateResultSetsTestProc(this.connectionType);
			try
			{
				FunctionResult result = Execute(this.connectionType, DatabaseHelpers.GetDefaultConnectionString(this.connectionType), "Test Proc", parameters, null, true, OutputOption.RowByRow);

				var executionPathResults = result.ExecutionPathResult.ToList();
				Assert.AreEqual(3, executionPathResults.Count());
				Assert.AreEqual("Result1", executionPathResults[0].Name);
				var row = executionPathResults[0].Value;
				Assert.AreEqual(1, row.Counter);
				Assert.AreEqual("one", row.StringValue);

				Assert.AreEqual("Result1", executionPathResults[1].Name);
				row = executionPathResults[1].Value;
				Assert.AreEqual(2, row.Counter);
				Assert.AreEqual("two", row.StringValue);

				Assert.AreEqual("Result2", executionPathResults[2].Name);
				row = executionPathResults[2].Value;
				Assert.AreEqual("qwer", row.StringValue);
			}
			finally
			{
				DatabaseObjectCreation.RemoveTestProc(this.connectionType);
			}
		}

		[Test]
		public void TestExecuteResultSetsListOfRows()
		{
			var parameters = DatabaseObjectCreation.CreateResultSetsTestProc(this.connectionType);
			try
			{
				var result = Execute(this.connectionType, DatabaseHelpers.GetDefaultConnectionString(this.connectionType), "Test Proc", parameters, null, true, OutputOption.ListOfRows).Value;

				Assert.AreEqual(2, result.Result1Rows.Count);
				var row = result.Result1Rows[0];
				Assert.AreEqual(1, row.Counter);
				Assert.AreEqual("one", row.StringValue);

				row = result.Result1Rows[1];
				Assert.AreEqual(2, row.Counter);
				Assert.AreEqual("two", row.StringValue);

				Assert.AreEqual(1, result.Result2Rows.Count);
				row = result.Result2Rows[0];
				Assert.AreEqual("qwer", row.StringValue);
			}
			finally
			{
				DatabaseObjectCreation.RemoveTestProc(this.connectionType);
			}
		}

		[Test]
		public void TestExecuteResultSetsFirstRow(
			[Values(OutputOption.FirstRow, OutputOption.FirstRowElseEmptyRow)]
			OutputOption outputOption)
		{
			var parameters = DatabaseObjectCreation.CreateResultSetsTestProc(this.connectionType);
			try
			{
				var result = Execute(this.connectionType, DatabaseHelpers.GetDefaultConnectionString(this.connectionType), "Test Proc", parameters, null, true, outputOption).Value;

				Assert.AreEqual(1, result.Result1.Counter);
				Assert.AreEqual("one", result.Result1.StringValue);

				Assert.AreEqual("qwer", result.Result2.StringValue);
			}
			finally
			{
				DatabaseObjectCreation.RemoveTestProc(this.connectionType);
			}
		}

		[Test]
		public void TestExecuteParameterAndResultSetsRowByRow()
		{
			var parameters = DatabaseObjectCreation.CreateParameterAndResultSetsTestProc(this.connectionType);
			try
			{
				List<byte> bytes = new List<byte>(new byte[] { 1, 2, 3, 4, 5 });
				FunctionResult result = Execute(this.connectionType, DatabaseHelpers.GetDefaultConnectionString(this.connectionType), "Test Proc", parameters, new object[] { 2, 1.0, "Qwer", string.Empty, new DateTime(1987, 1, 24), DatabaseModel.DefaultDateTime, bytes, new List<byte>() }, true, OutputOption.RowByRow);

				var executionPathResults = result.ExecutionPathResult.ToList();
				Assert.AreEqual(3, executionPathResults.Count());

				Assert.AreEqual("Result1", executionPathResults[0].Name);
				var row = executionPathResults[0].Value;
				Assert.AreEqual("qwer", row.StringValue);

				Assert.AreEqual("Result2", executionPathResults[1].Name);
				row = executionPathResults[1].Value;
				Assert.AreEqual(1, row.Counter);
				Assert.AreEqual("Qwer", row.StringValue);

				row = executionPathResults[2].Value;
				Assert.AreEqual(2, row.Counter);
				Assert.AreEqual("Qwer", row.StringValue);

				Assert.AreEqual("Qwer", result.Value.ResultParameters.StringValueOut);
				Assert.AreEqual(new DateTime(1987, 1, 24), result.Value.ResultParameters.DateValueOut);
				Assert.AreEqual(bytes, result.Value.ResultParameters.BytesValueOut);
			}
			finally
			{
				DatabaseObjectCreation.RemoveTestProc(this.connectionType);
			}
		}

		[Test]
		public void TestExecuteParameterAndResultSetsListOfRows()
		{
			var parameters = DatabaseObjectCreation.CreateParameterAndResultSetsTestProc(this.connectionType);
			try
			{
				List<byte> bytes = new List<byte>(new byte[] { 1, 2, 3, 4, 5 });
				var result = Execute(this.connectionType, DatabaseHelpers.GetDefaultConnectionString(this.connectionType), "Test Proc", parameters, new object[] { 2, 1.0, "Qwer", string.Empty, new DateTime(1987, 1, 24), DatabaseModel.DefaultDateTime, bytes, new List<byte>() }, true, OutputOption.ListOfRows).Value;

				Assert.AreEqual("Qwer", result.ResultParameters.StringValueOut);
				Assert.AreEqual(new DateTime(1987, 1, 24), result.ResultParameters.DateValueOut);
				Assert.AreEqual(bytes, result.ResultParameters.BytesValueOut);

				Assert.AreEqual(1, result.Result1Rows.Count);
				var row = result.Result1Rows[0];
				Assert.AreEqual("qwer", row.StringValue);

				Assert.AreEqual(2, result.Result2Rows.Count);
				row = result.Result2Rows[0];
				Assert.AreEqual(1, row.Counter);
				Assert.AreEqual("Qwer", row.StringValue);

				row = result.Result2Rows[1];
				Assert.AreEqual(2, row.Counter);
				Assert.AreEqual("Qwer", row.StringValue);
			}
			finally
			{
				DatabaseObjectCreation.RemoveTestProc(this.connectionType);
			}
		}

		[Test]
		public void TestExecuteParameterAndResultSetsFirstRow(
			[Values(OutputOption.FirstRow, OutputOption.FirstRowElseEmptyRow)]
			OutputOption outputOption)
		{
			var parameters = DatabaseObjectCreation.CreateParameterAndResultSetsTestProc(this.connectionType);
			try
			{
				List<byte> bytes = new List<byte>(new byte[] { 1, 2, 3, 4, 5 });
				var result = Execute(this.connectionType, DatabaseHelpers.GetDefaultConnectionString(this.connectionType), "Test Proc", parameters, new object[] { 2, 1.0, "Qwer", string.Empty, new DateTime(1987, 1, 24), DatabaseModel.DefaultDateTime, bytes, new List<byte>() }, true, outputOption).Value;

				Assert.AreEqual("Qwer", result.ResultParameters.StringValueOut);
				Assert.AreEqual(new DateTime(1987, 1, 24), result.ResultParameters.DateValueOut);
				Assert.AreEqual(bytes, result.ResultParameters.BytesValueOut);

				Assert.AreEqual("qwer", result.Result1.StringValue);

				Assert.AreEqual(1, result.Result2.Counter);
				Assert.AreEqual("Qwer", result.Result2.StringValue);
			}
			finally
			{
				DatabaseObjectCreation.RemoveTestProc(this.connectionType);
			}
		}

		[Test]
		public void TestExecuteWithTransaction()
		{
			DatabaseObjectCreation.CreateTestTable("CREATE TABLE TestTable ( IntValue INT )", this.connectionType);
			DatabaseObjectCreation.CreateTestProc(@"CREATE PROCEDURE ""Test Proc"" AS BEGIN INSERT INTO TestTable (IntValue) VALUES (0); END;", this.connectionType);
			try
			{
				foreach (var transaction in new BeginTransaction.BeginTransactionX(this.connectionType, Helpers.DatabaseHelpers.GetDefaultConnectionString(this.connectionType), IsolationLevel.ReadCommitted))
				{
					Execute(transaction, "Test Proc");
					Execute(transaction, "Test Proc");
				}

				DataTable results = DatabaseHelpers.GetDataTable("SELECT * FROM TestTable", this.connectionType);
				Assert.AreEqual(2, results.Rows.Count);
			}
			finally
			{
				DatabaseObjectCreation.RemoveTestProc(this.connectionType);
				DatabaseObjectCreation.RemoveTestTable(this.connectionType);
			}
		}

		[Test]
		public void TestExecuteWithTransactionAndResultSet()
		{
			string createProcedureStatement = this.connectionType == ConnectionType.Oracle
				? @"CREATE PROCEDURE ""Test Proc"" (
					ResultSet OUT SYS_REFCURSOR
				)
				AS BEGIN 
					INSERT INTO TestTable (IntValue) VALUES (0); 
					OPEN ResultSet FOR SELECT 'qwer' AS ""StringValue"" FROM Dual;
				END;"
				: @"CREATE PROCEDURE ""Test Proc"" 
				AS BEGIN 
					INSERT INTO TestTable (IntValue) VALUES (0); 
					SELECT 'qwer' AS StringValue;
				END;";

			DatabaseObjectCreation.CreateTestTable("CREATE TABLE TestTable ( IntValue INT )", this.connectionType);
			var parameters = DatabaseObjectCreation.CreateTestProc(createProcedureStatement, this.connectionType);
			try
			{
				foreach (var transaction in new BeginTransaction.BeginTransactionX(this.connectionType, Helpers.DatabaseHelpers.GetDefaultConnectionString(this.connectionType), IsolationLevel.ReadCommitted))
				{
					for (int i = 0; i < 2; i++)
					{
						var result = Execute(transaction, "Test Proc", parameters, new object[0], true, OutputOption.FirstRow).Value;
						Assert.AreEqual("qwer", result.Result.StringValue);
					}
				}

				DataTable results = DatabaseHelpers.GetDataTable("SELECT * FROM TestTable", this.connectionType);
				Assert.AreEqual(2, results.Rows.Count);
			}
			finally
			{
				DatabaseObjectCreation.RemoveTestProc(this.connectionType);
				DatabaseObjectCreation.RemoveTestTable(this.connectionType);
			}
		}

		[Test]
		public void TestExecuteWithTransactionOnException()
		{
			DatabaseObjectCreation.CreateTestTable("CREATE TABLE TestTable ( IntValue INT )", this.connectionType);
			DatabaseObjectCreation.CreateTestProc(@"CREATE PROCEDURE ""Test Proc"" AS BEGIN INSERT INTO TestTable (IntValue) VALUES (0); END;", this.connectionType);
			try
			{
				Assert.Catch<Exception>(() =>
				{
					foreach (var transaction in new BeginTransaction.BeginTransactionX(this.connectionType, Helpers.DatabaseHelpers.GetDefaultConnectionString(this.connectionType), IsolationLevel.ReadCommitted))
					{
						Execute(transaction, "Test Proc");
						throw new Exception("Bad things are happening!!");
					}
				});

				DataTable results = DatabaseHelpers.GetDataTable("SELECT * FROM TestTable", this.connectionType);
				Assert.AreEqual(0, results.Rows.Count);
			}
			finally
			{
				DatabaseObjectCreation.RemoveTestProc(this.connectionType);
				DatabaseObjectCreation.RemoveTestTable(this.connectionType);
			}
		}

		[Test]
		public void TestExecuteWithTransactionAndResultSetOnException()
		{
			string createProcedureStatement = this.connectionType == ConnectionType.Oracle
				? @"CREATE PROCEDURE ""Test Proc"" (
					ResultSet OUT SYS_REFCURSOR
				)
				AS BEGIN 
					INSERT INTO TestTable (IntValue) VALUES (0); 
					OPEN ResultSet FOR SELECT 'qwer' AS ""StringValue"" FROM Dual;
				END;"
				: @"CREATE PROCEDURE ""Test Proc"" 
				AS BEGIN 
					INSERT INTO TestTable (IntValue) VALUES (0); 
					SELECT 'qwer' AS StringValue;
				END;";

			DatabaseObjectCreation.CreateTestTable("CREATE TABLE TestTable ( IntValue INT )", this.connectionType);
			var parameters = DatabaseObjectCreation.CreateTestProc(createProcedureStatement, this.connectionType);
			try
			{
				Assert.Throws<Exception>(() =>
				{
					foreach (var transaction in new BeginTransaction.BeginTransactionX(this.connectionType, Helpers.DatabaseHelpers.GetDefaultConnectionString(this.connectionType), IsolationLevel.ReadCommitted))
					{
						var result = Execute(transaction, "Test Proc", parameters, new object[0], true, OutputOption.FirstRow).Value;
						Assert.AreEqual("qwer", result.Result.StringValue);
						throw new Exception("Bad things are happening!!");
					}
				});

				DataTable results = DatabaseHelpers.GetDataTable("SELECT * FROM TestTable", this.connectionType);
				Assert.AreEqual(0, results.Rows.Count);
			}
			finally
			{
				DatabaseObjectCreation.RemoveTestProc(this.connectionType);
				DatabaseObjectCreation.RemoveTestTable(this.connectionType);
			}
		}

		[Test]
		public void TestExecuteWithTransactionOnSqlError()
		{
			string createProcedureStatement = this.connectionType == ConnectionType.Oracle
				? @"CREATE PROCEDURE ""Test Proc"" (
					""@IntValue"" INT
				)
				AS BEGIN
					INSERT INTO TestTable (IntValue) VALUES (0 / ""@IntValue"");
				END;"
				: @"CREATE PROCEDURE ""Test Proc"" (
					@IntValue INT
				)
				AS BEGIN
					INSERT INTO TestTable (IntValue) VALUES (0 / @IntValue);
				END;";

			DatabaseObjectCreation.CreateTestTable("CREATE TABLE TestTable ( IntValue INT )", this.connectionType);
			var parameters = DatabaseObjectCreation.CreateTestProc(createProcedureStatement, this.connectionType);
			try
			{
				Assert.Catch<ExecuteException>(() =>
				{
					foreach (var transaction in new BeginTransaction.BeginTransactionX(this.connectionType, Helpers.DatabaseHelpers.GetDefaultConnectionString(this.connectionType), IsolationLevel.ReadCommitted))
					{
						Execute(transaction, "Test Proc", parameters, new object[] { 1 });
						Execute(transaction, "Test Proc", parameters, new object[] { 0 });
					}
				});

				DataTable results = DatabaseHelpers.GetDataTable("SELECT * FROM TestTable", this.connectionType);
				Assert.AreEqual(0, results.Rows.Count);
			}
			finally
			{
				DatabaseObjectCreation.RemoveTestProc(this.connectionType);
				DatabaseObjectCreation.RemoveTestTable(this.connectionType);
			}
		}

		private static FunctionResult Execute(ConnectionType connectionType, string storedProcedureName, DatabaseModel.ProcedureParameters parameters = null, params object[] parameterValues)
		{
			return Execute(connectionType, DatabaseHelpers.GetDefaultConnectionString(connectionType), storedProcedureName, parameters, parameterValues, false);
		}

		private static FunctionResult Execute(ConnectionType connectionType, string connectionString, string storedProcedureName, DatabaseModel.ProcedureParameters parameters = null, object[] parameterValues = null, bool fetchResultSets = false, OutputOption outputOption = OutputOption.RowByRow)
		{
			return Execute(connectionType, connectionString, storedProcedureName, parameters, parameterValues, fetchResultSets ? FetchResultSets(connectionType) : null, outputOption);
		}

		private static FunctionResult Execute(ConnectionType connectionType, string connectionString, string storedProcedureName, DatabaseModel.ProcedureParameters parameters = null, object[] parameterValues = null, DatabaseModel.ResultSets resultSets = null, OutputOption outputOption = OutputOption.RowByRow)
		{
			return Execute(connectionType.ToConnectionTypeSelection(), null, connectionString, storedProcedureName, parameters, parameterValues, resultSets, null, outputOption);
		}

		private static FunctionResult Execute(Transaction transaction, string storedProcedureName, DatabaseModel.ProcedureParameters parameters = null, object[] parameterValues = null, bool fetchResultSets = false, OutputOption outputOption = OutputOption.RowByRow)
		{
			return Execute(ConnectionTypeSelection.UseTransaction, transaction, null, storedProcedureName, parameters, parameterValues, fetchResultSets ? FetchResultSets(transaction.GetConnectionType()) : null, null, outputOption);
		}

		private static FunctionResult Execute(ConnectionTypeSelection connectionType, Transaction transaction, string connectionString, string storedProcedureName,
			DatabaseModel.ProcedureParameters parameters = null, object[] parameterValues = null, DatabaseModel.ResultSets resultSets = null, int? numberOfResultSets = null,
			OutputOption outputOption = OutputOption.RowByRow)
		{
			if (parameters == null)
				parameters = new DatabaseModel.ProcedureParameters();
			if (resultSets == null)
				resultSets = new DatabaseModel.ResultSets();

			int i = 0, j = 0;
			var tester = (new FunctionTester<ExecuteStoredProcedure.ExecuteStoredProcedure>()).Compile(new PropertyValue[] {
				new PropertyValue(DbShared.ConnectionTypePropertyName, connectionType),
				new PropertyValue(ExecuteStoredProcedureShared.ParametersPropertyName, parameters),
				new PropertyValue(ExecuteStoredProcedureShared.OutputOptionPropertyName, outputOption),
				new PropertyValue(ExecuteStoredProcedureShared.ResultSetCountPropertyName, (numberOfResultSets.HasValue)? numberOfResultSets.Value : resultSets.Count)
			}.Concat(
				resultSets.Select(r => new PropertyValue(string.Format(ExecuteStoredProcedureShared.ResultSetPropertyNameFormat, ++i), resultSets[j++]))
				).ToArray());
			i = 0;

			var executeParameters = new ParameterValue[]
				{
					new ParameterValue(ExecuteStoredProcedureShared.StoredProcedurePropertyName, storedProcedureName)
				};

			if (connectionType == ConnectionTypeSelection.UseTransaction)
				executeParameters = executeParameters.Concat(new ParameterValue[]{ new ParameterValue(DbShared.TransactionPropertyName, transaction) }).ToArray();
			else
				executeParameters = executeParameters.Concat(new ParameterValue[] { new ParameterValue(DbShared.ConnectionStringPropertyName, connectionString) }).ToArray();

			return tester.Execute(executeParameters
				.Concat(
				parameters.Where(p => (p.Direction == DatabaseModel.ParameterDirection.In) || (p.Direction == DatabaseModel.ParameterDirection.InOut))
				.Select(p => new ParameterValue(p.DisplayPropertyName, parameterValues[i++]))).ToArray());
		}

		private static DatabaseModel.ResultSets FetchResultSets(ConnectionType connectionType)
		{
			var databaseAssistant = DatabaseAssistant.GetDatabaseAssistant(connectionType);
			using (var connection = databaseAssistant.CreateConnection(DatabaseHelpers.GetDefaultConnectionString(connectionType)))
			{
				connection.Open();
				return databaseAssistant.GetResultSets(connection, "Test Proc");
			}
		}
	}
}
