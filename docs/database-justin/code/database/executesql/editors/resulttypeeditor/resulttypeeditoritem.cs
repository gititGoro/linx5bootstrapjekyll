using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using Twenty57.Linx.Components.Database.ExecuteSQL.Editors.ResultTypeEditor.ViewModel;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.UI;
using Twenty57.Linx.Plugin.UI.Editors;

namespace Twenty57.Linx.Components.Database.ExecuteSQL.Editors.ResultTypeEditor
{
	public class ResultTypeEditorItem : IPropertyEditor
	{
		public ResultTypeEditorItem()
		{
			InlineTemplate = EditorResources.ResultTypeInlineEditorTemplate;
		}

		public string Name { get { return "Result Type Editor"; } }
		public string IconResourcePath { get { return null; } }
		public DataTemplate InlineTemplate { get; private set; }

		public void EditValue(Property property, object designer)
		{
			ExecuteSQLDesigner executeSqlDesigner = (ExecuteSQLDesigner)designer;
			ResultTypeEditorWindow.Display(new ResultTypeViewModel(executeSqlDesigner));
		}
	}
}
