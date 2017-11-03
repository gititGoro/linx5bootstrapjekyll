using System.Windows;
using System.Windows.Media;
using Twenty57.Linx.Components.Database.ExecuteStoredProcedure.Editors.StoredProcedureEditor;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.UI.Editors;

namespace Twenty57.Linx.Components.Database.ExecuteStoredProcedure.Editors.RefreshResultSets
{
	public class RefreshResultSetsItem : IPropertyEditor
	{
		public string Name { get { return "Refresh from database"; } }
		public DataTemplate InlineTemplate { get { return null; } }
		public string IconResourcePath { get { return null; } }

		public void EditValue(Property property, object designer)
		{
			var executeStoredProcedureDesigner = (ExecuteStoredProcedureDesigner)designer;
			EditingInfo editingInfo = new EditingInfo(executeStoredProcedureDesigner)
			{
				ShouldUpdateConnectionString = false,
				ShouldUpdateStoredProcedure = false,
				ShouldUpdateProcedureParameters = false,
				ShouldUpdateResultSets = true
			};
			if (StoredProcedureEditorItem.EditStoredProcedure(editingInfo))
			{
				executeStoredProcedureDesigner.Context.TransactionManager.StartTransaction("Refresh result sets from database");
				editingInfo.UpdateDesigner(executeStoredProcedureDesigner);
				executeStoredProcedureDesigner.Context.TransactionManager.StopTransaction();
			}
		}
	}
}
