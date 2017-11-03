using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Twenty57.Linx.Components.File.Common;

namespace Twenty57.Linx.Components.File
{
	public class BinaryFileWriteX
	{
		public static string WriteFile(BinaryFileHandle fileHandle, bool closeFileHandle, IEnumerable<byte> contents, DoesNotExistOptions fileDoesNotExist, ExistOptions fileExist, Action<string> logger)
		{
			fileHandle.TryPrepareForWrite(fileDoesNotExist, fileExist);

			try
			{
				byte[] arrayContents = contents.ToArray();
				var fileStream = fileHandle.GetFileStream();
				logger(string.Format("Writing {0} bytes.", arrayContents.Length));
				fileStream.Write(arrayContents, 0, arrayContents.Length);
			}
			finally
			{
				if (closeFileHandle)
					fileHandle.Close();
			}

			return fileHandle.FilePath;
		}
	}
}