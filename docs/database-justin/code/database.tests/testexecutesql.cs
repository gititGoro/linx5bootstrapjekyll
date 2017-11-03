using Digiata.Resources;
using NUnit.Framework;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using Twenty57.Linx.Components.Database.Common;
using Twenty57.Linx.Components.Database.ExecuteSQL;
using Twenty57.Linx.Components.Database.Tests.Helpers;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.CodeGeneration;
using Twenty57.Linx.Plugin.Common.Types;
using Twenty57.Linx.Plugin.TestKit;

namespace Twenty57.Linx.Components.Database.Tests
{
	[TestFixture]
	public class TestExecuteSQL
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

		[SetUp]
		public void SetUp()
		{
			OracleConnection.ClearAllPools();
		}

		[Test]
		public void TestNoExceptionWithBadConnection()
		{
			var designer = (new FunctionTester<ExecuteSQL.ExecuteSQL>()).CreateDesigner();
			designer.Properties[DbShared.ConnectionTypePropertyName].Value = ConnectionType.OleDb;
			designer.Properties[DbShared.ConnectionStringPropertyName].Value = "Bad connection string";
			designer.Properties[ExecuteSQLShared.SqlStatementPropertyName].Value = "Trigger build-result-type";
		}

		[Test]
		public void TestExecuteWithRowByRowReturnMode()
		{
			string sql = @"DECLARE @t TABLE
				(
					IntValue INT,
					DecimalValue NUMERIC(10,2),
					DateTimeValue DATETIME,
					BitValue BIT,
					StringValue VARCHAR(50),
					GuidValue UNIQUEIDENTIFIER
				)
				INSERT INTO @t VALUES (1, 0.1, GETDATE(), 1, 'Some value', NEWID())
				INSERT INTO @t VALUES (NULL, NULL, NULL, NULL, NULL, NULL)
				SELECT * FROM @t";
			DataTable outputTable = Helpers.DatabaseHelpers.GetDataTable(sql, Common.ConnectionType.SqlServer, Utilities.SqlConnectionString);
			FunctionResult functionResult = Execute(sql, ExecuteSQLShared.ReturnModeType.RowByRow);

			List<NextResult> resultList = ((IEnumerable<NextResult>)functionResult.ExecutionPathResult).ToList();
			Assert.AreEqual(outputTable.Rows.Count, resultList.Count);

			dynamic row = resultList[0].Value;
			Assert.AreEqual(1, row.IntValue);
			Assert.AreEqual(0.1, row.DecimalValue);
			Assert.AreEqual(DateTime.Now.Date, row.DateTimeValue.Date);
			Assert.IsTrue(row.BitValue);
			Assert.AreEqual("Some value", row.StringValue);
			Assert.IsNotNull(Guid.Parse(row.GuidValue));

			row = resultList[1].Value;
			Assert.AreEqual(0, row.IntValue);
			Assert.AreEqual(0, row.DecimalValue);
			Assert.AreEqual(DateTime.MinValue, row.DateTimeValue);
			Assert.IsFalse(row.BitValue);
			Assert.AreEqual(String.Empty, row.StringValue);
			Assert.That(row.GuidValue, Is.Null.Or.Empty);
		}

		[Test]
		public void TestExecuteWithListOfRowsReturnMode()
		{
			string sql = @"DECLARE @t TABLE
				(
					IntValue INT,
					DecimalValue NUMERIC(10,2),
					DateTimeValue DATETIME,
					BitValue BIT,
					StringValue VARCHAR(50),
					GuidValue UNIQUEIDENTIFIER
				)
				INSERT INTO @t VALUES (1, 0.1, GETDATE(), 1, 'Some value', NEWID())
				INSERT INTO @t VALUES (NULL, NULL, NULL, NULL, NULL, NULL)
				SELECT * FROM @t";
			DataTable outputTable = Helpers.DatabaseHelpers.GetDataTable(sql, Common.ConnectionType.SqlServer, Utilities.SqlConnectionString);
			dynamic dataOut = Execute(sql, ExecuteSQLShared.ReturnModeType.ListOfRows).Value;

			Assert.AreEqual(outputTable.Rows.Count, dataOut.Count);

			dynamic row = dataOut[0];
			Assert.AreEqual(1, row.IntValue);
			Assert.AreEqual(0.1, row.DecimalValue);
			Assert.AreEqual(DateTime.Now.Date, row.DateTimeValue.Date);
			Assert.IsTrue(row.BitValue);
			Assert.AreEqual("Some value", row.StringValue);
			Assert.IsNotNull(Guid.Parse(row.GuidValue));

			row = dataOut[1];
			Assert.AreEqual(0, row.IntValue);
			Assert.AreEqual(0, row.DecimalValue);
			Assert.AreEqual(DateTime.MinValue, row.DateTimeValue);
			Assert.IsFalse(row.BitValue);
			Assert.AreEqual(String.Empty, row.StringValue);
			Assert.That(row.GuidValue, Is.Null.Or.Empty);
		}

