using System;
using System.Collections.Generic;
using Twenty57.Linx.Components.File.Common;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.Types;
using Twenty57.Linx.Plugin.Common.Validation;
using Twenty57.Linx.Plugin.UI.Editors;

namespace Twenty57.Linx.Components.File.FileOpen
{
	public class FileOpenDesigner : FunctionDesigner
	{
		public FileOpenDesigner(IDesignerContext context)
			: base(context)
		{
			Version = FunctionUpdater.Instance.CurrentVersion;

			Properties.Add(new Property(FileOpenShared.FilePathPropertyName, typeof(string), ValueUseOption.RuntimeRead, null));
			Properties.Add(new Property(FileOpenShared.IsTextPropertyName, typeof(bool), ValueUseOption.DesignTime, true));
			Properties.Add(new Property(FileOpenShared.CodepagePropertyName, typeof(TextCodepage), ValueUseOption.DesignTime, TextCodepage.Default));
			Properties.Add(new Property(FileOpenShared.FileDoesNotExistPropertyName, typeof(DoesNotExistOptions), ValueUseOption.DesignTime, DoesNotExistOptions.CreateFile));
			Properties.Add(new Property(FileOpenShared.FileExistsPropertyName, typeof(ExistOptions), ValueUseOption.DesignTime, ExistOptions.AppendData));
			SetPropertyAttributes();

			ExecutionPaths.Add(FileOpenShared.ExecutionPathName, FileOpenShared.ExecutionPathName, BuildExecutionPathOutput(), IterationHint.Once);
			Output = TypeReference.Create(typeof(string));
		}

		public FileOpenDesigner(IFunctionData data, IDesignerContext context) : base(data, context) { }

		protected override void InitializeProperties(IReadOnlyDictionary<string, IPropertyData> properties)
		{
			base.InitializeProperties(properties);
			SetPropertyAttributes();
		}

		protected override void InitializeExecutionPaths(IReadOnlyDictionary<string, IExecutionPathData> executionPaths)
		{
			base.InitializeExecutionPaths(executionPaths);
			RefreshExecutionPath();
		}

		private void SetPropertyAttributes()
		{
			Property filePathProperty = Properties[FileOpenShared.FilePathPropertyName];
			filePathProperty.Order = 0;
			filePathProperty.Description = "The full path of the file.";
			filePathProperty.Editor = typeof(FilePathEditor);
			filePathProperty.Validations.Add(new RequiredValidator());

			Property isTextProperty = Properties[FileOpenShared.IsTextPropertyName];
			isTextProperty.Order = 1;
			isTextProperty.Description = "Indicate if the file to open is a text file.";
			isTextProperty.ValueChanged += isTextProperty_ValueChanged;

			Property codepageProperty = Properties[FileOpenShared.CodepagePropertyName];
			codepageProperty.Order = 2;
			codepageProperty.Description = "The character encoding used by the file.";
			codepageProperty.IsVisible = isTextProperty.GetValue<bool>();

			Property fileDoesNotExistProperty = Properties[FileOpenShared.FileDoesNotExistPropertyName];
			fileDoesNotExistProperty.Order = 3;
			fileDoesNotExistProperty.Description = "Specify what to do if the file does not exist.";

			Property fileExistsProperty = Properties[FileOpenShared.FileExistsPropertyName];
			fileExistsProperty.Order = 4;
			fileExistsProperty.Description = "Specify what to do if the file does exist.";
		}

		private void isTextProperty_ValueChanged(object sender, EventArgs e)
		{
			RefreshExecutionPath();
		}

		private void RefreshExecutionPath()
		{
			ExecutionPaths[FileOpenShared.ExecutionPathName].Output = BuildExecutionPathOutput();
		}

		private ITypeReference BuildExecutionPathOutput()
		{
			bool isText = Properties[FileOpenShared.IsTextPropertyName].GetValue<bool>();

			var typeBuilder = new TypeBuilder();
			typeBuilder.AddProperty(FileOpenShared.OutputFilePathPropertyName, typeof(string), AccessType.Read);
			typeBuilder.AddProperty(FileOpenShared.OutputFileHandlePropertyName, TypeReference.CreateResource(isText ? typeof(TextFileHandle) : typeof(BinaryFileHandle)), AccessType.Read);
			return typeBuilder.CreateTypeReference();
		}
	}
}
