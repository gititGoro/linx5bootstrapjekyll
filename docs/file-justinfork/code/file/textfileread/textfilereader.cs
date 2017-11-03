using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Twenty57.Linx.Components.File.Common;

namespace Twenty57.Linx.Components.File.TextFileRead
{
	public class TextFileReader : IDisposable
	{
		private StreamReader streamReader;
		private TextFileHandle fileHandle;
		private readonly FileReadOptions readType;
		private readonly TextCodepage codePage;
		private readonly long skipHeaderLines;
		private readonly long skipFooterLines;
		private Queue<NextString> queue;
		private bool firstTime;
		private bool readToOutput;
		private int count;

		public TextFileReader(TextFileHandle fileHandle, FileReadOptions readType, TextCodepage codePage, long skipHeaderLines, long skipFooterLines)
		{
			System.Diagnostics.Debug.Assert(fileHandle != null);
			this.fileHandle = fileHandle;

			this.readType = readType;
			this.codePage = codePage;
			this.skipFooterLines = skipFooterLines;
			this.skipHeaderLines = skipHeaderLines;
		}

		public string ReadComplete()
		{
			if (readType != FileReadOptions.Complete)
				throw new NotSupportedException("FileReadOption must be set to Complete.");

			try
			{
				streamReader = GetStreamReader();
				string text = streamReader.ReadToEnd();
				Log(string.Format("Read <{0}>", text.Length > 100 ? text.Substring(0, 97) + "..." : text));
				return text;
			}
			finally
			{
				CleanUp();
			}
		}

		public List<string> ReadTableNoFields()
		{
			if (readType != FileReadOptions.ListOfLines)
				throw new NotSupportedException("FileReadOption must be set to LineByLine.");
			return Lines().Select(v => v.LineContents).ToList();
		}

		public IEnumerable<NextString> Lines()
		{
			InitializeLineByLine();
			if (readToOutput)
			{
				try
				{
					for (NextString nextString = ReturnNextString(); nextString != null; nextString = ReturnNextString())
					{
						Log(string.Format("Read line {0}: {1}", nextString.LineNumber, nextString.LineContents));
						yield return nextString;
					}
				}
				finally
				{
					CleanUp();
				}
			}
		}

		public void Dispose()
		{
			CleanUp();
		}

		public delegate void LogEventHandler(string message);
		public event LogEventHandler LogEvent;

		private void Log(string message)
		{
			if (LogEvent != null)
				LogEvent(message);
		}

		private void InitializeLineByLine()
		{
			readToOutput = true;
			streamReader = GetStreamReader();
			firstTime = true;
			queue = new Queue<NextString>();
			count = 0;
		}

		private string GetNextString(ref int lineNumber, out bool done)
		{
			if (readType == FileReadOptions.Complete)
				throw new NotSupportedException("Option Complete cannot be handled in GetNextString");

			string currentLine = streamReader.ReadLine();
			lineNumber++;
			while (null != currentLine)
			{
				if (currentLine.Trim().Length == 0)
				{
					currentLine = streamReader.ReadLine();
					lineNumber++;
					continue;
				}

				done = false;
				return currentLine;
			}

			lineNumber = -1;
			done = true;
			return String.Empty;
		}

		private StreamReader GetStreamReader()
		{
			if (fileHandle.TryPrepareForRead())
				fileHandle.TextCodepage = codePage;
			return fileHandle.CreateStreamReader();
		}

		private NextString ReturnNextString()
		{
			bool done = false;
			if (firstTime)
			{
				for (int index = 0; index < skipHeaderLines; index++)
				{
					GetNextString(ref count, out done);
					if (done)
						return null;
				}

				for (int index = 0; index < skipFooterLines; index++)
				{
					string footerCacheString = GetNextString(ref count, out done);
					if (done)
						return null;
					queue.Enqueue(new NextString(footerCacheString, count));
				}

				firstTime = false;
			}

			string nextString = GetNextString(ref count, out done);
			if (done)
				return null;

			if (skipFooterLines > 0)
			{
				queue.Enqueue(new NextString(nextString, count));
				return queue.Dequeue();
			}

			return new NextString(nextString, count);
		}

		private void CleanUp()
		{
			if (fileHandle != null)
			{
				fileHandle.Close();
				streamReader = null;
			}
			queue = null;
		}


		public class NextString
		{
			public NextString(string lineContents, int lineNumber)
			{
				LineContents = lineContents;
				LineNumber = lineNumber;
			}

			public string LineContents { get; private set; }
			public int LineNumber { get; private set; }
		}
	}
}