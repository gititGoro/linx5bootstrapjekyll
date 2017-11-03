using System.Windows;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.UI.Editors;

namespace Twenty57.Linx.Components.Database.DbBulkCopy.Editors.TableChooser
{
	public class TableChooserItem : IPropertyEditor
	{
		public string Name 
		{ 
			get { return "Table chooser"; } 
		}

		public string IconResourcePath { get { return null; } }

		public DataTemplate InlineTemplate 
		{ 
			get { return EditorResources.TableChooserInlineEditorTemplate; } 
		}

		public void EditValue(Property property, object designer)
		{
			DbBulkCopyDesigner dbBulkCopyDesigner = (DbBulkCopyDesigner)designer;
			var editingInfo = new EditingInfo(dbBulkCopyDesigner);
			if (TableChooserWindow.Display(editingInfo))
			{
				dbBulkCopyDesigner.Context.TransactionManager.StartTransaction("Change table");
				editingInfo.UpdateDesigner(dbBulkCopyDesigner);
				dbBulkCopyDesigner.Context.TransactionManager.StopTransaction();
			}
		}
	}
}
