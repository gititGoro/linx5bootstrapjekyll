using System;
using System.ComponentModel;
using System.Globalization;
using System.IO;

namespace Twenty57.Linx.Components.File.Common
{
	[TypeConverter(typeof(FileHandleTypeConverter))]
	public class FileHandle : IDisposable
	{
		private FileMode fileMode;
		private System.IO.FileAccess fileAccess;
		private bool isPrepared = false;
		private FileStream fileStream = null;

		public FileHandle(string filePath)
		{
			FilePath = filePath;
		}

		internal string FilePath { get; private set; }

		public bool TryPrepareForRead()
		{
			if (isPrepared)
				return false;

			if (!System.IO.File.Exists(FilePath))
				throw new FileNotFoundException(String.Format("File [{0}] does not exist.", FilePath));

			fileMode = FileMode.Open;
			fileAccess = System.IO.FileAccess.Read;
			isPrepared = true;
			return true;
		}

		public bool TryPrepareForWrite(DoesNotExistOptions fileDoesNotExist, ExistOptions fileExist)
		{
			if (isPrepared)
				return false;

			string newFilePath;
			GetWriteOptions(FilePath, fileDoesNotExist, fileExist, out newFilePath, out fileMode);
			FilePath = newFilePath;
			fileAccess = System.IO.FileAccess.Write;
			isPrepared = true;
			return true;
		}

		public FileStream GetFileStream()
		{
			if (fileStream == null)
			{
				if (!isPrepared)
					throw new Exception("FileHandle has not been prepared; call TryPrepareForRead() or TryPrepareForWrite() first.");
				Log(string.Format("Opening file {0}{1}\tFile mode: {2}{1}\tFile access: {3}", FilePath, Environment.NewLine, fileMode, fileAccess));
				fileStream = System.IO.File.Open(FilePath, fileMode, fileAccess, FileShare.ReadWrite);
				Log("Opened file.");
			}
			return fileStream;
		}

		public void Close()
		{
			if (fileStream != null)
			{
				try
				{
					fileStream.Flush();
					fileStream.Close();
					Log("Closed file.");
				}
				catch { }
			}
			fileStream = null;
		}

		public void Dispose()
		{
			Close();
		}

		public override string ToString()
		{
			return FilePath;
		}

		public delegate void LogEventHander(string message);
		public event LogEventHander LogEvent;

		protected void Log(string message)
		{
			if (LogEvent != null)
				LogEvent(message);
		}

		private void GetWriteOptions(string filePath, DoesNotExistOptions fileDoesNotExist, ExistOptions fileExist, out string newFilePath, out FileMode fileMode)
		{
			newFilePath = filePath;

			if (System.IO.File.Exists(filePath))
			{
				Log(string.Format("File at {0} exists.", filePath));
				switch (fileExist)
				{
					case ExistOptions.AppendData:
						fileMode = FileMode.Append;
						break;
					case ExistOptions.IncrementFileName:
						fileMode = FileMode.CreateNew;
						newFilePath = filePath.GetIncrementalFileName();
						break;
					case ExistOptions.OverwriteFile:
						fileMode = FileMode.Truncate;
						break;
					case ExistOptions.ThrowException:
						throw new IOException(String.Format("File [{0}] already exists.", filePath));
					default:
						throw new Exception(String.Format("Invalid ExistOptions [{0}] specified.", fileExist));
				}
			}
			else
			{
				Log(string.Format("File at {0} does not exist.", filePath));
				switch (fileDoesNotExist)
				{
					case DoesNotExistOptions.CreateFile:
						string directoryName = Path.GetDirectoryName(newFilePath);
						if (!Directory.Exists(directoryName))
						{
							Directory.CreateDirectory(directoryName);
							Log(string.Format("Created directory {0}.", directoryName));
						}
						fileMode = FileMode.CreateNew;
						break;
					case DoesNotExistOptions.ThrowException:
						throw new FileNotFoundException(String.Format("File [{0}] does not exist.", filePath));
					default:
						throw new Exception(String.Format("Invalid ExistOptions [{0}] specified.", fileDoesNotExist));
				}
			}
		}
	}


	public class FileHandleTypeConverter : TypeConverter
	{
		public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
		{
			return sourceType == typeof(string);
		}

		public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
		{
			return new FileHandle((string)value);
		}
	}
}
