using System;
using System.Linq;
using Twenty57.Linx.Components.File.Common;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Utilities;

namespace Twenty57.Linx.Components.File
{
	internal class BinaryFileReadFunctionUpdater : IFunctionUpdater
	{
		private static BinaryFileReadFunctionUpdater instance;

		public string CurrentVersion { get { return "1"; } }

		public static IFunctionUpdater Instance
		{
			get
			{
				if (instance == null)
					instance = new BinaryFileReadFunctionUpdater();
				return instance;
			}
		}

		public IFunctionData Update(IFunctionData data)
		{
			if (data.Version == CurrentVersion)
				return data;

			if (string.IsNullOrEmpty(data.Version))
				return UpdateToVersion1(data);

			throw new Exception(string.Format("Unknown version [{0}] specified.", data.Version));
		}

		private IFunctionData UpdateToVersion1(IFunctionData data)
		{
			return data
				.RenameProperty("File Path", FileShared.FilePathPropertyName)
				.UpdateVersion("1");
		}
	}
}
