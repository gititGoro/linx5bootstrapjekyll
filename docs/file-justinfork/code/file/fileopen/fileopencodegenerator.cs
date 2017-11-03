using Twenty57.Linx.Components.File.Common;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.CodeGeneration;

namespace Twenty57.Linx.Components.File.FileOpen
{
	public class FileOpenCodeGenerator : FunctionCodeGenerator
	{
		public FileOpenCodeGenerator(IFunctionData data) : base(data) { }

		public override void GenerateCode(IFunctionBuilder functionBuilder)
		{
			bool isText = FunctionData.Properties[FileOpenShared.IsTextPropertyName].GetValue<bool>();

			functionBuilder.AddCode(string.Format(
			@"var fileOpenX = new Twenty57.Linx.Components.File.FileOpen.FileOpenX({0}, {1}, {2}, {3}, {4}, message => {5}.Log(message));
			{6} = fileOpenX.Select(fileHandle => new Twenty57.Linx.Plugin.Common.CodeGeneration.NextResult({7}, new {8} {{ {9} = fileOpenX.FilePath, {10} = ({11})fileHandle }}));
			return fileOpenX.FilePath;",
				functionBuilder.GetParamName(FileOpenShared.FilePathPropertyName),
				CSharpUtilities.BoolAsString(isText),
				CSharpUtilities.EnumAsString(FunctionData.Properties[FileOpenShared.CodepagePropertyName].Value),
				CSharpUtilities.EnumAsString(FunctionData.Properties[FileOpenShared.FileDoesNotExistPropertyName].Value),
				CSharpUtilities.EnumAsString(FunctionData.Properties[FileOpenShared.FileExistsPropertyName].Value),
				functionBuilder.ContextParamName,
				functionBuilder.ExecutionPathOutParamName,
				CSharpUtilities.ToVerbatimString(FileOpenShared.ExecutionPathName),
				functionBuilder.GetTypeName(FunctionData.ExecutionPaths[FileOpenShared.ExecutionPathName].Output),
				FileOpenShared.OutputFilePathPropertyName,
				FileOpenShared.OutputFileHandlePropertyName,
				(isText ? typeof(TextFileHandle) : typeof(BinaryFileHandle)).FullName));

			functionBuilder.AddAssemblyReference(typeof(FileOpenX));
		}
	}
}
