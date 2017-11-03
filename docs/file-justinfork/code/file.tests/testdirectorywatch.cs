using NUnit.Framework;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.TestKit;

namespace Twenty57.Linx.Components.File.Tests
{
	[TestFixture]
	public class TestDirectoryWatch
	{
		private string testDirectory = "";
		private ServiceTester<DirectoryWatch> tester;

		[OneTimeSetUp]
		public void FixtureSetup()
		{
			if (!UriParser.IsKnownScheme("pack"))
				new System.Windows.Application();

			this.tester = new ServiceTester<DirectoryWatch>();
		}

		[SetUp]
		public void Setup()
		{
			this.testDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());
			Directory.CreateDirectory(this.testDirectory);
		}

		[TearDown]
		public void Teardown()
		{
			if (Directory.Exists(this.testDirectory))
				Directory.Delete(this.testDirectory, true);
		}

		[Test]
		public void TestGetInfo()
		{
			var parameterValues = GetParameterValues(testDirectory);
			var service = GetService(parameterValues, true, false, false, false);

			Assert.AreEqual(
				string.Format("Watching directory \"{0}\"{1}Watch for changes: No{1}Watch for creation: Yes{1}Watch for deletions: No{1}Watch for renaming: No{1}", testDirectory, Environment.NewLine),
				service.GetInfo());

			testDirectory = Path.Combine(testDirectory, "Sub");
			Directory.CreateDirectory(testDirectory);
			parameterValues = GetParameterValues(testDirectory, filter: "*temp*");
			service.Start(new ServiceContext(parameterValues));

			Assert.AreEqual(
				string.Format("Watching directory \"{0}\" with filter \"*temp*\"{1}Watch for changes: No{1}Watch for creation: Yes{1}Watch for deletions: No{1}Watch for renaming: No{1}", testDirectory, Environment.NewLine),
				service.GetInfo());

			service.Stop();
		}

		[Test]
		public void TestWatchForCreation()
		{
			Directory.CreateDirectory(Path.Combine(testDirectory, "Sub"));

			var parameterValues = GetParameterValues(testDirectory);
			var service = GetService(parameterValues, true, false, false, false);
			ServiceEventSink eventSink = new ServiceEventSink(service);
			service.Start(new ServiceContext(parameterValues));
			try
			{
				CreateFile(Path.Combine("Sub", "myfile"));
				var filePath = CreateFile();
				var results = eventSink.WaitForEvent(1);
				var args = results.First();
				Assert.AreEqual(DirectoryWatchShared.CreatedEventEventName, args.Name);
				Assert.AreEqual(Path.GetFileName(filePath), ((ChangedOutputValues)args.Data).Name);
				Assert.AreEqual(filePath, ((ChangedOutputValues)args.Data).FullPath);
			}
			finally
			{
				service.Stop();
			}
		}

		[Test]
		public void TestWatchForCreationSubdirectory()
		{
			Directory.CreateDirectory(Path.Combine(testDirectory, "Sub"));

			var parameterValues = GetParameterValues(testDirectory, includeSubdirectories: true);
			var service = GetService(parameterValues, true, false, false, false);
			ServiceEventSink eventSink = new ServiceEventSink(service);
			service.Start(new ServiceContext(parameterValues));
			try
			{
				string fileName = Path.Combine("Sub", "myfile");
				string filePath = CreateFile(fileName);
				var results = eventSink.WaitForEvent(1);
				var args = results.First();
				Assert.AreEqual(DirectoryWatchShared.CreatedEventEventName, args.Name);
				Assert.AreEqual(fileName, ((ChangedOutputValues)args.Data).Name);
				Assert.AreEqual(filePath, ((ChangedOutputValues)args.Data).FullPath);
			}
			finally
			{
				service.Stop();
			}
		}

		[Test]
		public void TestWatchForCreationFilter()
		{
			var parameterValues = GetParameterValues(testDirectory, filter: "*temp*");
			var service = GetService(parameterValues, true, false, false, false);
			ServiceEventSink eventSink = new ServiceEventSink(service);
			service.Start(new ServiceContext(parameterValues));
			try
			{
				CreateFile("Qwer");
				string filePath = CreateFile("contempt");
				var results = eventSink.WaitForEvent(1);
				var args = results.First();
				Assert.AreEqual(DirectoryWatchShared.CreatedEventEventName, args.Name);
				Assert.AreEqual(Path.GetFileName(filePath), ((ChangedOutputValues)args.Data).Name);
				Assert.AreEqual(filePath, ((ChangedOutputValues)args.Data).FullPath);
			}
			finally
			{
				service.Stop();
			}
		}

		[Test]
		public void TestWatchForChange()
		{
			var parameterValues = GetParameterValues(testDirectory);
			var service = GetService(parameterValues, false, true, false, false, new NotifyFilter { Size = true });
			var filePath = CreateFile();

			ServiceEventSink eventSink = new ServiceEventSink(service);
			service.Start(new ServiceContext(parameterValues));
			try
			{
				System.IO.File.AppendAllText(filePath, "blah");
				var results = eventSink.WaitForEvent(1);
				var args = results.First();
				Assert.AreEqual(DirectoryWatchShared.ChangedEventEventName, args.Name);
				Assert.AreEqual(Path.GetFileName(filePath), ((ChangedOutputValues)args.Data).Name);
				Assert.AreEqual(filePath, ((ChangedOutputValues)args.Data).FullPath);
			}
			finally
			{
				service.Stop();
			}
		}

