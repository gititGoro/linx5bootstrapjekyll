using System;
using System.IO;
using System.Linq;
using Twenty57.Linx.Components.File.Common;

namespace Twenty57.Linx.Components.File.V2
{
	public class DirectoryOperationsX
	{
		public void CopyOrMove(
			DirectoryOperationsShared.ActionType action,
			string sourceDirectoryPath,
			string targetDirectoryPath,
			DirectoryOperationsShared.ExistsOption existsOption,
			bool replaceExistingFile)
		{
			if (string.IsNullOrEmpty(sourceDirectoryPath))
				throw new Exception("Source directory path cannot be null or empty.");

			if (string.IsNullOrEmpty(targetDirectoryPath))
				throw new Exception("Target directory path cannot be null or empty.");

			if (!Directory.Exists(sourceDirectoryPath))
				throw new DirectoryNotFoundException(string.Format("Source directory [{0}] does not exist.", sourceDirectoryPath));

			sourceDirectoryPath = sourceDirectoryPath.TrimEnd(Path.DirectorySeparatorChar);
			targetDirectoryPath = targetDirectoryPath.TrimEnd(Path.DirectorySeparatorChar);

			PerformAction((action == DirectoryOperationsShared.ActionType.Copy), sourceDirectoryPath, targetDirectoryPath, existsOption, replaceExistingFile);
		}

		public string CreateDirectory(string directoryPath, DirectoryOperationsShared.CreateExistsOption existsOption)
		{
			if (string.IsNullOrEmpty(directoryPath))
				throw new Exception("Directory path cannot be null or empty.");

			switch (existsOption)
			{
				case DirectoryOperationsShared.CreateExistsOption.IncrementFolderName:
					directoryPath = directoryPath.GetIncrementalDirectoryName(); break;
				case DirectoryOperationsShared.CreateExistsOption.Clear:
					{
						if (Directory.Exists(directoryPath))
						{
							Log("Delete directory " + directoryPath);
							Directory.Delete(directoryPath, true);
						}
						break;
					}
				case DirectoryOperationsShared.CreateExistsOption.ThrowException:
					{
						if (Directory.Exists(directoryPath))
							throw new Exception(string.Format("Directory [{0}] already exists.", directoryPath));
						break;
					}
			}

			Log("Create directory " + directoryPath);
			Directory.CreateDirectory(directoryPath);
			return directoryPath;
		}

		public void DeleteDirectory(string directoryPath)
		{
			if (string.IsNullOrEmpty(directoryPath))
				throw new ArgumentNullException(nameof(directoryPath));

			if (!Directory.Exists(directoryPath))
				throw new DirectoryNotFoundException($"Directory [{directoryPath}] does not exist.");

			Log("Delete directory " + directoryPath);
			try
			{
				Directory.Delete(directoryPath, true);
			}
			catch (UnauthorizedAccessException)
			{
				Log("Delete failed.  Removing read-only attributes");
				RemoveReadOnlyAttribute(directoryPath);
				Directory.Delete(directoryPath, true);
			}
		}

		public bool DirectoryExists(string directoryPath)
		{
			if (string.IsNullOrEmpty(directoryPath))
				throw new Exception("Directory path cannot be null or empty.");

			bool exists = Directory.Exists(directoryPath);
			Log(string.Format(exists ? "Directory {0} exists." : "Directory {0} does not exist.", directoryPath));
			return exists;
		}

		public delegate void LogEventHandler(string message);
		public event LogEventHandler LogEvent;

		private void Log(string message)
		{
			if (LogEvent != null)
				LogEvent(message);
		}

		private void PerformAction(bool isCopyAction, string sourceDirectoryPath, string targetDirectoryPath,
			DirectoryOperationsShared.ExistsOption existsOption, bool replaceExistingFile)
		{
			if (IsValidDoNothingOption(isCopyAction, sourceDirectoryPath, targetDirectoryPath, existsOption))
				return;

			ProcessDirectory(isCopyAction, sourceDirectoryPath, targetDirectoryPath, existsOption, replaceExistingFile, isCopyAction);
		}