		[Test]
		public void TestExecuteWithFirstRowReturnMode()
		{
			string sql = @"DECLARE @t TABLE
				(
					IntValue INT,
					DecimalValue NUMERIC(10,2),
					DateTimeValue DATETIME,
					BitValue BIT,
					StringValue VARCHAR(50),
					GuidValue UNIQUEIDENTIFIER
				)
				INSERT INTO @t VALUES (1, 0.1, GETDATE(), 1, 'Some value', NEWID())
				INSERT INTO @t VALUES (NULL, NULL, NULL, NULL, NULL, NULL)
				SELECT * FROM @t";
			dynamic dataOut = Execute(sql, ExecuteSQLShared.ReturnModeType.FirstRow).Value;

			Assert.AreEqual(1, dataOut.IntValue);
			Assert.AreEqual(0.1, dataOut.DecimalValue);
			Assert.AreEqual(DateTime.Now.Date, dataOut.DateTimeValue.Date);
			Assert.IsTrue(dataOut.BitValue);
			Assert.AreEqual("Some value", dataOut.StringValue);
			Assert.IsNotNull(Guid.Parse(dataOut.GuidValue));
		}

		[Test]
		public void TestExecuteWithFirstRowReturnModeNoRows()
		{
			string sql = @"DECLARE @t TABLE
				(
					IntValue INT,
					DecimalValue NUMERIC(10,2),
					DateTimeValue DATETIME,
					BitValue BIT,
					StringValue VARCHAR(50),
					GuidValue UNIQUEIDENTIFIER
				)
				SELECT * FROM @t";

			Assert.That(() => Execute(sql, ExecuteSQLShared.ReturnModeType.FirstRow),
				Throws.Exception.TypeOf<ExecuteException>()
				.With.Property("Message").EqualTo("No rows returned by query.\r\nSee Code and Parameter properties for more information."));
		}

		[Test]
		public void TestExecuteWithFirstRowOrElseEmptyRowReturnMode()
		{
			string sql = @"DECLARE @t TABLE
				(
					IntValue INT,
					DecimalValue NUMERIC(10,2),
					DateTimeValue DATETIME,
					BitValue BIT,
					StringValue VARCHAR(50),
					GuidValue UNIQUEIDENTIFIER
				)
				INSERT INTO @t VALUES (1, 0.1, GETDATE(), 1, 'Some value', NEWID())
				INSERT INTO @t VALUES (NULL, NULL, NULL, NULL, NULL, NULL)
				SELECT * FROM @t";
			dynamic dataOut = Execute(sql, ExecuteSQLShared.ReturnModeType.FirstRowElseEmptyRow).Value;

			Assert.AreEqual(1, dataOut.IntValue);
			Assert.AreEqual(0.1, dataOut.DecimalValue);
			Assert.AreEqual(DateTime.Now.Date, dataOut.DateTimeValue.Date);
			Assert.IsTrue(dataOut.BitValue);
			Assert.AreEqual("Some value", dataOut.StringValue);
			Assert.IsNotNull(Guid.Parse(dataOut.GuidValue));
		}

		[Test]
		public void TestExecuteWithFirstRowOrElseEmptyRowReturnModeNoRows()
		{
			string sql = @"DECLARE @t TABLE
				(
					IntValue INT,
					DecimalValue NUMERIC(10,2),
					DateTimeValue DATETIME,
					BitValue BIT,
					StringValue VARCHAR(50),
					GuidValue UNIQUEIDENTIFIER
				)
				SELECT * FROM @t";
			dynamic dataOut = Execute(sql, ExecuteSQLShared.ReturnModeType.FirstRowElseEmptyRow).Value;

			Assert.AreEqual(0, dataOut.IntValue);
			Assert.AreEqual(0, dataOut.DecimalValue);
			Assert.AreEqual(DateTime.MinValue, dataOut.DateTimeValue);
			Assert.IsFalse(dataOut.BitValue);
			Assert.AreEqual(string.Empty, dataOut.StringValue);
			Assert.That(dataOut.GuidValue, Is.Null.Or.Empty);
		}

