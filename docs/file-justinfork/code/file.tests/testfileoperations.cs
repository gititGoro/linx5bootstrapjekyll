using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twenty57.Linx.Plugin.TestKit;

namespace Twenty57.Linx.Components.File.Tests
{
	[TestFixture]
	public class TestFileOperations
	{
		private const string fileContents = "Linx";

		private string fixtureBaseFolderPath, fixtureSourceFilePath, fixtureDestinationFolderPath;

		[SetUp]
		public void Setup()
		{
			fixtureBaseFolderPath = Path.Combine(Path.GetTempPath(), "TestFileOperations");
			if (Directory.Exists(fixtureBaseFolderPath))
				Directory.Delete(fixtureBaseFolderPath, true);
			string fixtureSourceFolderPath = Path.Combine(fixtureBaseFolderPath, "Source");
			Directory.CreateDirectory(fixtureSourceFolderPath);
			fixtureSourceFilePath = Path.Combine(fixtureSourceFolderPath, "TestFile.txt");
			System.IO.File.WriteAllText(fixtureSourceFilePath, fileContents);
			fixtureDestinationFolderPath = Path.Combine(fixtureBaseFolderPath, "Destination");
			Directory.CreateDirectory(fixtureDestinationFolderPath);
		}

		[OneTimeTearDown]
		public void TeardownFixture()
		{
			if (Directory.Exists(fixtureBaseFolderPath))
				Directory.Delete(fixtureBaseFolderPath, true);
		}

		[Test]
		public void TestExecuteWithActionCopyWithoutExistingFile([Values(
			FileOperationsShared.ExistsOption.DoNothing,
			FileOperationsShared.ExistsOption.IncrementFileName,
			FileOperationsShared.ExistsOption.OverwriteFile)] FileOperationsShared.ExistsOption fileExistsOption)
		{
			TestExecuteWithActionCopyWithoutExistingFile(fixtureSourceFilePath, true, fixtureDestinationFolderPath, fileExistsOption);
		}

		[Test]
		public void TestExecuteWithActionCopyWithoutExistingFileDifferentFileName([Values(
			FileOperationsShared.ExistsOption.DoNothing,
			FileOperationsShared.ExistsOption.IncrementFileName,
			FileOperationsShared.ExistsOption.OverwriteFile)] FileOperationsShared.ExistsOption fileExistsOption)
		{
			TestExecuteWithActionCopyWithoutExistingFile(fixtureSourceFilePath, false, Path.Combine(fixtureDestinationFolderPath, "DifferentFileName.txt"), fileExistsOption);
		}

		[Test]
		public void TestExecuteCopyWithMissingDirectory()
		{
			TestExecuteWithActionCopyWithoutExistingFile(fixtureSourceFilePath, true, Path.Combine(fixtureDestinationFolderPath, "Non", "Existing", "Path"), FileOperationsShared.ExistsOption.DoNothing);
		}

		[Test]
		public void TestExecuteCopyWithMissingDirectoryDifferentFileName()
		{
			TestExecuteWithActionCopyWithoutExistingFile(fixtureSourceFilePath, false, Path.Combine(fixtureDestinationFolderPath, "Non", "Existing", "Path", "DifferentFileName.txt"), FileOperationsShared.ExistsOption.DoNothing);
		}

		[Test]
		public void TestExecuteCopyWithInvalidSourcePath()
		{
			Assert.That(() => TestExecuteWithActionCopyWithoutExistingFile("Not a path", true, fixtureDestinationFolderPath, FileOperationsShared.ExistsOption.DoNothing),
				Throws.Exception.TypeOf<ExecuteException>()
				.With.Property("Message").EqualTo("Invalid source file path: Not a path.\r\nSee Code and Parameter properties for more information."));
		}

		[Test]
		public void TestExecuteCopyWithNonExistingSourcePath()
		{
			Assert.That(() => TestExecuteWithActionCopyWithoutExistingFile("c:\\Hello\\this file does not exist", true, fixtureDestinationFolderPath, FileOperationsShared.ExistsOption.DoNothing),
				Throws.Exception.TypeOf<ExecuteException>()
				.With.Property("Message").EqualTo("Source file path [c:\\Hello\\this file does not exist] does not exist.\r\nSee Code and Parameter properties for more information."));
		}

		[Test]
		public void TestExecuteCopyWithInvalidDestinationPath()
		{
			Assert.That(() => Execute(fixtureSourceFilePath, true, "Not a path", FileOperationsShared.ActionType.Copy, FileOperationsShared.ExistsOption.DoNothing),
				Throws.Exception.TypeOf<ExecuteException>()
				.With.Property("Message").EqualTo("Invalid path: Not a path.\r\nSee Code and Parameter properties for more information."));
		}

