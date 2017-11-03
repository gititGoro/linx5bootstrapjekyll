using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Twenty57.Linx.Plugin.TestKit;

namespace Twenty57.Linx.Components.File.Tests
{
	[TestFixture]
	public class TestRobocopy
	{
		private const string fileContents = "Linx";

		private string fixtureBaseFolderPath, fixtureDestinationFolderPath, fixtureSourceFolderPath, fixtureTestFilePath, fixtureEmptyDirectory;

		private FunctionTester<Robocopy> functionTester;
		private PropertyValue[] propertyValues;
		private ParameterValue[] parameterValues;

		[SetUp]
		public void Setup()
		{
			//TestRoboCopy1
			//  Source
			//      TestFile1.txt
			//      TestFile2.txt
			//      TestFile3.doc
			//      EmptyDirectorySource
			//  Destination
			//      EmptyDirectoryDest

			fixtureBaseFolderPath = Path.Combine(Path.GetTempPath(), "TestRobocopy");
			if (Directory.Exists(fixtureBaseFolderPath))
				Directory.Delete(fixtureBaseFolderPath, true);
			fixtureSourceFolderPath = Path.Combine(fixtureBaseFolderPath, "Source");
			Directory.CreateDirectory(fixtureSourceFolderPath);

			fixtureTestFilePath = Path.Combine(fixtureSourceFolderPath, "TestFile1.txt");
			System.IO.File.WriteAllText(fixtureTestFilePath, fileContents);

			fixtureTestFilePath = Path.Combine(fixtureSourceFolderPath, "TestFile2.txt");
			System.IO.File.WriteAllText(fixtureTestFilePath, fileContents);

			fixtureTestFilePath = Path.Combine(fixtureSourceFolderPath, "TestFile3.doc");
			System.IO.File.WriteAllText(fixtureTestFilePath, fileContents);

			fixtureEmptyDirectory = Path.Combine(fixtureSourceFolderPath, "EmptyDirectorySource");
			Directory.CreateDirectory(fixtureEmptyDirectory);

			fixtureDestinationFolderPath = Path.Combine(fixtureBaseFolderPath, "Destination");
			Directory.CreateDirectory(fixtureDestinationFolderPath);
			Directory.CreateDirectory(Path.Combine(fixtureDestinationFolderPath, "EmptyDirectoryDest"));

			this.functionTester = new FunctionTester<Robocopy>();

			this.propertyValues = new PropertyValue[]
			{
					new PropertyValue(RobocopyShared.ModePropertyName, RobocopyShared.ModeOption.Mirror)
				, new PropertyValue(RobocopyShared.CopySubdirectoriesPropertyName, true)
				, new PropertyValue(RobocopyShared.IncludeEmptySubdirectoriesPropertyName, false)
				, new PropertyValue(RobocopyShared.RestartModePropertyName, false)
				, new PropertyValue(RobocopyShared.BackupModePropertyName, false)
				, new PropertyValue(RobocopyShared.ExcludesChangedFilesPropertyName, false)
				, new PropertyValue(RobocopyShared.ExcludesNewerFilesPropertyName, false)
				, new PropertyValue(RobocopyShared.ExcludesOlderFilesPropertyName, false)
				, new PropertyValue(RobocopyShared.ExcludesExtraFilesAndDirectoriesPropertyName, false)
				, new PropertyValue(RobocopyShared.ExcludesLonelyFilesAndDirectoriesPropertyName, false)
				, new PropertyValue(RobocopyShared.IncludesSameFilesPropertyName, false)
				, new PropertyValue(RobocopyShared.IncludesTweakedFilesPropertyName, false)
				, new PropertyValue(RobocopyShared.OverwriteFilePropertyName, false)
				, new PropertyValue(RobocopyShared.ListFilesOnlyPropertyName, false)
				, new PropertyValue(RobocopyShared.LogAllExtraFilesPropertyName, false)
				, new PropertyValue(RobocopyShared.VerbosePropertyName, false)
				, new PropertyValue(RobocopyShared.IncludeSourceFileTimestampsPropertyName, false)
				, new PropertyValue(RobocopyShared.IncludeFullPathPropertyName, false)
				, new PropertyValue(RobocopyShared.LogSizeAsBytesPropertyName, false)
				, new PropertyValue(RobocopyShared.ExcludeFileClassPropertyName, false)
				, new PropertyValue(RobocopyShared.ExcludeFileNamesPropertyName, false)
				, new PropertyValue(RobocopyShared.ExcludeDirectoryNamesPropertyName, false)
				, new PropertyValue(RobocopyShared.ExcludeProgressPropertyName, false)
				, new PropertyValue(RobocopyShared.IncludeETAPropertyName, false)
			};

			this.parameterValues = new ParameterValue[]
			{
				new ParameterValue(RobocopyShared.SourceDirectoryPropertyName, string.Empty)
				, new ParameterValue(RobocopyShared.TargetDirectoryPropertyName, string.Empty)
				, new ParameterValue(RobocopyShared.NumberOfRetriesPropertyName, 10)
				, new ParameterValue(RobocopyShared.TimeBetweenRetriesPropertyName, 10)
				, new ParameterValue(RobocopyShared.FilePatternPropertyName, string.Empty)
				, new ParameterValue(RobocopyShared.ExcludeFilesPropertyName, string.Empty)
				, new ParameterValue(RobocopyShared.ExcludeDirectoriesPropertyName, string.Empty)
				, new ParameterValue(RobocopyShared.MaxFileSizePropertyName, 0)
				, new ParameterValue(RobocopyShared.MinFileSizePropertyName, 0)
				, new ParameterValue(RobocopyShared.MaxAgePropertyName, string.Empty)
				, new ParameterValue(RobocopyShared.MinAgePropertyName, string.Empty)
				, new ParameterValue(RobocopyShared.MaxLastAccessDatePropertyName, string.Empty)
				, new ParameterValue(RobocopyShared.MinLastAccessDatePropertyName, string.Empty)
				, new ParameterValue(RobocopyShared.LogFilePropertyName, string.Empty)
			};
		}

