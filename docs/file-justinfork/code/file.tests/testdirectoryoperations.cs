using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using Twenty57.Linx.Plugin.TestKit;

namespace Twenty57.Linx.Components.File.Tests
{
	[TestFixture]
	public class TestDirectoryOperations
	{
		private const string fileContents = "Linx";

		private string fixtureBaseFolderPath, fixtureSourceDirectoryPath, fixtureTargetFolderPath;
		private int fixtureSourceDirectoryCount, fixtureSourceFileCount, fixtureTargetDirectoryCount, fixtureTargetFileCount = 0;

		[SetUp]
		public void Setup()
		{
			fixtureBaseFolderPath = Path.Combine(Path.GetTempPath(), "TestDirectoryOperations");
			if (Directory.Exists(fixtureBaseFolderPath))
				Directory.Delete(fixtureBaseFolderPath, true);

			fixtureSourceDirectoryPath = Path.Combine(fixtureBaseFolderPath, "Source");
			CreateSourceDirectory(fixtureSourceDirectoryPath);
			fixtureTargetFolderPath = Path.Combine(fixtureBaseFolderPath, "Destination");
		}

		[OneTimeTearDown]
		public void TeardownFixture()
		{
			if (Directory.Exists(fixtureBaseFolderPath))
				Directory.Delete(fixtureBaseFolderPath, true);
		}

		[Test]
		public void TestExecuteWithActionCopyWithEmptySourceDirectory()
		{
			Assert.That(() => Execute(V2.DirectoryOperationsShared.ActionType.Copy, string.Empty, fixtureTargetFolderPath),
				Throws.Exception.TypeOf<ExecuteException>()
				.With.Property("Message").EqualTo("Source directory path cannot be null or empty.\r\nSee Code and Parameter properties for more information."));
		}

		[Test]
		public void TestExecuteWithActionCopyWithEmptyTargetDirectory()
		{
			Assert.That(() => Execute(V2.DirectoryOperationsShared.ActionType.Copy, fixtureSourceDirectoryPath, string.Empty),
				Throws.Exception.TypeOf<ExecuteException>()
				.With.Property("Message").EqualTo("Target directory path cannot be null or empty.\r\nSee Code and Parameter properties for more information."));
		}

		[Test]
		public void TestExecuteWithActionCopyAndInvalidSourceDirectory()
		{
			Assert.That(() => Execute(V2.DirectoryOperationsShared.ActionType.Copy, @"Z:\Bad", fixtureTargetFolderPath),
				Throws.Exception.TypeOf<ExecuteException>()
				.With.Property("Message").EqualTo("Source directory [Z:\\Bad] does not exist.\r\nSee Code and Parameter properties for more information."));
		}

		[Test]
		public void TestExecuteWithActionCopy([Values(
					V2.DirectoryOperationsShared.ExistsOption.DoNothing,
					V2.DirectoryOperationsShared.ExistsOption.MergeDirectory,
					V2.DirectoryOperationsShared.ExistsOption.OverwriteDirectory
				)]   V2.DirectoryOperationsShared.ExistsOption fileExistsOption, [Values(true, false)] bool replaceExistingFiles)
		{
			CreateTargetDirectory(fixtureTargetFolderPath, true);
			TestExecuteWithActionCopy(fixtureSourceDirectoryPath, fixtureTargetFolderPath, fileExistsOption, replaceExistingFiles);
		}

		[Test]
		public void TestExecuteWithActionMove([Values(
					V2.DirectoryOperationsShared.ExistsOption.DoNothing,
					V2.DirectoryOperationsShared.ExistsOption.OverwriteDirectory,
					V2.DirectoryOperationsShared.ExistsOption.MergeDirectory
				)]   V2.DirectoryOperationsShared.ExistsOption fileExistsOption, [Values(true, false)] bool replaceExistingFiles)
		{
			CreateTargetDirectory(fixtureTargetFolderPath, false);
			TestExecuteWithActionMove(fixtureSourceDirectoryPath, fixtureTargetFolderPath, fileExistsOption, replaceExistingFiles);
		}

