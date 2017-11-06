using System;
using System.Collections.Generic;
using System.IO;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.Types;
using Twenty57.Linx.Plugin.Common.Validation;
using Twenty57.Linx.Plugin.UI.Editors;

namespace Twenty57.Linx.Components.File
{
	public class FileOperationsDesigner : FunctionDesigner
	{
		public FileOperationsDesigner(IDesignerContext context)
			: base(context)
		{
			SetPropertyAttributes();
		}

		public FileOperationsDesigner(IFunctionData functionData, IDesignerContext context)
			: base(functionData, context)
		{ }

		private FileOperationsShared.ActionType Action
		{
			get { return Properties[FileOperationsShared.ActionPropertyName].GetValue<FileOperationsShared.ActionType>(); }
		}

		private Property GetProperty(string propertyName)
		{
			if (Properties.Contains(propertyName))
				return Properties[propertyName];

			Property property = null;
			switch (propertyName)
			{
				case FileOperationsShared.ActionPropertyName: property = new Property(propertyName, typeof(FileOperationsShared.ActionType), ValueUseOption.DesignTime, FileOperationsShared.ActionType.Copy); break;
				case FileOperationsShared.SourceFilePathPropertyName: property = new Property(propertyName, typeof(string), ValueUseOption.RuntimeRead, string.Empty); break;
				case FileOperationsShared.KeepFileNamePropertyName: property = new Property(propertyName, typeof(bool), ValueUseOption.DesignTime, true); break;
				case FileOperationsShared.DestinationFolderPathPropertyName: property = new Property(propertyName, typeof(string), ValueUseOption.RuntimeRead, string.Empty); break;
				case FileOperationsShared.DestinationFilePathPropertyName: property = new Property(propertyName, typeof(string), ValueUseOption.RuntimeRead, string.Empty); break;
				case FileOperationsShared.FileExistsPropertyName: property = new Property(propertyName, typeof(FileOperationsShared.ExistsOption), ValueUseOption.DesignTime, FileOperationsShared.ExistsOption.OverwriteFile); break;
			}
			Properties.Add(property);
			return property;
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

		private void SetPropertyAttributes()
		{
			Property actionProperty = GetProperty(FileOperationsShared.ActionPropertyName);
			actionProperty.Description = "The action to perform on the source file.";
			actionProperty.Order = 0;
			actionProperty.ValueChanged += actionProperty_ValueChanged;

			Property sourcePathProperty = GetProperty(FileOperationsShared.SourceFilePathPropertyName);
			sourcePathProperty.Description = "The full path of the file on which to perform the action.";
			sourcePathProperty.Order = 1;
			sourcePathProperty.Validations.Add(new RequiredValidator());
			sourcePathProperty.Editor = typeof(FilePathEditor);
			sourcePathProperty.IsVisible = (Action != FileOperationsShared.ActionType.CreateTempFile);

			Property keepFileNameProperty = GetProperty(FileOperationsShared.KeepFileNamePropertyName);
			keepFileNameProperty.Description = "Keep the same file name during the operation.";
			keepFileNameProperty.Order = 2;
			keepFileNameProperty.IsVisible = (Action == FileOperationsShared.ActionType.Copy) || (Action == FileOperationsShared.ActionType.Move);

			Property destinationFolderPathProperty = GetProperty(FileOperationsShared.DestinationFolderPathPropertyName);
			destinationFolderPathProperty.Description = "The path to the destination folder.";
			destinationFolderPathProperty.Order = 3;
			destinationFolderPathProperty.IsVisible = (keepFileNameProperty.IsVisible) && (keepFileNameProperty.GetValue<bool>());
			destinationFolderPathProperty.Validations.Add(new RequiredValidator());
			destinationFolderPathProperty.Editor = typeof(DirectoryPathEditor);

			Property destinationFilePathProperty = GetProperty(FileOperationsShared.DestinationFilePathPropertyName);
			destinationFilePathProperty.Description = "The destination file path.";
			destinationFilePathProperty.Order = 3;
			destinationFilePathProperty.IsVisible = (keepFileNameProperty.IsVisible) && (!keepFileNameProperty.GetValue<bool>());
			destinationFilePathProperty.Validations.Add(new RequiredValidator());
			destinationFilePathProperty.Editor = typeof(FilePathEditor);

			Property fileExistsProperty = GetProperty(FileOperationsShared.FileExistsPropertyName);
			fileExistsProperty.Description = "Action to take when the destination file exists.";
			fileExistsProperty.Order = 4;
			fileExistsProperty.IsVisible = (Action == FileOperationsShared.ActionType.Copy) || (Action == FileOperationsShared.ActionType.Move);
		}

		private void actionProperty_ValueChanged(object sender, EventArgs e)
		{
			BuildDataOut();
		}

		private void BuildDataOut()
		{
			switch (Action)
			{
				case FileOperationsShared.ActionType.Copy:
				case FileOperationsShared.ActionType.Move:
				case FileOperationsShared.ActionType.CreateTempFile: Output = TypeReference.Create(typeof(string)); break;
				case FileOperationsShared.ActionType.FileExists: Output = TypeReference.Create(typeof(bool)); break;
				case FileOperationsShared.ActionType.Delete: Output = null; break;
			}
		}
	}
}