		[OneTimeTearDown]
		public void TeardownFixture()
		{
			if (Directory.Exists(fixtureBaseFolderPath))
				Directory.Delete(fixtureBaseFolderPath, true);

			if (Directory.Exists(fixtureDestinationFolderPath))
				Directory.Delete(fixtureDestinationFolderPath, true);
		}

		[Test]
		[TestCase(RobocopyShared.ModeOption.Copy)]
		[TestCase(RobocopyShared.ModeOption.Mirror)]
		[TestCase(RobocopyShared.ModeOption.MoveFiles)]
		[TestCase(RobocopyShared.ModeOption.MoveFilesAndDirs)]
		public void TestExecuteWithIncludeEmptySubDirectoriesTrue(RobocopyShared.ModeOption modeOptions)
		{
			SetPropertyValue(RobocopyShared.IncludeEmptySubdirectoriesPropertyName, true);
			SetParameterValue(RobocopyShared.SourceDirectoryPropertyName, fixtureSourceFolderPath);
			SetParameterValue(RobocopyShared.TargetDirectoryPropertyName, fixtureDestinationFolderPath);
			SetPropertyValue(RobocopyShared.ModePropertyName, modeOptions);

			List<dynamic> result = Execute();
			Assert.IsTrue(Directory.Exists(fixtureDestinationFolderPath));
			Assert.AreEqual(3, Directory.GetFiles(fixtureDestinationFolderPath).Count());

			if (modeOptions == RobocopyShared.ModeOption.Mirror)
				Assert.AreEqual(1, Directory.GetDirectories(fixtureDestinationFolderPath).Count());
			else
				Assert.AreEqual(2, Directory.GetDirectories(fixtureDestinationFolderPath).Count());
		}

