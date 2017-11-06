using System.Windows;
using System.Windows.Media;
using Twenty57.Linx.Components.Database.Common;
using Twenty57.Linx.Components.Database.ExecuteStoredProcedure.Editors.ResultSetEditor.ViewModel;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.UI.Editors;

namespace Twenty57.Linx.Components.Database.ExecuteStoredProcedure.Editors.ResultSetEditor
{
	public class ResultSetEditorItem : IPropertyEditor
	{
		public ResultSetEditorItem()
		{
			InlineTemplate = EditorResources.DataSetInlineEditorTemplate;
		}

		public string Name { get { return "Result set editor"; } }
		public DataTemplate InlineTemplate { get; private set; }
		public string IconResourcePath { get { return null; } }

		public void EditValue(Property property, object designer)
		{
			ResultSetViewModel model = new ResultSetViewModel(property.GetValue<DatabaseModel.ResultSet>());
			if (ResultSetEditorWindow.Display(model, ((FunctionDesigner)designer).Context))
				property.Value = model.SavedResultSet;
		}
	}
}
