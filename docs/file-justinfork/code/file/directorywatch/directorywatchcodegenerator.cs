using System;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.CodeGeneration;

namespace Twenty57.Linx.Components.File
{
	public class DirectoryWatchCodeGenerator : ServiceCodeGenerator
	{
		public override void GenerateCode(IServiceBuilder serviceBuilder)
		{
			var data = ServiceUpdater.Instance.Update(serviceBuilder.Data);

			var notifyFilterValue = data.Properties[DirectoryWatchShared.NotifyFilterPropertyName].Value as NotifyFilter;

			var code = String.Format(
				@"return new Twenty57.Linx.Components.File.DirectoryWatchX({0}, {1}, {2}, {3}, Twenty57.Linx.Components.File.NotifyFilter.ConvertFromString(""{4}""), {5}, {6}, {7}, {8});",
				serviceBuilder.GetParamName(DirectoryWatchShared.PathPropertyName),
				serviceBuilder.GetParamName(DirectoryWatchShared.FilterPropertyName),
				serviceBuilder.GetParamName(DirectoryWatchShared.IncludeSubdirectoriesPropertyName),
				serviceBuilder.GetParamName(DirectoryWatchShared.BufferSizePropertyName),
				notifyFilterValue.ConvertToString(),
				CSharpUtilities.BoolAsString(data.Properties[DirectoryWatchShared.WatchForChangesPropertyName].GetValue<bool>()),
				CSharpUtilities.BoolAsString(data.Properties[DirectoryWatchShared.WatchForCreationPropertyName].GetValue<bool>()),
				CSharpUtilities.BoolAsString(data.Properties[DirectoryWatchShared.WatchForDeletionsPropertyName].GetValue<bool>()),
				CSharpUtilities.BoolAsString(data.Properties[DirectoryWatchShared.WatchForRenamingPropertyName].GetValue<bool>()));

			serviceBuilder.AddCode(code);

			serviceBuilder.AddAssemblyReference(typeof(DirectoryWatchX));
		}
	}
}