		[Test]
		public void TestExecuteNoResultType(
			[Values(ExecuteSQLShared.ReturnModeType.RowByRow, ExecuteSQLShared.ReturnModeType.ListOfRows, ExecuteSQLShared.ReturnModeType.FirstRow, ExecuteSQLShared.ReturnModeType.FirstRowElseEmptyRow)]
			ExecuteSQLShared.ReturnModeType returnModeType)
		{
			string sql = "DECLARE @t TABLE ( AColumn INT )";
			FunctionResult result = Execute(sql, returnModeType);
			Assert.IsNull(result.Value);
			Assert.IsNull(result.ExecutionPathResult);
		}

		[Test]
		public void TestExecuteWithRowByRowReturnModeCustomType()
		{
			var customType = TypeReference.CreateGeneratedType(new TypeProperty("Name", typeof(string)));
			string sql = @"DECLARE @t TABLE
				(
					IntValue INT,
					StringValue VARCHAR(50)
				)
				INSERT INTO @t VALUES (1, 'one')
				INSERT INTO @t VALUES (2, 'two')
				SELECT * FROM @t";
			var resultType = new ResultType { CustomType = customType };
			resultType.Fields.Add(new ResultTypeField { ColumnName = "IntValue", Type = typeof(int), Name = string.Empty });
			resultType.Fields.Add(new ResultTypeField { ColumnName = "StringValue", Type = typeof(string), Name = "Name" });
			DataTable outputTable = Helpers.DatabaseHelpers.GetDataTable(sql, Common.ConnectionType.SqlServer, Utilities.SqlConnectionString);
			FunctionResult functionResult = Execute(sql, ExecuteSQLShared.ReturnModeType.RowByRow, resultType);

			List<NextResult> executionPathResults = ((IEnumerable<NextResult>)functionResult.ExecutionPathResult).ToList();
			Assert.AreEqual(outputTable.Rows.Count, executionPathResults.Count);
			Assert.AreEqual(Names.GetValidName(customType.Name), executionPathResults[0].Value.GetType().Name);
			Assert.AreEqual("one", executionPathResults[0].Value.Name);
			Assert.AreEqual("two", executionPathResults[1].Value.Name);
		}

		[Test]
		[TestCase(ConnectionType.Odbc)]
		[TestCase(ConnectionType.OleDb)]
		[TestCase(ConnectionType.Oracle)]
		[TestCase(ConnectionType.SqlServer)]
		public void TestExecuteWithExpressionsInSQLString(ConnectionType connectionType)
		{
			DatabaseObjectCreation.CreateTestTable(connectionType);
			try
			{
				string sql = @"INSERT INTO TestTable 
											(IntValue, DoubleValue, StringValue, DateValue, BytesValue)
											VALUES (@{SomeIntValue - 3}, @{NullableDoubleValue}, @{Name + "" "" + 
											Surname}, @{Date}, @{Bytes})";

				Execute(connectionType.ToConnectionTypeSelection(), null, sql, ExecuteSQLShared.ReturnModeType.FirstRowElseEmptyRow, new ResultType(), false,
					new ParameterValue(ExecuteSQLShared.SqlValuePropertyPrefix + 1, 30),
					new ParameterValue(ExecuteSQLShared.SqlValuePropertyPrefix + 2, null),
					new ParameterValue(ExecuteSQLShared.SqlValuePropertyPrefix + 3, "John Doe"),
					new ParameterValue(ExecuteSQLShared.SqlValuePropertyPrefix + 4, DateTime.Today),
					new ParameterValue(ExecuteSQLShared.SqlValuePropertyPrefix + 5, new List<byte> { 1, 2, 3 }));

				sql = @"SELECT INTVALUE, DOUBLEVALUE, STRINGVALUE, DATEVALUE, BYTESVALUE FROM TestTable WHERE IntValue = @{TheIntValue}";

				var resultType = new ResultType();
				resultType.Fields.Add(new ResultTypeField { ColumnName = "INTVALUE", Type = typeof(int), Name = "IntValue" });
				resultType.Fields.Add(new ResultTypeField { ColumnName = "DOUBLEVALUE", Type = typeof(double), Name = "DoubleValue" });
				resultType.Fields.Add(new ResultTypeField { ColumnName = "STRINGVALUE", Type = typeof(string), Name = "StringValue" });
				resultType.Fields.Add(new ResultTypeField { ColumnName = "DATEVALUE", Type = typeof(DateTime), Name = "DateValue" });
				resultType.Fields.Add(new ResultTypeField { ColumnName = "BYTESVALUE", Type = typeof(List<byte>), Name = "BytesValue" });

				dynamic dataOut = Execute(connectionType.ToConnectionTypeSelection(), null, sql, ExecuteSQLShared.ReturnModeType.ListOfRows, resultType, false,
					new ParameterValue(ExecuteSQLShared.SqlValuePropertyPrefix + 1, 30)).Value;

				Assert.AreEqual(1, dataOut.Count);
				dynamic row = dataOut[0];
				Assert.AreEqual(30, row.IntValue);
				Assert.AreEqual(0, row.DoubleValue);
				Assert.AreEqual("John Doe", row.StringValue);
				Assert.AreEqual(DateTime.Today, row.DateValue);
				Assert.AreEqual(new byte[] { 1, 2, 3 }, row.BytesValue);
			}
			finally
			{
				DatabaseObjectCreation.RemoveTestTable(connectionType);
			}
		}

