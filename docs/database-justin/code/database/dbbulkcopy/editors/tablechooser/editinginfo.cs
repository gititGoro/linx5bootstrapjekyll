using Twenty57.Linx.Components.Database.Common;

namespace Twenty57.Linx.Components.Database.DbBulkCopy.Editors.TableChooser
{
	public class EditingInfo
	{
		public EditingInfo(DbBulkCopyDesigner designer)
		{
			AllowedConnectionTypes = designer.SupportedConnectionTypes;
			ConnectionType = designer.DesignTimeConnectionType;
			ConnectionString = designer.DesignTimeConnectionString;
			TableName = designer.TableName;
			TableColumns = designer.TableColumns;
		}

		public bool ShouldUpdateConnectionString { get; set; }

		public Common.ConnectionType[] AllowedConnectionTypes { get; private set; }
		public Common.ConnectionType ConnectionType { get; set; }
		public string ConnectionString { get; set; }
		public string TableName { get; set; }
		public DatabaseModel.Columns TableColumns { get; set; }

		public void UpdateDesigner(DbBulkCopyDesigner designer)
		{
			if (ShouldUpdateConnectionString)
			{
				designer.ConnectionType = ConnectionType;
				designer.ConnectionString = ConnectionString;
			}
			else
			{
				designer.DesignTimeConnectionType = ConnectionType;
				designer.DesignTimeConnectionString = ConnectionString;
			}

			designer.TableColumns = TableColumns;
			designer.TableName = TableName;
		}
	}
}