		[Test]
		public void TestExecuteWithActionCopyAndFileExistsDoNothingWithExistingFile()
		{
			string destinationFilePath = Path.Combine(fixtureDestinationFolderPath, Path.GetFileName(fixtureSourceFilePath));
			System.IO.File.WriteAllText(destinationFilePath, Guid.NewGuid().ToString());
			FunctionResult result = Execute(fixtureSourceFilePath, true, fixtureDestinationFolderPath, FileOperationsShared.ActionType.Copy, FileOperationsShared.ExistsOption.DoNothing);

			Assert.That(result.Value, Is.Null.Or.Empty);
			Assert.IsTrue(System.IO.File.Exists(fixtureSourceFilePath));
			Assert.IsTrue(System.IO.File.Exists(destinationFilePath));
			Assert.AreEqual(1, Directory.GetFiles(fixtureDestinationFolderPath).Length);
			Assert.That(System.IO.File.ReadAllText(destinationFilePath), Is.Not.Null.Or.Empty);
			Assert.AreNotEqual(System.IO.File.ReadAllText(fixtureSourceFilePath), System.IO.File.ReadAllText(destinationFilePath));
		}

		[Test]
		public void TestExecuteWithActionCopyAndFileExistsOverwriteFileWithExistingFile()
		{
			string destinationFilePath = Path.Combine(fixtureDestinationFolderPath, Path.GetFileName(fixtureSourceFilePath));
			System.IO.File.WriteAllText(destinationFilePath, Guid.NewGuid().ToString());
			FunctionResult result = Execute(fixtureSourceFilePath, true, fixtureDestinationFolderPath, FileOperationsShared.ActionType.Copy, FileOperationsShared.ExistsOption.OverwriteFile);

			Assert.AreEqual(result.Value, destinationFilePath);
			Assert.IsTrue(System.IO.File.Exists(fixtureSourceFilePath));
			Assert.IsTrue(System.IO.File.Exists(destinationFilePath));
			Assert.AreEqual(1, Directory.GetFiles(fixtureDestinationFolderPath).Length);
			Assert.AreEqual(System.IO.File.ReadAllText(fixtureSourceFilePath), System.IO.File.ReadAllText(destinationFilePath));
		}

		[Test]
		public void TestExecuteWithActionCopyAndFileExistsIncrementFileNameWithExistingFile()
		{
			string destinationFilePath1 = Path.Combine(fixtureDestinationFolderPath, Path.GetFileName(fixtureSourceFilePath));
			System.IO.File.WriteAllText(destinationFilePath1, Guid.NewGuid().ToString());
			FunctionResult result = Execute(fixtureSourceFilePath, true, fixtureDestinationFolderPath, FileOperationsShared.ActionType.Copy, FileOperationsShared.ExistsOption.IncrementFileName);

			string destinationFilePath2 = Path.Combine(Path.GetDirectoryName(destinationFilePath1), Path.GetFileNameWithoutExtension(destinationFilePath1) + "_1" + Path.GetExtension(destinationFilePath1));
			Assert.AreEqual(result.Value, destinationFilePath2);
			Assert.IsTrue(System.IO.File.Exists(fixtureSourceFilePath));
			Assert.IsTrue(System.IO.File.Exists(destinationFilePath2));
			Assert.AreEqual(2, Directory.GetFiles(fixtureDestinationFolderPath).Length);
			Assert.AreNotEqual(System.IO.File.ReadAllText(destinationFilePath1), System.IO.File.ReadAllText(destinationFilePath2));
			Assert.AreEqual(System.IO.File.ReadAllText(fixtureSourceFilePath), System.IO.File.ReadAllText(destinationFilePath2));

			System.IO.File.WriteAllText(destinationFilePath2, Guid.NewGuid().ToString());
			result = Execute(fixtureSourceFilePath, true, fixtureDestinationFolderPath, FileOperationsShared.ActionType.Copy, FileOperationsShared.ExistsOption.IncrementFileName);

			string destinationFilePath3 = Path.Combine(Path.GetDirectoryName(destinationFilePath1), Path.GetFileNameWithoutExtension(destinationFilePath1) + "_2" + Path.GetExtension(destinationFilePath1));
			Assert.AreEqual(result.Value, destinationFilePath3);
			Assert.IsTrue(System.IO.File.Exists(fixtureSourceFilePath));
			Assert.IsTrue(System.IO.File.Exists(destinationFilePath3));
			Assert.AreEqual(3, Directory.GetFiles(fixtureDestinationFolderPath).Length);
			Assert.AreNotEqual(System.IO.File.ReadAllText(destinationFilePath1), System.IO.File.ReadAllText(destinationFilePath3));
			Assert.AreNotEqual(System.IO.File.ReadAllText(destinationFilePath2), System.IO.File.ReadAllText(destinationFilePath3));
			Assert.AreEqual(System.IO.File.ReadAllText(fixtureSourceFilePath), System.IO.File.ReadAllText(destinationFilePath3));
		}

