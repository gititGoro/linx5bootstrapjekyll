using System.ComponentModel;

namespace Twenty57.Linx.Components.File.V2
{
	public static class DirectoryOperationsShared
	{
		public const string ActionPropertyName = "Action";
		public const string SourceDirectoryPropertyName = "Source directory";
		public const string TargetDirectoryPropertyName = "Target directory";
		public const string DirectoryExistsPropertyName = "Directory exists";
		public const string CreateDirectoryExistsPropertyName = "Directory exists\u200B";
		public const string DirectoryPropertyName = "Directory";
		public const string ReplaceExistingFilePropertyName = "Replace existing files";

		public enum ActionType { Copy, Move, Create, Delete, [Description("Directory exists")] DirectoryExists }

		public enum ExistsOption
		{
			[Description("Overwrite Directory")]
			OverwriteDirectory,
			[Description("Merge Directory")]
			MergeDirectory,
			[Description("Do nothing")]
			DoNothing
		};

		public enum CreateExistsOption
		{
			[Description("Do nothing")]
			DoNothing,
			[Description("Increment folder name")]
			IncrementFolderName,
			Clear,
			[Description("Throw exception")]
			ThrowException
		}
	}
}