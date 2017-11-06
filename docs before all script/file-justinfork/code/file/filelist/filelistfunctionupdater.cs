using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Twenty57.Linx.Components.File.Common;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Utilities;

namespace Twenty57.Linx.Components.File
{
	public class FileListFunctionUpdater : IFunctionUpdater
	{
		private static FileListFunctionUpdater instance;

		public string CurrentVersion
		{
			get
			{
				return "1";
			}
		}

		public static IFunctionUpdater Instance
		{
			get
			{
				if (instance == null)
					instance = new FileListFunctionUpdater();
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
				.RenameProperty("File path", FileListShared.FolderPathPropertyName)
				.UpdateVersion("1");
		}
	}
}
