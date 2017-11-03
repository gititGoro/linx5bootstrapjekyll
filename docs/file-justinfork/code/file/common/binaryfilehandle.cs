using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace Twenty57.Linx.Components.File.Common
{
	[TypeConverter(typeof(BinaryFileHandleTypeConverter))]
	public class BinaryFileHandle : FileHandle
	{
		public BinaryFileHandle(string filePath) : base(filePath) { }

		public List<byte> Append
		{
			set
			{
				GetFileStream().Write(value.ToArray(), 0, value.Count);
			}
		}


		public static implicit operator BinaryFileHandle(string filePath)
		{
			return new BinaryFileHandle(filePath);
		}
	}


	public class BinaryFileHandleTypeConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			return new BinaryFileHandle((string)value);
		}
	}
}
