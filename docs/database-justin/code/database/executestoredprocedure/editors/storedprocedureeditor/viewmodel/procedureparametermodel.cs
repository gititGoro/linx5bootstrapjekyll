using Twenty57.Linx.Components.Database.Common;

namespace Twenty57.Linx.Components.Database.ExecuteStoredProcedure.Editors.StoredProcedureEditor.ViewModel
{
	public class ProcedureParameterModel
	{
		public ProcedureParameterModel(DatabaseModel.ProcedureParameter procedureParameter)
		{
			ProcedureParameter = procedureParameter;
		}

		public DatabaseModel.ProcedureParameter ProcedureParameter { get; private set; }

		public string Name { get { return ProcedureParameter.Name; } }
		public EnumWrapper<DatabaseModel.ParameterDirection> Direction { get { return ProcedureParameter.Direction; } }
		public EnumWrapper<DatabaseModel.DataType> DataType { get { return ProcedureParameter.DataType; } }
		public string Precision { get { return ProcedureParameter.Precision.HasValue ? ProcedureParameter.Precision.ToString() : "N/A"; } }
		public string Scale { get { return ProcedureParameter.Scale.HasValue ? ProcedureParameter.Scale.ToString() : "N/A"; } }
		public string Size { get { return ProcedureParameter.Size.HasValue ? ProcedureParameter.Size.ToString() : "N/A"; } }
		public string Nullable { get { return ProcedureParameter.IsNullable ? "Yes" : "No"; } }
	}
}