		[Test]
		public void TestExecuteWithActionMoveWithoutExistingFile([Values(
			FileOperationsShared.ExistsOption.DoNothing,
			FileOperationsShared.ExistsOption.IncrementFileName,
			FileOperationsShared.ExistsOption.OverwriteFile)] FileOperationsShared.ExistsOption fileExistsOption)
		{
			TestExecuteWithActionMoveWithoutExistingFile(fixtureSourceFilePath, true, fixtureDestinationFolderPath, fileExistsOption);
		}

		[Test]
		public void TestExecuteWithActionMoveWithoutExistingFileDifferentFileName([Values(
			FileOperationsShared.ExistsOption.DoNothing,
			FileOperationsShared.ExistsOption.IncrementFileName,
			FileOperationsShared.ExistsOption.OverwriteFile)] FileOperationsShared.ExistsOption fileExistsOption)
		{
			TestExecuteWithActionMoveWithoutExistingFile(fixtureSourceFilePath, false, Path.Combine(fixtureDestinationFolderPath, "DifferentFileName.txt"), fileExistsOption);
		}

		[Test]
		public void TestExecuteMoveWithMissingDirectory()
		{
			TestExecuteWithActionMoveWithoutExistingFile(fixtureSourceFilePath, true, Path.Combine(fixtureDestinationFolderPath, "Non", "Existing", "Path"), FileOperationsShared.ExistsOption.DoNothing);
		}

		[Test]
		public void TestExecuteMoveWithMissingDirectoryDifferentFileName()
		{
			TestExecuteWithActionCopyWithoutExistingFile(fixtureSourceFilePath, false, Path.Combine(fixtureDestinationFolderPath, "Non", "Existing", "Path", "DifferentFileName.txt"), FileOperationsShared.ExistsOption.DoNothing);
		}

		[Test]
		public void TestExecuteWithActionMoveAndFileExistsDoNothingWithExistingFile()
		{
			string destinationFilePath = Path.Combine(fixtureDestinationFolderPath, Path.GetFileName(fixtureSourceFilePath));
			System.IO.File.WriteAllText(destinationFilePath, Guid.NewGuid().ToString());
			FunctionResult result = Execute(fixtureSourceFilePath, true, fixtureDestinationFolderPath, FileOperationsShared.ActionType.Move, FileOperationsShared.ExistsOption.DoNothing);

			Assert.That(result.Value, Is.Null.Or.Empty);
			Assert.IsTrue(System.IO.File.Exists(fixtureSourceFilePath));
			Assert.IsTrue(System.IO.File.Exists(destinationFilePath));
			Assert.AreEqual(1, Directory.GetFiles(fixtureDestinationFolderPath).Length);
			Assert.That(System.IO.File.ReadAllText(destinationFilePath), Is.Not.Null.Or.Empty);
			Assert.AreNotEqual(System.IO.File.ReadAllText(fixtureSourceFilePath), System.IO.File.ReadAllText(destinationFilePath));
		}

		[Test]
		public void TestExecuteWithActionMoveAndFileExistsOverwriteFileWithExistingFile()
		{
			string sourceFileContents = System.IO.File.ReadAllText(fixtureSourceFilePath);
			string destinationFilePath = Path.Combine(fixtureDestinationFolderPath, Path.GetFileName(fixtureSourceFilePath));
			System.IO.File.WriteAllText(destinationFilePath, Guid.NewGuid().ToString());
			FunctionResult result = Execute(fixtureSourceFilePath, true, fixtureDestinationFolderPath, FileOperationsShared.ActionType.Move, FileOperationsShared.ExistsOption.OverwriteFile);

			Assert.AreEqual(result.Value, destinationFilePath);
			Assert.IsFalse(System.IO.File.Exists(fixtureSourceFilePath));
			Assert.IsTrue(System.IO.File.Exists(destinationFilePath));
			Assert.AreEqual(1, Directory.GetFiles(fixtureDestinationFolderPath).Length);
			Assert.AreEqual(sourceFileContents, System.IO.File.ReadAllText(destinationFilePath));
		}

