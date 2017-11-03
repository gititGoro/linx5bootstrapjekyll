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
	public class TestFileList
	{
		private string path;
		private List<FileInfo> expectedFiles;

		[OneTimeSetUp]
		public void SetupFixture()
		{
			this.path = Path.Combine(Path.GetTempPath(), "TestFileList");
			this.expectedFiles = ConfigureExpectedFiles(this.path);
		}

		[OneTimeTearDown]
		public void TeardownFixture()
		{
			FileHelpers.ForceDeleteDirectory(this.path);
		}

		[Test]
		public void TestExecuteCompiled()
		{
			dynamic output = Execute("*.*", false, false, false).Value;
			Assert.AreEqual(4, output.Files.Count);
			Assert.AreEqual(4, output.NumberOfFiles);

			foreach (dynamic info in output.Files)
			{
				string name = info.FileName;

				var expected = this.expectedFiles.First(x => x.Name == name);
				Assert.AreEqual(expected.Name, info.FileName);
				Assert.AreEqual(expected.CreationTime, info.CreationTime);
				Assert.AreEqual(expected.LastAccessTime, info.LastAccessTime);
				Assert.AreEqual(expected.LastWriteTime, info.LastWriteTime);
				Assert.AreEqual(expected.Length, info.Size);
				Assert.AreEqual(expected.IsReadOnly, info.ReadOnly);
				Assert.AreEqual((expected.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden, info.Hidden);
			}
		}

		[Test]
		public void TestExecuteWithIncludeSubfolders()
		{
			dynamic output = Execute("*.*", true, false, false).Value;

			Assert.AreEqual(5, output.Files.Count);
			for (var index = 0; index < this.expectedFiles.Count; index++)
				Assert.IsTrue(this.expectedFiles.Any(info => info.Name == output.Files[index].FileName));
			Assert.AreEqual(5, output.NumberOfFiles);
		}

		[Test]
		public void TestExecuteWithOnly3CharExtension()
		{
			dynamic output = Execute("*.txt", false, false, false).Value;
			Assert.AreEqual(2, output.Files.Count);
			for (var index = 0; index < 2; index++)
				Assert.IsTrue(this.expectedFiles.Any(info => info.Name == output.Files[index].FileName));
			Assert.AreEqual(2, output.NumberOfFiles);
		}

		[Test]
		public void TestExecuteWithExtensionEndingInWildCard()
		{
			dynamic output = Execute("*.tx*", false, false, false).Value;
			Assert.AreEqual(3, output.Files.Count);
			for (var index = 0; index < 3; index++)
				Assert.IsTrue(this.expectedFiles.Any(info => info.Name == output.Files[index].FileName));
			Assert.AreEqual(3, output.NumberOfFiles);
		}

		[Test]
		public void TestExecuteWithExtensionLongerThan3Chars()
		{
			dynamic output = Execute("*.txt1*", false, false, false).Value;
			Assert.AreEqual(1, output.Files.Count);
			Assert.AreEqual(this.expectedFiles[2].Name, output.Files[0].FileName);
			Assert.AreEqual(1, output.NumberOfFiles);
		}

		[Test]
		public void TestExecuteWithoutSearchPattern()
		{
			dynamic output = Execute("", false, false, false).Value;
			Assert.AreEqual(4, output.Files.Count);
			for (var index = 0; index < 3; index++)
				Assert.IsTrue(this.expectedFiles.Any(info => info.Name == output.Files[index].FileName));
			Assert.AreEqual(4, output.NumberOfFiles);
		}

		[Test]
		public void TestExecuteWithReturnFullPathTrue()
		{
			dynamic output = Execute("*.*", false, true, false).Value;
			Assert.AreEqual(4, output.Files.Count);
			for (var index = 0; index < 3; index++)
				Assert.IsTrue(this.expectedFiles.Any(info => info.FullName == output.Files[index].FileName));
			Assert.AreEqual(4, output.NumberOfFiles);
		}

		[Test]
		public void TestExecuteWithLoopResultsTrue()
		{
			var result = Execute("*.*", false, false, true);
			Assert.AreEqual(4, result.ExecutionPathResult.Count());
			Assert.AreEqual(4, result.Value.NumberOfFiles);

			foreach (var r in result.ExecutionPathResult)
			{
				dynamic info = r.Value;
				string name = info.FileName;

				var expected = this.expectedFiles.First(x => x.Name == name);
				Assert.AreEqual(expected.Name, info.FileName);
				Assert.AreEqual(expected.CreationTime, info.CreationTime);
				Assert.AreEqual(expected.LastAccessTime, info.LastAccessTime);
				Assert.AreEqual(expected.LastWriteTime, info.LastWriteTime);
				Assert.AreEqual(expected.Length, info.Size);
				Assert.AreEqual(expected.IsReadOnly, info.ReadOnly);
				Assert.AreEqual((expected.Attributes & FileAttributes.Hidden) == FileAttributes.Hidden, info.Hidden);
			}
		}

		[Test]
		public void TestExecuteWithMultipleSearchPatterns()
		{
			dynamic output = Execute("      *.txt1 ; *.jpg  ", false, false, false).Value;
			Assert.AreEqual(2, output.Files.Count);
			Assert.AreEqual(2, output.NumberOfFiles);
			Assert.AreEqual(this.expectedFiles[2].Name, output.Files[0].FileName);
			Assert.AreEqual(this.expectedFiles[3].Name, output.Files[1].FileName);
		}

		private FunctionResult Execute(string searchPattern, bool includeSubFolders, bool returnFullPath, bool loopResults)
		{
			var tester = new FunctionTester<FileList>().Compile(
				new PropertyValue(FileListShared.IncludeSubfoldersPropertyName, includeSubFolders),
				new PropertyValue(FileListShared.ReturnFullPathPropertyName, returnFullPath),
				new PropertyValue(FileListShared.LoopResultsPropertyName, loopResults));

			return tester.Execute(new ParameterValue(FileListShared.FolderPathPropertyName, this.path),
				new ParameterValue(FileListShared.SearchPatternPropertyName, searchPattern));
		}

		private static List<FileInfo> ConfigureExpectedFiles(string basePath)
		{
			FileHelpers.ForceDeleteDirectory(basePath);

			string subPath = Path.Combine(basePath, "SubDirFileList");
			Directory.CreateDirectory(subPath);

			IList<FileInfo> expectedFiles = new List<FileInfo>();

			string rootFile1 = Path.Combine(basePath, "RootFile1.txt");
			System.IO.File.WriteAllText(rootFile1, rootFile1);
			FileInfo rootFile1Info = new FileInfo(rootFile1);
			rootFile1Info.CreationTime = DateTime.Today.AddDays(-7);
			rootFile1Info.LastWriteTime = DateTime.Today.AddDays(-6);
			rootFile1Info.LastAccessTime = DateTime.Today.AddDays(-5);
			rootFile1Info.Attributes = rootFile1Info.Attributes | FileAttributes.ReadOnly;

			string rootFile2 = Path.Combine(basePath, "RootFile2.txt");
			System.IO.File.WriteAllText(rootFile2, rootFile2);
			FileInfo rootFile2Info = new FileInfo(rootFile2);
			rootFile2Info.CreationTime = DateTime.Today.AddDays(-4);
			rootFile2Info.LastWriteTime = DateTime.Today.AddDays(-3);
			rootFile2Info.LastAccessTime = DateTime.Today.AddDays(-2);
			rootFile2Info.Attributes = rootFile2Info.Attributes | FileAttributes.Hidden;

			string rootFile3 = Path.Combine(basePath, "RootFile3.txt1");
			System.IO.File.WriteAllText(rootFile3, rootFile3);
			FileInfo rootFile3Info = new FileInfo(rootFile3);
			rootFile3Info.CreationTime = DateTime.Today;
			rootFile3Info.LastWriteTime = DateTime.Today;
			rootFile3Info.LastAccessTime = DateTime.Today;

			string rootFile4 = Path.Combine(basePath, "RootFile4.jpg");
			System.IO.File.WriteAllBytes(rootFile4, new byte[] { 1, 2, 3 });
			FileInfo rootFile4Info = new FileInfo(rootFile4);
			rootFile3Info.CreationTime = DateTime.Today;
			rootFile3Info.LastWriteTime = DateTime.Today;
			rootFile3Info.LastAccessTime = DateTime.Today;

			string subFile1 = Path.Combine(subPath, "SubFile1.txt");
			System.IO.File.WriteAllText(subFile1, subFile1);
			FileInfo subFile1Info = new FileInfo(subFile1);
			subFile1Info.CreationTime = DateTime.Today;
			subFile1Info.LastWriteTime = DateTime.Today;
			subFile1Info.LastAccessTime = DateTime.Today;

			return new List<FileInfo>
			{
				rootFile1Info,
				rootFile2Info,
				rootFile3Info,
				rootFile4Info,
				subFile1Info
			};
		}

	}
}


