using System.Collections.Generic;
using Twenty57.Linx.Components.File.Common;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.Types;
using Twenty57.Linx.Plugin.Common.Validation;
using Twenty57.Linx.Plugin.UI.Editors;

namespace Twenty57.Linx.Components.File
{
	public class BinaryFileReadDesigner : FunctionDesigner
	{
		public BinaryFileReadDesigner(IDesignerContext context)
			: base(context)
		{
			Version = BinaryFileReadFunctionUpdater.Instance.CurrentVersion;

			Properties.Add(new Property(FileShared.FilePathPropertyName, typeof(string), ValueUseOption.RuntimeRead, string.Empty));
			SetPropertyAttributes();

			Output = TypeReference.CreateList(typeof(byte));
		}

		public BinaryFileReadDesigner(IFunctionData data, IDesignerContext context)
			: base(data, context) { }

		protected override void InitializeProperties(IReadOnlyDictionary<string, IPropertyData> properties)
		{
			base.InitializeProperties(properties);
			SetPropertyAttributes();
		}

		private void SetPropertyAttributes()
		{
			Property filePathProperty = Properties[FileShared.FilePathPropertyName];
			filePathProperty.Order = 0;
			filePathProperty.Description = "The full path of the file.";
			filePathProperty.Editor = typeof(FilePathEditor);
			filePathProperty.Validations.Add(new RequiredValidator());
		}
	}
}