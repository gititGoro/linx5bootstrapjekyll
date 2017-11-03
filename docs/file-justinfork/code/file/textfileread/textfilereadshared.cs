using System.ComponentModel;

namespace Twenty57.Linx.Components.File.TextFileRead
{
	public enum FileReadOptions
	{
		Complete,
		[Description("Line by line")]
		LineByLine,
		[Description("List of lines")]
		ListOfLines
	}

	public static class TextFileReadShared
	{
		public const string CodepagePropertyName = "Codepage";
		public const string ReadTypePropertyName = "Read type";
		public const string SkipHeaderLinesPropertyName = "Skip header lines";
		public const string SkipFooterLinesPropertyName = "Skip footer lines";
		public const string FieldsPropertyName = "Fields";

		public const string FileContentsName = "FileContents";
		public const string LineNumberName = "LineNumber";
		public const string LineContentsName = "LineContents";
		public const string ExecutionPathName = "Results";
	}
}
