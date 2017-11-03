using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;

namespace Twenty57.Linx.Components.File
{
	public static class FileListX
	{
		public static IEnumerable<FileInfo> GetFiles(bool includeSubfolders, string filePath, string searchPattern)
		{
			var searchOption = (includeSubfolders) ? SearchOption.AllDirectories : SearchOption.TopDirectoryOnly;
			FileInfo[] files;

			if (string.IsNullOrWhiteSpace(searchPattern))
				files = new DirectoryInfo(filePath).GetFiles(@"*", searchOption);
			else
				files = searchPattern.Split(';').SelectMany(p => new DirectoryInfo(filePath).GetFiles(
					String.IsNullOrEmpty(searchPattern) ? "*" : p.Trim(),
					searchOption)).Distinct().ToArray();
			/* 
			 * Ensure correct _files results - Directory.GetFiles() in Getfiles is not adequately specific
			 * 
			 * Check for exactly 3 chars in extension
			 * Directory.GetFiles returns files with extensions longer than 3
			 * when the first 3 are found - see msdn documentation
			 * e.g. abc.txt will also return abc.txt1 and abc.txtabcdef
			 */
			bool patternExtensionIs3CharLong = (searchPattern.Length - searchPattern.LastIndexOf(".") - 1 == 3);
			bool doesNotEndWithWildcard = !searchPattern.EndsWith("*");
			return (patternExtensionIs3CharLong && doesNotEndWithWildcard) ?
				files.Where(file => file.Extension.Length == 4) :
				files;
		}
	}
}
