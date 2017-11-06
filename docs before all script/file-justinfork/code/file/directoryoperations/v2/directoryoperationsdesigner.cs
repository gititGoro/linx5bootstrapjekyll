using System;
using System.Collections.Generic;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.Types;
using Twenty57.Linx.Plugin.Common.Validation;
using Twenty57.Linx.Plugin.UI.Editors;

namespace Twenty57.Linx.Components.File.V2
{
	public class DirectoryOperationsDesigner : FunctionDesigner
	{

		public DirectoryOperationsDesigner(IDesignerContext context)
			: base(context)
		{
			Version = "2";
			SetPropertyAttributes();
		}

		public DirectoryOperationsDesigner(IFunctionData functionData, IDesignerContext context)
			: base(functionData, context)
		{ }

		protected override void InitializeProperties(IReadOnlyDictionary<string, IPropertyData> properties)
		{
			base.InitializeProperties(properties);
			SetPropertyAttributes();
		}

		protected override void InitializeOutput(ITypeReference output)
		{
			BuildDataOut();
		}

		private DirectoryOperationsShared.ActionType Action
		{
			get { return Properties[DirectoryOperationsShared.ActionPropertyName].GetValue<DirectoryOperationsShared.ActionType>(); }
		}

		private DirectoryOperationsShared.ExistsOption DirectoryExists
		{
			get { return Properties[DirectoryOperationsShared.DirectoryExistsPropertyName].GetValue<DirectoryOperationsShared.ExistsOption>(); }
		}

		private Property GetProperty(string propertyName)
		{
			if (Properties.Contains(propertyName))
				return Properties[propertyName];

			Property property = null;
			switch (propertyName)
			{
				case DirectoryOperationsShared.ActionPropertyName: property = new Property(propertyName, typeof(DirectoryOperationsShared.ActionType), ValueUseOption.DesignTime, DirectoryOperationsShared.ActionType.Copy); break;
				case DirectoryOperationsShared.SourceDirectoryPropertyName: property = new Property(propertyName, typeof(string), ValueUseOption.RuntimeRead, string.Empty); break;
				case DirectoryOperationsShared.TargetDirectoryPropertyName: property = new Property(propertyName, typeof(string), ValueUseOption.RuntimeRead, string.Empty); break;
				case DirectoryOperationsShared.DirectoryExistsPropertyName: property = new Property(propertyName, typeof(DirectoryOperationsShared.ExistsOption), ValueUseOption.DesignTime, DirectoryOperationsShared.ExistsOption.OverwriteDirectory); break;
				case DirectoryOperationsShared.DirectoryPropertyName: property = new Property(propertyName, typeof(string), ValueUseOption.RuntimeRead, string.Empty); break;
				case DirectoryOperationsShared.ReplaceExistingFilePropertyName: property = new Property(propertyName, typeof(bool), ValueUseOption.DesignTime, true); break;
				case DirectoryOperationsShared.CreateDirectoryExistsPropertyName: property = new Property(propertyName, typeof(DirectoryOperationsShared.CreateExistsOption), ValueUseOption.DesignTime, DirectoryOperationsShared.CreateExistsOption.DoNothing); break;
			}
			Properties.Add(property);
			return property;
		}

		private void SetPropertyAttributes()
		{
			Property actionProperty = GetProperty(DirectoryOperationsShared.ActionPropertyName);
			actionProperty.Description = "The action to perform on the source directory.";
			actionProperty.Order = 0;
			actionProperty.ValueChanged += (sender, args) => BuildDataOut();

			Property sourcePathProperty = GetProperty(DirectoryOperationsShared.SourceDirectoryPropertyName);
			sourcePathProperty.Description = "The full path of the directory on which to perform the action.";
			sourcePathProperty.Order = 1;
			sourcePathProperty.IsVisible = (Action == DirectoryOperationsShared.ActionType.Copy) || (Action == DirectoryOperationsShared.ActionType.Move);
			sourcePathProperty.Validations.Add(new RequiredValidator());
			sourcePathProperty.Editor = typeof(DirectoryPathEditor);

			Property targetFolderPathProperty = GetProperty(DirectoryOperationsShared.TargetDirectoryPropertyName);
			targetFolderPathProperty.Description = "The path to the destination folder.";
			targetFolderPathProperty.Order = 2;
			targetFolderPathProperty.IsVisible = (Action == DirectoryOperationsShared.ActionType.Copy) || (Action == DirectoryOperationsShared.ActionType.Move);
			targetFolderPathProperty.Validations.Add(new RequiredValidator());
			targetFolderPathProperty.Editor = typeof(DirectoryPathEditor);

			Property directoryExistsProperty = GetProperty(DirectoryOperationsShared.DirectoryExistsPropertyName);
			directoryExistsProperty.Description = "Action to take when the destination folder exists.";
			directoryExistsProperty.Order = 3;
			directoryExistsProperty.IsVisible = (Action == DirectoryOperationsShared.ActionType.Copy) || (Action == DirectoryOperationsShared.ActionType.Move);

			Property replaceFileExistsProperty = GetProperty(DirectoryOperationsShared.ReplaceExistingFilePropertyName);
			replaceFileExistsProperty.Description = "Choose if existing files in the destination folder should be replaced.";
			replaceFileExistsProperty.Order = 4;
			replaceFileExistsProperty.IsVisible = ((DirectoryExists == DirectoryOperationsShared.ExistsOption.MergeDirectory) &&
					(Action == DirectoryOperationsShared.ActionType.Copy || Action == DirectoryOperationsShared.ActionType.Move));

			Property directoryPathProperty = GetProperty(DirectoryOperationsShared.DirectoryPropertyName);
			directoryPathProperty.Description = "The destination folder path.";
			directoryPathProperty.Order = 5;
			directoryPathProperty.IsVisible = (Action == DirectoryOperationsShared.ActionType.Create) || (Action == DirectoryOperationsShared.ActionType.Delete) || (Action == DirectoryOperationsShared.ActionType.DirectoryExists);
			directoryPathProperty.Validations.Add(new RequiredValidator());
			directoryPathProperty.Editor = typeof(DirectoryPathEditor);

			Property createDirectoryExistsProperty = GetProperty(DirectoryOperationsShared.CreateDirectoryExistsPropertyName);
			createDirectoryExistsProperty.Description = "Action to take when the folder already exists.";
			createDirectoryExistsProperty.Order = 6;
			createDirectoryExistsProperty.IsVisible = Action == DirectoryOperationsShared.ActionType.Create;
		}

		private void BuildDataOut()
		{
			switch (Action)
			{
				case DirectoryOperationsShared.ActionType.Create:
					Output = TypeReference.Create(typeof(string));
					break;
				case DirectoryOperationsShared.ActionType.DirectoryExists:
					Output = TypeReference.Create(typeof(bool));
					break;
				default:
					Output = null;
					break;
			}
		}
	}
}
