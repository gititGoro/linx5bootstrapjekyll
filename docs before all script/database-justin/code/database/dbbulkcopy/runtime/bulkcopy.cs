using Oracle.DataAccess.Client;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Reflection;
using Twenty57.Linx.Components.Database.DbBulkCopy.Helpers;

namespace Twenty57.Linx.Components.Database.DbBulkCopy.Runtime
{
	public abstract class BulkCopy : IDisposable
	{
		public abstract int Timeout { get; set; }
		public abstract int BatchSize { get; set; }
		public abstract string DestinationTableName { get; set; }

		public abstract void Dispose();
		public abstract void AddColumnMapping(string sourceColumn, string destinationColumn);
		public abstract void WriteToServer(IDataReader reader);


		public static BulkCopy GetBulkCopy(Common.ConnectionType connectionType, string connectionString)
		{
			switch (connectionType)
			{
				case Common.ConnectionType.SqlServer: return new SqlBulkCopy(connectionString);
				case Common.ConnectionType.Oracle: return new OracleBulkCopy(connectionString);
				default: throw new ArgumentException("Unsupported ConnectionType: " + connectionType);
			}
		}
	}


	public class SqlBulkCopy : BulkCopy
	{
		private SqlConnection connection;
		private System.Data.SqlClient.SqlBulkCopy bulkCopy;

		public SqlBulkCopy(string connectionString)
		{
			connection = new SqlConnection(connectionString);
			connection.Open();
			bulkCopy = new System.Data.SqlClient.SqlBulkCopy(connection);
		}

		public override int Timeout
		{
			get { return this.bulkCopy.BulkCopyTimeout; }
			set { this.bulkCopy.BulkCopyTimeout = value; }
		}

		public override int BatchSize
		{
			get { return bulkCopy.BatchSize; }
			set { bulkCopy.BatchSize = value; }
		}

		public override string DestinationTableName
		{
			get { return bulkCopy.DestinationTableName; }
			set { bulkCopy.DestinationTableName = value; }
		}

		public override void Dispose()
		{
			bulkCopy.Close();
			connection.Close();
		}

		public override void AddColumnMapping(string sourceColumn, string destinationColumn)
		{
			bulkCopy.ColumnMappings.Add(sourceColumn, destinationColumn);
		}

		public override void WriteToServer(IDataReader reader)
		{
			bulkCopy.WriteToServer(reader);
		}
	}


	public class OracleBulkCopy : BulkCopy
	{
		private OracleConnection connection;
		private Oracle.DataAccess.Client.OracleBulkCopy bulkCopy;

		public OracleBulkCopy(string connectionString)
		{
			connection = new OracleConnection(connectionString);
			connection.Open();
			bulkCopy = new Oracle.DataAccess.Client.OracleBulkCopy(connection);
		}

		static OracleBulkCopy()
		{
			AppDomain.CurrentDomain.AssemblyResolve += (_AppDomain, e) =>
			{
				if (e.Name.Contains(".resources"))
					return null;

				if (e.Name.StartsWith("Oracle.DataAccess"))
				{
					string filePath = GacUtil.GetAssemblyPath("Oracle.DataAccess");
					return filePath == null ? null : Assembly.LoadFile(filePath);
				}
				return null;
			};
		}

		public override int Timeout
		{
			get { return this.bulkCopy.BulkCopyTimeout; }
			set { this.bulkCopy.BulkCopyTimeout = value; }
		}

		public override int BatchSize
		{
			get { return bulkCopy.BatchSize; }
			set { bulkCopy.BatchSize = value; }
		}

		public override string DestinationTableName
		{
			get { return bulkCopy.DestinationTableName; }
			set { bulkCopy.DestinationTableName = value; }
		}

		public override void Dispose()
		{
			bulkCopy.Close();
			connection.Close();
		}

		public override void AddColumnMapping(string sourceColumn, string destinationColumn)
		{
			bulkCopy.ColumnMappings.Add(sourceColumn, destinationColumn);
		}

		public override void WriteToServer(IDataReader reader)
		{
			bulkCopy.WriteToServer(reader);
		}
	}
}
