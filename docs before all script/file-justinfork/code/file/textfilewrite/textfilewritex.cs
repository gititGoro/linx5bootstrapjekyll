using System;
using System.Globalization;
using System.IO;
using System.Text;
using Twenty57.Linx.Components.File.Common;

namespace Twenty57.Linx.Components.File.TextFileWrite
{
	public class TextFileWriteX
	{
		public static string WriteFile(TextFileHandle fileHandle, bool closeFileHandle, string contents, DoesNotExistOptions fileDoesNotExist, ExistOptions fileExist, TextCodepage destinationCodepage, Action<string> logger)
		{
			if (fileHandle.TryPrepareForWrite(fileDoesNotExist, fileExist))
				fileHandle.TextCodepage = destinationCodepage;

			StreamWriter writer = fileHandle.CreateStreamWriter();
			try
			{
				logger(string.Format("Writing <{0}>", contents));
				writer.Write(contents);
				writer.Flush();
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