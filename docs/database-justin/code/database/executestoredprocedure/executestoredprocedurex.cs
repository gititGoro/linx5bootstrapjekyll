using System;
using System.Collections.Generic;
using System.Data;
using Twenty57.Linx.Components.Database.Common;

namespace Twenty57.Linx.Components.Database.ExecuteStoredProcedure
{
	public class ExecuteStoredProcedureX : IDisposable
	{
		private ConnectionType connectionType;
		private string connectionString;
		private IDbTransaction transaction = null;
		private string storedProcedureName;
		private DatabaseModel.ProcedureParameterValue[] parameters;
		private IDataParameterCollection commandParameters;

		public ExecuteStoredProcedureX(ConnectionType connectionType, string connectionString, string storedProcedureName, params DatabaseModel.ProcedureParameterValue[] parameters)
			: this(storedProcedureName, parameters)
		{
			this.connectionType = connectionType;
			this.connectionString = connectionString;
		}

		public ExecuteStoredProcedureX(ConnectionType connectionType, IDbTransaction transaction, string storedProcedureName, params DatabaseModel.ProcedureParameterValue[] parameters)
			: this(storedProcedureName, parameters)
		{
			this.connectionType = connectionType;
			this.transaction = transaction;
		}

		private ExecuteStoredProcedureX(string storedProcedureName, params DatabaseModel.ProcedureParameterValue[] parameters)
		{
			this.storedProcedureName = storedProcedureName;
			this.parameters = parameters;
		}

		public IDataReader Reader { get; private set; }

		public void Execute(bool openReader = false)
		{
			IDbCommand command = CreateCommand();
			LogCommand(command);
			try
			{
				if (openReader)
					Reader = command.ExecuteReader(transaction == null ? CommandBehavior.CloseConnection : CommandBehavior.Default);
				else
				{
					command.ExecuteNonQuery();
					if (transaction == null)
					{
						command.Connection.Close();
						Log("Closed connection.");
						command.Connection.Dispose();
					}
				}
				commandParameters = command.Parameters;
			}
			catch
			{
				if (Reader == null)
				{
					if (transaction == null)
					{
						command.Connection.Close();
						command.Connection.Dispose();
					}
				}
				else
				{
					Reader.Close();
					Reader.Dispose();
					Reader = null;
				}
				command.Dispose();
				throw;
			}
		}

		public object GetParameterValue(string parameterName)
		{
			foreach (IDataParameter nextParameter in commandParameters)
				if (nextParameter.ParameterName == parameterName)
					return nextParameter.Value;
			throw new Exception(string.Format("Parameter not found {0}.", parameterName));
		}

		public IEnumerable<T> NextResultRows<T>(Func<IDataReader, T> rowBuilder)
		{
			if (Reader == null)
				yield break;

			try
			{
				while (Reader.Read())
					yield return rowBuilder(Reader);
			}
			finally
			{
				if (!Reader.NextResult())
					Dispose();
			}
		}

		public void Dispose()
		{
			if (Reader != null)
			{
				Reader.Close();
				Log("Closed reader and connection.");
				Reader.Dispose();
				Reader = null;

				if (ReaderClosedEvent != null)
					ReaderClosedEvent(this, new EventArgs());
			}
		}

		public delegate void ReaderClosedEventHandler(object sender, EventArgs args);
		public event ReaderClosedEventHandler ReaderClosedEvent;

		public delegate void LogEventHandler(string message);
		public event LogEventHandler LogEvent;

		private IDbCommand CreateCommand()
		{
			var databaseAssistant = DatabaseAssistant.GetDatabaseAssistant(connectionType);
			if (transaction == null)
			{
				IDbConnection connection = databaseAssistant.CreateConnection(connectionString);
				Log(string.Format("Opening connection <{0}>", connectionString));
				connection.Open();
				Log("Opened connection.");
				return databaseAssistant.BuildStoredProcedureCommand(connection, storedProcedureName, parameters);
			}
			else
			{
				IDbCommand command = databaseAssistant.BuildStoredProcedureCommand(transaction.Connection, storedProcedureName, parameters);
				command.Transaction = transaction;
				return command;
			}
		}

		private void LogCommand(IDbCommand command)
		{
			Log(string.Format("Created command with: {0}{1}", Environment.NewLine, command.GetDescription()));
		}

		private void Log(string message)
		{
			if (LogEvent != null)
				LogEvent(message);
		}
	}
}
