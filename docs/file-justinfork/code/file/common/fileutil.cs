using System;
using System.IO;

namespace Twenty57.Linx.Components.File.Common
{
	internal static class FileUtil
	{
		internal static string GetIncrementalFileName(this string filePath)
		{
			string directory = Path.GetDirectoryName(filePath);
			string fileName = Path.GetFileNameWithoutExtension(filePath);
			string extension = Path.GetExtension(filePath);

			string newFilePath = filePath;
			int index = 0;
			while (System.IO.File.Exists(newFilePath))
			{
				newFilePath = String.Format("{0}{1}{2}_{3}{4}", directory, Path.DirectorySeparatorChar, fileName, ++index, extension);
			}

			return newFilePath;
		}

		internal static string GetIncrementalDirectoryName(this string directory)
		{
			string parentDirectory = Directory.GetParent(directory).FullName;
			string directoryName = Path.GetFileName(directory);

			string newDirectory = directory;
			int index = 0;
			while (Directory.Exists(newDirectory))
				newDirectory = Path.Combine(parentDirectory, string.Format("{0}_{1}", directoryName, ++index));

			return newDirectory;
		}
	}
}
