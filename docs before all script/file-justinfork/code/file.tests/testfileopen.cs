using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Twenty57.Linx.Components.File.Common;
using Twenty57.Linx.Components.File.FileOpen;
using Twenty57.Linx.Plugin.Common.CodeGeneration;
using Twenty57.Linx.Plugin.TestKit;

namespace Twenty57.Linx.Components.File.Tests
{
	[TestFixture]
	public class TestFileOpen
	{
		public enum WriteMethod { UseStreamWriter, AppendProperty, AppendLineProperty };

		private string defaultFolderPath;
		private string defaultFilePath;

		[OneTimeSetUp]
		public void SetupFixture()
		{
			defaultFolderPath = Path.Combine(Path.GetTempPath(), "TestFileOpen");
			defaultFilePath = Path.Combine(defaultFolderPath, "textfile.txt");
		}

		[TearDown]
		public void TearDown()
		{
			try
			{
				if (System.IO.File.Exists(defaultFilePath))
					System.IO.File.Delete(defaultFilePath);
			}
			catch (FileNotFoundException) { }

			FileHelpers.ForceDeleteDirectory(defaultFolderPath);
		}

		[Test]
		public void TestFileDoesNotExistAndCreateFile([Values(true, false)] bool textFile)
		{
			Assert.IsFalse(System.IO.File.Exists(defaultFilePath));
			var result = Execute(defaultFilePath, DoesNotExistOptions.CreateFile, ExistOptions.OverwriteFile, isText: textFile);

			Assert.AreEqual(defaultFilePath, result.Value);
			Assert.IsFalse(System.IO.File.Exists(defaultFilePath));
			AssertExecutionPath(defaultFilePath, result.ExecutionPathResult);
			Assert.IsTrue(System.IO.File.Exists(defaultFilePath));
		}

		[Test]
		public void TestFileDoesNotExistAndThrowException([Values(true, false)] bool textFile)
		{
			Assert.IsFalse(System.IO.File.Exists(defaultFilePath));
			try
			{
				Execute(defaultFilePath, DoesNotExistOptions.ThrowException, ExistOptions.OverwriteFile, isText: textFile);
			}
			catch (Exception exception)
			{
				Assert.AreEqual(string.Format("File [{0}] does not exist.\r\nSee Code and Parameter properties for more information.", defaultFilePath), exception.Message);
				Assert.IsFalse(System.IO.File.Exists(defaultFilePath));
				return;
			}
			Assert.Fail("An exception was expected.");
		}

		[Test]
		public void TestFileDoesExistAndAppendData([Values(WriteMethod.UseStreamWriter, WriteMethod.AppendProperty, WriteMethod.AppendLineProperty)] WriteMethod writeMethod)
		{
			Directory.CreateDirectory(defaultFolderPath);
			System.IO.File.WriteAllText(defaultFilePath, "!@#", Encoding.UTF7);

			var result = Execute(defaultFilePath, DoesNotExistOptions.CreateFile, ExistOptions.AppendData, codepage: TextCodepage.UTF7);

			Assert.AreEqual(defaultFilePath, result.Value);
			AssertExecutionPath(defaultFilePath, result.ExecutionPathResult);
			string expectedOutput = Write("↨ₐᾗ", result.ExecutionPathResult, writeMethod);

			Assert.AreEqual("!@#" + expectedOutput, System.IO.File.ReadAllText(defaultFilePath, Encoding.UTF7));
		}

		[Test]
		public void TestFileDoesExistAndOverwriteFile([Values(WriteMethod.UseStreamWriter, WriteMethod.AppendProperty, WriteMethod.AppendLineProperty)] WriteMethod writeMethod)
		{
			Directory.CreateDirectory(defaultFolderPath);
			System.IO.File.WriteAllText(defaultFilePath, "!@#", Encoding.UTF7);

			var result = Execute(defaultFilePath, DoesNotExistOptions.CreateFile, ExistOptions.OverwriteFile, codepage: TextCodepage.UTF7);

			Assert.AreEqual(defaultFilePath, result.Value);
			AssertExecutionPath(defaultFilePath, result.ExecutionPathResult);
			string expectedOutput = Write("↨ₐᾗ", result.ExecutionPathResult, writeMethod);

			Assert.AreEqual(expectedOutput, System.IO.File.ReadAllText(defaultFilePath, Encoding.UTF7));
		}

		[Test]
		public void TestBytesAppendProperty()
		{
			Directory.CreateDirectory(defaultFolderPath);
			System.IO.File.WriteAllBytes(defaultFilePath, new byte[] { 1, 2, 3 });

			var result = Execute(defaultFilePath, DoesNotExistOptions.CreateFile, ExistOptions.AppendData, isText: false);

			Assert.AreEqual(defaultFilePath, result.Value);
			AssertExecutionPath(defaultFilePath, result.ExecutionPathResult);
			foreach (NextResult next in result.ExecutionPathResult)
				next.Value.FileHandle.Append = new List<byte> { 4, 5, 6 };

			Assert.AreEqual(new byte[] { 1, 2, 3, 4, 5, 6 }, System.IO.File.ReadAllBytes(defaultFilePath));
		}

