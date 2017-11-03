using System.Collections.Generic;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.CodeGeneration;
using Twenty57.Linx.Plugin.Common.Types;

namespace Twenty57.Linx.Components.File
{
	public class FileListCodeGenerator : FunctionCodeGenerator
	{
		private IFunctionBuilder functionBuilder;

		public FileListCodeGenerator(IFunctionData data)
			: base(data)
		{ }

		public override void GenerateCode(IFunctionBuilder functionBuilder)
		{
			this.functionBuilder = functionBuilder;

			FileListX_Gen generator = new FileListX_Gen();
			generator.Session = new Dictionary<string, object>();
			generator.Session.Add("FunctionContextPropertyName", functionBuilder.ContextParamName);

			generator.Session.Add("IncludeSubfolders", CSharpUtilities.BoolAsString(FunctionData.Properties[FileListShared.IncludeSubfoldersPropertyName].GetValue<bool>()));
			generator.Session.Add("FolderPath", functionBuilder.GetParamName(FileListShared.FolderPathPropertyName));
			generator.Session.Add("SearchPattern", functionBuilder.GetParamName(FileListShared.SearchPatternPropertyName));
			generator.Session.Add("LoopResults", LoopResults);
			generator.Session.Add("ReturnFullPath", FunctionData.Properties[FileListShared.ReturnFullPathPropertyName].GetValue<bool>());

			generator.Session.Add("FileInfoTypeName", GetFileInfoTypeName());
			generator.Session.Add("OutputTypeName", functionBuilder.GetTypeName(FunctionData.Output));
			generator.Session.Add("ExecutionPathName", FileListShared.OutputFileProperty);
			generator.Session.Add("ExecutionPathOutputName", functionBuilder.ExecutionPathOutParamName);

			generator.Initialize();
			functionBuilder.AddCode(generator.TransformText());

			functionBuilder.AddAssemblyReference(typeof(FileListX));
		}

		private bool LoopResults { get { return FunctionData.Properties[FileListShared.LoopResultsPropertyName].GetValue<bool>(); } }

		private string GetFileInfoTypeName()
		{
			ITypeReference outputTypeReference = LoopResults ?
				FunctionData.ExecutionPaths[FileListShared.OutputFileProperty].Output :
				FunctionData.Output.GetProperty(FileListShared.OutputFileInfoName).TypeReference.GetEnumerableContentType();
			return
				functionBuilder.GetTypeName(outputTypeReference);
		}
	}
}