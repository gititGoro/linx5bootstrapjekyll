using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Twenty57.Linx.Components.Database.Common;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.CodeGeneration;

namespace Twenty57.Linx.Components.Database.ExecuteStoredProcedure
{
	public class ExecuteStoredProcedureCodeGenerator : FunctionCodeGenerator
	{
		public ExecuteStoredProcedureCodeGenerator(IFunctionData data) : base(data) { }

		public override void GenerateCode(IFunctionBuilder functionBuilder)
		{
			ExecuteStoredProcedureX_Gen generator = new ExecuteStoredProcedureX_Gen();
			generator.Session = new Dictionary<string, object>();
			generator.Session.Add("FunctionContextProperty", functionBuilder.ContextParamName);
			ConnectionTypeSelection connectionType = FunctionData.Properties[DbShared.ConnectionTypePropertyName].GetValue<ConnectionTypeSelection>();
			bool useTransaction = connectionType == ConnectionTypeSelection.UseTransaction;
			generator.Session.Add("UseTransaction", useTransaction);
			if (useTransaction)
				generator.Session.Add("TransactionProperty", functionBuilder.GetParamName(DbShared.TransactionPropertyName));
			else
			{
				generator.Session.Add("ConnectionType", connectionType.ToConnectionType());
				generator.Session.Add("ConnectionStringProperty", functionBuilder.GetParamName(DbShared.ConnectionStringPropertyName));
			}
			generator.Session.Add("StoredProcedureProperty", functionBuilder.GetParamName(ExecuteStoredProcedureShared.StoredProcedurePropertyName));
			var parameters = FunctionData.Properties[ExecuteStoredProcedureShared.ParametersPropertyName].GetValue<DatabaseModel.ProcedureParameters>();
			generator.Session.Add("Parameters", parameters);
			generator.Session.Add("GetParamName", new Func<string, string>(functionBuilder.GetParamName));

			generator.Session.Add("OutputTypeName", FunctionData.Output == null ? null : functionBuilder.GetTypeName(FunctionData.Output));
			bool hasOutParameters = parameters.Any(p => (p.Direction != DatabaseModel.ParameterDirection.In) && (p.DataType != DatabaseModel.DataType.RefCursor));
			generator.Session.Add("OutParametersOutputPropertyName", hasOutParameters ? ExecuteStoredProcedureShared.OutParametersOutputPropertyName : null);
			generator.Session.Add("OutParametersOutputTypeName", hasOutParameters ? functionBuilder.GetTypeName(FunctionData.Output.GetProperty(ExecuteStoredProcedureShared.OutParametersOutputPropertyName).TypeReference) : null);

			OutputOption outputOption = FunctionData.Properties[ExecuteStoredProcedureShared.OutputOptionPropertyName].GetValue<OutputOption>();
			generator.Session.Add("OutputOption", outputOption);
			int resultSetCount = FunctionData.Properties[ExecuteStoredProcedureShared.ResultSetCountPropertyName].GetValue<int>();
			generator.Session.Add("ResultSets", Enumerable.Range(0, resultSetCount).Select(i => FunctionData.Properties[string.Format(ExecuteStoredProcedureShared.ResultSetPropertyNameFormat, i + 1)].GetValue<DatabaseModel.ResultSet>()).ToArray());
			if (resultSetCount != 0)
			{
				if (outputOption == OutputOption.RowByRow)
				{
					string[] executionPathKeys = Enumerable.Range(0, resultSetCount).Select(i => string.Format(ExecuteStoredProcedureShared.ResultSetExecutionPathNameFormat, i + 1)).ToArray();
					string[] executionPathNames = resultSetCount == 1 ? new string[] { string.Format(ExecuteStoredProcedureShared.ResultSetExecutionPathNameFormat, string.Empty) } : executionPathKeys;
					generator.Session.Add("RowTypeNames", executionPathKeys.Select(p => functionBuilder.GetTypeName(FunctionData.ExecutionPaths[p].Output)).ToArray());
					generator.Session.Add("ExecutionPathOutputName", functionBuilder.ExecutionPathOutParamName);
					generator.Session.Add("ExecutionPathNames", executionPathNames);
					generator.Session.Add("ResultSetRowOutputPropertyNames", null);
				}
				else if (outputOption == OutputOption.ListOfRows)
				{
					string[] resultSetRowOutputPropertyNames = resultSetCount == 1 ? new string[] { string.Format(ExecuteStoredProcedureShared.ResultSetRowsOutputPropertyNameFormat, string.Empty) } : Enumerable.Range(0, resultSetCount).Select(i => string.Format(ExecuteStoredProcedureShared.ResultSetRowsOutputPropertyNameFormat, i + 1)).ToArray();
					generator.Session.Add("RowTypeNames", resultSetRowOutputPropertyNames.Select(p => functionBuilder.GetTypeName(FunctionData.Output.GetProperty(p).TypeReference.GetEnumerableContentType())).ToArray());
					generator.Session.Add("ResultSetRowOutputPropertyNames", resultSetRowOutputPropertyNames);
					generator.Session.Add("ExecutionPathOutputName", null);
					generator.Session.Add("ExecutionPathNames", null);
				}
				else
				{
					string[] resultSetRowOutputPropertyNames = resultSetCount == 1 ? new string[] { string.Format(ExecuteStoredProcedureShared.ResultSetRowOutputPropertyNameFormat, string.Empty) } : Enumerable.Range(0, resultSetCount).Select(i => string.Format(ExecuteStoredProcedureShared.ResultSetRowOutputPropertyNameFormat, i + 1)).ToArray();
					generator.Session.Add("RowTypeNames", resultSetRowOutputPropertyNames.Select(p => functionBuilder.GetTypeName(FunctionData.Output.GetProperty(p).TypeReference)).ToArray());
					generator.Session.Add("ResultSetRowOutputPropertyNames", resultSetRowOutputPropertyNames);
					generator.Session.Add("ExecutionPathOutputName", null);
					generator.Session.Add("ExecutionPathNames", null);
				}
			}

			generator.Initialize();
			functionBuilder.AddCode(generator.TransformText());

			functionBuilder.AddAssemblyReference(typeof(ExecuteStoredProcedure));
			functionBuilder.AddAssemblyReference(typeof(IDbCommand));
		}
	}
}