		private bool IsValidDoNothingOption(bool isCopyAction, string sourceDirectoryPath, string targetDirectoryPath, DirectoryOperationsShared.ExistsOption existsOption)
		{
			string leastFolderName = Path.GetFileNameWithoutExtension(sourceDirectoryPath);
			string targetSubDirectoryPath = (isCopyAction) ? Path.Combine(targetDirectoryPath, leastFolderName) : targetDirectoryPath;

			if (Directory.Exists(targetSubDirectoryPath) && existsOption == DirectoryOperationsShared.ExistsOption.DoNothing)
			{
				Log(string.Format("Directory {0} exists. Do nothing.", targetSubDirectoryPath));
				return true;
			}

			return false;
		}

		private void ProcessDirectory(bool isCopyAction, string sourceDirectoryPath, string targetDirectoryPath,
			DirectoryOperationsShared.ExistsOption existsOption, bool replaceExistingFile, bool createSubTargetDirectory)
		{
			var leastFolderName = Path.GetFileNameWithoutExtension(sourceDirectoryPath);
			var currentTargetDirectoryPath = (createSubTargetDirectory) ? Path.Combine(targetDirectoryPath, leastFolderName) : targetDirectoryPath;

			Log("Create directory " + currentTargetDirectoryPath);
			Directory.CreateDirectory(currentTargetDirectoryPath);

			foreach (string nextSourceDirectory in Directory.EnumerateDirectories(sourceDirectoryPath))
				ProcessDirectory(isCopyAction, nextSourceDirectory, currentTargetDirectoryPath, existsOption, replaceExistingFile, true);

			ProcessFiles(isCopyAction, sourceDirectoryPath, currentTargetDirectoryPath, existsOption, replaceExistingFile);

			if (!Directory.GetFileSystemEntries(sourceDirectoryPath, "*", SearchOption.AllDirectories).Any() && !isCopyAction)
			{
				Log("Delete directory " + sourceDirectoryPath);
				Directory.Delete(sourceDirectoryPath);
			}
		}

		private void ProcessFiles(bool isCopyAction, string sourceDirectoryPath, string targetDirectoryPath,
			DirectoryOperationsShared.ExistsOption existsOption, bool replaceExistingFile)
		{
			foreach (string nextFilePath in Directory.EnumerateFiles(sourceDirectoryPath, "*", SearchOption.TopDirectoryOnly))
			{
				var fileName = Path.GetFileName(nextFilePath);
				var destinationPath = Path.Combine(targetDirectoryPath, fileName);

				ProcessFile(isCopyAction, nextFilePath, destinationPath, existsOption, replaceExistingFile);
			}
		}

		private void ProcessFile(bool isCopyAction, string sourceFilePath, string targetFilePath,
			DirectoryOperationsShared.ExistsOption existsOption, bool replaceExistingFile)
		{
			var destinationFileExist = System.IO.File.Exists(targetFilePath);

			if (destinationFileExist
				&& ((existsOption == DirectoryOperationsShared.ExistsOption.MergeDirectory && !replaceExistingFile)
					|| (existsOption == DirectoryOperationsShared.ExistsOption.DoNothing)))
			{
				Log(string.Format("File {0} exists. Skip.", targetFilePath));
				return;
			}

			if (destinationFileExist
				&& ((existsOption == DirectoryOperationsShared.ExistsOption.MergeDirectory && replaceExistingFile)
					|| (existsOption == DirectoryOperationsShared.ExistsOption.OverwriteDirectory)))
			{
				Log("Delete file " + targetFilePath);
				System.IO.File.Delete(targetFilePath);
			}

			if (isCopyAction)
			{
				Log(string.Format("Copy {0} to {1}", sourceFilePath, targetFilePath));
				System.IO.File.Copy(sourceFilePath, targetFilePath);
			}
			else
			{
				Log(string.Format("Move {0} to {1}", sourceFilePath, targetFilePath));
				System.IO.File.Move(sourceFilePath, targetFilePath);
			}
		}

		private static void RemoveReadOnlyAttribute(string directoryPath)
		{
			var allFileNames = Directory.GetFiles(directoryPath, "*.*", SearchOption.AllDirectories);
			foreach (string filename in allFileNames)
			{
				FileAttributes attributes = System.IO.File.GetAttributes(filename);
				System.IO.File.SetAttributes(filename, attributes & ~FileAttributes.ReadOnly);
			}
		}
	}
}
