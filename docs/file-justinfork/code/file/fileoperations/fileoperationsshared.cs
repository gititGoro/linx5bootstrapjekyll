using System.ComponentModel;

namespace Twenty57.Linx.Components.File
{
	public static class FileOperationsShared
	{
		public const string SourceFilePathPropertyName = "Source file path";
		public const string KeepFileNamePropertyName = "Keep file name";
		public const string DestinationFolderPathPropertyName = "Dest. folder path";
		public const string DestinationFilePathPropertyName = "Dest. file path";
		public const string ActionPropertyName = "Action";
		public const string FileExistsPropertyName = "File exists";

		public enum ActionType { Copy, Move, Delete, [Description("File exists")] FileExists, [Description("Create temp file")] CreateTempFile };

		public enum ExistsOption
		{
			[Description("Overwrite file")]
			OverwriteFile,
			[Description("Increment file name")]
			IncrementFileName,
			[Description("Do nothing")]
			DoNothing
		};
	}
}
