using System;
using System.Linq;
using System.Collections.Generic;
using Twenty57.Linx.Components.Database.Common;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.CodeGeneration;
using Twenty57.Linx.Plugin.Common.Types;

namespace Twenty57.Linx.Components.Database.ExecuteSQL
{
	public class ExecuteSQLCodeGenerator : FunctionCodeGenerator
	{
		public ExecuteSQLCodeGenerator(IFunctionData data)
			: base(data)
		{ }

		public override void GenerateCode(IFunctionBuilder functionBuilder)
		{
			ExecuteSQL_Gen generator = new ExecuteSQL_Gen();
			generator.Session = new Dictionary<string, object>();
			generator.Session.Add("FunctionContextProperty", functionBuilder.ContextParamName);
			generator.Session.Add("GetParamName", new Func<string, string>(functionBuilder.GetParamName));

			// Function properties
			generator.Session.Add("Timeout", FunctionData.Properties[ExecuteSQLShared.TimeoutPropertyName].GetValue<int>());
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
			generator.Session.Add("Sql", functionBuilder.GetParamName(ExecuteSQLShared.SqlStatementPropertyName));
			ExecuteSQLShared.ReturnModeType returnMode = FunctionData.Properties[ExecuteSQLShared.ReturnOptionsPropertyName].GetValue<ExecuteSQLShared.ReturnModeType>();
			generator.Session.Add("ReturnMode", returnMode);

			// SQL placeholders
			SqlStringHandler sqlStringHandler = SqlStringHandler.GetSqlStringHandler(StaticSqlStatementValue);
			generator.Session.Add("SqlIdentifiers", CSharpUtilities.ArrayAsString(sqlStringHandler.Expressions.Select(ex => SqlStringHandler.CreateSqlExpression(ex.ExpressionText))));
			int parameterIndex = 0;
			generator.Session.Add("SqlValues", "new object[] { " + string.Join(", ", sqlStringHandler.Expressions.Select(ex => functionBuilder.GetParamName(ExecuteSQLShared.SqlValuePropertyPrefix + (++parameterIndex)))) + " }");

			// Output columns
			var resultType = FunctionData.Properties[ExecuteSQLShared.ResultTypePropertyName].GetValue<ResultType>();
			generator.Session.Add("ResultTypeFields", resultType.Fields);

			ITypeReference rowTypeReference = null;
			if (resultType.Fields.Count != 0)
			{
				switch (returnMode)
				{
					case ExecuteSQLShared.ReturnModeType.RowByRow:
						rowTypeReference = FunctionData.ExecutionPaths[ExecuteSQLShared.ExecutionPathName].Output;
						break;
					case ExecuteSQLShared.ReturnModeType.ListOfRows:
						rowTypeReference = FunctionData.Output.GetEnumerableContentType();
						break;
					case ExecuteSQLShared.ReturnModeType.FirstRow:
					case ExecuteSQLShared.ReturnModeType.FirstRowElseEmptyRow:
						rowTypeReference = FunctionData.Output;
						break;
				}
			}
			generator.Session.Add("RowTypeName", rowTypeReference == null ? null : functionBuilder.GetTypeName(rowTypeReference));
			generator.Session.Add("RowType", rowTypeReference);
			generator.Session.Add("ExecutionPathName", ExecuteSQLShared.ExecutionPathName);
			generator.Session.Add("ExecutionPathOutputName", functionBuilder.ExecutionPathOutParamName);

			generator.Initialize();
			functionBuilder.AddCode(generator.TransformText());
			functionBuilder.AddAssemblyReference(typeof(ExecuteSQL));
			functionBuilder.AddAssemblyReference(typeof(System.Data.IDataReader));
		}

		private string StaticSqlStatementValue
		{
			get
			{
				var sqlValue = FunctionData.Properties[ExecuteSQLShared.SqlStatementPropertyName].Value;
				return (sqlValue as string) ?? string.Empty;
			}
		}
	}
}