		[Test]
		public void TestFileDoesExistAndIncrementFilename()
		{
			Assert.IsFalse(System.IO.File.Exists(defaultFilePath));
			var result = Execute(defaultFilePath, DoesNotExistOptions.CreateFile, ExistOptions.IncrementFileName);
			Assert.AreEqual(defaultFilePath, result.Value);
			AssertExecutionPath(defaultFilePath, result.ExecutionPathResult);
			Assert.IsTrue(System.IO.File.Exists(defaultFilePath));

			string expectedFilePath = Path.Combine(defaultFolderPath,
				string.Format("{0}_1{1}", Path.GetFileNameWithoutExtension(defaultFilePath), Path.GetExtension(defaultFilePath)));

			result = Execute(defaultFilePath, DoesNotExistOptions.CreateFile, ExistOptions.IncrementFileName);
			Assert.AreEqual(expectedFilePath, result.Value);
			AssertExecutionPath(expectedFilePath, result.ExecutionPathResult);
			Assert.IsTrue(System.IO.File.Exists(expectedFilePath));

			expectedFilePath = Path.Combine(defaultFolderPath,
				string.Format("{0}_2{1}", Path.GetFileNameWithoutExtension(defaultFilePath), Path.GetExtension(defaultFilePath)));

			result = Execute(defaultFilePath, DoesNotExistOptions.CreateFile, ExistOptions.IncrementFileName);
			Assert.AreEqual(expectedFilePath, result.Value);
			AssertExecutionPath(expectedFilePath, result.ExecutionPathResult);
			Assert.IsTrue(System.IO.File.Exists(expectedFilePath));
		}

		[Test]
		public void TestFileDoesExistAndThrowException()
		{
			Directory.CreateDirectory(defaultFolderPath);
			System.IO.File.Create(defaultFilePath).Close();

			try
			{
				Execute(defaultFilePath, DoesNotExistOptions.CreateFile, ExistOptions.ThrowException);
			}
			catch (Exception exception)
			{
				Assert.AreEqual(string.Format("File [{0}] already exists.\r\nSee Code and Parameter properties for more information.", defaultFilePath), exception.Message);
				return;
			}
			Assert.Fail("An exception was expected.");
		}


		private void AssertExecutionPath(string filePath, IEnumerable<NextResult> executionPath)
		{
			Assert.IsFalse(FileHelpers.IsFileLocked(filePath), "File must not open before the execution path is active.");
			int resultCount = 0;
			foreach (NextResult result in executionPath)
			{
				result.Value.FileHandle.GetFileStream();
				Assert.IsTrue(FileHelpers.IsFileLocked(filePath));
				result.Value.FileHandle.GetFileStream();
				Assert.IsTrue(FileHelpers.IsFileLocked(filePath));
				Assert.AreEqual(filePath, result.Value.FilePath);
				resultCount++;
			}
			Assert.AreEqual(1, resultCount, "Execution path must run only once.");
			Assert.IsFalse(FileHelpers.IsFileLocked(filePath), "File must immediately close after execution path.");
		}

		private string Write(string content, IEnumerable<NextResult> executionPath, WriteMethod writeMethod = WriteMethod.UseStreamWriter)
		{
			foreach (NextResult next in executionPath)
			{
				switch (writeMethod)
				{
					case WriteMethod.UseStreamWriter:
						{
							StreamWriter writer = next.Value.FileHandle.CreateStreamWriter();
							writer.Write(content);
							writer.Flush();
							return content;
						}
					case WriteMethod.AppendProperty:
						{
							next.Value.FileHandle.Append = content;
							return content;
						}
					case WriteMethod.AppendLineProperty:
						{
							next.Value.FileHandle.AppendLine = content;
							return content + Environment.NewLine;
						}
				}
			}
			return null;
		}

		private FunctionResult Execute(string filePath, DoesNotExistOptions fileDoesNotExist, ExistOptions fileExists, TextCodepage codepage = TextCodepage.Default, bool isText = true)
		{
			FunctionExecutor tester = (new FunctionTester<FileOpen.FileOpen>()).Compile(
				new PropertyValue(FileOpenShared.IsTextPropertyName, isText),
				new PropertyValue(FileOpenShared.CodepagePropertyName, codepage),
				new PropertyValue(FileOpenShared.FileDoesNotExistPropertyName, fileDoesNotExist),
				new PropertyValue(FileOpenShared.FileExistsPropertyName, fileExists));
			var result = tester.Execute(new ParameterValue(FileOpenShared.FilePathPropertyName, filePath));
			Assert.IsFalse(FileHelpers.IsFileLocked(filePath), "File must not open before the execution path is active.");
			return result;
		}
	}
}