		[Test]
		public void TestExecuteWithActionMoveAndFileExistsIncrementFileNameWithExistingFile()
		{
			string sourceFileContents = System.IO.File.ReadAllText(fixtureSourceFilePath);
			string destinationFilePath1 = Path.Combine(fixtureDestinationFolderPath, Path.GetFileName(fixtureSourceFilePath));
			System.IO.File.WriteAllText(destinationFilePath1, Guid.NewGuid().ToString());
			FunctionResult result = Execute(fixtureSourceFilePath, true, fixtureDestinationFolderPath, FileOperationsShared.ActionType.Move, FileOperationsShared.ExistsOption.IncrementFileName);

			string destinationFilePath2 = Path.Combine(Path.GetDirectoryName(destinationFilePath1), Path.GetFileNameWithoutExtension(destinationFilePath1) + "_1" + Path.GetExtension(destinationFilePath1));
			Assert.AreEqual(result.Value, destinationFilePath2);
			Assert.IsFalse(System.IO.File.Exists(fixtureSourceFilePath));
			Assert.IsTrue(System.IO.File.Exists(destinationFilePath2));
			Assert.AreEqual(2, Directory.GetFiles(fixtureDestinationFolderPath).Length);
			Assert.AreNotEqual(System.IO.File.ReadAllText(destinationFilePath1), System.IO.File.ReadAllText(destinationFilePath2));
			Assert.AreEqual(sourceFileContents, System.IO.File.ReadAllText(destinationFilePath2));

			System.IO.File.WriteAllText(fixtureSourceFilePath, fileContents);
			System.IO.File.WriteAllText(destinationFilePath2, Guid.NewGuid().ToString());
			result = Execute(fixtureSourceFilePath, true, fixtureDestinationFolderPath, FileOperationsShared.ActionType.Move, FileOperationsShared.ExistsOption.IncrementFileName);

			string destinationFilePath3 = Path.Combine(Path.GetDirectoryName(destinationFilePath1), Path.GetFileNameWithoutExtension(destinationFilePath1) + "_2" + Path.GetExtension(destinationFilePath1));
			Assert.AreEqual(result.Value, destinationFilePath3);
			Assert.IsFalse(System.IO.File.Exists(fixtureSourceFilePath));
			Assert.IsTrue(System.IO.File.Exists(destinationFilePath3));
			Assert.AreEqual(3, Directory.GetFiles(fixtureDestinationFolderPath).Length);
			Assert.AreNotEqual(System.IO.File.ReadAllText(destinationFilePath1), System.IO.File.ReadAllText(destinationFilePath3));
			Assert.AreNotEqual(System.IO.File.ReadAllText(destinationFilePath2), System.IO.File.ReadAllText(destinationFilePath3));
			Assert.AreEqual(sourceFileContents, System.IO.File.ReadAllText(destinationFilePath3));
		}

		[Test]
		public void TestExecuteWithActionDeleteWhenFileExists()
		{
			FunctionResult result = Execute(fixtureSourceFilePath, false, null, FileOperationsShared.ActionType.Delete, FileOperationsShared.ExistsOption.DoNothing);
			Assert.IsNull(result.Value);
			Assert.IsFalse(System.IO.File.Exists(fixtureSourceFilePath));
		}

		[Test]
		public void TestExecuteWithActionDeleteWhenFileDoesNotExist()
		{
			System.IO.File.Delete(fixtureSourceFilePath);
			FunctionResult result = Execute(fixtureSourceFilePath, false, null, FileOperationsShared.ActionType.Delete, FileOperationsShared.ExistsOption.DoNothing);
			Assert.IsNull(result.Value);
			Assert.IsFalse(System.IO.File.Exists(fixtureSourceFilePath));
		}

		[Test]
		public void TestExecuteWithActionDeleteFolder()
		{
			string sourceFolderPath = Path.GetDirectoryName(fixtureSourceFilePath);
			Console.WriteLine(fixtureSourceFilePath + ", " + sourceFolderPath);
			Assert.IsTrue(Directory.Exists(sourceFolderPath));
			FunctionResult result = Execute(sourceFolderPath, false, null, FileOperationsShared.ActionType.Delete, FileOperationsShared.ExistsOption.DoNothing);

			Assert.IsNull(result.Value);
			Assert.IsFalse(Directory.Exists(sourceFolderPath));
		}

