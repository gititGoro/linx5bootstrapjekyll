using Twenty57.Linx.Components.Database.Common;

namespace Twenty57.Linx.Components.Database.DbBulkCopy.Editors.TableChooser.ViewModel
{
	public class TableColumnModel
	{
		public TableColumnModel(DatabaseModel.Column column)
		{
			Column = column;
		}

		public DatabaseModel.Column Column { get; private set; }

		public string Name { get { return Column.Name; } }
		public EnumWrapper<DatabaseModel.DataType> DataType { get { return Column.DataType; } }
		public string Precision { get { return Column.Precision.HasValue ? Column.Precision.ToString() : "N/A"; } }
		public string Scale { get { return Column.Scale.HasValue ? Column.Scale.ToString() : "N/A"; } }
		public string Size { get { return Column.Size.HasValue ? Column.Size.ToString() : "N/A"; } }
		public string Nullable { get { return Column.IsNullable ? "Yes" : "No"; } }
	}
}