		[Test]
		public void TestExecuteWithActionDeleteWithEmptyDirectoryPath()
		{
			Assert.That(() => Execute(V2.DirectoryOperationsShared.ActionType.Delete, directory: string.Empty),
				Throws.Exception.TypeOf<ExecuteException>()
				.With.Property("Message").EqualTo("Value cannot be null.\r\nParameter name: directoryPath\r\nSee Code and Parameter properties for more information."));
		}

		[Test]
		public void TestExecuteWithActionDeleteWhenDirectoryExists()
		{
			Assert.IsTrue(Directory.Exists(fixtureSourceDirectoryPath));

			string readOnlyFilePath = Path.Combine(this.fixtureSourceDirectoryPath, $"A{Guid.NewGuid().ToString("N")}.txt");
			System.IO.File.WriteAllText(readOnlyFilePath, "ReadOnly");
			FileAttributes attributes = System.IO.File.GetAttributes(readOnlyFilePath);
			System.IO.File.SetAttributes(readOnlyFilePath, attributes | FileAttributes.ReadOnly);
			attributes = System.IO.File.GetAttributes(readOnlyFilePath);
			Assert.AreEqual(FileAttributes.ReadOnly, attributes & FileAttributes.ReadOnly);

			FunctionResult result = Execute(V2.DirectoryOperationsShared.ActionType.Delete, directory: fixtureSourceDirectoryPath);

			Assert.IsNull(result.Value);
			Assert.IsFalse(Directory.Exists(fixtureSourceDirectoryPath));
		}

		[Test]
		public void TestExecuteWithActionDeleteWhenDirectoryDoesNotExist()
		{
			var directoryPath = Path.Combine(fixtureSourceDirectoryPath, "testData");
			Assert.IsFalse(Directory.Exists(directoryPath));

			Assert.Throws<ExecuteException>(() => Execute(V2.DirectoryOperationsShared.ActionType.Delete, directory: directoryPath),
				string.Format("Directory [{0}] does not exist.\r\nSee Code and Parameter properties for more information.", directoryPath));
		}

		[Test]
		public void TestExecuteWithActionCreateWithEmptyDirectoryPath()
		{
			Assert.That(() => Execute(V2.DirectoryOperationsShared.ActionType.Create, directory: string.Empty),
				Throws.Exception.TypeOf<ExecuteException>()
				.With.Property("Message").EqualTo("Directory path cannot be null or empty.\r\nSee Code and Parameter properties for more information."));
		}

		[Test]
		public void TestExecuteWithActionCreateWhenDirectoryDoesNotExist(
			[Values(
					V2.DirectoryOperationsShared.CreateExistsOption.DoNothing,
					V2.DirectoryOperationsShared.CreateExistsOption.IncrementFolderName,
					V2.DirectoryOperationsShared.CreateExistsOption.Clear,
					V2.DirectoryOperationsShared.CreateExistsOption.ThrowException)]
				V2.DirectoryOperationsShared.CreateExistsOption existsOption)
		{
			string folder = Path.Combine(fixtureBaseFolderPath, "very", "deep");
			Assert.IsFalse(Directory.Exists(folder));

			FunctionResult result = Execute(V2.DirectoryOperationsShared.ActionType.Create, createExistsOption: existsOption, directory: folder);

			Assert.AreEqual(folder, result.Value);
			Assert.IsTrue(Directory.Exists(folder));
		}