		[Test]
		[TestCase(ConnectionType.Odbc)]
		[TestCase(ConnectionType.OleDb)]
		[TestCase(ConnectionType.Oracle)]
		[TestCase(ConnectionType.SqlServer)]
		public void TestExecuteWithDuplicateExpressionsInSQLString(ConnectionType connectionType)
		{
			var stringType = (connectionType == ConnectionType.Oracle) ? "NVARCHAR2" : "NVARCHAR";
			var createTable = string.Format(@"CREATE TABLE Duplicates ( Duplicate1 {0}(50), Duplicate2 {0}(50), Different {0}(50), Duplicate3 {0}(50))", stringType);
			var dropTable = "DROP TABLE Duplicates";

			try { Helpers.DatabaseHelpers.ExecuteSqlStatement(dropTable, connectionType); } catch { }
			Helpers.DatabaseHelpers.ExecuteSqlStatement(createTable, connectionType);

			try
			{
				string sql = @"INSERT INTO Duplicates
											(Duplicate1, Duplicate2, Different, Duplicate3)
											VALUES(@{Same}, @{Same}, @{Different}, @{Same})";

				Execute(connectionType.ToConnectionTypeSelection(), null, sql, ExecuteSQLShared.ReturnModeType.FirstRowElseEmptyRow, new ResultType(), false,
					new ParameterValue(ExecuteSQLShared.SqlValuePropertyPrefix + 1, "A"),
					new ParameterValue(ExecuteSQLShared.SqlValuePropertyPrefix + 2, "A"),
					new ParameterValue(ExecuteSQLShared.SqlValuePropertyPrefix + 3, "B"),
					new ParameterValue(ExecuteSQLShared.SqlValuePropertyPrefix + 4, "A"));

				sql = @"SELECT Duplicate1, Duplicate2, Different, Duplicate3 FROM Duplicates WHERE Duplicate1 = 'A'";

				var resultType = new ResultType();
				resultType.Fields.Add(new ResultTypeField { ColumnName = "DUPLICATE1", Type = typeof(string), Name = "Duplicate1" });
				resultType.Fields.Add(new ResultTypeField { ColumnName = "DUPLICATE2", Type = typeof(string), Name = "Duplicate2" });
				resultType.Fields.Add(new ResultTypeField { ColumnName = "DIFFERENT", Type = typeof(string), Name = "Different" });
				resultType.Fields.Add(new ResultTypeField { ColumnName = "DUPLICATE3", Type = typeof(string), Name = "Duplicate3" });

				dynamic dataOut = Execute(connectionType.ToConnectionTypeSelection(), null, sql, ExecuteSQLShared.ReturnModeType.ListOfRows, resultType, false).Value;

				Assert.AreEqual(1, dataOut.Count);
				dynamic row = dataOut[0];
				Assert.AreEqual("A", row.Duplicate1);
				Assert.AreEqual("A", row.Duplicate2);
				Assert.AreEqual("B", row.Different);
				Assert.AreEqual("A", row.Duplicate3);
			}
			finally
			{
				Helpers.DatabaseHelpers.ExecuteSqlStatement(dropTable, connectionType);
			}
		}