		[Test]
		public void TestWatchForRename()
		{
			var parameterValues = GetParameterValues(testDirectory);
			var service = GetService(parameterValues, false, false, true, false);
			var filePath = CreateFile();

			ServiceEventSink eventSink = new ServiceEventSink(service);
			service.Start(new ServiceContext(parameterValues));
			try
			{
				var newFilePath = filePath + "new";
				System.IO.File.Move(filePath, newFilePath);
				var results = eventSink.WaitForEvent(1);
				var args = results.First();
				Assert.AreEqual(DirectoryWatchShared.RenamedEventEventName, args.Name);

				Assert.AreEqual(Path.GetFileName(filePath), ((OldChangedOutputValues)args.Data).OldName);
				Assert.AreEqual(filePath, ((OldChangedOutputValues)args.Data).OldFullPath);
				Assert.AreEqual(Path.GetFileName(newFilePath), ((OldChangedOutputValues)args.Data).Name);
				Assert.AreEqual(newFilePath, ((OldChangedOutputValues)args.Data).FullPath);
			}
			finally
			{
				service.Stop();
			}
		}

		[Test]
		public void TestWatchForDelete()
		{
			var parameterValues = GetParameterValues(testDirectory);
			var service = GetService(parameterValues, false, false, false, true);
			var filePath = CreateFile();

			ServiceEventSink eventSink = new ServiceEventSink(service);
			service.Start(new ServiceContext(parameterValues));
			try
			{
				System.IO.File.Delete(filePath);
				var results = eventSink.WaitForEvent(1);
				var args = results.First();
				Assert.AreEqual(DirectoryWatchShared.DeletedEventEventName, args.Name);
				Assert.AreEqual(Path.GetFileName(filePath), ((ChangedOutputValues)args.Data).Name);
				Assert.AreEqual(filePath, ((ChangedOutputValues)args.Data).FullPath);
			}
			finally
			{
				service.Stop();
			}
		}

		[Test]
		public void TestWatchForError()
		{
			var parameterValues = GetParameterValues(testDirectory);
			var service = GetService(parameterValues, true, false, false, false);
			ServiceEventSink eventSink = new ServiceEventSink(service);
			service.Start(new ServiceContext(parameterValues));

			Assert.IsTrue(service.Started);

			Directory.Delete(this.testDirectory, true);

			var results = eventSink.WaitForEvent(1);
			var args = results.First();
			Assert.AreEqual(DirectoryWatchShared.ErrorEventEventName, args.Name);
			Assert.AreEqual("Access is denied", args.Data);
			Thread.Sleep(200);
			Assert.IsFalse(service.Started);
		}

		[Test]
		public void TestStartWithEmptyPath()
		{
			var parameterValues = GetParameterValues(string.Empty);
			var service = GetService(parameterValues, false, false, false, true);

			Assert.That(() => service.Start(new ServiceContext(parameterValues)),
				Throws.Exception.TypeOf<Exception>()
				.With.Property("Message").EqualTo("The directory to watch has not been specified."));

			Assert.IsFalse(service.Started);
		}

		[Test]
		public void TestStartWithInvalidPath()
		{
			var parameterValues = GetParameterValues(@"Z:\Nowhere");
			var service = GetService(parameterValues, false, false, false, true);

			Assert.That(() => service.Start(new ServiceContext(parameterValues)),
				Throws.Exception.TypeOf<DirectoryNotFoundException>()
				.With.Property("Message").EqualTo(@"The directory to watch [Z:\Nowhere] does not exist."));

			Assert.IsFalse(service.Started);
		}

		[Test]
		public void TestStartStop()
		{
			var parameterValues = GetParameterValues(testDirectory);
			var service = GetService(parameterValues, true, false, false, false);
			Assert.IsFalse(service.Started);
			service.Start(new ServiceContext(parameterValues));
			Assert.IsTrue(service.Started);
			service.Stop();
			Assert.IsFalse(service.Started);
		}

		private IService GetService(string path, bool watchForCreate, bool watchForChanges, bool watchForRename, bool watchForDelete, NotifyFilter notifyFilter = null, string filter = null, bool includeSubdirectories = false)
		{
			return GetService(GetParameterValues(path, filter, includeSubdirectories), watchForCreate, watchForChanges, watchForRename, watchForDelete, notifyFilter);
		}

		private IService GetService(ParameterValue[] parameterValues, bool watchForCreate, bool watchForChanges, bool watchForRename, bool watchForDelete, NotifyFilter notifyFilter = null)
		{
			var designer = tester.CreateDesigner();
			designer.Properties[DirectoryWatchShared.WatchForChangesPropertyName].Value = watchForChanges;
			designer.Properties[DirectoryWatchShared.WatchForCreationPropertyName].Value = watchForCreate;
			designer.Properties[DirectoryWatchShared.WatchForDeletionsPropertyName].Value = watchForDelete;
			designer.Properties[DirectoryWatchShared.WatchForRenamingPropertyName].Value = watchForRename;
			if (notifyFilter != null)
				designer.Properties[DirectoryWatchShared.NotifyFilterPropertyName].Value = notifyFilter;

			return tester.Compile(designer).Execute(parameterValues);
		}

		private ParameterValue[] GetParameterValues(string path, string filter = null, bool includeSubdirectories = false)
		{
			return new ParameterValue[] {
				new ParameterValue(DirectoryWatchShared.BufferSizePropertyName, 8192),
				new ParameterValue(DirectoryWatchShared.FilterPropertyName, filter ?? string.Empty),
				new ParameterValue(DirectoryWatchShared.IncludeSubdirectoriesPropertyName, includeSubdirectories),
				new ParameterValue(DirectoryWatchShared.PathPropertyName, path)
			};
		}

		private string CreateFile(string fileName = null)
		{
			var filePath = Path.Combine(this.testDirectory, fileName ?? Guid.NewGuid().ToString());
			System.IO.File.Create(filePath).Close();
			return filePath;
		}
	}
}