		[Test]
		public void TestExecuteWithActionCreateAndExistsDoNothingWhenDirectoryExists()
		{
			string folderPath = Path.Combine(fixtureBaseFolderPath, "exists");
			Directory.CreateDirectory(folderPath);
			string filePath = Path.Combine(folderPath, "myfile");
			System.IO.File.Create(filePath).Close();

			FunctionResult result = Execute(V2.DirectoryOperationsShared.ActionType.Create, createExistsOption: V2.DirectoryOperationsShared.CreateExistsOption.DoNothing, directory: folderPath);

			Assert.AreEqual(folderPath, result.Value);
			Assert.IsTrue(Directory.Exists(folderPath));
			Assert.IsTrue(System.IO.File.Exists(filePath));
		}

		[Test]
		public void TestExecuteWithActionCreateAndExistsClearWhenDirectoryExists()
		{
			string folderPath = Path.Combine(fixtureBaseFolderPath, "exists");
			Directory.CreateDirectory(folderPath);
			string filePath = Path.Combine(folderPath, "myfile");
			System.IO.File.Create(filePath).Close();

			FunctionResult result = Execute(V2.DirectoryOperationsShared.ActionType.Create, createExistsOption: V2.DirectoryOperationsShared.CreateExistsOption.Clear, directory: folderPath);

			Assert.AreEqual(folderPath, result.Value);
			Assert.IsTrue(Directory.Exists(folderPath));
			Assert.IsFalse(System.IO.File.Exists(filePath));
		}

		[Test]
		public void TestExecuteWithActionCreateAndExistsThrowExceptionWhenDirectoryExists()
		{
			string folderPath = Path.Combine(fixtureBaseFolderPath, "exists");
			Directory.CreateDirectory(folderPath);
			string filePath = Path.Combine(folderPath, "myfile");
			System.IO.File.Create(filePath).Close();

			Assert.Throws<ExecuteException>(() =>
				Execute(V2.DirectoryOperationsShared.ActionType.Create, createExistsOption: V2.DirectoryOperationsShared.CreateExistsOption.ThrowException, directory: folderPath),
				string.Format("Directory [{0}] already exists.\r\nSee Code and Parameter properties for more information.", folderPath));

			Assert.IsTrue(Directory.Exists(folderPath));
			Assert.IsTrue(System.IO.File.Exists(filePath));
		}

		[Test]
		public void TestExecuteWithActionCreateAndExistsIncrementFolderNameWhenDirectoryExists()
		{
			string folderPath = Path.Combine(fixtureBaseFolderPath, "exists");
			Directory.CreateDirectory(folderPath);
			string filePath = Path.Combine(folderPath, "myfile");
			System.IO.File.Create(filePath).Close();

			FunctionResult result = Execute(V2.DirectoryOperationsShared.ActionType.Create, createExistsOption: V2.DirectoryOperationsShared.CreateExistsOption.IncrementFolderName, directory: folderPath);

			Assert.IsTrue(Directory.Exists(folderPath));
			Assert.IsTrue(System.IO.File.Exists(filePath));

			string expectedFolderPath = string.Format("{0}_1", folderPath);
			Assert.AreEqual(expectedFolderPath, result.Value);
			Assert.IsTrue(Directory.Exists(expectedFolderPath));

			result = Execute(V2.DirectoryOperationsShared.ActionType.Create, createExistsOption: V2.DirectoryOperationsShared.CreateExistsOption.IncrementFolderName, directory: folderPath);

			expectedFolderPath = string.Format("{0}_2", folderPath);
			Assert.AreEqual(expectedFolderPath, result.Value);
			Assert.IsTrue(Directory.Exists(expectedFolderPath));
		}

		[Test]
		public void TestExecuteWithActionExistsWhenDirectoryPathEmpty()
		{
			Assert.That(() => Execute(V2.DirectoryOperationsShared.ActionType.DirectoryExists, directory: string.Empty),
				Throws.Exception.TypeOf<ExecuteException>()
				.With.Property("Message").EqualTo("Directory path cannot be null or empty.\r\nSee Code and Parameter properties for more information."));
		}