		[Test]
		public void TestExecuteReusesExistingReturnFields()
		{
			string sql = @"DECLARE @t TABLE
				(
					IntValue INT,
					DecimalValue NUMERIC(10,2),
					DateTimeValue DATETIME,
					BitValue BIT,
					StringValue VARCHAR(50),
					GuidValue UNIQUEIDENTIFIER
				)
				INSERT INTO @t VALUES (1, 0.1, GETDATE(), 1, 'Some value', NEWID())
				INSERT INTO @t VALUES (NULL, NULL, NULL, NULL, NULL, NULL)
				SELECT * FROM @t";
			ResultType resultType = new ResultType();
			resultType.Fields.Add(new ResultTypeField() { ColumnName = "DateTimeValue", Type = typeof(DateTime), Name = "Date" });
			dynamic dataOut = Execute(sql, ExecuteSQLShared.ReturnModeType.FirstRow, resultType).Value;

			Assert.AreEqual(1, dataOut.IntValue);
			Assert.AreEqual(0.1, dataOut.DecimalValue);
			// The name of the date-property should still be "Date", and not "DateTimeValue".
			Assert.AreEqual(DateTime.Now.Date, dataOut.Date.Date);
			Assert.IsTrue(dataOut.BitValue);
			Assert.AreEqual("Some value", dataOut.StringValue);
			Assert.IsNotNull(Guid.Parse(dataOut.GuidValue));
		}

		[Test]
		public void TestExecuteWithUnknownColumnMapping()
		{
			string sql = @"DECLARE @Test TABLE	( IntValue INT )
				INSERT INTO @Test VALUES (1)
				SELECT IntValue FROM @Test";
			ResultType resultType = new ResultType();
			resultType.Fields.Add(new ResultTypeField() { ColumnName = "Missing", Type = typeof(string), Name = "IntValue" });

			Assert.That(() => Execute(sql, ExecuteSQLShared.ReturnModeType.ListOfRows, resultType, true),
				Throws.Exception.TypeOf<ExecuteException>()
				.With.Property("Message").EqualTo("IntValue mapped to Missing column but column not found.\r\nSee Code and Parameter properties for more information."));
		}

		[Test]
		[TestCase(ConnectionType.SqlServer)]
		public void TestExecuteWithBinaryTypes(ConnectionType connectionType)
		{
			string testImagePath = Path.Combine(Path.GetDirectoryName(new Uri(GetType().Assembly.CodeBase).LocalPath), "TestExecuteSQLImage.png");
			ResourceHelper.WriteResourceToFile("Twenty57.Linx.Components.Database.Tests.TestFiles.TestExecuteSQL.TestImage.png", GetType().Assembly, testImagePath);
			var testImageBytes = File.ReadAllBytes(testImagePath);

			var sql = string.Format(@"DECLARE @t TABLE
					(
						ID INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
						PhotoVar VARBINARY(MAX) NULL,
						PhotoImage IMAGE NULL
					)
					INSERT INTO @t (PhotoVar, PhotoImage) VALUES ({0}, {0})
					INSERT INTO @t (PhotoVar, PhotoImage) VALUES ({1}, {1})
					INSERT INTO @t (PhotoVar, PhotoImage) VALUES (@{{MyImage}}, @{{MyImage}})
					INSERT INTO @t (PhotoVar, PhotoImage) VALUES (NULL, NULL)

					SELECT * FROM @t",
									 "0x" + BitConverter.ToString(testImageBytes).Replace("-", ""),
									 string.Format("(SELECT * FROM OPENROWSET(BULK N'{0}', SINGLE_BLOB) AS TestImage)", testImagePath));
			List<byte> imageBytes = new List<byte>(testImageBytes);

			ResultType resultType = new ResultType();
			resultType.Fields.Add(new ResultTypeField() { Name = "ID", Type = typeof(int), ColumnName = "ID" });
			resultType.Fields.Add(new ResultTypeField() { Name = "PhotoVar", Type = typeof(Byte[]), ColumnName = "PhotoVar" });
			resultType.Fields.Add(new ResultTypeField() { Name = "PhotoImage", Type = typeof(Byte[]), ColumnName = "PhotoImage" });

			dynamic dataOut = Execute(connectionType.ToConnectionTypeSelection(), null, sql, ExecuteSQLShared.ReturnModeType.ListOfRows, resultType, true,
				new ParameterValue(ExecuteSQLShared.SqlValuePropertyPrefix + 1, imageBytes),
				new ParameterValue(ExecuteSQLShared.SqlValuePropertyPrefix + 2, imageBytes)).Value;

			Assert.AreEqual(4, dataOut.Count);

			dynamic row;
			for (int i = 0; i < 3; i++)
			{
				row = dataOut[i];
				Assert.AreEqual(i + 1, row.ID);
				Assert.IsInstanceOf<List<byte>>(row.PhotoVar);
				Assert.IsInstanceOf<List<byte>>(row.PhotoImage);
				Assert.AreEqual(testImageBytes, row.PhotoVar);
				Assert.AreEqual(testImageBytes, row.PhotoImage);
			}

			row = dataOut[3];
			Assert.AreEqual(4, row.ID);
			Assert.IsNull(row.PhotoVar);
			Assert.IsNull(row.PhotoImage);

			File.Delete(testImagePath);
		}

