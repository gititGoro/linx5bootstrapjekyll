using System;
using Twenty57.Linx.Components.Database.Common;
using Twenty57.Linx.Components.Database.Mongo.Common;
using Twenty57.Linx.Components.MongoDB;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.CodeGeneration;

namespace Twenty57.Linx.Components.Database.Mongo.MongoDBWrite
{
	internal class MongoDBWriteCodeGenerator : FunctionCodeGenerator
	{
		public MongoDBWriteCodeGenerator(IFunctionData data)
			: base(data)
		{ }

		public override void GenerateCode(IFunctionBuilder functionBuilder)
		{
			IPropertyData opProp = FunctionData.Properties[MongoDBWriteShared.Names.Operation];
			var Operation = FunctionData.Properties[MongoDBWriteShared.Names.Operation].GetValue<MongoDBWriteOperation>();
			var InsertIfNotFound = FunctionData.Properties[MongoDBWriteShared.Names.InsertIfNotFound].GetValue<bool>();
			string operation = "";
			var updateOperation = functionBuilder.GetParamName(MongoDBWriteShared.Names.UpdateOperation);
			var connectionString = functionBuilder.GetParamName(MongoDBWriteShared.Names.ConnectionString);

			ITypeReference dataTypeReference = functionBuilder.GetTypeReference(
				FunctionData.Properties[MongoDBWriteShared.Names.Data].Value
				);

			string dataType = functionBuilder.GetTypeName(dataTypeReference);

			if (!dataTypeReference.IsGenerated && dataTypeReference.IsCompiled)
			{
				functionBuilder.AddAssemblyReference(dataTypeReference.GetUnderlyingType());
			}

			MongoDBCodeGenerator.CompileExpression(functionBuilder,
				FunctionData,
				MongoDBWriteShared.Names.UpdateOperation,
				MongoDBWriteShared.Names.UpdateOperationExpressions
				);

			MongoDBCodeGenerator.CompileExpression(functionBuilder,
				FunctionData,
				MongoDBWriteShared.Names.Criteria,
				MongoDBWriteShared.Names.CriteriaExpressions
				);

			switch (Operation)
			{
				case MongoDBWriteOperation.DeleteAll:
					operation = "DeleteAll(Collection);";
					break;
				case MongoDBWriteOperation.Delete:
					operation = "Delete(Collection, Criteria);";
					break;

				case MongoDBWriteOperation.Update:
					operation = "Update(Collection, Criteria, " + updateOperation + ", " + (InsertIfNotFound ? "true" : "false") + ");";
					break;
				case MongoDBWriteOperation.Replace:
					operation = "Replace(Collection, Criteria, (" + dataType + ")Data, " + (InsertIfNotFound ? "true" : "false") + ");";
					break;
				case MongoDBWriteOperation.Insert:
				default:
					operation = "Insert(Collection, (" + dataType + ")Data);";
					break;
			}

			functionBuilder.AddAssemblyReference(typeof(MongoDBX));
			functionBuilder.AddAssemblyReference(typeof(SqlStringHandler));
			functionBuilder.AddCode(String.Format(
				@"using (var mongoX = new Twenty57.Linx.Components.MongoDB.MongoDBX({0})){{
						mongoX.LogEvent += message => {1}.Log(message);
						mongoX.{2}
					}}", connectionString, functionBuilder.ContextParamName, operation));
		}
	}
}