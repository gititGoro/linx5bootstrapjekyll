using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.SqlTypes;
using System.Linq;
using Twenty57.Linx.Plugin.Common;

namespace Twenty57.Linx.Components.Database.Common
{
	public static class DatabaseModel
	{
		public static readonly DateTime DefaultDateTime = (DateTime)SqlDateTime.MinValue;

		public enum ConnectionStringComponentType { Text, Flag }

		public class ConnectionStringComponent
		{
			private Func<ConnectionParameterLookup, bool> isApplicableFunc;

			public ConnectionStringComponent(string name, string description = null, string succinctDescription = null, ConnectionStringComponentType type = ConnectionStringComponentType.Text, Func<ConnectionParameterLookup, bool> isApplicableFunc = null)
			{
				Name = name;
				Description = description;
				SuccinctDescription = succinctDescription;
				Type = type;
				this.isApplicableFunc = isApplicableFunc;

				DefaultTextValue = string.Empty;
				TrueValue = "True";
				FalseValue = "False";
			}

			public string Name { get; private set; }
			public string Description { get; private set; }
			public string SuccinctDescription { get; private set; }
			public ConnectionStringComponentType Type { get; private set; }

			public string DefaultTextValue { get; internal set; }
			public string TrueValue { get; internal set; }
			public string FalseValue { get; internal set; }

			public bool IsApplicable(ILookup<string, string> connectionParameterMap)
			{
				return isApplicableFunc == null ? true : isApplicableFunc(new ConnectionParameterLookup(connectionParameterMap));
			}


			public class ConnectionParameterLookup
			{
				private ILookup<string, string> lookup;

				internal ConnectionParameterLookup(ILookup<string, string> lookup)
				{
					this.lookup = lookup;
				}

				public string this[string key]
				{
					get { return lookup.SingleOrDefault(p => p.Key.Equals(key, StringComparison.CurrentCultureIgnoreCase)).First(); }
				}

				public bool ContainsKey(string key)
				{
					return lookup.Any(p => p.Key.Equals(key, StringComparison.CurrentCultureIgnoreCase));
				}
			}
		}


		[AttributeUsage(AttributeTargets.Field)]
		public class RequiresSize : Attribute
		{
			public RequiresSize(int defaultColumnSize = 30)
			{
				DefaultColumnSize = defaultColumnSize;
			}

			public int DefaultColumnSize { get; private set; }
		}


		public enum DataType
		{
			[Description("BIGINT")]
			Int64,
			[Description("BIT")]
			Boolean,
			[Description("CHAR")]
			[RequiresSize]
			AnsiStringFixedLength,
			[Description("DATE")]
			Date,
			[Description("DATETIME")]
			DateTime,
			[Description("DATETIMEOFFSET")]
			TimeSpan,
			[Description("DECIMAL")]
			Decimal,
			[Description("DOUBLE")]
			Double,
			[Description("GUID")]
			[RequiresSize(16)]
			Guid,
			[Description("IMAGE")]
			[RequiresSize(100)]
			Binary,
			[Description("INT")]
			Int32,
			[Description("MONEY")]
			Currency,
			[Description("NCHAR")]
			[RequiresSize]
			StringFixedLength,
			[Description("NVARCHAR")]
			[RequiresSize]
			String,
			[Description("REFCURSOR")]
			RefCursor,
			[Description("SMALLINT")]
			Int16,
			[Description("TIME")]
			Time,
			[Description("TIMESTAMP")]
			TimeStamp,
			[Description("TINYINT")]
			Byte,
			[Description("VARCHAR")]
			[RequiresSize]
			AnsiString,
			[Description("XML")]
			[RequiresSize(100)]
			Xml
		}

		public class Column : ICloneable
		{
			public string Name { get; set; }
			public DataType DataType { get; set; }
			public int? Precision { get; set; }
			public int? Scale { get; set; }
			public int? Size { get; set; }
			public bool IsNullable { get; set; }

			public override int GetHashCode()
			{
				return Name.ToLower().GetHashCode() * 13 + (int)DataType * 7 + (Size ?? 0);
			}

			public override bool Equals(object obj)
			{
				if (!(obj is Column))
					return false;
				Column other = (Column)obj;
				return (Name.Equals(other.Name, StringComparison.InvariantCultureIgnoreCase)) && (DataType == other.DataType) && (Size == other.Size);
			}

			public object Clone()
			{
				return base.MemberwiseClone();
			}
		}

		public class Columns : Collection<Column>
		{
			public Columns() { }
			public Columns(IList<Column> columns) : base(columns) { }
			public Columns(IEnumerable<Column> columns) : this(columns.ToList()) { }
		}

