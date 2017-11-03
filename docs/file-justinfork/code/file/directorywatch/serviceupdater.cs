using System;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Utilities;

namespace Twenty57.Linx.Components.File
{
	internal class ServiceUpdater : IServiceUpdater
	{
		private static ServiceUpdater instance = null;

		public static ServiceUpdater Instance
		{
			get
			{
				if (instance == null)
					instance = new ServiceUpdater();
				return instance;
			}
		}

		private ServiceUpdater() { }

		public string CurrentVersion
		{
			get { return "1"; }
		}

		public IServiceData Update(IServiceData data)
		{
			if (string.IsNullOrEmpty(data.Version))
			{
				data = data
					.ReplaceProperty(data.Properties[DirectoryWatchShared.BufferSizePropertyName], new Property(DirectoryWatchShared.BufferSizePropertyName, typeof(int), ValueUseOption.RuntimeRead, 8192) { Value = data.Properties[DirectoryWatchShared.BufferSizePropertyName].Value })
					.ReplaceProperty(data.Properties[DirectoryWatchShared.FilterPropertyName], new Property(DirectoryWatchShared.FilterPropertyName, typeof(string), ValueUseOption.RuntimeRead, string.Empty) { Value = data.Properties[DirectoryWatchShared.FilterPropertyName].Value })
					.ReplaceProperty(data.Properties[DirectoryWatchShared.IncludeSubdirectoriesPropertyName], new Property(DirectoryWatchShared.IncludeSubdirectoriesPropertyName, typeof(bool), ValueUseOption.RuntimeRead, false) { Value = data.Properties[DirectoryWatchShared.IncludeSubdirectoriesPropertyName].Value })
					.UpdateVersion("1");
			}

			if (data.Version == CurrentVersion)
				return data;

			throw new Exception(string.Format("Unknown version [{0}] specified.", data.Version));
		}
	}
}