		[Test]
		public void TestExecuteFileExistsWhenFileExists()
		{
			FunctionResult result = Execute(fixtureSourceFilePath, false, null, FileOperationsShared.ActionType.FileExists, FileOperationsShared.ExistsOption.DoNothing);
			Assert.IsTrue(result.Value);
		}

		[Test]
		public void TestExecuteFileExistsWhenFileDoesNotExist()
		{
			FunctionResult result = Execute(Path.Combine(fixtureBaseFolderPath, "this file does not exist"), false, null, FileOperationsShared.ActionType.FileExists, FileOperationsShared.ExistsOption.DoNothing);
			Assert.IsFalse(result.Value);
		}

		[Test]
		public void TestExecuteFileExistsInvalidFilePath()
		{
			Assert.That(() => Execute("Not a path", false, null, FileOperationsShared.ActionType.FileExists, FileOperationsShared.ExistsOption.DoNothing),
				Throws.Exception.TypeOf<ExecuteException>()
				.With.Property("Message").EqualTo("Invalid file path: Not a path.\r\nSee Code and Parameter properties for more information."));
		}

		[Test]
		public void TestExecuteCreateTempFile()
		{
			FunctionExecutor tester = (new FunctionTester<FileOperations>()).Compile(
				new PropertyValue(FileOperationsShared.ActionPropertyName, FileOperationsShared.ActionType.CreateTempFile));
			FunctionResult result = tester.Execute(
					new ParameterValue(FileOperationsShared.SourceFilePathPropertyName, null),
					new ParameterValue(FileOperationsShared.DestinationFolderPathPropertyName, null),
					new ParameterValue(FileOperationsShared.DestinationFilePathPropertyName, null));

			Assert.IsTrue(System.IO.File.Exists(result.Value), "Temp file not exists.");
		}

		private void TestExecuteWithActionCopyWithoutExistingFile(string sourceFilePath, bool keepFileName, string destinationPath, FileOperationsShared.ExistsOption fileExistsOption)
		{
			FunctionResult result = Execute(sourceFilePath, keepFileName, destinationPath, FileOperationsShared.ActionType.Copy, fileExistsOption);

			string destinationFilePath = keepFileName ? Path.Combine(destinationPath, Path.GetFileName(sourceFilePath)) : destinationPath;
			Assert.AreEqual(result.Value, destinationFilePath);
			Assert.IsTrue(System.IO.File.Exists(sourceFilePath));
			Assert.IsTrue(System.IO.File.Exists(destinationFilePath));
			Assert.AreEqual(System.IO.File.ReadAllText(sourceFilePath), System.IO.File.ReadAllText(destinationFilePath));
		}

		private void TestExecuteWithActionMoveWithoutExistingFile(string sourceFilePath, bool keepFileName, string destinationPath, FileOperationsShared.ExistsOption fileExistsOption)
		{
			string sourceFileContents = System.IO.File.ReadAllText(sourceFilePath);
			FunctionResult result = Execute(sourceFilePath, keepFileName, destinationPath, FileOperationsShared.ActionType.Move, fileExistsOption);

			string destinationFilePath = keepFileName ? Path.Combine(destinationPath, Path.GetFileName(sourceFilePath)) : destinationPath;
			Assert.AreEqual(result.Value, destinationFilePath);
			Assert.IsFalse(System.IO.File.Exists(sourceFilePath));
			Assert.IsTrue(System.IO.File.Exists(destinationFilePath));
			Assert.AreEqual(sourceFileContents, System.IO.File.ReadAllText(destinationFilePath));
		}

		private FunctionResult Execute(string sourceFilePath, bool keepFileName, string destinationPath, FileOperationsShared.ActionType action, FileOperationsShared.ExistsOption fileExistsOption)
		{
			FunctionExecutor tester = (new FunctionTester<FileOperations>()).Compile(
				new PropertyValue(FileOperationsShared.ActionPropertyName, action),
				new PropertyValue(FileOperationsShared.KeepFileNamePropertyName, keepFileName),
				new PropertyValue(FileOperationsShared.FileExistsPropertyName, fileExistsOption));
			return tester.Execute(
				new ParameterValue(FileOperationsShared.SourceFilePathPropertyName, sourceFilePath),
				new ParameterValue(FileOperationsShared.DestinationFolderPathPropertyName, keepFileName ? destinationPath : null),
				new ParameterValue(FileOperationsShared.DestinationFilePathPropertyName, keepFileName ? null : destinationPath));
		}
	}
}
