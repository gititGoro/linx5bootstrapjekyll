using System.ComponentModel;

namespace Twenty57.Linx.Components.File
{
	public static class RobocopyShared
	{
		public const string SourceDirectoryPropertyName = "Source directory";
		public const string TargetDirectoryPropertyName = "Target directory";
		public const string CopySubdirectoriesPropertyName = "Copy subdirectories";
		public const string IncludeEmptySubdirectoriesPropertyName = "Include empty subdirectories";
		public const string ModePropertyName = "Mode";
		public const string RestartModePropertyName = "Restart mode";
		public const string BackupModePropertyName = "Backup mode";
		public const string NumberOfRetriesPropertyName = "Number of retries";
		public const string TimeBetweenRetriesPropertyName = "Time between retries";
		public const string FilePatternPropertyName = "File pattern";
		public const string ExcludeFilesPropertyName = "Exclude files";
		public const string ExcludeDirectoriesPropertyName = "Exclude directories";
		public const string ExcludesChangedFilesPropertyName = "Excludes changed files";
		public const string ExcludesNewerFilesPropertyName = "Excludes newer files";
		public const string ExcludesOlderFilesPropertyName = "Excludes older files";
		public const string ExcludesExtraFilesAndDirectoriesPropertyName = "Excludes extra files and directories";
		public const string ExcludesLonelyFilesAndDirectoriesPropertyName = "Excludes lonely files and directories";
		public const string IncludesSameFilesPropertyName = "Includes same files";
		public const string IncludesTweakedFilesPropertyName = "Includes tweaked files";
		public const string MaxFileSizePropertyName = "Max file size";
		public const string MinFileSizePropertyName = "Min file size";
		public const string MaxAgePropertyName = "Max age";
		public const string MinAgePropertyName = "Min age";
		public const string MaxLastAccessDatePropertyName = "Max last access date";
		public const string MinLastAccessDatePropertyName = "Min last access date";
		public const string LogFilePropertyName = "Log file";
		public const string OverwriteFilePropertyName = "Overwrite file";
		public const string ListFilesOnlyPropertyName = "List files only";
		public const string LogAllExtraFilesPropertyName = "Log all extra files";
		public const string VerbosePropertyName = "Verbose";
		public const string IncludeSourceFileTimestampsPropertyName = "Include source file timestamps";
		public const string IncludeFullPathPropertyName = "Include full path";
		public const string LogSizeAsBytesPropertyName = "Log size as bytes";
		public const string ExcludeFileSizePropertyName = "Exclude file size";
		public const string ExcludeFileClassPropertyName = "Exclude file class";
		public const string ExcludeFileNamesPropertyName = "Exclude file names";
		public const string ExcludeDirectoryNamesPropertyName = "Exclude directory names";
		public const string ExcludeProgressPropertyName = "Exclude progress";
		public const string IncludeETAPropertyName = "Include ETA";

		public const string CopyCategoryName = "Copy";
		public const string FilterCategoryName = "Filter";
		public const string LoggingCategoryName = "Logging";

		public enum ModeOption
		{
			Copy,
			Mirror,
			[Description("Move files")]
			MoveFiles,
			[Description("Move files and directories")]
			MoveFilesAndDirs
		}
	}
}
