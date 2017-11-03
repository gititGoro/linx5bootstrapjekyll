using System.Windows;
using Twenty57.Linx.Components.Database.Common;
using Twenty57.Linx.Components.Database.UI.Editors.ConnectionEditor;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.UI.Editors;

namespace Twenty57.Linx.Components.Database.UI.Editors.ConnectionEditor
{
	public class ConnectionEditorItem : IPropertyEditor
	{
		public string Name { get { return "Connection Editor"; } }
		public string IconResourcePath { get { return "/Resources/Connection Wizard.xaml"; } }
		public DataTemplate InlineTemplate { get { return null; } }

		public virtual void EditValue(Property property, object designer)
		{
			DbDesignerBase dbDesigner = designer as DbDesignerBase;
			ConnectionType connectionType = dbDesigner.ConnectionType;
			string connectionString = dbDesigner.ResolvedConnectionString;
			if ((ConnectionEditorWindow.EditConnectionString(ref connectionType, ref connectionString, dbDesigner.SupportedConnectionTypes)) 
				&& ((connectionType != dbDesigner.ConnectionType) || (connectionString != dbDesigner.ResolvedConnectionString)))
			{
				dbDesigner.Context.TransactionManager.StartTransaction("Change connection string");
				dbDesigner.ConnectionType = connectionType;
				dbDesigner.ConnectionString = connectionString;
				dbDesigner.Context.TransactionManager.StopTransaction();
			}
		}
	}
}
