using System.Collections.Generic;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.Validation;
using Twenty57.Linx.Plugin.UI.Editors;

namespace Twenty57.Linx.Components.File
{
	public class RobocopyDesigner : FunctionDesigner
	{
		public RobocopyDesigner(IDesignerContext context)
				: base(context)
		{
			SetPropertyAttributes();
		}

		public RobocopyDesigner(IFunctionData functionData, IDesignerContext context)
				: base(functionData, context)
		{ }

		private Property GetProperty(string propertyName)
		{
			if (Properties.Contains(propertyName))
				return Properties[propertyName];

			Property property = null;
			switch (propertyName)
			{
				case RobocopyShared.ModePropertyName: property = new Property(propertyName, typeof(RobocopyShared.ModeOption), ValueUseOption.DesignTime, RobocopyShared.ModeOption.Copy); break;
				case RobocopyShared.SourceDirectoryPropertyName: property = new Property(propertyName, typeof(string), ValueUseOption.RuntimeRead, string.Empty); break;
				case RobocopyShared.TargetDirectoryPropertyName: property = new Property(propertyName, typeof(string), ValueUseOption.RuntimeRead, string.Empty); break;
				case RobocopyShared.CopySubdirectoriesPropertyName: property = new Property(propertyName, typeof(bool), ValueUseOption.DesignTime, true); break;
				case RobocopyShared.IncludeEmptySubdirectoriesPropertyName: property = new Property(propertyName, typeof(bool), ValueUseOption.DesignTime, false); break;
				case RobocopyShared.RestartModePropertyName: property = new Property(propertyName, typeof(bool), ValueUseOption.DesignTime, false); break;
				case RobocopyShared.BackupModePropertyName: property = new Property(propertyName, typeof(bool), ValueUseOption.DesignTime, false); break;
				case RobocopyShared.NumberOfRetriesPropertyName: property = new Property(propertyName, typeof(int), ValueUseOption.RuntimeRead, 10); break;
				case RobocopyShared.TimeBetweenRetriesPropertyName: property = new Property(propertyName, typeof(int), ValueUseOption.RuntimeRead, 10); break;

				case RobocopyShared.FilePatternPropertyName: property = new Property(propertyName, typeof(string), ValueUseOption.RuntimeRead, string.Empty); break;
				case RobocopyShared.ExcludeFilesPropertyName: property = new Property(propertyName, typeof(string), ValueUseOption.RuntimeRead, string.Empty); break;
				case RobocopyShared.ExcludeDirectoriesPropertyName: property = new Property(propertyName, typeof(string), ValueUseOption.RuntimeRead, string.Empty); break;
				case RobocopyShared.ExcludesChangedFilesPropertyName: property = new Property(propertyName, typeof(bool), ValueUseOption.DesignTime, false); break;
				case RobocopyShared.ExcludesNewerFilesPropertyName: property = new Property(propertyName, typeof(bool), ValueUseOption.DesignTime, false); break;
				case RobocopyShared.ExcludesOlderFilesPropertyName: property = new Property(propertyName, typeof(bool), ValueUseOption.DesignTime, false); break;
				case RobocopyShared.ExcludesExtraFilesAndDirectoriesPropertyName: property = new Property(propertyName, typeof(bool), ValueUseOption.DesignTime, false); break;
				case RobocopyShared.ExcludesLonelyFilesAndDirectoriesPropertyName: property = new Property(propertyName, typeof(bool), ValueUseOption.DesignTime, false); break;
				case RobocopyShared.IncludesSameFilesPropertyName: property = new Property(propertyName, typeof(bool), ValueUseOption.DesignTime, false); break;
				case RobocopyShared.IncludesTweakedFilesPropertyName: property = new Property(propertyName, typeof(bool), ValueUseOption.DesignTime, false); break;
				case RobocopyShared.MaxFileSizePropertyName: property = new Property(propertyName, typeof(int), ValueUseOption.RuntimeRead, 0); break;
				case RobocopyShared.MinFileSizePropertyName: property = new Property(propertyName, typeof(int), ValueUseOption.RuntimeRead, 0); break;
				case RobocopyShared.MaxAgePropertyName: property = new Property(propertyName, typeof(string), ValueUseOption.RuntimeRead, string.Empty); break;
				case RobocopyShared.MinAgePropertyName: property = new Property(propertyName, typeof(string), ValueUseOption.RuntimeRead, string.Empty); break;
				case RobocopyShared.MaxLastAccessDatePropertyName: property = new Property(propertyName, typeof(string), ValueUseOption.RuntimeRead, string.Empty); break;
				case RobocopyShared.MinLastAccessDatePropertyName: property = new Property(propertyName, typeof(string), ValueUseOption.RuntimeRead, string.Empty); break;

				case RobocopyShared.LogFilePropertyName: property = new Property(propertyName, typeof(string), ValueUseOption.RuntimeRead, string.Empty); break;
				case RobocopyShared.OverwriteFilePropertyName: property = new Property(propertyName, typeof(bool), ValueUseOption.DesignTime, false); break;
				case RobocopyShared.ListFilesOnlyPropertyName: property = new Property(propertyName, typeof(bool), ValueUseOption.DesignTime, false); break;
				case RobocopyShared.LogAllExtraFilesPropertyName: property = new Property(propertyName, typeof(bool), ValueUseOption.DesignTime, false); break;
				case RobocopyShared.VerbosePropertyName: property = new Property(propertyName, typeof(bool), ValueUseOption.DesignTime, false); break;
				case RobocopyShared.IncludeSourceFileTimestampsPropertyName: property = new Property(propertyName, typeof(bool), ValueUseOption.DesignTime, false); break;
				case RobocopyShared.IncludeFullPathPropertyName: property = new Property(propertyName, typeof(bool), ValueUseOption.DesignTime, false); break;
				case RobocopyShared.LogSizeAsBytesPropertyName: property = new Property(propertyName, typeof(bool), ValueUseOption.DesignTime, false); break;
				case RobocopyShared.ExcludeFileSizePropertyName: property = new Property(propertyName, typeof(bool), ValueUseOption.DesignTime, false); break;
				case RobocopyShared.ExcludeFileClassPropertyName: property = new Property(propertyName, typeof(bool), ValueUseOption.DesignTime, false); break;
				case RobocopyShared.ExcludeFileNamesPropertyName: property = new Property(propertyName, typeof(bool), ValueUseOption.DesignTime, false); break;
				case RobocopyShared.ExcludeDirectoryNamesPropertyName: property = new Property(propertyName, typeof(bool), ValueUseOption.DesignTime, false); break;
				case RobocopyShared.ExcludeProgressPropertyName: property = new Property(propertyName, typeof(bool), ValueUseOption.DesignTime, false); break;
				case RobocopyShared.IncludeETAPropertyName: property = new Property(propertyName, typeof(bool), ValueUseOption.DesignTime, false); break;
			}

			Properties.Add(property);
			return property;
		}

