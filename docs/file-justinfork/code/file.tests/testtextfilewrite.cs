using NUnit.Framework;
using System.Globalization;
using System.IO;
using System.Text;
using Twenty57.Linx.Plugin.TestKit;
using Twenty57.Linx.Components.File.TextFileWrite;
using Twenty57.Linx.Components.File.Common;

namespace Twenty57.Linx.Components.File.Tests
{
	[TestFixture]
	public class TestTextFileWrite
	{
		private const string DefaultContents = "Test";

		private string defaultFolderPath;
		private string defaultFilePath;

		[OneTimeSetUp]
		public void SetupFixture()
		{
			defaultFolderPath = Path.Combine(Path.GetTempPath(), "TestFileWrite");
			defaultFilePath = Path.Combine(defaultFolderPath, "Textfile.txt");
		}

		[OneTimeTearDown]
		public void TeardownFixture()
		{
			FileHelpers.ForceDeleteDirectory(defaultFolderPath);
		}

		[TearDown]
		public void TearDown()
		{
			try
			{
				System.IO.File.Delete(defaultFilePath);
			}
			catch (FileNotFoundException) { }
		}

		[Test]
		public void TestCreateFile()
		{
			var result = Execute();
			Assert.AreEqual(defaultFilePath, result.Value);
			AssertFileContents();
		}

		[Test]
		public void TestCreateDirectory()
		{
			string filePath = Path.Combine(defaultFolderPath, @"newDir\Test");
			var result = Execute(filePath);
			Assert.AreEqual(filePath, result.Value);
			AssertFileContents(filePath);
		}

		[Test]
		public void TestThrowExceptionWhenFileMissing()
		{
			Assert.That(() => Execute(Path.Combine(defaultFolderPath, @"Missing"), DefaultContents, DoesNotExistOptions.ThrowException),
				Throws.Exception.TypeOf<ExecuteException>());
		}

		[Test]
		public void TestAppend()
		{
			var result = Execute();
			Assert.AreEqual(defaultFilePath, result.Value);
			AssertFileContents(defaultFilePath);
			result = Execute(DoesNotExistOptions.CreateFile, ExistOptions.AppendData);
			Assert.AreEqual(defaultFilePath, result.Value);
			AssertFileContents(defaultFilePath, DefaultContents + DefaultContents);
		}

		[Test]
		public void TestOverwrite()
		{
			var result = Execute(defaultFilePath);
			Assert.AreEqual(defaultFilePath, result.Value);
			AssertFileContents();
			result = Execute(defaultFilePath, "New Content", DoesNotExistOptions.CreateFile, ExistOptions.OverwriteFile);
			Assert.AreEqual(defaultFilePath, result.Value);
			AssertFileContents(defaultFilePath, "New Content");
		}

		[Test]
		public void TestIncrementFilename()
		{
			string firstFile = Path.Combine(defaultFolderPath, "TestFile.txt");
			string incrementedFile = Path.Combine(defaultFolderPath, "TestFile_1.txt");
			string incrementedFile2 = Path.Combine(defaultFolderPath, "TestFile_2.txt");
			var result = Execute(firstFile, DefaultContents);
			Assert.AreEqual(firstFile, result.Value);
			AssertFileContents(firstFile);
			result = Execute(firstFile, DefaultContents, DoesNotExistOptions.CreateFile, ExistOptions.IncrementFileName);
			Assert.AreEqual(incrementedFile, result.Value);
			AssertFileContents(incrementedFile);
			result = Execute(firstFile, DefaultContents, DoesNotExistOptions.CreateFile, ExistOptions.IncrementFileName);
			Assert.AreEqual(incrementedFile2, result.Value);
			AssertFileContents(incrementedFile2);
		}

		[Test]
		public void TestExecuteWithNonDefaultCodePage()
		{
			var result = Execute(DoesNotExistOptions.CreateFile, ExistOptions.OverwriteFile, TextCodepage.EBCDIC);
			Assert.AreEqual(defaultFilePath, result.Value);
			AssertFileContents(defaultFilePath, DefaultContents, CultureInfo.CurrentCulture.TextInfo.EBCDICCodePage);
		}

		private FunctionResult Execute(DoesNotExistOptions fileDoesNotExist = DoesNotExistOptions.CreateFile, ExistOptions fileExists = ExistOptions.OverwriteFile, TextCodepage destinationCodepage = TextCodepage.ASCII)
		{
			return Execute(defaultFilePath, DefaultContents, fileDoesNotExist, fileExists, destinationCodepage);
		}

		private FunctionResult Execute(string filePath, string contents = DefaultContents, DoesNotExistOptions fileDoesNotExist = DoesNotExistOptions.CreateFile, ExistOptions fileExists = ExistOptions.OverwriteFile, TextCodepage destinationCodepage = TextCodepage.ASCII)
		{
			FunctionExecutor tester = (new FunctionTester<TextFileWrite.TextFileWrite>()).Compile(
				new PropertyValue(TextFileWriteShared.DestinationCodepagePropertyName, destinationCodepage),
				new PropertyValue(TextFileWriteShared.FileDoesNotExistPropertyName, fileDoesNotExist),
				new PropertyValue(TextFileWriteShared.FileExistsPropertyName, fileExists));
			var result = tester.Execute(new ParameterValue(FileShared.FilePathPropertyName, (TextFileHandle)filePath),
				new ParameterValue(TextFileWriteShared.ContentsPropertyName, contents));
			Assert.IsFalse(FileHelpers.IsFileLocked(result.Value));
			return result;
		}

		private void AssertFileContents()
		{
			AssertFileContents(defaultFilePath);
		}

		private void AssertFileContents(string fileName, string expected = DefaultContents)
		{
			string actual = System.IO.File.ReadAllText(fileName);
			Assert.AreEqual(expected, actual);
		}

		private void AssertFileContents(string fileName, string expected, int encoding)
		{
			string actual = System.IO.File.ReadAllText(fileName, Encoding.GetEncoding(encoding));
			Assert.AreEqual(expected, actual);
		}
	}
}