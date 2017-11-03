using System.Collections.Generic;
using Twenty57.Linx.Components.File.Common;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.Types;
using Twenty57.Linx.Plugin.Common.Validation;

namespace Twenty57.Linx.Components.File.TextFileWrite
{
	public class TextFileWriteDesigner : TextFileDesignerBase
	{
		public TextFileWriteDesigner(IDesignerContext context)
			: base(context)
		{
			Version = FunctionUpdater.Instance.CurrentVersion;

			Properties.Add(new Property(TextFileWriteShared.ContentsPropertyName, typeof(string), ValueUseOption.RuntimeRead, string.Empty));
			Properties.Add(new Property(TextFileWriteShared.DestinationCodepagePropertyName, typeof(TextCodepage), ValueUseOption.DesignTime, TextCodepage.Default));
			Properties.Add(new Property(TextFileWriteShared.FileDoesNotExistPropertyName, typeof(DoesNotExistOptions), ValueUseOption.DesignTime, DoesNotExistOptions.CreateFile));
			Properties.Add(new Property(TextFileWriteShared.FileExistsPropertyName, typeof(ExistOptions), ValueUseOption.DesignTime, ExistOptions.AppendData));
			SetPropertyAttributes();

			BuildOutput();
		}

		public TextFileWriteDesigner(IFunctionData data, IDesignerContext context) : base(data, context) { }

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
			Property contentsProperty = Properties[TextFileWriteShared.ContentsPropertyName];
			contentsProperty.Order = 1;
			contentsProperty.Description = "The text data to write to the file.";
			contentsProperty.Validations.Add(new RequiredValidator());

			bool ownsFileHandle = OwnsFileHandle;
			Property destinationCodepageProperty = Properties[TextFileWriteShared.DestinationCodepagePropertyName];
			destinationCodepageProperty.Order = 2;
			destinationCodepageProperty.Description = "The encoding type to use when writing to the file.";
			destinationCodepageProperty.IsVisible = ownsFileHandle;

			Property doesNotExistProperty = Properties[TextFileWriteShared.FileDoesNotExistPropertyName];
			doesNotExistProperty.Order = 3;
			doesNotExistProperty.Description = "Specify what to do if the output file does not exist.";
			doesNotExistProperty.IsVisible = ownsFileHandle;

			Property existsProperty = Properties[TextFileWriteShared.FileExistsPropertyName];
			existsProperty.Order = 4;
			existsProperty.Description = "Specify what to do if the output file does exist.";
			existsProperty.IsVisible = ownsFileHandle;
		}

		private void BuildOutput()
		{
			Output = TypeReference.Create(typeof(string));
		}
	}
}
