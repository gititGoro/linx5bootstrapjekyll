using System;
using System.Linq;
using Twenty57.Linx.Components.File.Common;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.Types;
using Twenty57.Linx.Plugin.Utilities;

namespace Twenty57.Linx.Components.File.TextFileWrite
{
	internal class FunctionUpdater : IFunctionUpdater
	{
		private static FunctionUpdater instance;

		public string CurrentVersion { get { return "1"; } }

		public static IFunctionUpdater Instance
		{
			get
			{
				if (instance == null)
					instance = new FunctionUpdater();
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
			IPropertyData filePathProperty = data.FindPropertyById(FileShared.FilePathPropertyName);
			object value;
			if (filePathProperty.Value is IExpression)
				value = filePathProperty.Value;
			else if (filePathProperty.Value is string)
				value = (TextFileHandle)filePathProperty.Value.ToString();
			else
				value = filePathProperty.Value;

			return data
				.ReplaceProperty(filePathProperty, new Property(FileShared.FilePathPropertyName, typeof(TextFileHandle), ValueUseOption.RuntimeRead, null) { Value = value })
				.UpdateOutput(TypeReference.Create(typeof(string)))
				.UpdateVersion("1");
		}
	}
}
