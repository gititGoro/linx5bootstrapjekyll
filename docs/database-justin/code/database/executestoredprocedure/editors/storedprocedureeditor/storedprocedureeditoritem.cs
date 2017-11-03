using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Media;
using Twenty57.Linx.Components.Database.ExecuteStoredProcedure.Editors.StoredProcedureEditor.ViewModel;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.UI.Editors;

namespace Twenty57.Linx.Components.Database.ExecuteStoredProcedure.Editors.StoredProcedureEditor
{
	public class StoredProcedureEditorItem : IPropertyEditor
	{
		public string Name { get { return "Stored Procedure Editor"; } }
		public DataTemplate InlineTemplate { get { return null; } }
		public string IconResourcePath { get { return "/Resources/Stored Procedure Editor.xaml"; } }

		public void EditValue(Property storedProcedureProperty, object designer)
		{
			ExecuteStoredProcedureDesigner executeStoredProcedureDesigner = designer as ExecuteStoredProcedureDesigner;
			EditingInfo editingInfo = new EditingInfo(executeStoredProcedureDesigner)
			{
				ShouldUpdateConnectionString = false,
				ShouldUpdateStoredProcedure = true,
				ShouldUpdateProcedureParameters = true,
				ShouldUpdateResultSets = true
			};
			if (EditStoredProcedure(editingInfo))
			{
				executeStoredProcedureDesigner.Context.TransactionManager.StartTransaction("Change stored procedure");
				editingInfo.UpdateDesigner(executeStoredProcedureDesigner);
				executeStoredProcedureDesigner.Context.TransactionManager.StopTransaction();
			}
		}

		public static bool EditStoredProcedure(EditingInfo editingInfo)
		{
			return StoredProcedureEditorWindow.Display(editingInfo);
		}
	}
}
