using System;
using System.Collections.Generic;
using System.Linq;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.Types;
using Twenty57.Linx.Plugin.Common.Validation;
using Twenty57.Linx.Plugin.UI.Editors;

namespace Twenty57.Linx.Components.File
{
	public class FileListDesigner : FunctionDesigner
	{
		public FileListDesigner(IDesignerContext context)
			: base(context)
		{
			Version = FileListFunctionUpdater.Instance.CurrentVersion;

			Properties.Add(new Property(FileListShared.FolderPathPropertyName, typeof(string), ValueUseOption.RuntimeRead, ""));
			Properties.Add(new Property(FileListShared.SearchPatternPropertyName, typeof(string), ValueUseOption.RuntimeRead, ""));
			Properties.Add(new Property(FileListShared.IncludeSubfoldersPropertyName, typeof(bool), ValueUseOption.DesignTime, false));
			Properties.Add(new Property(FileListShared.ReturnFullPathPropertyName, typeof(bool), ValueUseOption.DesignTime, false));
			Properties.Add(new Property(FileListShared.LoopResultsPropertyName, typeof(bool), ValueUseOption.DesignTime, false));

			SetPropertyAttributes();
			BuildDataOut();
		}

		public FileListDesigner(IFunctionData data, IDesignerContext context)
			: base(data, context)
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

		protected override void InitializeExecutionPaths(IReadOnlyDictionary<string, IExecutionPathData> executionPaths) { }

		private void SetPropertyAttributes()
		{
			var folderPathProp = Properties[FileListShared.FolderPathPropertyName];
			folderPathProp.Description = "The folder from which to retrieve the file list.";
			folderPathProp.Validations.Add(new RequiredValidator());
			folderPathProp.Editor = typeof(DirectoryPathEditor);

			var searchPatternProp = Properties[FileListShared.SearchPatternPropertyName];
			searchPatternProp.Description = "The file pattern to search for.  If blank, all files will be returned.  Separate multiple patterns with a semi-colon(;).";

			var includeSubFoldersProp = Properties[FileListShared.IncludeSubfoldersPropertyName];
			includeSubFoldersProp.Description = "Set to true to recursively search all subdirectories of the specified file path.";

			var returnFullPathProp = Properties[FileListShared.ReturnFullPathPropertyName];
			returnFullPathProp.Description = "If true, will return the full path.  When false, only the name of the file will be returned.";

			var loopResultsProp = Properties[FileListShared.LoopResultsPropertyName];
			loopResultsProp.Description = "If true, the file paths will be returned line by line, else a table containing all the file paths will be created.";
			loopResultsProp.ValueChanged += loopResultsProp_ValueChanged;
		}

		private void loopResultsProp_ValueChanged(object sender, EventArgs e)
		{
			BuildDataOut();
		}

		private void BuildDataOut()
		{
			if (Properties[FileListShared.LoopResultsPropertyName].GetValue<bool>())
			{
				Output = TypeReference.CreateGeneratedType(new TypeProperty(FileListShared.OutputNumberOfFilesName, typeof(Int32)));

				if (!ExecutionPaths.Any())
					ExecutionPaths.Add(FileListShared.OutputFileProperty, FileListShared.OutputFileProperty, CreateFileInfoType(), IterationHint.ZeroOrMore);
			}
			else
			{
				ExecutionPaths.Clear();

				Output = TypeReference.CreateGeneratedType(
					new TypeProperty(FileListShared.OutputFileInfoName, TypeReference.CreateList(CreateFileInfoType())),
					new TypeProperty(FileListShared.OutputNumberOfFilesName, typeof(Int32)));
			}
		}

		private GeneratedTypeReference CreateFileInfoType()
		{
			return TypeReference.CreateGeneratedType(
				new TypeProperty(FileListShared.OutputFileNameName, typeof(string)),
				new TypeProperty(FileListShared.OutputCreationTimeName, typeof(DateTime)),
				new TypeProperty(FileListShared.OutputLastAccessTimeName, typeof(DateTime)),
				new TypeProperty(FileListShared.OutputLastWriteTimeName, typeof(DateTime)),
				new TypeProperty(FileListShared.OutputSizeName, typeof(double)),
				new TypeProperty(FileListShared.OutputReadOnlyName, typeof(bool)),
				new TypeProperty(FileListShared.OutputHiddenName, typeof(bool))
				);
		}
	}
}