		[Test]
		[TestCase(ConnectionType.Odbc)]
		[TestCase(ConnectionType.OleDb)]
		[TestCase(ConnectionType.Oracle)]
		[TestCase(ConnectionType.SqlServer)]
		public void TestExecuteWithTransaction(ConnectionType connectionType)
		{
			DatabaseObjectCreation.CreateTestTable(connectionType);
			try
			{
				string sql = @"INSERT INTO TestTable (IntValue) VALUES (@{Value})";
				foreach (var transaction in new BeginTransaction.BeginTransactionX(connectionType, Helpers.DatabaseHelpers.GetDefaultConnectionString(connectionType), IsolationLevel.ReadCommitted))
				{
					Execute(transaction, sql, new ParameterValue(ExecuteSQLShared.SqlValuePropertyPrefix + 1, 0));
					Execute(transaction, sql, new ParameterValue(ExecuteSQLShared.SqlValuePropertyPrefix + 1, 1));
				}

				object rowCount = Helpers.DatabaseHelpers.ExecuteSqlScalar("SELECT COUNT(*) FROM TestTable", connectionType);
				int rowCountInt = connectionType == ConnectionType.Oracle ? (int)(decimal)rowCount : (int)rowCount;
				Assert.AreEqual(2, rowCountInt);
			}
			finally
			{
				DatabaseObjectCreation.RemoveTestTable(connectionType);
			}
		}

		[Test]
		[TestCase(ConnectionType.Odbc)]
		[TestCase(ConnectionType.OleDb)]
		[TestCase(ConnectionType.Oracle)]
		[TestCase(ConnectionType.SqlServer)]
		public void TestExecuteWithTransactionOnException(ConnectionType connectionType)
		{
			DatabaseObjectCreation.CreateTestTable(connectionType);
			try
			{
				Assert.Catch<Exception>(() =>
				{
					foreach (var transaction in new BeginTransaction.BeginTransactionX(connectionType, Helpers.DatabaseHelpers.GetDefaultConnectionString(connectionType), IsolationLevel.ReadCommitted))
					{
						Execute(transaction, "INSERT INTO TestTable (IntValue) VALUES (@{Value})",
							new ParameterValue(ExecuteSQLShared.SqlValuePropertyPrefix + 1, 0));
						throw new Exception("Bad things are happening!!");
					}
				});

				object rowCount = Helpers.DatabaseHelpers.ExecuteSqlScalar("SELECT COUNT(*) FROM TestTable", connectionType);
				int rowCountInt = connectionType == ConnectionType.Oracle ? (int)(decimal)rowCount : (int)rowCount;
				Assert.AreEqual(0, rowCountInt);
			}
			finally
			{
				DatabaseObjectCreation.RemoveTestTable(connectionType);
			}
		}

