using System.Windows;
using System.Windows.Media;
using Twenty57.Linx.Components.Database.ExecuteSQL.Editors.SQLEditor.ViewModel;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.UI.Editors;

namespace Twenty57.Linx.Components.Database.ExecuteSQL.Editors.SQLEditor
{
	public class SQLEditorItem : IPropertyEditor
	{
		public string Name { get { return "SQL Editor"; } }
		public string IconResourcePath { get { return null; } }
		public DataTemplate InlineTemplate { get; private set; }

		public void EditValue(Property property, object designer)
		{
			SQLViewModel viewModel = new SQLViewModel((ExecuteSQLDesigner)designer);
			if (SQLEditor.Display(viewModel))
				property.Value = viewModel.SQL;
		}
	}
}