		[Test]
		[TestCase(RobocopyShared.ModeOption.Copy)]
		[TestCase(RobocopyShared.ModeOption.Mirror)]
		[TestCase(RobocopyShared.ModeOption.MoveFiles)]
		[TestCase(RobocopyShared.ModeOption.MoveFilesAndDirs)]
		public void TestExecuteWithIncludeEmptySubDirectoriesFalse(RobocopyShared.ModeOption modeOptions)
		{
			SetPropertyValue(RobocopyShared.IncludeEmptySubdirectoriesPropertyName, false);
			SetParameterValue(RobocopyShared.SourceDirectoryPropertyName, fixtureSourceFolderPath);
			SetParameterValue(RobocopyShared.TargetDirectoryPropertyName, fixtureDestinationFolderPath);
			SetPropertyValue(RobocopyShared.ModePropertyName, modeOptions);

			List<dynamic> result = Execute();
			Assert.IsTrue(Directory.Exists(fixtureDestinationFolderPath));
			Assert.AreEqual(3, Directory.GetFiles(fixtureDestinationFolderPath).Count());
			if (modeOptions != RobocopyShared.ModeOption.Mirror)
				Assert.AreEqual(1, Directory.GetDirectories(fixtureDestinationFolderPath).Count());
			else
				Assert.AreEqual(0, Directory.GetDirectories(fixtureDestinationFolderPath).Count());
		}

		[Test]
		[TestCase(RobocopyShared.ModeOption.Copy)]
		[TestCase(RobocopyShared.ModeOption.Mirror)]
		[TestCase(RobocopyShared.ModeOption.MoveFiles)]
		[TestCase(RobocopyShared.ModeOption.MoveFilesAndDirs)]
		public void TestExecuteWithFilePatternAsTextFiles(RobocopyShared.ModeOption modeOptions)
		{
			SetPropertyValue(RobocopyShared.IncludeEmptySubdirectoriesPropertyName, false);
			SetParameterValue(RobocopyShared.SourceDirectoryPropertyName, fixtureSourceFolderPath);
			SetParameterValue(RobocopyShared.TargetDirectoryPropertyName, fixtureDestinationFolderPath);
			SetPropertyValue(RobocopyShared.ModePropertyName, modeOptions);
			SetParameterValue(RobocopyShared.FilePatternPropertyName, "*.txt");

			List<dynamic> result = Execute();
			Assert.IsTrue(Directory.Exists(fixtureDestinationFolderPath));
			Assert.AreEqual(2, Directory.GetFiles(fixtureDestinationFolderPath).Count());
			if (modeOptions != RobocopyShared.ModeOption.Mirror)
				Assert.AreEqual(1, Directory.GetDirectories(fixtureDestinationFolderPath).Count());
			else
				Assert.AreEqual(0, Directory.GetDirectories(fixtureDestinationFolderPath).Count());
		}

		[Test]
		[TestCase(RobocopyShared.ModeOption.Copy)]
		[TestCase(RobocopyShared.ModeOption.Mirror)]
		[TestCase(RobocopyShared.ModeOption.MoveFiles)]
		[TestCase(RobocopyShared.ModeOption.MoveFilesAndDirs)]
		public void TestExecuteWithLogFile(RobocopyShared.ModeOption modeOptions)
		{
			string logFilePath = Path.Combine(this.fixtureBaseFolderPath, "robocopy_log.txt");

			SetPropertyValue(RobocopyShared.IncludeEmptySubdirectoriesPropertyName, false);
			SetParameterValue(RobocopyShared.SourceDirectoryPropertyName, fixtureSourceFolderPath);
			SetParameterValue(RobocopyShared.TargetDirectoryPropertyName, fixtureDestinationFolderPath);
			SetPropertyValue(RobocopyShared.ModePropertyName, modeOptions);
			SetParameterValue(RobocopyShared.LogFilePropertyName, logFilePath);
			SetParameterValue(RobocopyShared.FilePatternPropertyName, "*.txt");

			List<dynamic> result = Execute();

			Assert.IsTrue(Directory.Exists(fixtureDestinationFolderPath));
			Assert.AreEqual(2, Directory.GetFiles(fixtureDestinationFolderPath).Count());
			if (modeOptions == RobocopyShared.ModeOption.Mirror)
				Assert.AreEqual(0, Directory.GetDirectories(fixtureDestinationFolderPath).Count());
			else
				Assert.AreEqual(1, Directory.GetDirectories(fixtureDestinationFolderPath).Count());
			Assert.IsTrue(System.IO.File.Exists(logFilePath));
			Assert.That(System.IO.File.ReadAllText(logFilePath), Is.Not.Null.Or.Empty);
		}

