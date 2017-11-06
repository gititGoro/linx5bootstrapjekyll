using System;
using System.Collections.Generic;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.Types;

namespace Twenty57.Linx.Components.File.TextFileRead
{
	public class FieldType
	{
		public FieldType(Type type)
		{
			TypeReference = type;
		}

		public string Name
		{
			get { return TypeReference.Name; }
		}

		public Type TypeReference { get; set; }

		public bool Is(Type type)
		{
			return TypeReference == type;
		}

		public Type GetGeneratedType()
		{
			return TypeReference;
		}

		public override bool Equals(object obj)
		{
			return (obj is FieldType) && (((FieldType)obj).TypeReference.Equals(TypeReference));
		}

		public override int GetHashCode()
		{
			return TypeReference.GetHashCode();
		}

		public override string ToString()
		{
			return Name;
		}
	}


	public class Afield : ICloneable
	{
		public Afield()
		{
			Name = "";
			Skip = false;
			Format = "";
			Length = 0;
			Type = new FieldType(typeof(string));
		}

		public string Name { get; set; }
		public bool Skip { get; set; }
		public string Format { get; set; }
		public int Length { get; set; }
		public FieldType Type { get; set; }

		public object Clone()
		{
			return new Afield
			{
				Format = (string)Format.Clone(),
				Name = (string)Name.Clone(),
				Skip = Skip,
				Length = Length
			};
		}

		public Property GetProperty()
		{
			return new Property(Name, Type.TypeReference, ValueUseOption.DesignTime, null);
		}
	}


	public class AfieldCollection : List<Afield>, ICloneable
	{
		public object Clone()
		{
			var clonedAfieldCollection = new AfieldCollection();
			foreach (Afield afield in this)
				clonedAfieldCollection.Add((Afield)afield.Clone());
			return clonedAfieldCollection;
		}
	}


	public class TextFileReaderFields : ICloneable
	{
		public TextFileReaderFields()
		{
			TextFileType = FileType.Delimited;
			Delimiter = DelimiterType.Comma;
			TextQualifier = TextQualifierType.None;
			OtherDelimiter = "";
			FieldList = new AfieldCollection();
		}

		public FileType TextFileType { get; set; }
		public DelimiterType Delimiter { get; set; }
		public TextQualifierType TextQualifier { get; set; }
		public string OtherDelimiter { get; set; }

		public AfieldCollection FieldList { get; set; }

		public TypeReference CreateTypeReference()
		{
			TypeBuilder typeBuilder = new TypeBuilder();
			foreach (Afield field in FieldList)
				if (!field.Skip)
					typeBuilder.AddProperty(field.Name, field.Type.TypeReference);
			return typeBuilder.CreateTypeReference();
		}

		public object Clone()
		{
			return new TextFileReaderFields
			{
				TextFileType = TextFileType,
				Delimiter = Delimiter,
				TextQualifier = TextQualifier,
				OtherDelimiter = OtherDelimiter,
				FieldList = (AfieldCollection)FieldList.Clone()
			};
		}
	}

	public enum FileType
	{
		Delimited,
		FixedLength
	}

	public enum DelimiterType
	{
		Comma,
		Tab,
		Other
	}

	public enum TextQualifierType
	{
		DoubleQuotes,
		SingleQuotes,
		None
	}
}