		[Test]
		public void TestExecuteWithActionExistsWhenDirectoryExists()
		{
			Assert.IsTrue(Directory.Exists(fixtureSourceDirectoryPath));
			FunctionResult result = Execute(V2.DirectoryOperationsShared.ActionType.DirectoryExists, directory: fixtureSourceDirectoryPath);

			Assert.IsTrue(result.Value);
		}

		[Test]
		public void TestExecuteWithActionExistsWhenDirectoryDoesNotExist()
		{
			var directoryPath = Path.Combine(fixtureSourceDirectoryPath, "testData");
			Assert.IsFalse(Directory.Exists(directoryPath));
			FunctionResult result = Execute(V2.DirectoryOperationsShared.ActionType.DirectoryExists, directory: directoryPath);

			Assert.IsFalse(result.Value);
		}

		private void TestExecuteWithActionCopy(string sourceDirectoryPath, string targetDirectoryPath, V2.DirectoryOperationsShared.ExistsOption existsOption, bool replaceFileIfExists)
		{
			FunctionResult result = Execute(V2.DirectoryOperationsShared.ActionType.Copy, sourceDirectoryPath, targetDirectoryPath, existsOption, replaceIfFileExist: replaceFileIfExists);

			Assert.IsTrue(System.IO.Directory.Exists(sourceDirectoryPath));
			Assert.IsTrue(System.IO.Directory.Exists(Path.Combine(targetDirectoryPath, "Source")), "Target directory does not exist.");

			var sourceFolderCount = Directory.GetDirectories(fixtureSourceDirectoryPath, "*", SearchOption.AllDirectories).Count();
			var targetFolderCount = Directory.GetDirectories(Path.Combine(fixtureTargetFolderPath, "Source"), "*", SearchOption.AllDirectories).Count();

			var sourceFileCount = Directory.GetFiles(fixtureSourceDirectoryPath, "*", SearchOption.AllDirectories).Count();
			var targetFileCount = Directory.GetFiles(Path.Combine(fixtureTargetFolderPath, "Source"), "*", SearchOption.AllDirectories).Count();

			if (existsOption == V2.DirectoryOperationsShared.ExistsOption.DoNothing)
			{
				Assert.AreEqual(fixtureTargetDirectoryCount, targetFolderCount, "The folder count should be same");
				Assert.AreEqual(fixtureTargetFileCount, targetFileCount, "The file count should be same");
			}
			else
			{
				Assert.AreEqual(sourceFolderCount, targetFolderCount, "Source folder count and target folder count should match.");
				Assert.AreEqual(sourceFileCount, targetFileCount, "Source file count and target file count should match.");
			}

		}

		private void TestExecuteWithActionMove(string sourceDirectoryPath, string targetDirectoryPath, V2.DirectoryOperationsShared.ExistsOption existsOption, bool replaceFileIfExists)
		{
			FunctionResult result = Execute(V2.DirectoryOperationsShared.ActionType.Move, sourceDirectoryPath, targetDirectoryPath, existsOption, replaceIfFileExist: replaceFileIfExists);

			var targetFolderCount = Directory.GetDirectories(fixtureTargetFolderPath, "*", SearchOption.AllDirectories).Count();
			var targetFileCount = Directory.GetFiles(fixtureTargetFolderPath, "*", SearchOption.AllDirectories).Count();

			if (existsOption == V2.DirectoryOperationsShared.ExistsOption.DoNothing)
			{
				Assert.AreEqual(fixtureTargetDirectoryCount, targetFolderCount, "The folder count should be same");
				Assert.AreEqual(fixtureTargetFileCount, targetFileCount, "The file count should be same");
			}
			else
			{

				if (!replaceFileIfExists)
				{
					Assert.AreEqual(fixtureSourceDirectoryCount, targetFolderCount, "Source folder count and target folder count should match.");
					Assert.AreEqual(fixtureSourceFileCount, targetFileCount, "Source file count and target file count should match.");
				}
				else
				{
					Assert.IsFalse(Directory.Exists(fixtureSourceDirectoryPath), "The source directory should be deleted.");

					Assert.AreEqual(fixtureSourceDirectoryCount, targetFolderCount, "Source folder count and target folder count should match.");
					Assert.AreEqual(fixtureSourceFileCount, targetFileCount, "Source file count and target file count should match.");
				}
			}
		}

