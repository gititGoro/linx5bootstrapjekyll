using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Twenty57.Linx.Components.File.Common;

namespace Twenty57.Linx.Components.File
{
	public static class BinaryFileReadX
	{
		public static List<byte> ReadContents(BinaryFileHandle fileHandle, Action<string> logger)
		{
			fileHandle.TryPrepareForRead();

			try
			{
				FileStream fileStream = fileHandle.GetFileStream();
				byte[] buffer = new byte[fileStream.Length];
				fileStream.Read(buffer, 0, Convert.ToInt32(fileStream.Length));
				logger(string.Format("Read {0} bytes.", buffer.Length));
				return buffer.ToList();
			}
			finally
			{
				fileHandle.Close();
			}
		}
	}
}