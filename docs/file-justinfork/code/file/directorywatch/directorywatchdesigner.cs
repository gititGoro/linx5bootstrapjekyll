using System;
using System.Linq;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.Types;
using Twenty57.Linx.Plugin.Common.Validation;
using Twenty57.Linx.Plugin.UI.Editors;

namespace Twenty57.Linx.Components.File
{
	public class DirectoryWatchDesigner : ServiceDesigner
	{
		private const string watchOptionsCategoryName = "Watch options";

		public DirectoryWatchDesigner(IDesignerContext context)
			: base(context)
		{
			Version = ServiceUpdater.Instance.CurrentVersion;

			Events.Add(new Event
			{
				Id = DirectoryWatchShared.ErrorEventEventName,
				Name = DirectoryWatchShared.ErrorEventEventName,
				DataType = TypeReference.Create(typeof(string))
			});

			Properties.Add(new Property(DirectoryWatchShared.BufferSizePropertyName, typeof(int), ValueUseOption.RuntimeRead, 8192));
			Properties.Add(new Property(DirectoryWatchShared.FilterPropertyName, typeof(string), ValueUseOption.RuntimeRead, string.Empty));
			Properties.Add(new Property(DirectoryWatchShared.NotifyFilterPropertyName, typeof(NotifyFilter), ValueUseOption.DesignTime, new NotifyFilter()));
			Properties.Add(new Property(DirectoryWatchShared.PathPropertyName, typeof(string), ValueUseOption.RuntimeRead, string.Empty));
			Properties.Add(new Property(DirectoryWatchShared.IncludeSubdirectoriesPropertyName, typeof(bool), ValueUseOption.RuntimeRead, false));

			Properties.Add(new Property(DirectoryWatchShared.WatchForChangesPropertyName, typeof(bool), ValueUseOption.DesignTime, false) { Category = watchOptionsCategoryName });
			Properties.Add(new Property(DirectoryWatchShared.WatchForCreationPropertyName, typeof(bool), ValueUseOption.DesignTime, false) { Category = watchOptionsCategoryName });
			Properties.Add(new Property(DirectoryWatchShared.WatchForDeletionsPropertyName, typeof(bool), ValueUseOption.DesignTime, false) { Category = watchOptionsCategoryName });
			Properties.Add(new Property(DirectoryWatchShared.WatchForRenamingPropertyName, typeof(bool), ValueUseOption.DesignTime, false) { Category = watchOptionsCategoryName });

			SetPropertyAttributes();
			Properties[DirectoryWatchShared.WatchForCreationPropertyName].Value = true;
		}

		public DirectoryWatchDesigner(IServiceData data, IDesignerContext context)
			: base(data, context)
		{
			SetPropertyAttributes();
		}

		private void SetPropertyAttributes()
		{
			var bufferSizeProperty = Properties[DirectoryWatchShared.BufferSizePropertyName];
			bufferSizeProperty.Description = "Size of buffer to keep operating system notifications in.";

			var filterProperty = Properties[DirectoryWatchShared.FilterPropertyName];
			filterProperty.Description = "Filters for specified files.  Wildcards are accepted. If not specified, all files will be watched.";

			var notifyFilterProperty = Properties[DirectoryWatchShared.NotifyFilterPropertyName];
			notifyFilterProperty.Description = "Configure the type of changes to watch for.";

			var pathProperty = Properties[DirectoryWatchShared.PathPropertyName];
			pathProperty.Description = "Path of directory to watch.";
			pathProperty.Editor = typeof(DirectoryPathEditor);
			pathProperty.Validations.Add(new RequiredValidator());

			var includeSubDirsProperty = Properties[DirectoryWatchShared.IncludeSubdirectoriesPropertyName];
			includeSubDirsProperty.Description = "If set to true, subdirectories will also be evaluated.";

			var watchForChangesProperty = Properties[DirectoryWatchShared.WatchForChangesPropertyName];
			watchForChangesProperty.Category = watchOptionsCategoryName;
			watchForChangesProperty.ValueChanged += OnChangeChanged;

			var watchForChangesValue = watchForChangesProperty.GetValue<bool>();
			notifyFilterProperty.IsVisible = watchForChangesValue;

			var watchForCreationProperty = Properties[DirectoryWatchShared.WatchForCreationPropertyName];
			watchForCreationProperty.Category = watchOptionsCategoryName;
			watchForCreationProperty.ValueChanged += OnCreationChanged;

			var watchForDeletionProperty = Properties[DirectoryWatchShared.WatchForDeletionsPropertyName];
			watchForDeletionProperty.Category = watchOptionsCategoryName;
			watchForDeletionProperty.ValueChanged += OnDeletionChanged;

			var watchForRenamingProperty = Properties[DirectoryWatchShared.WatchForRenamingPropertyName];
			watchForRenamingProperty.Category = watchOptionsCategoryName;
			watchForRenamingProperty.ValueChanged += OnRenameChanged;
		}

		private void OnChangeChanged(object sender, EventArgs eventArgs)
		{
			var prop = sender as Property;

			var show = (bool)prop.Value;

			AlterEvent(DirectoryWatchShared.ChangedEventEventName, show);

			var notifyFilterProperty = Properties[DirectoryWatchShared.NotifyFilterPropertyName];
			notifyFilterProperty.IsVisible = show;
		}

		private void OnCreationChanged(object sender, EventArgs eventArgs)
		{
			var prop = sender as Property;

			var show = (bool)prop.Value;

			AlterEvent(DirectoryWatchShared.CreatedEventEventName, show);
		}

		private void OnDeletionChanged(object sender, EventArgs eventArgs)
		{
			var prop = sender as Property;

			var show = (bool)prop.Value;

			AlterEvent(DirectoryWatchShared.DeletedEventEventName, show);
		}

		private void OnRenameChanged(object sender, EventArgs eventArgs)
		{
			var prop = sender as Property;

			var show = (bool)prop.Value;

			AlterEvent(DirectoryWatchShared.RenamedEventEventName, show);
		}

		private void AlterEvent(string name, bool show)
		{
			if (show)
			{
				var nextEvent = Events.FirstOrDefault(e => String.Compare(e.Name, name, StringComparison.InvariantCultureIgnoreCase) == 1);

				var index = nextEvent != null ? Events.IndexOf(nextEvent) : Events.Count - 1;

				if (Events.All(x => x.Name != name))
				{
					var outputType = typeof(ChangedOutputValues);
					if (name == DirectoryWatchShared.RenamedEventEventName)
						outputType = typeof(OldChangedOutputValues);

					Events.Insert(index, new Event
					{
						Id = name,
						Name = name,
						DataType = TypeReference.Create(outputType)
					});
				}
			}
			else
			{
				var @event = Events.FirstOrDefault(x => x.Name == name);
				if (@event != null)
					Events.Remove(@event);
			}
		}
	}
}