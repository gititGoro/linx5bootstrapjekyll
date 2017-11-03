using System;
using System.IO;
using NUnit.Framework;
using Twenty57.Linx.Plugin.TestKit;
using Twenty57.Linx.Components.File.Common;

namespace Twenty57.Linx.Components.File.Tests
{
	[TestFixture]
	public class TestBinaryFileWrite
	{
		[SetUp]
		public void Setup()
		{
		}

		[Test]
		public void TestExecuteWithFileDoesNotExistAndCreateFile()
		{
			byte[] data = { 1, 2, 3 };
			string fileName = GetFileName();

			string FilePath = fileName;
			var FileDoesNotExist = DoesNotExistOptions.CreateFile;
			var FileExists = ExistOptions.OverwriteFile;

			Assert.IsFalse(System.IO.File.Exists(fileName));
			var result = Execute(FileExists, FileDoesNotExist, FilePath, data);
			Assert.AreEqual(fileName, result.Value);
			Assert.IsTrue(System.IO.File.Exists(fileName));
			Assert.AreEqual(data, System.IO.File.ReadAllBytes(fileName));
		}

		[Test]
		public void TestExecuteWithFileDoesNotExistAndCreateFileWithMissingDirectory()
		{
			byte[] data = { 1, 2, 3 };
			string fileName = Path.Combine(Path.GetTempPath(), "Test", "Binary", "File", "Write", "Output.txt");
			string directory = Path.GetDirectoryName(fileName);

			if (Directory.Exists(directory))
				Directory.Delete(directory, true);
			Assert.IsFalse(Directory.Exists(directory));

			string FilePath = fileName;
			var FileDoesNotExist = DoesNotExistOptions.CreateFile;
			var FileExists = ExistOptions.OverwriteFile;

			Assert.IsFalse(System.IO.File.Exists(fileName));
			var result = Execute(FileExists, FileDoesNotExist, FilePath, data);
			Assert.AreEqual(fileName, result.Value);
			Assert.IsTrue(System.IO.File.Exists(fileName));
			Assert.AreEqual(data, System.IO.File.ReadAllBytes(fileName));
		}

		[Test]
		public void TestExecuteWithFileDoesNotExistAndThrowException()
		{
			string fileName = GetFileName();
			try
			{
				string FilePath = fileName;
				var FileDoesNotExist = DoesNotExistOptions.ThrowException;
				var FileExists = ExistOptions.OverwriteFile;

				Assert.IsFalse(System.IO.File.Exists(fileName));
				Execute(FileExists, FileDoesNotExist, FilePath, new byte[] { });
			}
			catch (Exception exception)
			{
				Assert.IsInstanceOf<Exception>(exception);
				Assert.AreEqual(
					String.Format("File [{0}] does not exist.\r\nSee Code and Parameter properties for more information.", fileName),
					exception.Message);
				Assert.IsFalse(System.IO.File.Exists(fileName));
				return;
			}
			Assert.Fail("An exception was expected.");
		}

		[Test]
		public void TestExecuteWithFileDoesExistAndAppendData()
		{
			string fileName = GetFileName(true, new byte[] { 1, 2, 3 });

			string FilePath = fileName;
			var FileExists = ExistOptions.AppendData;
			var FileDoesNotExist = DoesNotExistOptions.CreateFile;

			Assert.IsTrue(System.IO.File.Exists(fileName));
			Assert.AreEqual(new byte[] { 1, 2, 3 }, System.IO.File.ReadAllBytes(fileName));

			var result = Execute(FileExists, FileDoesNotExist, FilePath, new byte[] { 4, 5, 6 });

			Assert.AreEqual(fileName, result.Value);
			Assert.IsTrue(System.IO.File.Exists(fileName));
			Assert.AreEqual(new byte[] { 1, 2, 3, 4, 5, 6 }, System.IO.File.ReadAllBytes(fileName));
		}

		[Test]
		public void TestExecuteWithFileDoesExistAndOverwriteFile()
		{
			string fileName = GetFileName(true, new byte[] { 1, 2, 3 });

			Assert.IsTrue(System.IO.File.Exists(fileName));
			Assert.AreEqual(new byte[] { 1, 2, 3 }, System.IO.File.ReadAllBytes(fileName));

			string FilePath = fileName;
			var FileExists = ExistOptions.OverwriteFile;
			var FileDoesNotExist = DoesNotExistOptions.CreateFile;

			var result = Execute(FileExists, FileDoesNotExist, FilePath, new byte[] { 4, 5, 6 });

			Assert.AreEqual(fileName, result.Value);
			Assert.IsTrue(System.IO.File.Exists(fileName));
			Assert.AreEqual(new byte[] { 4, 5, 6 }, System.IO.File.ReadAllBytes(fileName));
		}