		[Test]
		[TestCase(RobocopyShared.ModeOption.Copy)]
		[TestCase(RobocopyShared.ModeOption.Mirror)]
		[TestCase(RobocopyShared.ModeOption.MoveFiles)]
		[TestCase(RobocopyShared.ModeOption.MoveFilesAndDirs)]
		public void TestExecuteWithIncludeEmptySubDirectoriesFalseBackupMode(RobocopyShared.ModeOption modeOptions)
		{
			SetPropertyValue(RobocopyShared.IncludeEmptySubdirectoriesPropertyName, false);
			SetParameterValue(RobocopyShared.SourceDirectoryPropertyName, fixtureSourceFolderPath);
			SetParameterValue(RobocopyShared.TargetDirectoryPropertyName, fixtureDestinationFolderPath);
			SetPropertyValue(RobocopyShared.ModePropertyName, modeOptions);

			if (!Directory.Exists(Path.Combine(this.fixtureSourceFolderPath, "NewDirectory")))
				Directory.CreateDirectory(Path.Combine(this.fixtureSourceFolderPath, "NewDirectory"));
			List<dynamic> result = Execute();
			Assert.IsTrue(Directory.Exists(fixtureDestinationFolderPath));

			if (modeOptions != RobocopyShared.ModeOption.Mirror)
				Assert.AreEqual(1, Directory.GetDirectories(fixtureDestinationFolderPath).Count());
			else
				Assert.AreEqual(0, Directory.GetDirectories(fixtureDestinationFolderPath).Count());
		}

		[Test]
		[TestCase(RobocopyShared.ModeOption.Copy)]
		[TestCase(RobocopyShared.ModeOption.Mirror)]
		[TestCase(RobocopyShared.ModeOption.MoveFiles)]
		[TestCase(RobocopyShared.ModeOption.MoveFilesAndDirs)]
		public void TestExecuteWithSourceFolderIsEmpty(RobocopyShared.ModeOption modeOptions)
		{
			SetPropertyValue(RobocopyShared.IncludeEmptySubdirectoriesPropertyName, true);
			SetParameterValue(RobocopyShared.TargetDirectoryPropertyName, fixtureDestinationFolderPath);
			SetPropertyValue(RobocopyShared.ModePropertyName, modeOptions);

			Assert.That(() => Execute(),
				Throws.Exception.TypeOf<ExecuteException>()
				.With.Property("Message").EqualTo("Source directory cannot be null or empty.\r\nSee Code and Parameter properties for more information."));
		}

		[Test]
		[TestCase(RobocopyShared.ModeOption.Copy)]
		[TestCase(RobocopyShared.ModeOption.Mirror)]
		[TestCase(RobocopyShared.ModeOption.MoveFiles)]
		[TestCase(RobocopyShared.ModeOption.MoveFilesAndDirs)]
		public void TestExecuteWithListFiles(RobocopyShared.ModeOption modeOptions)
		{
			SetPropertyValue(RobocopyShared.ListFilesOnlyPropertyName, true);
			SetParameterValue(RobocopyShared.SourceDirectoryPropertyName, fixtureSourceFolderPath);
			SetParameterValue(RobocopyShared.TargetDirectoryPropertyName, fixtureDestinationFolderPath);
			SetPropertyValue(RobocopyShared.ModePropertyName, modeOptions);

			if (!Directory.Exists(Path.Combine(this.fixtureSourceFolderPath, "NewDirectory")))
				Directory.CreateDirectory(Path.Combine(this.fixtureSourceFolderPath, "NewDirectory"));
			List<dynamic> result = Execute();
			Assert.IsTrue(Directory.Exists(fixtureDestinationFolderPath));
			Assert.AreEqual(0, Directory.GetFiles(fixtureDestinationFolderPath).Count());
			Assert.AreEqual(1, Directory.GetDirectories(fixtureDestinationFolderPath).Count());
		}

