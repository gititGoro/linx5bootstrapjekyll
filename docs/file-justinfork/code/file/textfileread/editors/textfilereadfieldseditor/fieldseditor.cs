using System.Windows;
using System.Windows.Media;
using Twenty57.Linx.Components.File.TextFileRead.Editors.TextFileReadFieldsEditor.ViewModel;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.UI;
using Twenty57.Linx.Plugin.UI.Editors;

namespace Twenty57.Linx.Components.File.TextFileRead.Editors.TextFileReadFieldsEditor
{
	public class FieldsEditor : IPropertyEditor
	{
		public FieldsEditor()
		{
			InlineTemplate = EditorResources.TextFileReadFieldsInlineEditorTemplate;
		}

		public string Name { get { return null; } }
		public string IconResourcePath { get { return null; } }
		public DataTemplate InlineTemplate { get; private set; }

		public void EditValue(Property property, object designer)
		{
			var fieldsEditorViewModel = new FieldsEditorViewModel(property.GetValue<TextFileReaderFields>());
			var textFileReadDesigner = (TextFileReadDesigner)designer;
			fieldsEditorViewModel.SkipHeaderLines = textFileReadDesigner.SkipHeaderLines;
			fieldsEditorViewModel.SkipFooterLines = textFileReadDesigner.SkipFooterLines;

			if (TextFileReadFieldsEditor.Display(fieldsEditorViewModel))
			{
				property.Value = fieldsEditorViewModel.UpdatedFields;
				textFileReadDesigner.SkipHeaderLines = fieldsEditorViewModel.SkipHeaderLines;
				textFileReadDesigner.SkipFooterLines = fieldsEditorViewModel.SkipFooterLines;
			}
		}
	}
}
