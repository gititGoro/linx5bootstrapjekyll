using System.Collections.Generic;
using System.Linq;
using Twenty57.Linx.Components.File.Common;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.Types;
using Twenty57.Linx.Plugin.Common.Validation;
using Twenty57.Linx.Plugin.UI.Editors;

namespace Twenty57.Linx.Components.File
{
	public class BinaryFileWriteDesigner : BinaryFileDesignerBase
	{
		public BinaryFileWriteDesigner(IDesignerContext context)
			: base(context)
		{
			Version = BinaryFileWriteFunctionUpdater.Instance.CurrentVersion;

			Properties.Add(new Property(BinaryFileWriteShared.ContentsPropertyName, typeof(IEnumerable<byte>), ValueUseOption.RuntimeRead, null));
			Properties.Add(new Property(BinaryFileWriteShared.FileExistsPropertyName, typeof(ExistOptions), ValueUseOption.DesignTime, ExistOptions.AppendData));
			Properties.Add(new Property(BinaryFileWriteShared.FileDoesNotExistPropertyName, typeof(DoesNotExistOptions), ValueUseOption.DesignTime, DoesNotExistOptions.CreateFile));
			SetPropertyAttributes();

			BuildOutput();
		}

		public BinaryFileWriteDesigner(IFunctionData data, IDesignerContext context)
			: base(data, context) { }

		protected override void InitializeProperties(IReadOnlyDictionary<string, IPropertyData> properties)
		{
			base.InitializeProperties(properties);
			SetPropertyAttributes();
		}

		protected override void InitializeOutput(ITypeReference output)
		{
			base.InitializeOutput(output);
			BuildOutput();
		}

		private void SetPropertyAttributes()
		{
			var contentsProp = Properties[BinaryFileWriteShared.ContentsPropertyName];
			contentsProp.Description = "The binary data to write to the file.";
			contentsProp.Validations.Add(new RequiredValidator());
			contentsProp.Order = 1;

			bool ownsFileHandle = OwnsFileHandle;

			var fileNotExists = Properties[BinaryFileWriteShared.FileDoesNotExistPropertyName];
			fileNotExists.Description = "Specify what to do if the output file does not exist.";
			fileNotExists.Order = 2;
			fileNotExists.IsVisible = ownsFileHandle;

			var fileExists = Properties[BinaryFileWriteShared.FileExistsPropertyName];
			fileExists.Description = "Specify what to do if the output file does exist.";
			fileExists.Order = 3;
			fileExists.IsVisible = ownsFileHandle;
		}

		private void BuildOutput()
		{
			Output = TypeReference.Create(typeof(string));
		}
	}
}