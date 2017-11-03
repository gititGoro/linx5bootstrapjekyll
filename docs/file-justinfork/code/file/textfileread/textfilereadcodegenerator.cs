using System;
using System.Collections.Generic;
using System.Linq;
using Twenty57.Linx.Components.File.Common;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.CodeGeneration;
using Twenty57.Linx.Plugin.Common.Types;

namespace Twenty57.Linx.Components.File.TextFileRead
{
	public class TextFileReadCodeGenerator : FunctionCodeGenerator
	{
		public TextFileReadCodeGenerator(IFunctionData data)
			: base(data)
		{ }

		public override void GenerateCode(IFunctionBuilder functionBuilder)
		{
			TextFileReaderFields fields = FunctionData.Properties[TextFileReadShared.FieldsPropertyName].GetValue<TextFileReaderFields>();
			bool hasFields = (fields != null) && (fields.FieldList != null) && (fields.FieldList.Count != 0);

			switch (FunctionData.Properties[TextFileReadShared.ReadTypePropertyName].GetValue<FileReadOptions>())
			{
				case FileReadOptions.Complete:
					functionBuilder.AddCode(GenerateCodeForStrings(functionBuilder));
					break;
				case FileReadOptions.LineByLine:
					functionBuilder.AddCode(hasFields
						? GenerateCodeForFieldsAndLoop(functionBuilder)
						: GenerateCodeForStringsAndLoop(functionBuilder));
					break;
				case FileReadOptions.ListOfLines:
					functionBuilder.AddCode(hasFields
						? GenerateCodeForFields(functionBuilder)
						: GenerateCodeForStrings(functionBuilder));
					break;
			}
			functionBuilder.AddAssemblyReference(typeof(TextFileReader));
		}

		private string GenerateCodeForStrings(IFunctionBuilder functionBuilder)
		{
			var generator = new TextFileReadStrings_Gen();
			generator.Session = GetStandardSession(functionBuilder);
			generator.Initialize();
			return generator.TransformText();
		}

		private string GenerateCodeForFields(IFunctionBuilder functionBuilder)
		{
			var generator = new TextFileReadFields_Gen();

			generator.Session = GetStandardSession(functionBuilder);
			generator.Session.Add("fields", FunctionData.Properties[TextFileReadShared.FieldsPropertyName].GetValue<TextFileReaderFields>());
			generator.Session.Add("outputFileContentsType", functionBuilder.GetTypeName(FunctionData.Output.GetEnumerableContentType()));
			generator.Initialize();
			return generator.TransformText();
		}

		private string GenerateCodeForStringsAndLoop(IFunctionBuilder functionBuilder)
		{
			var generator = new TextFileReadStringsAndLoop_Gen();

			generator.Session = GetStandardSession(functionBuilder);
			generator.Session.Add("pathOutputTypeName", functionBuilder.GetTypeName(FunctionData.ExecutionPaths[TextFileReadShared.ExecutionPathName].Output));
			generator.Session.Add("resultsExecutionPathName", TextFileReadShared.ExecutionPathName);

			generator.Session.Add("outputLineNumberName", TextFileReadShared.LineNumberName);
			generator.Session.Add("outputLineContentsName", TextFileReadShared.LineContentsName);
			generator.Session.Add("executionPathOutputName", functionBuilder.ExecutionPathOutParamName);
			generator.Initialize();
			return generator.TransformText();
		}

		private string GenerateCodeForFieldsAndLoop(IFunctionBuilder functionBuilder)
		{
			var generator = new TextFileReadFieldsAndLoop_Gen();

			var executionPathOutputType = FunctionData.ExecutionPaths[TextFileReadShared.ExecutionPathName].Output;
			generator.Session = GetStandardSession(functionBuilder);
			generator.Session.Add("fields", FunctionData.Properties[TextFileReadShared.FieldsPropertyName].GetValue<TextFileReaderFields>());
			generator.Session.Add("pathOutputTypeName", functionBuilder.GetTypeName(executionPathOutputType));
			generator.Session.Add("resultsExecutionPathName", TextFileReadShared.ExecutionPathName);

			generator.Session.Add("outputLineNumberName", TextFileReadShared.LineNumberName);
			generator.Session.Add("recordInnerTypeName",
				functionBuilder.GetTypeName(
					executionPathOutputType.GetProperties().First(v => v.Name == TextFileReadShared.LineContentsName).TypeReference));

			generator.Session.Add("recordInnerMemberName", TextFileReadShared.LineContentsName);
			generator.Session.Add("executionPathOutputName", functionBuilder.ExecutionPathOutParamName);
			generator.Initialize();
			return generator.TransformText();
		}

		private IDictionary<string, object> GetStandardSession(IFunctionBuilder functionBuilder)
		{
			var session = new Dictionary<string, object>();
			session.Add("functionContextPropertyName", functionBuilder.ContextParamName);
			session.Add("filePathPropertyName", functionBuilder.GetParamName(FileShared.FilePathPropertyName));
			session.Add("readType", FunctionData.Properties[TextFileReadShared.ReadTypePropertyName].GetValue<FileReadOptions>());
			session.Add("codePage", FunctionData.Properties[TextFileReadShared.CodepagePropertyName].GetValue<TextCodepage>());
			session.Add("retryInterval", 0);
			session.Add("retryTimes", 0);
			session.Add("skipHeaderLines", FunctionData.Properties[TextFileReadShared.SkipHeaderLinesPropertyName].Value.ToString());
			session.Add("skipFooterLines", FunctionData.Properties[TextFileReadShared.SkipFooterLinesPropertyName].Value.ToString());
			return session;
		}
	}
}