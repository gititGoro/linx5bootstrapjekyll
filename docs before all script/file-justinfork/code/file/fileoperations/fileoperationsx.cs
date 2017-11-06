using System;
using System.IO;
using System.Linq;
using Twenty57.Linx.Components.File.Common;

namespace Twenty57.Linx.Components.File
{
	public static class FileOperationsX
	{
		public static string Execute(string sourceFilePath, bool keepFileName, string destinationPath, FileOperationsShared.ActionType action, FileOperationsShared.ExistsOption fileExistsOption, Action<string> logger)
		{
			if (string.IsNullOrEmpty(sourceFilePath))
				throw new Exception("Source file path cannot be null or empty.");

			if (action == FileOperationsShared.ActionType.Delete)
			{
				if (Directory.Exists(sourceFilePath))
				{
					logger("Delete directory " + sourceFilePath);
					Directory.Delete(sourceFilePath, true);
				}
				else if (System.IO.File.Exists(sourceFilePath))
				{
					logger("Delete file " + sourceFilePath);
					System.IO.File.Delete(sourceFilePath);
				}
				else
					logger(string.Format("File {0} does not exist. Do nothing.", sourceFilePath));
				return null;
			}
			else
			{
				if (!System.IO.File.Exists(sourceFilePath))
				{
					if (!IsValidFilePath(sourceFilePath))
						throw new Exception(string.Format("Invalid source file path: {0}.", sourceFilePath));
					throw new FileNotFoundException(string.Format("Source file path [{0}] does not exist.", sourceFilePath));
				}
				if (string.IsNullOrEmpty(destinationPath))
					throw new Exception("Destination path cannot be null or empty.");
				if (!IsValidFilePath(destinationPath))
					throw new Exception(string.Format("Invalid path: {0}.", destinationPath));

				string destinationFileNamePath;
				if (keepFileName)
				{
					destinationFileNamePath = Path.Combine(destinationPath, Path.GetFileName(sourceFilePath));
					logger("Create directory " + destinationPath);
					Directory.CreateDirectory(destinationPath);
				}
				else
				{
					destinationFileNamePath = destinationPath;
					string directoryName = Path.GetDirectoryName(destinationPath);
					logger("Create directory " + directoryName);
					Directory.CreateDirectory(directoryName);
				}
				if ((fileExistsOption == FileOperationsShared.ExistsOption.DoNothing) && (System.IO.File.Exists(destinationFileNamePath)))
				{
					logger(string.Format("File {0} exists. Do nothing.", destinationFileNamePath));
					return string.Empty;
				}
				if (fileExistsOption == FileOperationsShared.ExistsOption.IncrementFileName)
					destinationFileNamePath = FileUtil.GetIncrementalFileName(destinationFileNamePath);

				if (action == FileOperationsShared.ActionType.Copy)
				{
					logger(string.Format("Copy {0} to {1}.", sourceFilePath, destinationFileNamePath));
					System.IO.File.Copy(sourceFilePath, destinationFileNamePath, true);
				}
				else
				{
					if (System.IO.File.Exists(destinationFileNamePath))
					{
						logger("Delete file " + destinationFileNamePath);
						System.IO.File.Delete(destinationFileNamePath);
					}
					logger(string.Format("Move {0} to {1}.", sourceFilePath, destinationFileNamePath));
					System.IO.File.Move(sourceFilePath, destinationFileNamePath);
				}
				return destinationFileNamePath;
			}
		}

		public static bool FileExists(string filePath)
		{
			if (string.IsNullOrEmpty(filePath))
				throw new Exception("File path cannot be null or empty.");

			if (!IsValidFilePath(filePath))
				throw new Exception(string.Format("Invalid file path: {0}.", filePath));

			return System.IO.File.Exists(filePath);
		}

		public static string CreateTempFile()
		{
			return System.IO.Path.GetTempFileName();
		}

		private static bool IsValidFilePath(string filePath)
		{
			try
			{
				Uri uri = new Uri(filePath);
				if (!uri.IsAbsoluteUri)
					return false;
				return (uri.IsUnc) || (uri.IsFile);
			}
			catch
			{
				return false;
			}
		}
	}
}
