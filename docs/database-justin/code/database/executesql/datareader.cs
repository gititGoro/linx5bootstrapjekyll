using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Twenty57.Linx.Components.Database.ExecuteSQL
{
	public class DataReader : IDataReader
	{
		private IDataReader dataReader;
		IDictionary<string, int> columnNameMap;

		public DataReader(IDataReader dataReader, IDictionary<string, int> columnNameMap)
		{
			this.dataReader = dataReader;
			this.columnNameMap = columnNameMap;
        }

		public object this[string name]
		{
			get
			{
				return this.dataReader[columnNameMap[name]];
			}
		}

		public object this[int i]
		{
			get
			{
				return this.dataReader[i];
            }
		}

		public int Depth
		{
			get
			{
				return this.dataReader.Depth;
            }
		}

		public int FieldCount
		{
			get
			{
				return this.dataReader.Depth;
			}
		}

		public bool IsClosed
		{
			get
			{
				return this.dataReader.IsClosed;
			}
		}

		public int RecordsAffected
		{
			get
			{
				return this.dataReader.RecordsAffected;
			}
		}

		public void Close()
		{
			this.dataReader.Close();
		}

		public void Dispose()
		{
			this.dataReader.Dispose();
		}

		public bool GetBoolean(int i)
		{
			return this.dataReader.GetBoolean(i);
		}

		public byte GetByte(int i)
		{
			return this.dataReader.GetByte(i);
		}

		public long GetBytes(int i, long fieldOffset, byte[] buffer, int bufferoffset, int length)
		{
			return this.dataReader.GetByte(i);
		}

		public char GetChar(int i)
		{
			return this.dataReader.GetChar(i);
		}

		public long GetChars(int i, long fieldoffset, char[] buffer, int bufferoffset, int length)
		{
			return this.dataReader.GetChars(i, fieldoffset, buffer, bufferoffset, length);
		}

		public IDataReader GetData(int i)
		{
			return this.dataReader.GetData(i);
		}

		public string GetDataTypeName(int i)
		{
			return this.dataReader.GetDataTypeName(i);
		}

		public DateTime GetDateTime(int i)
		{
			return this.dataReader.GetDateTime(i);
		}

		public decimal GetDecimal(int i)
		{
			return this.dataReader.GetDecimal(i);
		}

		public double GetDouble(int i)
		{
			return this.dataReader.GetDouble(i);
		}

		public Type GetFieldType(int i)
		{
			return this.dataReader.GetFieldType(i);
		}

		public float GetFloat(int i)
		{
			return this.dataReader.GetFloat(i);
		}

		public Guid GetGuid(int i)
		{
			return this.dataReader.GetGuid(i);
		}

		public short GetInt16(int i)
		{
			return this.dataReader.GetInt16(i);
		}

		public int GetInt32(int i)
		{
			return this.dataReader.GetInt32(i);
		}

		public long GetInt64(int i)
		{
			return this.dataReader.GetInt64(i);
		}

		public string GetName(int i)
		{
			return this.dataReader.GetName(i);
		}

		public int GetOrdinal(string name)
		{
			return this.columnNameMap[name];
		}

		public DataTable GetSchemaTable()
		{
			return this.dataReader.GetSchemaTable();
		}

		public string GetString(int i)
		{
			return this.dataReader.GetString(i);
		}

		public object GetValue(int i)
		{
			return this.dataReader.GetValue(i);
		}

		public int GetValues(object[] values)
		{
			return this.dataReader.GetValues(values);
		}

		public bool IsDBNull(int i)
		{
			return this.dataReader.IsDBNull(i);
		}

		public bool NextResult()
		{
			return this.dataReader.NextResult();
		}

		public bool Read()
		{
			return this.dataReader.Read();
		}
	}
}
