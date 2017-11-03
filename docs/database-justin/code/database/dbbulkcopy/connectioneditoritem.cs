using System;
using Twenty57.Linx.Plugin.Common;

namespace Twenty57.Linx.Components.Database.DbBulkCopy
{
	public class ConnectionEditorItem : UI.Editors.ConnectionEditor.ConnectionEditorItem
	{
		public override void EditValue(Property property, object designer)
		{
			base.EditValue(property, designer);
			var bulkCopyDesigner = (DbBulkCopyDesigner)designer;
			bulkCopyDesigner.Properties[DbBulkCopyShared.ConnectionTypePropertyName].Value = Enum.Parse(typeof(ConnectionType), bulkCopyDesigner.ConnectionType.ToString());
		}
	}
}
