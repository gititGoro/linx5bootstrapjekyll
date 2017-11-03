using System;
using Twenty57.Linx.Components.File.Common;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.CodeGeneration;

namespace Twenty57.Linx.Components.File.TextFileWrite
{
	public class TextFileWriteCodeGenerator : FunctionCodeGenerator
	{
		public TextFileWriteCodeGenerator(IFunctionData data)
			: base(data)
		{ }

		public override void GenerateCode(IFunctionBuilder functionBuilder)
		{
			bool closeFileHandle = FunctionData.Properties.ContainsKey(FileShared.OwnsFileHandlePropertyName) ? FunctionData.Properties[FileShared.OwnsFileHandlePropertyName].GetValue<bool>() : true;
			string code = String.Format(
				@"var fileHandle = (Twenty57.Linx.Components.File.Common.TextFileHandle){0};{1}
				return Twenty57.Linx.Components.File.TextFileWrite.TextFileWriteX.WriteFile(
					fileHandle,
					{2},
					{3},
					{4},
					{5},
					{6},
					message => {7}.Log(message));",
				functionBuilder.GetParamName(FileShared.FilePathPropertyName),
				closeFileHandle ? string.Format("{0}fileHandle.LogEvent += message => {1}.Log(message);", Environment.NewLine, functionBuilder.ContextParamName) : string.Empty,
				CSharpUtilities.BoolAsString(closeFileHandle),
				functionBuilder.GetParamName(TextFileWriteShared.ContentsPropertyName),
				CSharpUtilities.EnumAsString(FunctionData.Properties[TextFileWriteShared.FileDoesNotExistPropertyName].GetValue<DoesNotExistOptions>()),
				CSharpUtilities.EnumAsString(FunctionData.Properties[TextFileWriteShared.FileExistsPropertyName].GetValue<ExistOptions>()),
				CSharpUtilities.EnumAsString(FunctionData.Properties[TextFileWriteShared.DestinationCodepagePropertyName].GetValue<TextCodepage>()),
				functionBuilder.ContextParamName
				);

			functionBuilder.AddAssemblyReference(typeof(TextFileWriteX));
			functionBuilder.AddCode(code);
		}
	}
}