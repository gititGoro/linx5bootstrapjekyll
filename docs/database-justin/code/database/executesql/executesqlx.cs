using System;
using System.Collections.Generic;
using System.Data;
using System.Text.RegularExpressions;
using Twenty57.Linx.Components.Database.Common;

namespace Twenty57.Linx.Components.Database.ExecuteSQL
{
	public class ExecuteSQLX : IDisposable
	{
		private ConnectionType connectionType;
		private string connectionString;
		private IDbTransaction transaction = null;
		private int timeout;
		private string[] outputNames;
		private string[] columnNames;
		private string sql;
		private object[] sqlValues;

		private IDataReader reader;

		public ExecuteSQLX(ConnectionType connectionType, string connectionString, int timeout, string[] outputNames, string[] columnNames, string sqlToExecute, string[] sqlIdentifiers, object[] sqlValues)
			: this(timeout, outputNames, columnNames, sqlToExecute, sqlIdentifiers, sqlValues)
		{
			this.connectionType = connectionType;
			this.connectionString = connectionString;
		}

		public ExecuteSQLX(ConnectionType connectionType, IDbTransaction transaction, int timeout, string[] outputNames, string[] columnNames, string sqlToExecute, string[] sqlIdentifiers, object[] sqlValues)
			: this(timeout, outputNames, columnNames, sqlToExecute, sqlIdentifiers, sqlValues)
		{
			this.connectionType = connectionType;
			this.transaction = transaction;
		}

		private ExecuteSQLX(int timeout, string[] outputNames, string[] columnNames, string sqlToExecute, string[] sqlIdentifiers, object[] sqlValues)
		{
			if (outputNames.Length != columnNames.Length)
				throw new Exception("The output name count must match the column name count.");

			this.timeout = timeout;
			this.outputNames = outputNames;
			this.columnNames = columnNames;
			this.sql = SqlStringHandler.GetParameterizedSql(sqlToExecute, sqlIdentifiers);
			this.sqlValues = sqlValues;
		}

		public IDataReader Reader { get { return this.reader; } }

		public void OpenReader()
		{
			this.reader = GetDataReader();
		}

		public void Dispose()
		{
			try
			{
				if (null != this.reader)
				{
					if (!this.reader.IsClosed)
					{
						this.reader.Close();
						Log(transaction == null ? "Closed reader and connection." : "Closed reader.");
					}

					this.reader.Dispose();
					this.reader = null;
				}
			}
			catch
			{ }
		}

		public IEnumerable<T> Rows<T>(Func<IDataReader, T> rowBuilder)
		{
			try
			{
				while (Reader.Read())
					yield return rowBuilder(Reader);
			}
			finally
			{
				Dispose();
			}
		}

		public delegate void LogEventHandler(string message);
		public event LogEventHandler LogEvent;

		private IDataReader GetDataReader()
		{
			IDbCommand command = null;
			IDataReader baseReader = null;
			try
			{
				command = CreateCommand();
				baseReader = command.ExecuteReader(transaction == null ? CommandBehavior.CloseConnection : CommandBehavior.Default);
				var mapping = GetColumnMapping(baseReader);
				return new DataReader(baseReader, mapping);
			}
			catch
			{
				if (baseReader != null && !baseReader.IsClosed)
					baseReader.Close();

				if (command != null)
				{
					if ((transaction == null) && (command.Connection != null))
						command.Connection.Close();

					command.Dispose();
				}

				throw;
			}
		}

		private IDbCommand CreateCommand()
		{
			IDbCommand command;
			if (transaction == null)
			{
				command = DatabaseHelpers.CreateCommand(connectionType, connectionString, sql, sqlValues, timeout);
				LogCommand(command);
				Log(string.Format("Opening connection <{0}>", connectionString));
				command.Connection.Open();
				Log("Opened connection.");
			}
			else
			{
				command = DatabaseHelpers.CreateCommand(connectionType, transaction.Connection, sql, sqlValues, timeout);
				LogCommand(command);
				command.Transaction = transaction;
			}
			return command;
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

		private IDictionary<string, int> GetColumnMapping(IDataReader reader)
		{
			var mapping = new Dictionary<string, int>(reader.FieldCount);
			for (int fieldIndex = 0; fieldIndex < this.outputNames.Length; fieldIndex++)
			{
				var columnIndex = GetColumnIndex(reader, this.columnNames[fieldIndex]);
				if(columnIndex >= 0)
					mapping.Add(this.columnNames[fieldIndex], columnIndex);
				else
					throw new Exception(String.Format("{0} mapped to {1} column but column not found.", this.outputNames[fieldIndex], this.columnNames[fieldIndex]));
			}
			return mapping;
		}

		private static int GetColumnIndex(IDataReader reader, string columnName)
		{
			try
			{
				return reader.GetOrdinal(columnName);
			}
			catch (Exception exception)
			{
				if (exception is IndexOutOfRangeException)
					return GetColumnWithNoNameIndex(reader, columnName);

				return -1;
			}
		}

		private static int GetColumnWithNoNameIndex(IDataReader reader, string columnName)
		{
			Match match = new Regex(@"(?<=Column)\d").Match(columnName);
			if (!match.Success)
				return -1;

			int emptyRowNumber = int.Parse(match.Value);
			for (int index = 0; index < reader.FieldCount; index++)
			{
				string name = reader.GetName(index);
				if (String.IsNullOrEmpty(name))
					emptyRowNumber--;

				if (emptyRowNumber == 0)
					return index;
			}

			return -1;
		}
	}
}
