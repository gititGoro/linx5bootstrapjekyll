using NUnit.Framework;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using Twenty57.Linx.Components.Database.BeginTransaction;
using Twenty57.Linx.Components.Database.Common;
using Twenty57.Linx.Components.Database.Tests.Helpers;
using Twenty57.Linx.Plugin.Common.CodeGeneration;
using Twenty57.Linx.Plugin.TestKit;

namespace Twenty57.Linx.Components.Database.Tests
{
	[TestFixture]
	public class TestBeginTransaction
	{
		[OneTimeSetUp]
		public void SetUpFixture()
		{
			OdbcHelpers.CreateSqlServerDSN("LinxSqlServerTest", "Test Linx Database component through ODBC connection.", ConfigurationManager.AppSettings["SqlServerDriverPath"], ConfigurationManager.AppSettings["OdbcServer"], "TestLinx", "ved");
		}

		[OneTimeTearDown]
		public void TearDownFixture()
		{
			OdbcHelpers.RemoveSqlServerDSN("LinxSqlServerTest");
		}

		[Test]
		public void TestExecute(
			[Values(ConnectionType.Odbc, ConnectionType.OleDb, ConnectionType.Oracle, ConnectionType.SqlServer)] 
			ConnectionType connectionType)
		{
			foreach (object nextIsolationLevel in GetIsolationLevels(connectionType))
				TestExecute(connectionType, nextIsolationLevel);
		}

		public void TestExecute(ConnectionType connectionType, object isolationLevel)
		{
			DatabaseObjectCreation.CreateTestTable("CREATE TABLE TestTable ( IntValue INT )", connectionType);
			DatabaseObjectCreation.CreateTestProc(@"CREATE PROCEDURE ""Test Proc"" AS BEGIN INSERT INTO TestTable (IntValue) VALUES (0); END;", connectionType);
			try
			{
				var functionResult = Execute(connectionType, DatabaseHelpers.GetDefaultConnectionString(connectionType), isolationLevel);

				RunExecutionPath(functionResult.ExecutionPathResult,
					transaction =>
					{
						for (int i = 0; i < 2; i++)
						{
							var command = transaction.GetDbTransaction().Connection.CreateCommand();
							command.Transaction = transaction.GetDbTransaction();
							command.CommandText = "\"Test Proc\"";
							command.CommandType = CommandType.StoredProcedure;
							command.ExecuteNonQuery();
						}
					});
				
				DataTable results = DatabaseHelpers.GetDataTable("SELECT * FROM TestTable", connectionType);
				Assert.AreEqual(2, results.Rows.Count);
			}
			finally
			{
				DatabaseObjectCreation.RemoveTestProc(connectionType);
				DatabaseObjectCreation.RemoveTestTable(connectionType);
			}
		}

		[Test]
		public void TestExecuteRollback(
			[Values(ConnectionType.Odbc, ConnectionType.OleDb, ConnectionType.Oracle, ConnectionType.SqlServer)] 
			ConnectionType connectionType)
		{
			foreach (object nextIsolationLevel in GetIsolationLevels(connectionType))
				TestExecuteRollback(connectionType, nextIsolationLevel);
		}

		public void TestExecuteRollback(ConnectionType connectionType, object isolationLevel)
		{
			DatabaseObjectCreation.CreateTestTable("CREATE TABLE TestTable ( IntValue INT )", connectionType);
			DatabaseObjectCreation.CreateTestProc(@"CREATE PROCEDURE ""Test Proc"" AS BEGIN INSERT INTO TestTable (IntValue) VALUES (0); END;", connectionType);
			try
			{
				var functionResult = Execute(connectionType, DatabaseHelpers.GetDefaultConnectionString(connectionType), isolationLevel);

				Assert.Catch<Exception>(
					() => RunExecutionPath(functionResult.ExecutionPathResult,
						transaction =>
						{
							var command = transaction.GetDbTransaction().Connection.CreateCommand();
							command.Transaction = transaction.GetDbTransaction();
							command.CommandText = "\"Test Proc\"";
							command.CommandType = CommandType.StoredProcedure;
							command.ExecuteNonQuery();

							throw new Exception("Bad things are happening!!");
						}));

				DataTable results = DatabaseHelpers.GetDataTable("SELECT * FROM TestTable", connectionType);
				Assert.AreEqual(0, results.Rows.Count);
			}
			finally
			{
				DatabaseObjectCreation.RemoveTestProc(connectionType);
				DatabaseObjectCreation.RemoveTestTable(connectionType);
			}
		}


		private FunctionResult Execute(ConnectionType connectionType, string connectionString, object isolationLevel)
		{
			string isolationLevelPropertyName = null;
			switch (connectionType)
			{
				case ConnectionType.Odbc: isolationLevelPropertyName = BeginTransactionShared.OdbcIsolationLevelPropertyName; break;
				case ConnectionType.OleDb: isolationLevelPropertyName = BeginTransactionShared.OleDbIsolationLevelPropertyName; break;
				case ConnectionType.Oracle: isolationLevelPropertyName = BeginTransactionShared.OracleIsolationLevelPropertyName; break;
				case ConnectionType.SqlServer: isolationLevelPropertyName = BeginTransactionShared.SqlServerIsolationLevelPropertyName; break;
			}

			var tester = (new FunctionTester<BeginTransaction.BeginTransaction>()).Compile(
				new PropertyValue(BeginTransactionShared.ConnectionTypePropertyName, connectionType),
				new PropertyValue(isolationLevelPropertyName, isolationLevel));
			return tester.Execute(
				new ParameterValue(DbShared.ConnectionStringPropertyName, connectionString));
		}

		private void RunExecutionPath(IEnumerable<NextResult> executionPath, Action<Transaction> action)
		{
			IDbConnection connection = null;
			int resultCount = 0;
			foreach (Transaction transaction in executionPath.Select(r => r.Value))
			{
				action(transaction);
				connection = transaction.GetDbTransaction().Connection;
				Assert.AreEqual(ConnectionState.Open, connection.State);
				resultCount++;
			}

			Assert.AreEqual(1, resultCount, "Execution path must run only once.");
			Assert.AreEqual(ConnectionState.Closed, connection.State, "Connection must close immediately after execution path.");
		}

		private static IEnumerable GetIsolationLevels(ConnectionType connectionType)
		{
			switch (connectionType)
			{
				case ConnectionType.Odbc: return Enum.GetValues(typeof(OdbcIsolationLevel));
				case ConnectionType.OleDb: return Enum.GetValues(typeof(OleDbIsolationLevel));
				case ConnectionType.Oracle: return Enum.GetValues(typeof(OracleIsolationLevel));
				case ConnectionType.SqlServer: return Enum.GetValues(typeof(SqlServerIsolationLevel));
			}
			return null;
		}
	}
}
