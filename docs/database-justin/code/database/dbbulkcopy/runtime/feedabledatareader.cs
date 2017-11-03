using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Threading;

namespace Twenty57.Linx.Components.Database.DbBulkCopy.Runtime
{
	public class FeedableDataReader : DbDataReader
	{
		private bool isClosed = false;
		private object[] currentRow, nextRow;
		private EventWaitHandle availableRowHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
		private EventWaitHandle expectingNextRowHandle = new EventWaitHandle(true, EventResetMode.AutoReset);
		private EventWaitHandle firstRowHandle = new EventWaitHandle(false, EventResetMode.ManualReset);

		public FeedableDataReader()
		{
			Columns = new List<string>();
		}

		public List<string> Columns { get; private set; }

		public override void Close()
		{
			isClosed = true;
			availableRowHandle.Set();
		}

		public override int Depth
		{
			get { return 0; }
		}

		public override int FieldCount
		{
			get { return Columns.Count; }
		}

		public override string GetDataTypeName(int ordinal)
		{
			return GetFieldType(ordinal).Name;
		}

		public override Type GetFieldType(int ordinal)
		{
			firstRowHandle.WaitOne();
			object value = nextRow[ordinal];
			return value == null ? typeof(string) : value.GetType();
		}

		public override bool GetBoolean(int ordinal)
		{
			return (bool)currentRow[ordinal];
		}

		public override byte GetByte(int ordinal)
		{
			return (byte)currentRow[ordinal];
		}

		public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
		{
			byte[] sourceBytes = (byte[])currentRow[ordinal];
			int bytesToCopy = Math.Min(sourceBytes.Length, length);
			Array.Copy(sourceBytes, 0, buffer, bufferOffset, bytesToCopy);
			return bytesToCopy;
		}

		public override char GetChar(int ordinal)
		{
			return (char)currentRow[ordinal];
		}

		public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
		{
			char[] sourceChars = (char[])currentRow[ordinal];
			int charsToCopy = Math.Min(sourceChars.Length, length);
			Array.Copy(sourceChars, 0, buffer, bufferOffset, charsToCopy);
			return charsToCopy;
		}

		public override DateTime GetDateTime(int ordinal)
		{
			return (DateTime)currentRow[ordinal];
		}

		public override decimal GetDecimal(int ordinal)
		{
			return (decimal)currentRow[ordinal];
		}

		public override double GetDouble(int ordinal)
		{
			return (double)currentRow[ordinal];
		}

		public override System.Collections.IEnumerator GetEnumerator()
		{
			return currentRow.GetEnumerator();
		}

		public override float GetFloat(int ordinal)
		{
			return (float)currentRow[ordinal];
		}

		public override Guid GetGuid(int ordinal)
		{
			return (Guid)currentRow[ordinal];
		}

		public override short GetInt16(int ordinal)
		{
			return (short)currentRow[ordinal];
		}

		public override int GetInt32(int ordinal)
		{
			return (int)currentRow[ordinal];
		}

		public override long GetInt64(int ordinal)
		{
			return (long)currentRow[ordinal];
		}

		public override string GetName(int ordinal)
		{
			return Columns[ordinal];
		}

		public override int GetOrdinal(string name)
		{
			return Columns.IndexOf(name);
		}

		public override DataTable GetSchemaTable()
		{
			throw new NotImplementedException();
		}

		public override string GetString(int ordinal)
		{
			object value = currentRow[ordinal];
			return value == null ? null : value.ToString();
		}

		public override object GetValue(int ordinal)
		{
			object value = currentRow[ordinal];
			return value == null ? DBNull.Value : value;
		}

		public override int GetValues(object[] values)
		{
			int itemCount = Math.Min(values.Length, currentRow.Length);
			for (int i = 0; i < itemCount; i++)
				values[i] = currentRow[i];
			return itemCount;
		}

		public override bool HasRows
		{
			get { return true; }
		}

		public override bool IsClosed
		{
			get { return isClosed; }
		}

		public override bool IsDBNull(int ordinal)
		{
			return currentRow[ordinal] == null;
		}

		public override int RecordsAffected
		{
			get { return 0; }
		}

		public override object this[string name]
		{
			get { return currentRow[GetOrdinal(name)]; }
		}

		public override object this[int ordinal]
		{
			get { return currentRow[ordinal]; }
		}

		public override bool NextResult()
		{
			return Read();
		}

		public override bool Read()
		{
			availableRowHandle.WaitOne();
			if (nextRow == null)
				return false;
			currentRow = nextRow;
			expectingNextRowHandle.Set();
			return true;
		}

		public void AcceptRow(params object[] newRow)
		{
			expectingNextRowHandle.WaitOne();
			nextRow = newRow;
			availableRowHandle.Set();
			firstRowHandle.Set();
		}

		public void TellNoMoreRows()
		{
			AcceptRow(null);
		}

		public void UnblockRowAccept()
		{
			expectingNextRowHandle.Set();
		}
	}
}
