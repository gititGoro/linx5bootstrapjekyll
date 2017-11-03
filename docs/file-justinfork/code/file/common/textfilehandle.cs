using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Text;

namespace Twenty57.Linx.Components.File.Common
{
	[TypeConverter(typeof(TextFileHandleTypeConverter))]
	public class TextFileHandle : FileHandle
	{
		public TextFileHandle(string filePath, TextCodepage textCodePage = TextCodepage.Default) : base(filePath)
		{
			TextCodepage = textCodePage;
		}

		internal TextCodepage TextCodepage { get; set; }

		internal Encoding Encoding
		{
			get
			{
				switch (TextCodepage)
				{
					case TextCodepage.Default: return Encoding.Default;
					case TextCodepage.ANSI: return Encoding.GetEncoding(CultureInfo.CurrentCulture.TextInfo.ANSICodePage);
					case TextCodepage.EBCDIC: return Encoding.GetEncoding(CultureInfo.CurrentCulture.TextInfo.EBCDICCodePage);
					case TextCodepage.OEM: return Encoding.GetEncoding(CultureInfo.CurrentCulture.TextInfo.OEMCodePage);
					case TextCodepage.Mac: return Encoding.GetEncoding(CultureInfo.CurrentCulture.TextInfo.MacCodePage);
					case TextCodepage.ASCII: return Encoding.ASCII;
					case TextCodepage.Unicode: return Encoding.Unicode;
					case TextCodepage.UTF7: return Encoding.UTF7;
					case TextCodepage.UTF8: return Encoding.UTF8;
					default: throw new Exception(string.Format("Codepage [{0}] not supported.", TextCodepage));
				}
			}
		}

		public string Append
		{
			set
			{
				var streamWriter = CreateStreamWriter();
				Log(string.Format("Append <{0}>", value));
				streamWriter.Write(value);
				streamWriter.Flush();
			}
		}

		public string AppendLine
		{
			set
			{
				var streamWriter = CreateStreamWriter();
				Log(string.Format("Append line <{0}>", value));
				streamWriter.WriteLine(value);
				streamWriter.Flush();
			}
		}

		public StreamReader CreateStreamReader()
		{
			var fileStream = GetFileStream();
			Log(string.Format("Prepare stream reader with encoding {0}.", Encoding.EncodingName));
			return new StreamReader(fileStream, Encoding);
		}

		public StreamWriter CreateStreamWriter()
		{
			var fileStream = GetFileStream();
			Log(string.Format("Prepare stream writer with encoding {0}.", Encoding.EncodingName));
			return new StreamWriter(fileStream, Encoding);
		}


		public static implicit operator TextFileHandle(string filePath)
		{
			return new TextFileHandle(filePath);
		}
	}


	public class TextFileHandleTypeConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			return new TextFileHandle((string)value);
		}
	}
}
