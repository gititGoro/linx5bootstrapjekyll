using System;
using System.Linq;
using System.Threading.Tasks;
using Twenty57.Linx.Components.Database.Common;

namespace Twenty57.Linx.Components.Database.DbBulkCopy.Runtime
{
	public class DbBulkCopierX : IDisposable
	{
		private bool didStart = false;
		private FeedableDataReader feedableDataReader = null;
		private BulkCopy bulkCopy = null;
		private Task bulkCopyTask = null;
		private Exception bulkCopyException = null;

		public DbBulkCopierX(Common.ConnectionType connectionType, string connectionString, string tableName, params string[] columnNames)
		{
			ConnectionType = connectionType;
			ConnectionString = connectionString;
			TableName = tableName;
			ColumnNames = columnNames;
		}

		public Common.ConnectionType ConnectionType { get; private set; }
		public string ConnectionString { get; private set; }
		public string TableName { get; private set; }
		public string[] ColumnNames { get; private set; }
		public int Timeout { get; set; }
		public int BatchSize { get; set; }

		public void Start()
		{
			if (didStart)
				throw new Exception("DbBulkCopierX can be started only once.");

			feedableDataReader = new FeedableDataReader();
			foreach (string nextColumn in ColumnNames)
				feedableDataReader.Columns.Add(nextColumn);

			Log(string.Format("Opening connection <{0}>", ConnectionString));
			bulkCopy = BulkCopy.GetBulkCopy(ConnectionType, ConnectionString);
			Log("Opened connection.");
			bulkCopy.DestinationTableName = TableName;
			foreach (string nextColumn in ColumnNames)
				bulkCopy.AddColumnMapping(nextColumn, nextColumn);
			Log("Setting timeout value to " + Timeout + ".");
			bulkCopy.Timeout = Timeout;
			Log("Setting batch value to " + BatchSize + ".");
			bulkCopy.BatchSize = BatchSize;

			bulkCopyTask = Task.Factory.StartNew(() =>
			{
				try
				{
					bulkCopy.WriteToServer(feedableDataReader);
				}
				catch (Exception exc)
				{
					Log("Exception occurred: " + exc.Message);
					bulkCopyException = exc;
					// Unblock possible thread that is running AcceptRow().
					feedableDataReader.UnblockRowAccept();
					// Unblock future call to TellNoMoreRows().
					feedableDataReader.UnblockRowAccept();
					bulkCopy.Dispose();
					bulkCopy = null;
					throw;
				}
			});
			didStart = true;
		}

		public void Stop()
		{
			Dispose();
			if (bulkCopyException != null)
				throw bulkCopyException;
		}

		public void AcceptRow(params object[] newRow)
		{
			if (bulkCopyException != null)
				throw bulkCopyException;
			Log("Copying values " + string.Join(", ", newRow.Select(v => LogHelpers.GetDisplayString(v))));
			feedableDataReader.AcceptRow(newRow);
		}

		public void Dispose()
		{
			if (feedableDataReader != null)
			{
				Log("Waiting for bulk copy task to end...");
				feedableDataReader.TellNoMoreRows();
				if (bulkCopyTask != null)
				{
					bulkCopyTask.Wait();
					bulkCopyTask = null;
				}
				if (bulkCopy != null)
				{
					bulkCopy.Dispose();
					bulkCopy = null;
				}
				Log("Closing reader and connection...");
				feedableDataReader.Close();
				feedableDataReader = null;
				Log("Closed reader and connection.");
			}
		}

		private void Log(string message)
		{
			if (LogEvent != null)
				LogEvent(message);
		}

		public delegate void LogEventHandler(string message);
		public event LogEventHandler LogEvent;
	}
}
