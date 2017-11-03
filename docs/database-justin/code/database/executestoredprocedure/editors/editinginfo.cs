using Twenty57.Linx.Components.Database.Common;

namespace Twenty57.Linx.Components.Database.ExecuteStoredProcedure.Editors
{
	public class EditingInfo
	{
		public EditingInfo(ExecuteStoredProcedureDesigner designer)
		{
			ConnectionType = designer.DesignTimeConnectionType;
			ConnectionString = designer.DesignTimeConnectionString;
			StoredProcedure = designer.DesignTimeStoredProcedure;
			ProcedureParameters = designer.Parameters;
			ResultSets = designer.ResultSets;
		}

		public bool ShouldUpdateConnectionString { get; set; }
		public bool ShouldUpdateStoredProcedure { get; set; }
		public bool ShouldUpdateProcedureParameters { get; set; }
		public bool ShouldUpdateResultSets { get; set; }

		public ConnectionType ConnectionType { get; internal set; }
		public string ConnectionString { get; internal set; }
		public string StoredProcedure { get; internal set; }
		public DatabaseModel.ProcedureParameters ProcedureParameters { get; internal set; }
		public DatabaseModel.ResultSets ResultSets { get; internal set; }

		public void UpdateDesigner(ExecuteStoredProcedureDesigner designer)
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

			if (ShouldUpdateStoredProcedure)
				designer.StoredProcedure = StoredProcedure;
			else
				designer.DesignTimeStoredProcedure = StoredProcedure;

			if (ShouldUpdateProcedureParameters)
				designer.Parameters = ProcedureParameters;

			if (ShouldUpdateResultSets)
				designer.ResultSets = ResultSets;
		}
	}
}