		[Test]
		[TestCase(RobocopyShared.ModeOption.Copy)]
		[TestCase(RobocopyShared.ModeOption.Mirror)]
		[TestCase(RobocopyShared.ModeOption.MoveFiles)]
		[TestCase(RobocopyShared.ModeOption.MoveFilesAndDirs)]
		public void TestExecuteWithListFilesSourceDirectoryDoesNotExists(RobocopyShared.ModeOption modeOptions)
		{
			SetPropertyValue(RobocopyShared.ListFilesOnlyPropertyName, true);
			SetParameterValue(RobocopyShared.SourceDirectoryPropertyName, Path.Combine(Path.GetTempPath(), "newfolder"));
			SetParameterValue(RobocopyShared.TargetDirectoryPropertyName, fixtureDestinationFolderPath);
			SetPropertyValue(RobocopyShared.ModePropertyName, modeOptions);

			if (!Directory.Exists(Path.Combine(this.fixtureSourceFolderPath, "NewDirectory")))
				Directory.CreateDirectory(Path.Combine(this.fixtureSourceFolderPath, "NewDirectory"));

			Assert.That(() => Execute(),
				Throws.Exception.TypeOf<ExecuteException>()
				.With.Property("Message").EqualTo(string.Format("Source directory [{0}] does not exist.\r\nSee Code and Parameter properties for more information.", Path.Combine(Path.GetTempPath(), "newfolder"))));
		}

		[Test]
		[TestCase(RobocopyShared.ModeOption.Copy)]
		[TestCase(RobocopyShared.ModeOption.Mirror)]
		[TestCase(RobocopyShared.ModeOption.MoveFiles)]
		[TestCase(RobocopyShared.ModeOption.MoveFilesAndDirs)]
		public void TestExecuteWithInvalidParameter(RobocopyShared.ModeOption modeOptions)
		{
			SetPropertyValue(RobocopyShared.ListFilesOnlyPropertyName, true);
			SetParameterValue(RobocopyShared.SourceDirectoryPropertyName, fixtureSourceFolderPath);
			SetParameterValue(RobocopyShared.TargetDirectoryPropertyName, fixtureDestinationFolderPath);
			SetPropertyValue(RobocopyShared.ModePropertyName, modeOptions);
			SetParameterValue(RobocopyShared.MaxAgePropertyName, "-1");

			if (!Directory.Exists(Path.Combine(this.fixtureSourceFolderPath, "NewDirectory")))
				Directory.CreateDirectory(Path.Combine(this.fixtureSourceFolderPath, "NewDirectory"));

			Assert.That(() => Execute(),
				Throws.Exception.TypeOf<ExecuteException>()
				.With.Property("Message").Contains("Robocopy error: ERROR : Invalid Parameter"));
		}

		[Test]
		[TestCase(RobocopyShared.ModeOption.Copy)]
		[TestCase(RobocopyShared.ModeOption.Mirror)]
		[TestCase(RobocopyShared.ModeOption.MoveFiles)]
		[TestCase(RobocopyShared.ModeOption.MoveFilesAndDirs)]
		public void TestExecuteWithTrailingBackslash(RobocopyShared.ModeOption modeOptions)
		{
			SetPropertyValue(RobocopyShared.ListFilesOnlyPropertyName, true);
			SetParameterValue(RobocopyShared.SourceDirectoryPropertyName, fixtureSourceFolderPath + Path.DirectorySeparatorChar);
			SetParameterValue(RobocopyShared.TargetDirectoryPropertyName, fixtureDestinationFolderPath + Path.DirectorySeparatorChar);
			SetPropertyValue(RobocopyShared.ModePropertyName, modeOptions);

			if (!Directory.Exists(Path.Combine(this.fixtureSourceFolderPath, "NewDirectory")))
				Directory.CreateDirectory(Path.Combine(this.fixtureSourceFolderPath, "NewDirectory"));
			List<dynamic> result = Execute();
			Assert.IsTrue(Directory.Exists(fixtureDestinationFolderPath));
			Assert.AreEqual(0, Directory.GetFiles(fixtureDestinationFolderPath).Count());
			Assert.AreEqual(1, Directory.GetDirectories(fixtureDestinationFolderPath).Count());
		}

		private void SetPropertyValue(string propertyName, object value)
		{
			propertyValues.Single(p => p.Name == propertyName).Value = value;
		}

		private void SetParameterValue(string parameterName, object value)
		{
			parameterValues.Single(p => p.Name == parameterName).Value = value;
		}

		private List<dynamic> Execute()
		{
			dynamic dataOut = this.functionTester.Execute(this.propertyValues, this.parameterValues);
			if (dataOut.ExecutionPathResult != null)
				return new List<dynamic>(dataOut.ExecutionPathResult[0].Value);
			else
				return null;
		}
	}
}
