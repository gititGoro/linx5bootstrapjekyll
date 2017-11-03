using System;
using Twenty57.Linx.Components.File.Common;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.CodeGeneration;

namespace Twenty57.Linx.Components.File
{
	public class BinaryFileWriteCodeGenerator : FunctionCodeGenerator
	{
		public BinaryFileWriteCodeGenerator(IFunctionData data)
			: base(data)
		{ }

		public override void GenerateCode(IFunctionBuilder functionBuilder)
		{
			bool closeFileHandle = FunctionData.Properties.ContainsKey(FileShared.OwnsFileHandlePropertyName) ? FunctionData.Properties[FileShared.OwnsFileHandlePropertyName].GetValue<bool>() : true;
			functionBuilder.AddCode(String.Format(
				@"var fileHandle = (Twenty57.Linx.Components.File.Common.BinaryFileHandle){0};{1}
				return Twenty57.Linx.Components.File.BinaryFileWriteX.WriteFile(
					fileHandle, 
					{2},
					{3}, 
					{4}, 
					{5},
					message => {6}.Log(message)
					);",
				functionBuilder.GetParamName(FileShared.FilePathPropertyName),
				closeFileHandle ? string.Format("{0}fileHandle.LogEvent += message => {1}.Log(message);", Environment.NewLine, functionBuilder.ContextParamName) : string.Empty,
				CSharpUtilities.BoolAsString(closeFileHandle),
				functionBuilder.GetParamName(BinaryFileWriteShared.ContentsPropertyName),
				CSharpUtilities.EnumAsString(FunctionData.Properties[BinaryFileWriteShared.FileDoesNotExistPropertyName].Value),
				CSharpUtilities.EnumAsString(FunctionData.Properties[BinaryFileWriteShared.FileExistsPropertyName].Value),
				functionBuilder.ContextParamName
				));

			functionBuilder.AddAssemblyReference(typeof(BinaryFileWriteX));
		}
	}
}