		private FunctionResult Execute(V2.DirectoryOperationsShared.ActionType action, string sourceDirectoryPath = null, string targetDirectoryPath = null,
				V2.DirectoryOperationsShared.ExistsOption existsOption = V2.DirectoryOperationsShared.ExistsOption.DoNothing,
				V2.DirectoryOperationsShared.CreateExistsOption createExistsOption = V2.DirectoryOperationsShared.CreateExistsOption.DoNothing,
			string directory = null, bool replaceIfFileExist = false)
		{
			FunctionExecutor tester = (new FunctionTester<DirectoryOperations>()).Compile(
					new PropertyValue(DirectoryOperationsShared.ActionPropertyName, action),
					new PropertyValue(DirectoryOperationsShared.ReplaceExistingFilePropertyName, replaceIfFileExist),
					new PropertyValue(DirectoryOperationsShared.DirectoryExistsPropertyName, existsOption),
					new PropertyValue(DirectoryOperationsShared.CreateDirectoryExistsPropertyName, createExistsOption));
			return tester.Execute(
					new ParameterValue(DirectoryOperationsShared.SourceDirectoryPropertyName, sourceDirectoryPath),
					new ParameterValue(DirectoryOperationsShared.TargetDirectoryPropertyName, targetDirectoryPath),
					new ParameterValue(DirectoryOperationsShared.DirectoryPropertyName, directory));
		}

		private void CreateSourceDirectory(string sourceDirectoryPath)
		{
			Directory.CreateDirectory(sourceDirectoryPath);
			int nestedDirectoryCount = 0;
			var directoryPath = sourceDirectoryPath;
			while (nestedDirectoryCount < 5)
			{
				directoryPath = Path.Combine(directoryPath, string.Format("Sub-{0}", nestedDirectoryCount));
				Directory.CreateDirectory(directoryPath);
				int fileCount = 0;
				while (fileCount < 5)
				{
					var filePath = Path.Combine(directoryPath, string.Format("file-{0}.txt", fileCount));
					System.IO.File.WriteAllText(filePath, fileContents);
					fileCount++;
				}

				nestedDirectoryCount++;
			}

			this.fixtureSourceDirectoryCount = Directory.GetDirectories(sourceDirectoryPath, "*", SearchOption.AllDirectories).Count();
			this.fixtureSourceFileCount = Directory.GetFiles(sourceDirectoryPath, "*", SearchOption.AllDirectories).Count();
		}

		private void CreateTargetDirectory(string targetDirectoryPath, bool createSourceDirectory)
		{
			Directory.CreateDirectory(targetDirectoryPath);
			var directoryPath = (createSourceDirectory) ? Path.Combine(targetDirectoryPath, "Source") : targetDirectoryPath;
			Directory.CreateDirectory(directoryPath);
			string rootPath = directoryPath;

			int nestedDirectoryCount = 0;
			while (nestedDirectoryCount < 2)
			{
				directoryPath = Path.Combine(directoryPath, string.Format("Sub-{0}", nestedDirectoryCount));
				Directory.CreateDirectory(directoryPath);

				int fileCount = 0;
				while (fileCount < 2)
				{
					var filePath = Path.Combine(directoryPath, string.Format("file-{0}.txt", fileCount));
					System.IO.File.WriteAllText(filePath, fileContents);
					fileCount++;
				}
				nestedDirectoryCount++;
			}

			this.fixtureTargetDirectoryCount = Directory.GetDirectories(rootPath, "*", SearchOption.AllDirectories).Count();
			this.fixtureTargetFileCount = Directory.GetFiles(rootPath, "*", SearchOption.AllDirectories).Count();
		}
	}
}