		protected override void InitializeProperties(IReadOnlyDictionary<string, IPropertyData> properties)
		{
			base.InitializeProperties(properties);
			SetPropertyAttributes();
		}

		private void SetPropertyAttributes()
		{
			Property modeProperty = GetProperty(RobocopyShared.ModePropertyName);
			modeProperty.Description = "The operation to be performed.";
			modeProperty.Order = 1;
			modeProperty.IsVisible = true;
			modeProperty.Category = RobocopyShared.CopyCategoryName;

			Property sourceDirectoryProperty = GetProperty(RobocopyShared.SourceDirectoryPropertyName);
			sourceDirectoryProperty.Description = "The path to the source directory.";
			sourceDirectoryProperty.Order = 2;
			sourceDirectoryProperty.Validations.Add(new RequiredValidator());
			sourceDirectoryProperty.Editor = typeof(DirectoryPathEditor);
			sourceDirectoryProperty.IsVisible = true;
			sourceDirectoryProperty.Category = RobocopyShared.CopyCategoryName;

			Property targetDirectoryProperty = GetProperty(RobocopyShared.TargetDirectoryPropertyName);
			targetDirectoryProperty.Description = "The path to the destination directory.";
			targetDirectoryProperty.Order = 3;
			targetDirectoryProperty.Validations.Add(new RequiredValidator());
			targetDirectoryProperty.Editor = typeof(DirectoryPathEditor);
			targetDirectoryProperty.IsVisible = true;
			targetDirectoryProperty.Category = RobocopyShared.CopyCategoryName;

			Property copySubdirectoriesProperty = GetProperty(RobocopyShared.CopySubdirectoriesPropertyName);
			copySubdirectoriesProperty.Description = "Copy subdirectories, but not empty ones (/s).";
			copySubdirectoriesProperty.Order = 4;
			copySubdirectoriesProperty.IsVisible = true;
			copySubdirectoriesProperty.Category = RobocopyShared.CopyCategoryName;

			Property includeEmptySubdirectoriesProperty = GetProperty(RobocopyShared.IncludeEmptySubdirectoriesPropertyName);
			includeEmptySubdirectoriesProperty.Description = "Include empty subdirectories (/e).";
			includeEmptySubdirectoriesProperty.Order = 5;
			includeEmptySubdirectoriesProperty.IsVisible = true;
			includeEmptySubdirectoriesProperty.Category = RobocopyShared.CopyCategoryName;

			Property restartModeProperty = GetProperty(RobocopyShared.RestartModePropertyName);
			restartModeProperty.Description = "Copies files in restart mode (/z).";
			restartModeProperty.Order = 6;
			restartModeProperty.IsVisible = true;
			restartModeProperty.Category = RobocopyShared.CopyCategoryName;

			Property backupModeProperty = GetProperty(RobocopyShared.BackupModePropertyName);
			backupModeProperty.Description = "Copies files in backup mode (/b).";
			backupModeProperty.Order = 7;
			backupModeProperty.IsVisible = true;
			backupModeProperty.Category = RobocopyShared.CopyCategoryName;

			Property numberOfRetriesProperty = GetProperty(RobocopyShared.NumberOfRetriesPropertyName);
			numberOfRetriesProperty.Description = "The number of retries on failed copies (/r:N).";
			numberOfRetriesProperty.Order = 8;
			numberOfRetriesProperty.IsVisible = true;
			numberOfRetriesProperty.Category = RobocopyShared.CopyCategoryName;

			Property timeBetweenRetriesProperty = GetProperty(RobocopyShared.TimeBetweenRetriesPropertyName);
			timeBetweenRetriesProperty.Description = "The wait time between retries, spefied in seconds (/w:N).";
			timeBetweenRetriesProperty.Order = 9;
			timeBetweenRetriesProperty.IsVisible = true;
			timeBetweenRetriesProperty.Category = RobocopyShared.CopyCategoryName;

			Property filePatternProperty = GetProperty(RobocopyShared.FilePatternPropertyName);
			filePatternProperty.Description = "File(s) to copy (filenames).";
			filePatternProperty.Order = 10;
			filePatternProperty.IsVisible = true;
			filePatternProperty.Category = RobocopyShared.FilterCategoryName;

			Property excludeFilesProperty = GetProperty(RobocopyShared.ExcludeFilesPropertyName);
			excludeFilesProperty.Description = "Excludes files that match the specified names or paths (/xf <FileName>[...]).";
			excludeFilesProperty.Order = 11;
			excludeFilesProperty.IsVisible = true;
			excludeFilesProperty.Category = RobocopyShared.FilterCategoryName;

			Property excludeDirectoriesProperty = GetProperty(RobocopyShared.ExcludeDirectoriesPropertyName);
			excludeDirectoriesProperty.Description = "Excludes directories that match the specified names and paths (/xd <Directory>[...]).";
			excludeDirectoriesProperty.Order = 12;
			excludeDirectoriesProperty.IsVisible = true;
			excludeDirectoriesProperty.Category = RobocopyShared.FilterCategoryName;

			Property excludesChangedFilesProperty = GetProperty(RobocopyShared.ExcludesChangedFilesPropertyName);
			excludesChangedFilesProperty.Description = "Excludes changed files (/xct).";
			excludesChangedFilesProperty.Order = 13;
			excludesChangedFilesProperty.IsVisible = true;
			excludesChangedFilesProperty.Category = RobocopyShared.FilterCategoryName;

			Property excludesNewerFilesProperty = GetProperty(RobocopyShared.ExcludesNewerFilesPropertyName);
			excludesNewerFilesProperty.Description = "Excludes newer files (/xn).";
			excludesNewerFilesProperty.Order = 14;
			excludesNewerFilesProperty.IsVisible = true;
			excludesNewerFilesProperty.Category = RobocopyShared.FilterCategoryName;

			Property excludesOlderFilesProperty = GetProperty(RobocopyShared.ExcludesOlderFilesPropertyName);
			excludesOlderFilesProperty.Description = "Excludes older files (/xo).";
			excludesOlderFilesProperty.Order = 15;
			excludesOlderFilesProperty.IsVisible = true;
			excludesOlderFilesProperty.Category = RobocopyShared.FilterCategoryName;

			Property excludesExtraFilesAndDirectoriesProperty = GetProperty(RobocopyShared.ExcludesExtraFilesAndDirectoriesPropertyName);
			excludesExtraFilesAndDirectoriesProperty.Description = "Excludes extra files and directories (/xx).";
			excludesExtraFilesAndDirectoriesProperty.Order = 16;
			excludesExtraFilesAndDirectoriesProperty.IsVisible = true;
			excludesExtraFilesAndDirectoriesProperty.Category = RobocopyShared.FilterCategoryName;

			Property excludesLonelyFilesAndDirectoriesProperty = GetProperty(RobocopyShared.ExcludesLonelyFilesAndDirectoriesPropertyName);
			excludesLonelyFilesAndDirectoriesProperty.Description = "Excludes lonely files and directories (/xl).";
			excludesLonelyFilesAndDirectoriesProperty.Order = 17;
			excludesLonelyFilesAndDirectoriesProperty.IsVisible = true;
			excludesLonelyFilesAndDirectoriesProperty.Category = RobocopyShared.FilterCategoryName;

			Property includesSameFilesProperty = GetProperty(RobocopyShared.IncludesSameFilesPropertyName);
			includesSameFilesProperty.Description = "Includes same files (/is).";
			includesSameFilesProperty.Order = 18;
			includesSameFilesProperty.IsVisible = true;
			includesSameFilesProperty.Category = RobocopyShared.FilterCategoryName;

			Property includesTweakedFilesProperty = GetProperty(RobocopyShared.IncludesTweakedFilesPropertyName);
			includesTweakedFilesProperty.Description = "Includes tweaked files (/it).";
			includesTweakedFilesProperty.Order = 19;
			includesTweakedFilesProperty.IsVisible = true;
			includesTweakedFilesProperty.Category = RobocopyShared.FilterCategoryName;

			Property maxFileSizeProperty = GetProperty(RobocopyShared.MaxFileSizePropertyName);
			maxFileSizeProperty.Description = "The maximum file size (to exclude files bigger than N bytes) (/max:<N>).";
			maxFileSizeProperty.Order = 20;
			maxFileSizeProperty.IsVisible = true;
			maxFileSizeProperty.Category = RobocopyShared.FilterCategoryName;

			Property minFileSizeProperty = GetProperty(RobocopyShared.MinFileSizePropertyName);
			minFileSizeProperty.Description = "The minimum file size (to exclude files smaller than N bytes) (/min:<N>).";
			minFileSizeProperty.Order = 21;
			minFileSizeProperty.IsVisible = true;
			minFileSizeProperty.Category = RobocopyShared.FilterCategoryName;

			Property maxAgeProperty = GetProperty(RobocopyShared.MaxAgePropertyName);
			maxAgeProperty.Description = "The maximum file age (to exclude files older than N days or date) (/maxage:<N>).";
			maxAgeProperty.Order = 22;
			maxAgeProperty.IsVisible = true;
			maxAgeProperty.Category = RobocopyShared.FilterCategoryName;

			Property minAgeProperty = GetProperty(RobocopyShared.MinAgePropertyName);
			minAgeProperty.Description = "The minimum file age (exclude files newer than N days or date) (/minage:<N>).";
			minAgeProperty.Order = 23;
			minAgeProperty.IsVisible = true;
			minAgeProperty.Category = RobocopyShared.FilterCategoryName;

			Property maxLastAccessDateProperty = GetProperty(RobocopyShared.MaxLastAccessDatePropertyName);
			maxLastAccessDateProperty.Description = "The maximum last access date (excludes files unused since N) (/maxlad:<N>).";
			maxLastAccessDateProperty.Order = 24;
			maxLastAccessDateProperty.IsVisible = true;
			maxLastAccessDateProperty.Category = RobocopyShared.FilterCategoryName;

			Property minLastAccessDateProperty = GetProperty(RobocopyShared.MinLastAccessDatePropertyName);
			minLastAccessDateProperty.Description = "The minimum last access date (excludes files used since N) (/minlad:<N>).";
			minLastAccessDateProperty.Order = 25;
			minLastAccessDateProperty.IsVisible = true;
			minLastAccessDateProperty.Category = RobocopyShared.FilterCategoryName;

			Property logFileProperty = GetProperty(RobocopyShared.LogFilePropertyName);
			logFileProperty.Description = "Output status to Log file (append to existing log) (/log+:filename).";
			logFileProperty.Order = 26;
			logFileProperty.IsVisible = true;
			logFileProperty.Category = RobocopyShared.LoggingCategoryName;

			Property overwriteFileProperty = GetProperty(RobocopyShared.OverwriteFilePropertyName);
			overwriteFileProperty.Description = "Output status to Log file (overwrite existing log) (/log:filename).";
			overwriteFileProperty.Order = 27;
			overwriteFileProperty.IsVisible = true;
			overwriteFileProperty.Category = RobocopyShared.LoggingCategoryName;

			Property listFilesOnlyProperty = GetProperty(RobocopyShared.ListFilesOnlyPropertyName);
			listFilesOnlyProperty.Description = "List only - don't copy, time stamp or delete any files (/l).";
			listFilesOnlyProperty.Order = 28;
			listFilesOnlyProperty.IsVisible = true;
			listFilesOnlyProperty.Category = RobocopyShared.LoggingCategoryName;

			Property logAllExtraFilesProperty = GetProperty(RobocopyShared.LogAllExtraFilesPropertyName);
			logAllExtraFilesProperty.Description = "Reports all extra files, not just those that are selected (/x).";
			logAllExtraFilesProperty.Order = 29;
			logAllExtraFilesProperty.IsVisible = true;
			logAllExtraFilesProperty.Category = RobocopyShared.LoggingCategoryName;

			Property verboseProperty = GetProperty(RobocopyShared.VerbosePropertyName);
			verboseProperty.Description = "Produces verbose output, and shows all skipped files (/v).";
			verboseProperty.Order = 30;
			verboseProperty.IsVisible = true;
			verboseProperty.Category = RobocopyShared.LoggingCategoryName;

			Property includeSourceFileTimestampsProperty = GetProperty(RobocopyShared.IncludeSourceFileTimestampsPropertyName);
			includeSourceFileTimestampsProperty.Description = "Includes source file time stamps in the output (/ts).";
			includeSourceFileTimestampsProperty.Order = 31;
			includeSourceFileTimestampsProperty.Category = RobocopyShared.LoggingCategoryName;

			Property includeFullPathProperty = GetProperty(RobocopyShared.IncludeFullPathPropertyName);
			includeFullPathProperty.Description = "Includes the full path names of the files in the output (/fp).";
			includeFullPathProperty.Order = 32;
			includeFullPathProperty.IsVisible = true;
			includeFullPathProperty.Category = RobocopyShared.LoggingCategoryName;

			Property logSizeAsBytesProperty = GetProperty(RobocopyShared.LogSizeAsBytesPropertyName);
			logSizeAsBytesProperty.Description = "Prints sizes, as bytes (/bytes).";
			logSizeAsBytesProperty.Order = 33;
			logSizeAsBytesProperty.IsVisible = true;
			logSizeAsBytesProperty.Category = RobocopyShared.LoggingCategoryName;

			Property excludeFileSizeProperty = GetProperty(RobocopyShared.ExcludeFileSizePropertyName);
			excludeFileSizeProperty.Description = "Specifies that file sizes are not to be logged (/ns).";
			excludeFileSizeProperty.Order = 34;
			excludeFileSizeProperty.IsVisible = true;
			excludeFileSizeProperty.Category = RobocopyShared.LoggingCategoryName;

			Property excludeFileClassProperty = GetProperty(RobocopyShared.ExcludeFileClassPropertyName);
			excludeFileClassProperty.Description = "Specifies that file classes are not to be logged (/nc).";
			excludeFileClassProperty.Order = 35;
			excludeFileClassProperty.IsVisible = true;
			excludeFileClassProperty.Category = RobocopyShared.LoggingCategoryName;

			Property excludeFileNamesProperty = GetProperty(RobocopyShared.ExcludeFileNamesPropertyName);
			excludeFileNamesProperty.Description = "Specifies that file names are not to be logged (/nfl).";
			excludeFileNamesProperty.Order = 36;
			excludeFileNamesProperty.IsVisible = true;
			excludeFileNamesProperty.Category = RobocopyShared.LoggingCategoryName;

			Property excludeDirectoryNamesProperty = GetProperty(RobocopyShared.ExcludeDirectoryNamesPropertyName);
			excludeDirectoryNamesProperty.Description = "Specifies that directory names are not to be logged (/ndl).";
			excludeDirectoryNamesProperty.Order = 37;
			excludeDirectoryNamesProperty.IsVisible = true;
			excludeDirectoryNamesProperty.Category = RobocopyShared.LoggingCategoryName;

			Property excludeProgressProperty = GetProperty(RobocopyShared.ExcludeProgressPropertyName);
			excludeProgressProperty.Description = "Specifies that the progress of the copying operation will not be displayed (/np).";
			excludeProgressProperty.Order = 38;
			excludeProgressProperty.IsVisible = true;
			excludeProgressProperty.Category = RobocopyShared.LoggingCategoryName;

			Property includeETAProperty = GetProperty(RobocopyShared.IncludeETAPropertyName);
			includeETAProperty.Description = "Includes the estimated time of arrival (ETA) of the copied files (/eta).";
			includeETAProperty.Order = 39;
			includeETAProperty.IsVisible = true;
			includeETAProperty.Category = RobocopyShared.LoggingCategoryName;
		}
	}
}
