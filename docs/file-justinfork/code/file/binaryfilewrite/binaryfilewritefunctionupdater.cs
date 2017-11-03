using System;
using System.Linq;
using Twenty57.Linx.Components.File.Common;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.Types;
using Twenty57.Linx.Plugin.Utilities;

namespace Twenty57.Linx.Components.File
{
	internal class BinaryFileWriteFunctionUpdater : IFunctionUpdater
	{
		private static BinaryFileWriteFunctionUpdater instance;

		public string CurrentVersion { get { return "1"; } }

		public static IFunctionUpdater Instance
		{
			get
			{
				if (instance == null)
					instance = new BinaryFileWriteFunctionUpdater();
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
			IPropertyData oldFilePathProperty = data.FindPropertyById("File Path");
			IPropertyData filePathProperty = data.FindPropertyById(FileShared.FilePathPropertyName);

			IPropertyData propertyToReplace = null;
			object propertyValue = null;

			if (filePathProperty != null)
			{
				propertyToReplace = filePathProperty;

				if (filePathProperty.Value is IExpression)
					propertyValue = filePathProperty.Value;
				else if (filePathProperty.Value is string)
					propertyValue = (BinaryFileHandle)filePathProperty.Value.ToString();
				else
					propertyValue = filePathProperty.Value;
			}
			else if (oldFilePathProperty != null)
			{
				propertyToReplace = oldFilePathProperty;

				if (oldFilePathProperty.Value is IExpression)
					propertyValue = oldFilePathProperty.Value;
				else if (oldFilePathProperty.Value is string)
					propertyValue = (BinaryFileHandle)oldFilePathProperty.Value.ToString();
				else
					propertyValue = oldFilePathProperty.Value;
			}

			return data
				.ReplaceProperty(propertyToReplace, new Property(FileShared.FilePathPropertyName, typeof(BinaryFileHandle), ValueUseOption.RuntimeRead, null) { Value = propertyValue })
				.UpdateVersion("1");
		}
	}
}