		[Test]
		public void TestExecuteWithFileDoesExistAndIncrementFilename()
		{
			string fileName = GetFileName(true, new byte[] { 1, 2, 3 });
			string expectedFileName = String.Format("{0}{1}{2}_1{3}", Path.GetDirectoryName(fileName),
				Path.DirectorySeparatorChar, Path.GetFileNameWithoutExtension(fileName), Path.GetExtension(fileName));

			string FilePath = fileName;
			var FileExists = ExistOptions.IncrementFileName;
			var FileDoesNotExist = DoesNotExistOptions.CreateFile;

			Assert.IsTrue(System.IO.File.Exists(fileName));
			Assert.AreEqual(new byte[] { 1, 2, 3 }, System.IO.File.ReadAllBytes(fileName));
			Assert.IsFalse(System.IO.File.Exists(expectedFileName));

			Execute(FileExists, FileDoesNotExist, FilePath, new byte[] { 4, 5, 6 });

			Assert.IsTrue(System.IO.File.Exists(fileName));
			Assert.AreEqual(new byte[] { 1, 2, 3 }, System.IO.File.ReadAllBytes(fileName));
			Assert.IsTrue(System.IO.File.Exists(expectedFileName));
			Assert.AreEqual(new byte[] { 4, 5, 6 }, System.IO.File.ReadAllBytes(expectedFileName));
		}

		[Test]
		public void TestExecuteWithFileDoesExistAndIncrementFilenameAlreadyExist()
		{
			string fileName = GetFileName(true, new byte[] { 1 });
			string incrementFileName = String.Format("{0}{1}{2}_1{3}", Path.GetDirectoryName(fileName),
				Path.DirectorySeparatorChar, Path.GetFileNameWithoutExtension(fileName), Path.GetExtension(fileName));
			System.IO.File.WriteAllBytes(incrementFileName, new byte[] { 2 });

			string expectedFileName = String.Format("{0}{1}{2}_2{3}", Path.GetDirectoryName(fileName),
				Path.DirectorySeparatorChar, Path.GetFileNameWithoutExtension(fileName), Path.GetExtension(fileName));

			var FileExists = ExistOptions.IncrementFileName;
			var FileDoesNotExist = DoesNotExistOptions.CreateFile;
			string FilePath = fileName;
			Assert.IsTrue(System.IO.File.Exists(fileName));
			Assert.AreEqual(new byte[] { 1 }, System.IO.File.ReadAllBytes(fileName));
			Assert.IsTrue(System.IO.File.Exists(incrementFileName));
			Assert.AreEqual(new byte[] { 2 }, System.IO.File.ReadAllBytes(incrementFileName));
			Assert.IsFalse(System.IO.File.Exists(expectedFileName));

			var result = Execute(FileExists, FileDoesNotExist, FilePath, new byte[] { 3 });

			Assert.AreEqual(expectedFileName, result.Value);
			Assert.IsTrue(System.IO.File.Exists(fileName));
			Assert.AreEqual(new byte[] { 1 }, System.IO.File.ReadAllBytes(fileName));
			Assert.IsTrue(System.IO.File.Exists(incrementFileName));
			Assert.AreEqual(new byte[] { 2 }, System.IO.File.ReadAllBytes(incrementFileName));
			Assert.IsTrue(System.IO.File.Exists(expectedFileName));
			Assert.AreEqual(new byte[] { 3 }, System.IO.File.ReadAllBytes(expectedFileName));
		}

		[Test]
		public void TestExecuteWithFileDoesExistAndThrowException()
		{
			string fileName = GetFileName(true, new byte[] { 1, 2, 3 });

			string FilePath = fileName;
			var FileExists = ExistOptions.ThrowException;
			var FileDoesNotExist = DoesNotExistOptions.CreateFile;

			Assert.IsTrue(System.IO.File.Exists(fileName));
			Assert.AreEqual(new byte[] { 1, 2, 3 }, System.IO.File.ReadAllBytes(fileName));

			try
			{
				Execute(FileExists, FileDoesNotExist, FilePath, new byte[] { 4, 5, 6 });
			}
			catch (Exception exception)
			{
				Assert.IsInstanceOf<Exception>(exception);
				Assert.AreEqual(
					String.Format("File [{0}] already exists.\r\nSee Code and Parameter properties for more information.", fileName),
					exception.Message);
				Assert.IsTrue(System.IO.File.Exists(fileName));
				Assert.AreEqual(new byte[] { 1, 2, 3 }, System.IO.File.ReadAllBytes(fileName));
				return;
			}
			Assert.Fail("An exception was expected.");
		}

		private string GetFileName(bool createFile = false, byte[] data = default(byte[]))
		{
			string outputPath = Path.Combine(Path.GetTempPath(), "TestBinaryFileWrite");
			string fileName = Path.Combine(outputPath, String.Format("{0}.bin", Guid.NewGuid().ToString("N")));

			if (!Directory.Exists(outputPath))
				Directory.CreateDirectory(outputPath);

			if (createFile)
				System.IO.File.WriteAllBytes(fileName, data);

			return fileName;
		}

		private FunctionResult Execute(
			ExistOptions fileExists,
			DoesNotExistOptions fileNotExist,
			string filePathProp,
			object contentsProp
			)
		{
			FunctionExecutor tester = new FunctionTester<BinaryFileWrite>().Compile(
				new PropertyValue(BinaryFileWriteShared.FileExistsPropertyName, fileExists),
				new PropertyValue(BinaryFileWriteShared.FileDoesNotExistPropertyName, fileNotExist)
				);

			var result = tester.Execute(
				new ParameterValue(FileShared.FilePathPropertyName, (BinaryFileHandle)filePathProp),
				new ParameterValue(BinaryFileWriteShared.ContentsPropertyName, contentsProp)
				);
			Assert.IsFalse(FileHelpers.IsFileLocked(result.Value));
			return result;
		}
	}
}