using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using Twenty57.Linx.Components.File.Common;

namespace Twenty57.Linx.Components.File.FileOpen
{
	public class FileOpenX : IEnumerable<FileHandle>
	{
		public FileOpenX(string filePath, bool isText, TextCodepage codepage, DoesNotExistOptions fileDoesNotExist, ExistOptions fileExists, Action<string> logger)
		{
			FileHandle = isText ? (FileHandle)new TextFileHandle(filePath, codepage) : (FileHandle)new BinaryFileHandle(filePath);
			FileHandle.LogEvent += message => logger(message);
			FileHandle.TryPrepareForWrite(fileDoesNotExist, fileExists);
		}

		public FileHandle FileHandle { get; private set; }

		public string FilePath { get { return FileHandle.FilePath; } }

		public IEnumerator<FileHandle> GetEnumerator()
		{
			return new SingleEnumerator<FileHandle>(FileHandle);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}


		private class SingleEnumerator<T> : IEnumerator<T> where T : IDisposable
		{
			private T item;
			private bool done = false;

			public SingleEnumerator(T item)
			{
				this.item = item;
			}

			public void Reset()
			{
				done = false;
			}

			public bool MoveNext()
			{
				if (done)
					return false;
				done = true;
				return true;
			}

			public T Current
			{
				get { return item; }
			}

			object IEnumerator.Current
			{
				get { return item; }
			}

			public void Dispose()
			{
				item.Dispose();
			}
		}
	}
}
