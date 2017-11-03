using Microsoft.CSharp.RuntimeBinder;
using System.Collections.Generic;
using System.Linq;
using Twenty57.Linx.Components.Database.Common;
using Twenty57.Linx.Components.Database.DbBulkCopy.Runtime;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.CodeGeneration;

namespace Twenty57.Linx.Components.Database.DbBulkCopy
{
	public class DbBulkCopyCodeGenerator : FunctionCodeGenerator
	{
		public DbBulkCopyCodeGenerator(IFunctionData data) : base(data) { }

		public override void GenerateCode(IFunctionBuilder functionBuilder)
		{
			var generator = new DbBulkCopy_Gen();
			generator.Session = new Dictionary<string, object>();
			generator.Session.Add("FunctionContextProperty", functionBuilder.ContextParamName);
			generator.Session.Add("ConnectionType", FunctionData.Properties[DbShared.ConnectionTypePropertyName].GetValue<ConnectionTypeSelection>().ToConnectionType());
			generator.Session.Add("ConnectionStringProperty", functionBuilder.GetParamName(DbShared.ConnectionStringPropertyName));
			generator.Session.Add("Timeout", FunctionData.Properties[DbBulkCopyShared.TimeoutPropertyName].GetValue<int>());
			generator.Session.Add("BatchSize", FunctionData.Properties[DbBulkCopyShared.BatchSizePropertyName].GetValue<int>());
			generator.Session.Add("TableName", FunctionData.Properties[DbBulkCopyShared.TableNamePropertyName].GetValue<string>());
			var columns = FunctionData.Properties[DbBulkCopyShared.ColumnsPropertyName].GetValue<DatabaseModel.Columns>();
			generator.Session.Add("ColumnNames", columns.Select(c => c.Name).ToArray());
			generator.Session.Add("ColumnPropertyNames", columns.Select(c => DbBulkCopyShared.GetPropertyName(c.Name)).ToArray());
			generator.Session.Add("ColumnDataTypes", columns.Select(c => c.DataType).ToArray());
			generator.Session.Add("ExecutionPathOutputName", functionBuilder.ExecutionPathOutParamName);
			generator.Session.Add("ExecutionPathName", DbBulkCopyShared.ExecutionPathName);

			generator.Initialize();
			functionBuilder.AddCode(generator.TransformText());
			functionBuilder.AddAssemblyReference(typeof(Loader));
			functionBuilder.AddAssemblyReference(typeof(RuntimeBinderException));
		}
	}
}