		public enum ParameterDirection
		{
			[Description("IN")]
			In,
			[Description("OUT")]
			Out,
			[Description("IN / OUT")]
			InOut,
			[Description("Return Value")]
			ReturnValue
		}

		public class ProcedureParameter : Column
		{
			public ParameterDirection Direction { get; set; }
			public int Position { get; set; }

			public bool RequiresSize
			{
				get
				{
					return (Direction != ParameterDirection.In) && (TypeHelpers.GetEnumerationValueAttributes<RequiresSize>(DataType).Length != 0);
				}
			}

			public string DisplayPropertyId
			{
				get
				{
					return string.Format("{0} \u200B", Name);
				}
			}

			public string DisplayPropertyName
			{
				get
				{
					return string.Format("{0} \u200B", OutputPropertyName);
				}
			}

			public string OutputPropertyName
			{
				get
				{
					string parameterName = Name;
					if (parameterName.StartsWith("@"))
						parameterName = parameterName.Substring(1);
					parameterName = Names.GetValidName(parameterName.Replace(" ", string.Empty));
					return char.ToUpper(parameterName[0]) + parameterName.Substring(1);
				}
			}

			public override int GetHashCode()
			{
				return base.GetHashCode() * 7 + Position * 5 + (int)Direction;
			}

			public override bool Equals(object obj)
			{
				if (!base.Equals(obj))
					return false;
				if (!(obj is ProcedureParameter))
					return false;
				ProcedureParameter other = (ProcedureParameter)obj;
				return (Direction == other.Direction) && (Position == other.Position);
			}
		}

		public class ProcedureParameters : Collection<ProcedureParameter>
		{
			public ProcedureParameters() { }
			public ProcedureParameters(IList<ProcedureParameter> parameters) : base(parameters) { }
			public ProcedureParameters(IEnumerable<ProcedureParameter> parameters) : this(parameters.ToList()) { }
		}

		public class ProcedureParameterValue
		{
			public ProcedureParameterValue() { }
			public ProcedureParameterValue(string parameterName, object value, ParameterDirection direction = ParameterDirection.In, int? size = null, bool isRefCursor = false)
			{
				ParameterName = parameterName;
				Value = value;
				Direction = direction;
				Size = size;
				IsRefCursor = isRefCursor;
			}

			public string ParameterName { get; set; }
			public object Value { get; set; }
			public ParameterDirection Direction { get; set; }
			public int? Size { get; set; }
			public bool IsRefCursor { get; set; }
		}

		public class ResultSet
		{
			public ResultSet()
			{
				Fields = new Collection<ResultSetField>();
			}

			public Collection<ResultSetField> Fields { get; set; }
			public ITypeReference CustomType { get; set; }
		}

		public class ResultSets : Collection<ResultSet>
		{
			public ResultSets() { }

			public ResultSets(IList<ResultSet> resultSets) : base(resultSets) { }

			public ResultSets(IEnumerable<ResultSet> resultSets) : this(resultSets.ToList()) { }

			public ResultSets(System.Data.DataSet dataSet)
			{
				foreach (System.Data.DataTable nextTable in dataSet.Tables)
				{
					DatabaseModel.ResultSet nextResultSet = new DatabaseModel.ResultSet();
					foreach (System.Data.DataColumn nextColumn in nextTable.Columns)
						nextResultSet.Fields.Add(new DatabaseModel.ResultSetField(nextColumn.ColumnName, DatabaseModel.ParseDataType(nextColumn.DataType)));
					Add(nextResultSet);
				}
			}
		}

		public class ResultSetField : ICloneable
		{
			public ResultSetField() { }

			public ResultSetField(string columnName, DataType dataType) : this(columnName, dataType, Names.GetValidName(columnName)) { }

			public ResultSetField(string columnName, DataType dataType, string outputName)
			{
				ColumnName = columnName;
				DataType = dataType;
				OutputName = outputName;
			}

			public string ColumnName { get; set; }
			public DataType DataType { get; set; }
			public string OutputName { get; set; }

			public object Clone()
			{
				return MemberwiseClone();
			}
		}

		public static System.Data.ParameterDirection ToSystemDirection(ParameterDirection direction)
		{
			switch (direction)
			{
				case ParameterDirection.In: return System.Data.ParameterDirection.Input;
				case ParameterDirection.InOut: return System.Data.ParameterDirection.InputOutput;
				case ParameterDirection.Out: return System.Data.ParameterDirection.Output;
				case ParameterDirection.ReturnValue: return System.Data.ParameterDirection.ReturnValue;
				default: throw new Exception(string.Format("ParameterDirection not mapped: {0}.", direction));
			}
		}

