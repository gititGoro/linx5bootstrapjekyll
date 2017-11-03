using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Twenty57.Linx.Components.Database.Common;
using Twenty57.Linx.Components.Database.DbBulkCopy;
using Twenty57.Linx.Components.Database.Tests.Helpers;
using Twenty57.Linx.Plugin.TestKit;

namespace Twenty57.Linx.Components.Database.Tests
{
	[TestFixture]
	public class TestDbBulkCopy
	{
		[Test]
		public void TestExecute([Values(DbBulkCopy.ConnectionType.SqlServer, DbBulkCopy.ConnectionType.Oracle)] DbBulkCopy.ConnectionType connectionType)
		{
			DatabaseObjectCreation.CreateTestTable(ToCommonConnectionType(connectionType));

			try
			{
				var functionResult = Execute(connectionType, "TESTTABLE");

				foreach (var nextResult in functionResult.ExecutionPathResult)
				{
					dynamic row = new ExpandoObject();
					row.IntValue = row.INTVALUE = 1;
					row.DoubleValue = row.DOUBLEVALUE = 1.2;
					row.StringValue = row.STRINGVALUE = "Qwer";
					row.DateValue = row.DATEVALUE = new DateTime(2015, 5, 13);
					row.BytesValue = row.BYTESVALUE = new List<byte> { 1, 2, 3 };
					nextResult.Value.Write = row;

					row = new ExpandoObject();
					row.IntValue = row.INTVALUE = 2;
					row.DoubleValue = row.DOUBLEVALUE = null;
					row.StringValue = row.STRINGVALUE = null;
					row.DateValue = row.DATEVALUE = null;
					row.BytesValue = row.BYTESVALUE = null;
					nextResult.Value.Write = row;
				}

				var results = DatabaseHelpers.GetDataTable(@"SELECT * FROM TESTTABLE", ToCommonConnectionType(connectionType));
				Assert.AreEqual(2, results.Rows.Count);
				object[] rowItems = results.Rows[0].ItemArray;
				Assert.AreEqual(1, rowItems[0]);
				Assert.AreEqual(1.2, rowItems[1]);
				Assert.AreEqual("Qwer", rowItems[2]);
				Assert.AreEqual(new DateTime(2015, 5, 13), rowItems[3]);
				Assert.AreEqual(new byte[] { 1, 2, 3 }, rowItems[4]);

				rowItems = results.Rows[1].ItemArray;
				Assert.AreEqual(2, rowItems[0]);
				Assert.AreEqual(DBNull.Value, rowItems[1]);
				Assert.AreEqual(DBNull.Value, rowItems[2]);
				Assert.AreEqual(DBNull.Value, rowItems[3]);
				Assert.AreEqual(DBNull.Value, rowItems[4]);
			}
			finally
			{
				DatabaseObjectCreation.RemoveTestTable(ToCommonConnectionType(connectionType));
			}
		}

		private FunctionResult Execute(DbBulkCopy.ConnectionType connectionType, string tableName)
		{
			string connectionString = DatabaseHelpers.GetDefaultConnectionString(ToCommonConnectionType(connectionType));
			return Execute(connectionType, connectionString, tableName);
		}

		private FunctionResult Execute(DbBulkCopy.ConnectionType connectionType, string connectionString, string tableName)
		{
			DatabaseModel.Columns columns;
			var databaseAssistant = DatabaseAssistant.GetDatabaseAssistant(ToCommonConnectionType(connectionType));
			using (var connection = databaseAssistant.CreateConnection(connectionString))
			{
				connection.Open();
				columns = new DatabaseModel.Columns(databaseAssistant.GetTableColumns(connection, tableName));
			}

			var tester = (new FunctionTester<DbBulkCopy.DbBulkCopy>()).Compile(
				new PropertyValue(DbBulkCopyShared.ConnectionTypePropertyName, connectionType),
				new PropertyValue(DbBulkCopyShared.ColumnsPropertyName, columns),
				new PropertyValue(DbBulkCopyShared.TableNamePropertyName, tableName));
			return tester.Execute(
				new ParameterValue(DbShared.ConnectionStringPropertyName, connectionString));
		}

		private static Common.ConnectionType ToCommonConnectionType(DbBulkCopy.ConnectionType connectionType)
		{
			return (Common.ConnectionType)Enum.Parse(typeof(Common.ConnectionType), connectionType.ToString());
		}
	}
}
