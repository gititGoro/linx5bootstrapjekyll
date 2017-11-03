using System;
using System.Data;

namespace Twenty57.Linx.Components.Database.Common
{
	public class Transaction : IDisposable, IDone
	{
		private bool isDone = false;
		private ConnectionType connectionType;
		private string connectionString;
		private IsolationLevel isolationLevel;
		private IDbTransaction transaction = null;

		public Transaction(ConnectionType connectionType, string connectionString, IsolationLevel isolationLevel)
		{
			this.connectionType = connectionType;
			this.connectionString = connectionString;
			this.isolationLevel = isolationLevel;
		}

		public void SetIsDone()
		{
			isDone = true;
		}

		public bool IsDone()
		{
			return isDone;
		}

		public ConnectionType GetConnectionType()
		{
			return connectionType;
		}

		public IDbTransaction GetDbTransaction()
		{
			if (transaction == null)
			{
				var databaseAssistant = DatabaseAssistant.GetDatabaseAssistant(connectionType);
				Log(string.Format("Opening connection <{0}>", connectionString));
				IDbConnection connection = databaseAssistant.CreateConnection(connectionString);
				connection.Open();
				Log("Opened connection. Beginning transaction...");
				transaction = connection.BeginTransaction(isolationLevel);
				Log("Begun transaction.");
			}
			return transaction;
		}

		public void Dispose()
		{
			if (transaction != null)
			{
				var connection = transaction.Connection;
				try
				{
					if (isDone)
					{
						Log("Committing transaction.");
						transaction.Commit();
					}
					else
					{
						Log("Rolling back transaction.");
						transaction.Rollback();
					}
				}
				finally
				{
					Log("Closing connection...");
					connection.Close();
					Log("Closed connection.");
					connection.Dispose();
					transaction = null;
				}
			}
		}

		public delegate void LogEventHandler(string message);
		public event LogEventHandler LogEvent;

		private void Log(string message)
		{
			if (LogEvent != null)
				LogEvent(message);
		}
	}


	internal interface IDone
	{
		void SetIsDone();
		bool IsDone();
	}
}
