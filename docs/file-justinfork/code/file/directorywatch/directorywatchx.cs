using System;
using System.Text;
using System.IO;
using Twenty57.Linx.Plugin.Common;
using System.Windows.Media.Imaging;
using System.Windows.Media;
using System.Collections.Generic;
using Twenty57.Linx.Components.File;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Twenty57.Linx.Components.File
{
	public class DirectoryWatchX : IService
	{
		private string path = "";
		private string filter = "";
		private bool includeSubDirs;

		private int bufferSize = 8192;

		private readonly NotifyFilter notifyFilter = new NotifyFilter();

		private readonly bool watchChanged;
		private readonly bool watchCreated;
		private readonly bool watchDeleted;
		private readonly bool watchRenamed;

		private ConcurrentQueue<FileSystemEventArgs> fileEventsToProcess = null;
		private CancellationTokenSource fileEventsProcessingCancellationToken = null;
		private EventWaitHandle waitHandle;

		private FileSystemWatcher watcher;
		private object watcherLock = new object();

		public DirectoryWatchX(string path, string filter, bool includeSubDirectories, int bufferSize, NotifyFilter notifyFilter,
			bool watchForChanges, bool watchForCreation, bool watchForDeletions, bool watchForRenaming)
		{
			this.path = path;
			this.filter = filter;
			this.includeSubDirs = includeSubDirectories;
			this.bufferSize = bufferSize;
			this.notifyFilter = notifyFilter;
			this.watchChanged = watchForChanges;
			this.watchCreated = watchForCreation;
			this.watchDeleted = watchForDeletions;
			this.watchRenamed = watchForRenaming;
		}

		public bool Started
		{
			get { return this.watcher != null; }
		}

		public void Start(IServiceContext context = null)
		{
			lock (this.watcherLock)
			{
				if (Started)
					return;

				UpdateServiceValuesFromContext(context);

				ValidateWatchPath(path);

				StartFileEventsProcessor();

				watcher = new FileSystemWatcher(path);
				if (filter.Length > 0)
					watcher.Filter = filter;
				watcher.InternalBufferSize = bufferSize;
				watcher.IncludeSubdirectories = includeSubDirs;
				watcher.NotifyFilter = notifyFilter.CalcFilter();

				SubscribeToEvents();
				watcher.Error += WatchError;
				watcher.EnableRaisingEvents = true;
			}
		}

		public void Stop()
		{
			lock (this.watcherLock)
			{
				try
				{
					if (this.watcher == null)
						return;

					watcher.EnableRaisingEvents = false;
					UnSubscribeFromEvents();
					watcher.Error -= WatchError;
					watcher.Dispose();
					watcher = null;
					StopFileEventsProcessor();
				}
				catch (Exception e)
				{
					System.Diagnostics.Debug.WriteLine("DirectoryWatch.Stop exception: " + e.Message);
					//todo - log any errors
				}
			}
		}

		public string GetInfo()
		{
			var result = new StringBuilder();

			string watchDescription = String.Format("Watching directory \"{0}\"", this.path);
			if (!string.IsNullOrEmpty(this.filter))
				watchDescription += String.Format(" with filter \"{0}\"", this.filter);
			result.AppendLine(watchDescription);

			result.AppendLine(String.Format("Watch for changes: {0}", (this.watchChanged) ? "Yes" : "No"));
			result.AppendLine(String.Format("Watch for creation: {0}", (this.watchCreated) ? "Yes" : "No"));
			result.AppendLine(String.Format("Watch for deletions: {0}", (this.watchDeleted) ? "Yes" : "No"));
			result.AppendLine(String.Format("Watch for renaming: {0}", (this.watchRenamed) ? "Yes" : "No"));

			return result.ToString();
		}

		private void StartFileEventsProcessor()
		{
			fileEventsToProcess = new ConcurrentQueue<FileSystemEventArgs>();
			fileEventsProcessingCancellationToken = new CancellationTokenSource();
			waitHandle = new EventWaitHandle(false, EventResetMode.AutoReset);
			Task.Factory.StartNew((cancellationToken) =>
			{
				var eventsQueue = fileEventsToProcess;
				while (!((CancellationToken)cancellationToken).IsCancellationRequested)
				{
					waitHandle.WaitOne();
					FileSystemEventArgs fileEvent;
					while ((!((CancellationToken)cancellationToken).IsCancellationRequested) && (eventsQueue.TryDequeue(out fileEvent)))
						ProcessFileEvent(fileEvent);
				}
			}, fileEventsProcessingCancellationToken.Token, TaskCreationOptions.LongRunning);
		}

		private void StopFileEventsProcessor()
		{
			if (fileEventsProcessingCancellationToken != null)
			{
				fileEventsProcessingCancellationToken.Cancel();
				fileEventsProcessingCancellationToken = null;
				waitHandle.Set();
				fileEventsToProcess = null;
			}
		}

		private void SubscribeToEvents()
		{
			if (this.watchChanged)
				watcher.Changed += AcknowledgeFileEvent;

			if (this.watchCreated)
				watcher.Created += AcknowledgeFileEvent;

			if (this.watchDeleted)
				watcher.Deleted += AcknowledgeFileEvent;

			if (this.watchRenamed)
				watcher.Renamed += AcknowledgeFileEvent;
		}

		private void UnSubscribeFromEvents()
		{
			if (this.watchChanged)
				watcher.Changed -= AcknowledgeFileEvent;

			if (this.watchCreated)
				watcher.Created -= AcknowledgeFileEvent;

			if (this.watchDeleted)
				watcher.Deleted -= AcknowledgeFileEvent;

			if (this.watchRenamed)
				watcher.Renamed -= AcknowledgeFileEvent;
		}

		private void WatchError(object sender, ErrorEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine("WatchError start: " + e.GetException().Message);

			try
			{
				if (Event != null)
					Event(this, new ServiceEventArgs(DirectoryWatchShared.ErrorEventEventName, e.GetException().Message));
			}
			catch
			{ }

			Stop();
			try
			{
				Start();
			}
			catch
			{ }

			System.Diagnostics.Debug.WriteLine("WatchError finished: " + e.GetException().Message);
		}

		private void AcknowledgeFileEvent(object sender, FileSystemEventArgs e)
		{
			if (fileEventsToProcess != null)
			{
				fileEventsToProcess.Enqueue(e);
				waitHandle.Set();
			}
		}

		private void ProcessFileEvent(FileSystemEventArgs fileEvent)
		{
			System.Diagnostics.Debug.WriteLine("FileSystem: {0} start: {1}", fileEvent.ChangeType, fileEvent.FullPath);

			if (Event != null)
			{
				if (fileEvent.ChangeType == WatcherChangeTypes.Renamed)
				{
					RenamedEventArgs renamedEvent = (RenamedEventArgs)fileEvent;
					Event(this, new ServiceEventArgs(DirectoryWatchShared.RenamedEventEventName, new OldChangedOutputValues
					{
						FullPath = renamedEvent.FullPath,
						Name = renamedEvent.Name,
						OldFullPath = renamedEvent.OldFullPath,
						OldName = renamedEvent.OldName
					}));
				}
				else
				{
					string eventName = null;
					switch (fileEvent.ChangeType)
					{
						case WatcherChangeTypes.Changed: eventName = DirectoryWatchShared.ChangedEventEventName; break;
						case WatcherChangeTypes.Created: eventName = DirectoryWatchShared.CreatedEventEventName; break;
						case WatcherChangeTypes.Deleted: eventName = DirectoryWatchShared.DeletedEventEventName; break;
					}
					Event(this, new ServiceEventArgs(eventName, new ChangedOutputValues
					{
						FullPath = fileEvent.FullPath,
						Name = fileEvent.Name
					}));
				}
			}

			System.Diagnostics.Debug.WriteLine(string.Format("FileSystem: {0} finished: {1}", fileEvent.ChangeType, fileEvent.FullPath));
		}

		private void UpdateServiceValuesFromContext(IServiceContext context)
		{
			if (context == null)
				return;

			bufferSize = context.GetParameterValue<int>(DirectoryWatchShared.BufferSizePropertyName);
			filter = context.GetParameterValue<string>(DirectoryWatchShared.FilterPropertyName);
			path = context.GetParameterValue<string>(DirectoryWatchShared.PathPropertyName);
			includeSubDirs = context.GetParameterValue<bool>(DirectoryWatchShared.IncludeSubdirectoriesPropertyName);
		}

		private static void ValidateWatchPath(string path)
		{
			path = (path ?? string.Empty).Trim();

			if (string.IsNullOrEmpty(path))
				throw new Exception("The directory to watch has not been specified.");

			if (!Directory.Exists(path))
				throw new DirectoryNotFoundException(string.Format("The directory to watch [{0}] does not exist.", path));
		}

		public event ServiceEventHandler Event;
	}
}