using System;
using System.Collections.Generic;
using System.Linq;

namespace Twenty57.Linx.Components.File.TextFileRead
{
	public class FieldParser
	{
		private readonly FileType fileType;
		private readonly DelimiterType delimiterType;
		private readonly TextQualifierType textQualifierType;
		private readonly string otherDelimiter;
		private readonly string fixedFieldLengths;

		public FieldParser() { }

		public FieldParser(FileType fileType, DelimiterType delimiterType, TextQualifierType textQualifierType, string otherDelimiter, string fixedFieldLengths)
		{
			this.fileType = fileType;
			this.delimiterType = delimiterType;
			this.textQualifierType = textQualifierType;
			this.otherDelimiter = otherDelimiter;
			this.fixedFieldLengths = fixedFieldLengths;
		}

		public string[] GetValues(string lineContents)
		{
			char delimiter = ',';
			if (DelimiterType.Other == delimiterType && !String.IsNullOrEmpty(otherDelimiter))
				delimiter = otherDelimiter[0];
			else if (DelimiterType.Tab == delimiterType)
				delimiter = Convert.ToChar(9);

			char textqualifier = '"';
			if (TextQualifierType.SingleQuotes == textQualifierType)
				textqualifier = '\'';

			string[] fields;
			if (FileType.Delimited == fileType)
			{
				fields = GetDelimitedFields(lineContents, delimiter, textqualifier);
			}
			else
				fields = GetFixedLengthFields(lineContents);

			int count = 0;

			if (TextQualifierType.None != textQualifierType)
				foreach (string field in fields)
				{
					if ((field != String.Empty) && FirstCharIsTextQualifier(field, textqualifier) &&
						LastCharIsTextQualifier(field, textqualifier))
						if (field.Length > 2)
							fields[count] = field.Substring(1, field.Length - 2);
						else
							fields[count] = String.Empty;

					fields[count] = fields[count].Replace(textqualifier.ToString() + textqualifier, textqualifier.ToString());
					count++;
				}

			return fields;
		}

		private string[] GetDelimitedFields(string lineContents, char delimiter, char textqualifier)
		{
			List<string> fields = lineContents.Split(delimiter).ToList();

			for (int idx = 0; idx < fields.Count(); idx++)
			{
				string field = fields[idx];
				while ((field != String.Empty) &&
					   (FirstCharIsTextQualifier(field.Trim(), textqualifier) &&
						!LastCharIsTextQualifier(field.Trim(), textqualifier) && fields.Count != idx + 1))
				{
					field = field + delimiter + fields[idx + 1];
					fields[idx] = field;
					fields.RemoveAt(idx + 1);
				}
			}

			for (int idx = 0; idx < fields.Count(); idx++)
				fields[idx] = fields[idx].Trim();

			return fields.ToArray();
		}

		private bool LastCharIsTextQualifier(string field, char textqualifier)
		{
			string trimmedField = field.TrimEnd(textqualifier);

			if (trimmedField == String.Empty)
				return field.Length % 2 == 0;
			return (field.Length - trimmedField.Length) % 2 == 1;
		}

		private bool FirstCharIsTextQualifier(string field, char textqualifier)
		{
			return (!String.IsNullOrEmpty(field)) && field[0] == textqualifier;
		}

		private string[] GetFixedLengthFields(string lineContents)
		{
			var fields = new List<string>();
			int placeHolder = 0;
			foreach (string strLength in fixedFieldLengths.Split(','))
			{
				if (strLength.Trim() == "")
					fields.Add("");
				else
				{
					int length = int.Parse(strLength);
					if (placeHolder + length > lineContents.Length)
						fields.Add(lineContents.Substring(placeHolder));
					else
					{
						fields.Add(lineContents.Substring(placeHolder, length));
						placeHolder += length;
					}
				}
			}

			return fields.Select(field => field.Trim()).ToArray();
		}
	}
}