		[Test]
		[TestCase(ConnectionType.Odbc)]
		[TestCase(ConnectionType.OleDb)]
		[TestCase(ConnectionType.Oracle)]
		[TestCase(ConnectionType.SqlServer)]
		public void TestExecuteWithTransactionOnSqlError(ConnectionType connectionType)
		{
			DatabaseObjectCreation.CreateTestTable(connectionType);
			try
			{
				Assert.Catch<ExecuteException>(() =>
				{
					foreach (var transaction in new BeginTransaction.BeginTransactionX(connectionType, Helpers.DatabaseHelpers.GetDefaultConnectionString(connectionType), IsolationLevel.ReadCommitted))
					{
						Execute(transaction, "INSERT INTO TestTable (IntValue) VALUES (@{Value})",
							new ParameterValue(ExecuteSQLShared.SqlValuePropertyPrefix + 1, 0));
						Execute(transaction, "PUT Some Values INTO TestTable");
					}
				});

				object rowCount = Helpers.DatabaseHelpers.ExecuteSqlScalar("SELECT COUNT(*) FROM TestTable", connectionType);
				int rowCountInt = connectionType == ConnectionType.Oracle ? (int)(decimal)rowCount : (int)rowCount;
				Assert.AreEqual(0, rowCountInt);
			}
			finally
			{
				DatabaseObjectCreation.RemoveTestTable(connectionType);
			}
		}


		private FunctionResult Execute(string sqlStatement, ExecuteSQLShared.ReturnModeType returnMode, params ParameterValue[] sqlFieldParameters)
		{
			return Execute(ConnectionTypeSelection.OleDb, null, sqlStatement, returnMode, new ResultType(), false, sqlFieldParameters);
		}

		private FunctionResult Execute(string sqlStatement, ExecuteSQLShared.ReturnModeType returnMode, ResultType resultType, bool connectionStringAsExpression = false,
			params ParameterValue[] sqlFieldParameters)
		{
			return Execute(ConnectionTypeSelection.OleDb, null, sqlStatement, returnMode, resultType, connectionStringAsExpression, sqlFieldParameters);
		}

		private FunctionResult Execute(Transaction transaction, string sqlStatement, params ParameterValue[] sqlFieldParameters)
		{
			return Execute(ConnectionTypeSelection.UseTransaction, transaction, sqlStatement, ExecuteSQLShared.ReturnModeType.FirstRow, new ResultType(), false, sqlFieldParameters);
		}

		private FunctionResult Execute(ConnectionTypeSelection connectionType, Transaction transaction, string sqlStatement,
			ExecuteSQLShared.ReturnModeType returnMode, ResultType resultType, bool connectionStringAsExpression,
			params ParameterValue[] sqlFieldParameters)
		{
			string connectionString = connectionType == ConnectionTypeSelection.UseTransaction ? string.Empty : Helpers.DatabaseHelpers.GetDefaultConnectionString(connectionType.ToConnectionType().Value);

			object dynamicConnectionString;
			if (connectionStringAsExpression)
				dynamicConnectionString = new MockExpression("\"" + connectionString + "\"");
			else
				dynamicConnectionString = connectionString;

			var functionProperties = new PropertyValue[] {
				new PropertyValue(ExecuteSQLShared.TimeoutPropertyName, 5),
				new PropertyValue(ExecuteSQLShared.ReturnOptionsPropertyName, returnMode),
				new PropertyValue(ExecuteSQLShared.ResultTypePropertyName, resultType),
				new PropertyValue(DbShared.ConnectionTypePropertyName, connectionType),
				new PropertyValue(ExecuteSQLShared.SqlStatementPropertyName, sqlStatement)
			};

			var executeParameters = new ParameterValue[]
				{
					new ParameterValue(ExecuteSQLShared.SqlStatementPropertyName, sqlStatement)
				};

			if (connectionType == ConnectionTypeSelection.UseTransaction)
				executeParameters = executeParameters.Concat(new ParameterValue[] { new ParameterValue(DbShared.TransactionPropertyName, transaction) }).ToArray();
			else
			{
				functionProperties = functionProperties.Concat(new PropertyValue[] { new PropertyValue(DbShared.ConnectionStringPropertyName, dynamicConnectionString) }).ToArray();
				executeParameters = executeParameters.Concat(new ParameterValue[] { new ParameterValue(DbShared.ConnectionStringPropertyName, connectionString) }).ToArray();
			}

			var tester = (new FunctionTester<ExecuteSQL.ExecuteSQL>()).Compile(functionProperties);

			return tester.Execute(executeParameters
				.Concat<ParameterValue>(sqlFieldParameters).ToArray<ParameterValue>());
		}

		private class MockExpression : Twenty57.Linx.Plugin.Common.IExpression
		{
			public MockExpression(string expression)
			{
				Expression = expression;
			}

			public bool IsEmpty { get { return !string.IsNullOrEmpty(Expression); } }
			public string Expression { get; set; }

			public string GetExpression()
			{
				return Expression;
			}
		}
	}
}
