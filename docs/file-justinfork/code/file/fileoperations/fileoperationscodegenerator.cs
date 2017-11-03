using System.Collections.Generic;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.CodeGeneration;

namespace Twenty57.Linx.Components.File
{
	public class FileOperationsCodeGenerator : FunctionCodeGenerator
	{
		public FileOperationsCodeGenerator(IFunctionData data)
			: base(data)
		{ }

		public override void GenerateCode(IFunctionBuilder functionBuilder)
		{
			FileOperationsShared.ActionType actionType = FunctionData.Properties[FileOperationsShared.ActionPropertyName].GetValue<FileOperationsShared.ActionType>();
			FileOperationsShared.ExistsOption existsOption = FunctionData.Properties[FileOperationsShared.FileExistsPropertyName].GetValue<FileOperationsShared.ExistsOption>();

			if (actionType == FileOperationsShared.ActionType.FileExists)
			{
				functionBuilder.AddCode(string.Format(
					@"bool exists = Twenty57.Linx.Components.File.FileOperationsX.FileExists({0});
					{1}.Log(System.String.Format(""File {{0}} {{1}}"", {0}, exists ? ""exists."" : ""does not exist.""));
					return exists;",
					functionBuilder.GetParamName(FileOperationsShared.SourceFilePathPropertyName),
					functionBuilder.ContextParamName));
			}
			else if (actionType == FileOperationsShared.ActionType.CreateTempFile)
			{
				functionBuilder.AddCode(string.Format(
					@"string tempFilePath = Twenty57.Linx.Components.File.FileOperationsX.CreateTempFile();
					{0}.Log(System.String.Format(""Temp file created at {{0}}."", tempFilePath));
					return tempFilePath;",
					functionBuilder.ContextParamName));
			}
			else
			{
				bool keepFileName = FunctionData.Properties.ContainsKey(FileOperationsShared.KeepFileNamePropertyName) ? FunctionData.Properties[FileOperationsShared.KeepFileNamePropertyName].GetValue<bool>() : true;
				functionBuilder.AddCode(string.Format(@"string destinationFilePath = 
					Twenty57.Linx.Components.File.FileOperationsX.Execute({0}, {1}, {2}, {3}, {4}, message => {5}.Log(message));"
					, functionBuilder.GetParamName(FileOperationsShared.SourceFilePathPropertyName)
					, CSharpUtilities.BoolAsString(keepFileName)
					, functionBuilder.GetParamName(keepFileName ? FileOperationsShared.DestinationFolderPathPropertyName : FileOperationsShared.DestinationFilePathPropertyName)
					, CSharpUtilities.EnumAsString(actionType), CSharpUtilities.EnumAsString(existsOption)
					, functionBuilder.ContextParamName));
				if (actionType != FileOperationsShared.ActionType.Delete)
					functionBuilder.AddCode("return destinationFilePath;");
			}
			functionBuilder.AddAssemblyReference(typeof(FileOperations));
		}
	}
}
