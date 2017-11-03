using System.IO;

namespace Twenty57.Linx.Components.File.Tests
{
	public static class FileHelpers
	{
		public static void ForceDeleteDirectory(string path)
		{
			if (!Directory.Exists(path))
				return;

			var directory = new DirectoryInfo(path) { Attributes = FileAttributes.Normal };
			foreach (FileSystemInfo info in directory.GetFileSystemInfos("*", SearchOption.AllDirectories))
				info.Attributes = FileAttributes.Normal;

			directory.Delete(true);
		}

		public static bool IsFileLocked(string filePath)
		{
			if (!System.IO.File.Exists(filePath))
				return false;

			FileStream stream = null;
			try
			{
				stream = System.IO.File.Open(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None);
			}
			catch (IOException)
			{
				return true;
			}
			finally
			{
				if (stream != null)
					stream.Close();
			}
			return false;
		}
	}
}
