using System.Windows;
using System.Windows.Media;
using Twenty57.Linx.Components.Database.ExecuteStoredProcedure.Editors.ProcedureParametersEditor.ViewModel;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.UI.Editors;

namespace Twenty57.Linx.Components.Database.ExecuteStoredProcedure.Editors.ProcedureParametersEditor
{
	public class ProcedureParametersEditorItem : IPropertyEditor
	{
		public ProcedureParametersEditorItem()
		{
			InlineTemplate = EditorResources.ProcedureParametersInlineEditorTemplate;
		}

		public string Name { get { return "Parameters Editor"; } }
		public string IconResourcePath { get { return null; } }
		public DataTemplate InlineTemplate { get; private set; }

		public void EditValue(Property property, object designer)
		{
			ExecuteStoredProcedureDesigner executeStoredProcedureDesigner = designer as ExecuteStoredProcedureDesigner;
			EditingInfo editingInfo = new EditingInfo(executeStoredProcedureDesigner)
			{
				ShouldUpdateConnectionString = false,
				ShouldUpdateStoredProcedure = false,
				ShouldUpdateProcedureParameters = true,
				ShouldUpdateResultSets = false
			};
			if (ProcedureParametersEditorWindow.Display(new ProcedureParametersViewModel(editingInfo)))
			{
				executeStoredProcedureDesigner.Context.TransactionManager.StartTransaction("Change procedure parameters");
				editingInfo.UpdateDesigner(executeStoredProcedureDesigner);
				executeStoredProcedureDesigner.Context.TransactionManager.StopTransaction();
			}
		}
	}
}
