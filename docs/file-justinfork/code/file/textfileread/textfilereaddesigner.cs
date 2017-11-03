using System;
using System.Collections.Generic;
using System.Linq;
using Twenty57.Linx.Components.File.Common;
using Twenty57.Linx.Components.File.TextFileRead.Editors.TextFileReadFieldsEditor;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.Types;
using Twenty57.Linx.Plugin.Common.Validation;
using Twenty57.Linx.Plugin.UI.Editors;

namespace Twenty57.Linx.Components.File.TextFileRead
{
	public class TextFileReadDesigner : FunctionDesigner
	{
		public TextFileReadDesigner(IDesignerContext context)
			: base(context)
		{
			Properties.Add(new Property(FileShared.FilePathPropertyName, typeof(string), ValueUseOption.RuntimeRead, string.Empty));
			Properties.Add(new Property(TextFileReadShared.CodepagePropertyName, typeof(TextCodepage), ValueUseOption.DesignTime, TextCodepage.Default));
			Properties.Add(new Property(TextFileReadShared.ReadTypePropertyName, typeof(FileReadOptions), ValueUseOption.DesignTime, FileReadOptions.Complete));
			Properties.Add(new Property(TextFileReadShared.FieldsPropertyName, typeof(TextFileReaderFields), ValueUseOption.DesignTime, new TextFileReaderFields()));
			Properties.Add(new Property(TextFileReadShared.SkipHeaderLinesPropertyName, typeof(int), ValueUseOption.DesignTime, 0));
			Properties.Add(new Property(TextFileReadShared.SkipFooterLinesPropertyName, typeof(int), ValueUseOption.DesignTime, 0));
			BuildExecutionPaths();
			SetPropertyAttributes();
		}

		public TextFileReadDesigner(IFunctionData data, IDesignerContext context) : base(data, context) { }

		internal int SkipHeaderLines
		{
			get { return Properties[TextFileReadShared.SkipHeaderLinesPropertyName].GetValue<int>(); }
			set { Properties[TextFileReadShared.SkipHeaderLinesPropertyName].Value = value; }
		}

		internal int SkipFooterLines
		{
			get { return Properties[TextFileReadShared.SkipFooterLinesPropertyName].GetValue<int>(); }
			set { Properties[TextFileReadShared.SkipFooterLinesPropertyName].Value = value; }
		}

		private FileReadOptions ReadType
		{
			get { return Properties[TextFileReadShared.ReadTypePropertyName].GetValue<FileReadOptions>(); }
		}

		private TextFileReaderFields Fields
		{
			get { return Properties[TextFileReadShared.FieldsPropertyName].GetValue<TextFileReaderFields>(); }
		}

		protected override void InitializeProperties(IReadOnlyDictionary<string, IPropertyData> properties)
		{
			base.InitializeProperties(properties);
			SetPropertyAttributes();
		}

		protected override void InitializeOutput(ITypeReference output)
		{
			BuildDataOut();
		}

		protected override void InitializeExecutionPaths(IReadOnlyDictionary<string, IExecutionPathData> executionPaths)
		{
			base.InitializeExecutionPaths(executionPaths);
			BuildExecutionPaths();
		}

		private void SetPropertyAttributes()
		{
			Property filePathProperty = Properties[FileShared.FilePathPropertyName];
			filePathProperty.Order = 0;
			filePathProperty.Description = "The full path of the file.";
			filePathProperty.Editor = typeof(FilePathEditor);
			filePathProperty.Validations.Add(new RequiredValidator());

			Property codepageProperty = Properties[TextFileReadShared.CodepagePropertyName];
			codepageProperty.Order = 1;
			codepageProperty.Description = "The encoding type to use when reading from the file.";

			Property readTypeProperty = Properties[TextFileReadShared.ReadTypePropertyName];
			readTypeProperty.Order = 2;
			readTypeProperty.Description = "The mechanism used to read the file.";
			readTypeProperty.ValueChanged += readTypeProperty_ValueChanged;

			bool readCompleteFile = ReadType == FileReadOptions.Complete;

			Property fieldsProperty = Properties[TextFileReadShared.FieldsPropertyName];
			fieldsProperty.Order = 3;
			fieldsProperty.Description = "Define the fields.";
			fieldsProperty.IsVisible = !readCompleteFile;
			fieldsProperty.Editor = typeof(FieldsEditor);
			fieldsProperty.ValueChanged += fieldsProperty_ValueChanged;

			Property skipHeaderLinesProperty = Properties[TextFileReadShared.SkipHeaderLinesPropertyName];
			skipHeaderLinesProperty.Order = 4;
			skipHeaderLinesProperty.Description = "The number of lines to skip at the top of the file.";
			skipHeaderLinesProperty.IsVisible = !readCompleteFile;
			skipHeaderLinesProperty.Validations.Add(new RangeValidator(0, Int32.MaxValue));

			Property skipFooterLinesProperty = Properties[TextFileReadShared.SkipFooterLinesPropertyName];
			skipFooterLinesProperty.Order = 5;
			skipFooterLinesProperty.Description = "The number of lines to skip at the bottom of the file.";
			skipFooterLinesProperty.IsVisible = !readCompleteFile;
			skipFooterLinesProperty.Validations.Add(new RangeValidator(0, Int32.MaxValue));
		}

		private void readTypeProperty_ValueChanged(object sender, EventArgs e)
		{
			BuildExecutionPaths();
			BuildDataOut();
		}

		private void fieldsProperty_ValueChanged(object sender, EventArgs e)
		{
			BuildExecutionPaths();
			BuildDataOut();
		}

		private void BuildExecutionPaths()
		{
			if (ReadType == FileReadOptions.LineByLine)
			{
				TypeBuilder typeBuilder = new TypeBuilder();
				typeBuilder.AddProperty(TextFileReadShared.LineNumberName, typeof(int));
				typeBuilder.AddProperty(TextFileReadShared.LineContentsName, Fields.FieldList.Any() ? Fields.CreateTypeReference() : TypeReference.Create(typeof(string)));
				GeneratedTypeReference outputType = typeBuilder.CreateTypeReference();
				if (ExecutionPaths.Contains(TextFileReadShared.ExecutionPathName))
					ExecutionPaths[TextFileReadShared.ExecutionPathName].Output = outputType;
				else
					ExecutionPaths.Add(TextFileReadShared.ExecutionPathName, TextFileReadShared.ExecutionPathName, outputType, IterationHint.ZeroOrMore);
			}
			else
				ExecutionPaths.Clear();
		}

		private void BuildDataOut()
		{
			switch (ReadType)
			{
				case FileReadOptions.Complete: Output = TypeReference.Create(typeof(string)); break;
				case FileReadOptions.ListOfLines: Output = TypeReference.CreateList(Fields.FieldList.Any() ? Fields.CreateTypeReference() : TypeReference.Create(typeof(string))); break;
				case FileReadOptions.LineByLine: Output = null; break;
			}
		}
	}
}
