using System;
using Twenty57.Linx.Components.File.Common;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.CodeGeneration;

namespace Twenty57.Linx.Components.File
{
	public class BinaryFileReadCodeGenerator : FunctionCodeGenerator
	{
		public BinaryFileReadCodeGenerator(IFunctionData data)
			: base(data)
		{ }

		public override void GenerateCode(IFunctionBuilder functionBuilder)
		{
			string code = String.Format(@"var fileHandle = (Twenty57.Linx.Components.File.Common.BinaryFileHandle){0};
				fileHandle.LogEvent += message => {1}.Log(message);
				return Twenty57.Linx.Components.File.BinaryFileReadX.ReadContents(fileHandle, message => {1}.Log(message));", 
				functionBuilder.GetParamName(FileShared.FilePathPropertyName),
				functionBuilder.ContextParamName);
			functionBuilder.AddCode(code);
			functionBuilder.AddAssemblyReference(typeof(BinaryFileReadX));
		}
	}
}