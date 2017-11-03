using System;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.CodeGeneration;

namespace Twenty57.Linx.Components.File.V2
{
	public class DirectoryOperationsCodeGenerator : FunctionCodeGenerator
	{
		public DirectoryOperationsCodeGenerator(IFunctionData data)
			: base(data)
		{ }

		public override void GenerateCode(Plugin.Common.CodeGeneration.IFunctionBuilder functionBuilder)
		{
			functionBuilder.AddCode(
				string.Format(@"var executor = new Twenty57.Linx.Components.File.V2.DirectoryOperationsX();
				executor.LogEvent += message => {0}.Log(message);", functionBuilder.ContextParamName));

			DirectoryOperationsShared.ActionType actionType = FunctionData.Properties[DirectoryOperationsShared.ActionPropertyName].GetValue<DirectoryOperationsShared.ActionType>();
			switch (actionType)
			{
				case DirectoryOperationsShared.ActionType.DirectoryExists:
					functionBuilder.AddCode(string.Format("return executor.DirectoryExists({0});",
						functionBuilder.GetParamName(DirectoryOperationsShared.DirectoryPropertyName)));
					break;
				case DirectoryOperationsShared.ActionType.Create:
					functionBuilder.AddCode(string.Format("return executor.CreateDirectory({0}, {1});",
						functionBuilder.GetParamName(DirectoryOperationsShared.DirectoryPropertyName),
						CSharpUtilities.EnumAsString(FunctionData.Properties[DirectoryOperationsShared.CreateDirectoryExistsPropertyName].GetValue<DirectoryOperationsShared.CreateExistsOption>())));
					break;
				case DirectoryOperationsShared.ActionType.Delete:
					functionBuilder.AddCode(string.Format("executor.DeleteDirectory({0});",
						functionBuilder.GetParamName(DirectoryOperationsShared.DirectoryPropertyName)));
					break;
				case DirectoryOperationsShared.ActionType.Copy:
				case DirectoryOperationsShared.ActionType.Move:
					functionBuilder.AddCode(string.Format(@"executor.CopyOrMove({0}, {1}, {2}, {3}, {4});",
						CSharpUtilities.EnumAsString(actionType),
						functionBuilder.GetParamName(DirectoryOperationsShared.SourceDirectoryPropertyName),
						functionBuilder.GetParamName(DirectoryOperationsShared.TargetDirectoryPropertyName),
						CSharpUtilities.EnumAsString(FunctionData.Properties[DirectoryOperationsShared.DirectoryExistsPropertyName].GetValue<DirectoryOperationsShared.ExistsOption>()),
						CSharpUtilities.BoolAsString(FunctionData.Properties[DirectoryOperationsShared.ReplaceExistingFilePropertyName].GetValue<bool>())));
					break;
				default:
					throw new Exception(string.Format("Invalid action type [{0}] specified.", actionType));
			}

			functionBuilder.AddAssemblyReference(typeof(DirectoryOperations));
		}
	}
}