		public static DataType ParseDataType(string dataType, int scale = -1)
		{
			switch (dataType.ToLower())
			{
				case "varchar":
				case "varchar2":
				case "text":
				case "clob": return DataType.AnsiString;
				case "char": return DataType.AnsiStringFixedLength;
				case "blob":
				case "binary":
				case "varbinary":
				case "image": return DataType.Binary;
				case "bit": return DataType.Boolean;
				case "tinyint": return DataType.Byte;
				case "smallmoney":
				case "money": return DataType.Currency;
				case "date": return DataType.Date;
				case "smalldatetime":
				case "datetime":
				case "datetime2": return DataType.DateTime;
				case "decimal": return DataType.Decimal;
				case "float":
				case "real": return DataType.Double;
				case "number":
				case "numeric": return scale == 0 ? DataType.Int32 : DataType.Double;
				case "uniqueidentifier": return DataType.Guid;
				case "smallint": return DataType.Int16;
				case "int": return DataType.Int32;
				case "bigint": return DataType.Int64;
				case "ref cursor": return DataType.RefCursor;
				case "nvarchar":
				case "nvarchar2":
				case "ntext":
				case "nclob": return DataType.String;
				case "nchar": return DataType.StringFixedLength;
				case "time": return DataType.Time;
				case "datetimeoffset": return DataType.TimeSpan;
				case "timestamp": return DataType.TimeStamp;
				case "xml": return DataType.Xml;
				default: return DataType.String;
			}
		}

		public static DataType ParseDataType(Type type)
		{
			if (type == typeof(bool))
				return DataType.Boolean;
			if ((type == typeof(byte)) || (type == typeof(sbyte)))
				return DataType.Byte;
			if (type == typeof(decimal))
				return DataType.Decimal;
			if ((type == typeof(double)) || (type == typeof(float)))
				return DataType.Double;
			if ((type == typeof(Int16)) || (type == typeof(UInt16)))
				return DataType.Int16;
			if ((type == typeof(Int32)) || (type == typeof(UInt32)))
				return DataType.Int32;
			if ((type == typeof(Int64)) || (type == typeof(UInt64)))
				return DataType.Int64;
			if (type == typeof(char))
				return DataType.AnsiStringFixedLength;
			if (type == typeof(DateTime))
				return DataType.DateTime;
			if (type == typeof(TimeSpan))
				return DataType.TimeSpan;
			if (type == typeof(Guid))
				return DataType.Guid;
			if ((type == typeof(string)) || (type == typeof(object)))
				return DataType.String;
			if ((type.IsArray) && (type.GetElementType() == typeof(byte)))
				return DataType.Binary;
			throw new Exception(string.Format("Type not mapped: {0}.", type));
		}

		public static DataType ParseDataType(ITypeReference typeReference)
		{
			if (typeReference.IsList)
			{
				if (typeReference.GetEnumerableContentType().GetUnderlyingType() == typeof(byte))
					return DataType.Binary;
				throw new Exception(string.Format("Type not mapped: {0}.", typeReference));
			}
			return ParseDataType(typeReference.GetUnderlyingType());
		}

		public static bool TryParseDataType(ITypeReference typeReference, out DataType dataType)
		{
			try
			{
				dataType = ParseDataType(typeReference);
				return true;
			}
			catch
			{
				dataType = DataType.String;
				return false;
			}
		}

		public static Type GetSystemType(this DataType dataType)
		{
			switch (dataType)
			{
				case DataType.Boolean: return typeof(bool);
				case DataType.Byte: return typeof(byte);
				case DataType.Date:
				case DataType.Time:
				case DataType.DateTime:
				case DataType.TimeStamp: return typeof(DateTime);
				case DataType.Currency:
				case DataType.Decimal: return typeof(decimal);
				case DataType.Double: return typeof(double);
				case DataType.Int16:
				case DataType.Int32:
				case DataType.Int64: return typeof(long);
				case DataType.AnsiString:
				case DataType.AnsiStringFixedLength:
				case DataType.String:
				case DataType.StringFixedLength:
				case DataType.Guid:
				case DataType.Xml: return typeof(string);
				case DataType.Binary: return typeof(List<>).MakeGenericType(typeof(byte));
				case DataType.RefCursor: return typeof(object);
				default: throw new Exception(string.Format("Data type not mapped: {0}.", dataType));
			}
		}

		public static object GetDbDefaultValue(this DataType dataType)
		{
			if (dataType == DataType.Binary)
				return new byte[0];

			Type systemType = dataType.GetSystemType();
			return systemType == typeof(DateTime) ? DefaultDateTime : systemType.GetDefaultValue();
		}
	}
}
