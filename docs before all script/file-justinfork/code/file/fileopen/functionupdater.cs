using System;
using System.Collections.Generic;
using System.Linq;
using Twenty57.Linx.Components.File.Common;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.Types;
using Twenty57.Linx.Plugin.Utilities;

namespace Twenty57.Linx.Components.File.FileOpen
{
	public class FunctionUpdater : IFunctionUpdater
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
			bool isText = data.Properties[FileOpenShared.IsTextPropertyName].GetValue<bool>();
			var updatedExecutionPath = new ExecutionPath
			{
				Key = FileOpenShared.ExecutionPathName,
				Name = FileOpenShared.ExecutionPathName,
				Output = TypeReference.CreateGeneratedType(
					new TypeProperty(FileOpenShared.OutputFilePathPropertyName, typeof(string), AccessType.Read),
					new TypeProperty(FileOpenShared.OutputFileHandlePropertyName, TypeReference.CreateResource(isText ? typeof(TextFileHandle) : typeof(BinaryFileHandle)), AccessType.Read)),
				IterationHint = Plugin.Common.IterationHint.Once
			};

			IExecutionPathData existingExecutionPath;
			if (data.TryFindExecutionPathByKey(FileOpenShared.ExecutionPathName, out existingExecutionPath))
				data = data.ReplaceExecutionPath(existingExecutionPath, updatedExecutionPath);
			else
				data = data.AddExecutionPath(updatedExecutionPath);

			return data.UpdateVersion("1");
		}
	}
}
