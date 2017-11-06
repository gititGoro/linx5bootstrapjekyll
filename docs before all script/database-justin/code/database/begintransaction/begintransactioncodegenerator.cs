using System.Collections.Generic;
using Twenty57.Linx.Components.Database.Common;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.CodeGeneration;

namespace Twenty57.Linx.Components.Database.BeginTransaction
{
	class BeginTransactionCodeGenerator : FunctionCodeGenerator
	{
		public BeginTransactionCodeGenerator(IFunctionData data) : base(data)
		{ }

		public override void GenerateCode(IFunctionBuilder functionBuilder)
		{
			BeginTransaction_Gen generator = new BeginTransaction_Gen();
			generator.Session = new Dictionary<string, object>();
			generator.Session.Add("FunctionContextProperty", functionBuilder.ContextParamName);
			ConnectionType connectionType = FunctionData.Properties[DbShared.ConnectionTypePropertyName].GetValue<ConnectionTypeSelection>().ToConnectionType().Value;
			generator.Session.Add("ConnectionType", connectionType);
			generator.Session.Add("ConnectionStringPropertyName", functionBuilder.GetParamName(DbShared.ConnectionStringPropertyName));
			string isolationLevelName = null;
			switch (connectionType)
			{
				case ConnectionType.SqlServer: isolationLevelName = FunctionData.Properties[BeginTransactionShared.SqlServerIsolationLevelPropertyName].Value.ToString(); break;
				case ConnectionType.Oracle: isolationLevelName = FunctionData.Properties[BeginTransactionShared.OracleIsolationLevelPropertyName].Value.ToString(); break;
				case ConnectionType.OleDb: isolationLevelName = FunctionData.Properties[BeginTransactionShared.OleDbIsolationLevelPropertyName].Value.ToString(); break;
				case ConnectionType.Odbc: isolationLevelName = FunctionData.Properties[BeginTransactionShared.OdbcIsolationLevelPropertyName].Value.ToString(); break;
			}
			generator.Session.Add("IsolationLevelName", isolationLevelName);
			generator.Session.Add("ExecutionPathName", BeginTransactionShared.ExecutionPathName);
			generator.Session.Add("ExecutionPathOutputName", functionBuilder.ExecutionPathOutParamName);

			generator.Initialize();
			functionBuilder.AddCode(generator.TransformText());

			functionBuilder.AddAssemblyReference(typeof(BeginTransaction));
			functionBuilder.AddAssemblyReference(typeof(ConnectionType));
			functionBuilder.AddAssemblyReference(typeof(System.Data.IDataReader));
		}
	}
}
