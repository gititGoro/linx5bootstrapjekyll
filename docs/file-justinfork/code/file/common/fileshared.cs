using System.ComponentModel;

namespace Twenty57.Linx.Components.File.Common
{
	public enum DoesNotExistOptions
	{
		[Description("Create file")]
		CreateFile,
		[Description("Throw exception")]
		ThrowException
	};

	public enum ExistOptions
	{
		[Description("Append data")]
		AppendData,
		[Description("Increment file name")]
		IncrementFileName,
		[Description("Overwrite file")]
		OverwriteFile,
		[Description("Throw exception")]
		ThrowException
	};

	public enum TextCodepage { Default, ANSI, ASCII, EBCDIC, Mac, OEM, Unicode, UTF7, UTF8 }

	public static class FileShared
	{
		public const string FilePathPropertyName = "File path";
		public const string OwnsFileHandlePropertyName = "Owns file handle";
	}
}