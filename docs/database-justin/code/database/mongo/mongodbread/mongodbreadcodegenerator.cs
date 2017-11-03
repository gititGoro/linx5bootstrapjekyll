using System;
using Twenty57.Linx.Components.Database.Mongo.Common;
using Twenty57.Linx.Components.MongoDB;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.CodeGeneration;

namespace Twenty57.Linx.Components.Database.Mongo.MongoDBRead
{
	public class MongoDBReadCodeGenerator : FunctionCodeGenerator
	{
		public MongoDBReadCodeGenerator(IFunctionData data)
			: base(data)
		{ }

		public override void GenerateCode(IFunctionBuilder functionBuilder)
		{
			var outputTypeReference = FunctionData.Properties[MongoDBReadShared.Names.OutputType].GetValue<ITypeReference>();
			string outputType = functionBuilder.GetTypeName(outputTypeReference);
			if (string.IsNullOrEmpty(outputType))
				throw new ArgumentException("MongoDBRead has no OutputType set");

			functionBuilder.AddAssemblyReference(typeof(MongoDBX));

			MongoDBCodeGenerator.CompileExpression(functionBuilder,
				FunctionData,
				MongoDBReadShared.Names.AggregationPipeline,
				MongoDBReadShared.Names.AggregationPipelineExpressions);

			MongoDBCodeGenerator.CompileExpression(functionBuilder,
				FunctionData,
				MongoDBReadShared.Names.Query,
				MongoDBReadShared.Names.QueryExpressions);

			MongoDBCodeGenerator.CompileExpression(functionBuilder,
				FunctionData,
				MongoDBReadShared.Names.Query,
				MongoDBReadShared.Names.QueryExpressions);

			MongoDBCodeGenerator.CompileExpression(functionBuilder,
				FunctionData,
				MongoDBReadShared.Names.Fields,
				MongoDBReadShared.Names.FieldsExpressions);


			MongoDBReadShared.OperationType operation = FunctionData.Properties[MongoDBReadShared.Names.Operation].GetValue<MongoDBReadShared.OperationType>();
			switch (operation)
			{
				case MongoDBReadShared.OperationType.Aggregate:
					AddAggregateCode(functionBuilder, outputTypeReference);
					break;
				case MongoDBReadShared.OperationType.Find:
					AddFindCode(functionBuilder, outputTypeReference);
					break;
				default:
					throw new NotSupportedException(string.Format("Invalid operation type [{0}] specified.", operation));
			}

			MongoDBReadShared.ReturnModeType returnMode = FunctionData.Properties[MongoDBReadShared.Names.ReturnOptionsPropertyName].GetValue<MongoDBReadShared.ReturnModeType>();
			switch (returnMode)
			{
				case MongoDBReadShared.ReturnModeType.RowByRow:
					{
						functionBuilder.AddCode(string.Format(@"{0} = results.Select(v => new Twenty57.Linx.Plugin.Common.CodeGeneration.NextResult(""{1}"", v));",
							functionBuilder.ExecutionPathOutParamName,
							MongoDBReadShared.Names.ExecutionPath));
						break;
					}
				case MongoDBReadShared.ReturnModeType.ListOfRows:
					{
						functionBuilder.AddCode("return results.ToList();");
						break;
					}
				case MongoDBReadShared.ReturnModeType.FirstRow:
					{
						functionBuilder.AddCode("if (results.Count() > 0) return results.First(); else throw new Exception(\"No rows returned by query.\");");
						break;
					}
				case MongoDBReadShared.ReturnModeType.FirstRowElseEmptyRow:
					{
						functionBuilder.AddCode("return results.FirstOrDefault();");
						break;
					}
			}
			functionBuilder.AddCode("}");
		}

		private static void AddFindCode(IFunctionBuilder functionBuilder, ITypeReference outputType)
		{
			if(outputType.GetUnderlyingType() == typeof(string))
			{
				AddFindCodeAsString(functionBuilder);
			}
			else
			{
				AddFindCode(functionBuilder, functionBuilder.GetTypeName(outputType));
			}
		}

		private static void AddFindCodeAsString(IFunctionBuilder functionBuilder)
		{
			functionBuilder.AddCode(string.Format(
				@"using (var mongoX = new Twenty57.Linx.Components.MongoDB.MongoDBX({0})){{
						mongoX.LogEvent += message => {1}.Log(message);
						var results = mongoX.FindAsJson({2},{3},{4},{5},{6},{7});",
				functionBuilder.GetParamName(MongoDBReadShared.Names.ConnectionString),
				functionBuilder.ContextParamName,
				functionBuilder.GetParamName(MongoDBReadShared.Names.Collection),
				functionBuilder.GetParamName(MongoDBReadShared.Names.Query),
				functionBuilder.GetParamName(MongoDBReadShared.Names.Fields),
				functionBuilder.GetParamName(MongoDBReadShared.Names.Sort),
				functionBuilder.GetParamName(MongoDBReadShared.Names.Skip),
				functionBuilder.GetParamName(MongoDBReadShared.Names.Limit)));
		}

		private static void AddFindCode(IFunctionBuilder functionBuilder, string outputTypeName)
		{
			functionBuilder.AddCode(string.Format(
				@"using (var mongoX = new Twenty57.Linx.Components.MongoDB.MongoDBX({0})){{
						mongoX.LogEvent += message => {1}.Log(message);
						var results = mongoX.Find<{2}>({3},{4},{5},{6},{7},{8});",
				functionBuilder.GetParamName(MongoDBReadShared.Names.ConnectionString),
				functionBuilder.ContextParamName,
				outputTypeName,
				functionBuilder.GetParamName(MongoDBReadShared.Names.Collection),
				functionBuilder.GetParamName(MongoDBReadShared.Names.Query),
				functionBuilder.GetParamName(MongoDBReadShared.Names.Fields),
				functionBuilder.GetParamName(MongoDBReadShared.Names.Sort),
				functionBuilder.GetParamName(MongoDBReadShared.Names.Skip),
				functionBuilder.GetParamName(MongoDBReadShared.Names.Limit)));
		}

		private static void AddAggregateCode(IFunctionBuilder functionBuilder, ITypeReference outputType)
		{
			if(outputType.GetUnderlyingType() == typeof(string))
			{
				AddAggregateCodeAsString(functionBuilder);
			}
			else
			{
				AddAggregateCode(functionBuilder, functionBuilder.GetTypeName(outputType));
			}
		}

		private static void AddAggregateCode(IFunctionBuilder functionBuilder, string outputTypeName)
		{
			functionBuilder.AddCode(string.Format(
				@"using (var mongoX = new Twenty57.Linx.Components.MongoDB.MongoDBX({0})){{
						mongoX.LogEvent += message => {1}.Log(message);
						var results = mongoX.Aggregate<{2}>({3},{4});",
				functionBuilder.GetParamName(MongoDBReadShared.Names.ConnectionString),
				functionBuilder.ContextParamName,
				outputTypeName,
				functionBuilder.GetParamName(MongoDBReadShared.Names.Collection),
				functionBuilder.GetParamName(MongoDBReadShared.Names.AggregationPipeline)));
		}

		private static void AddAggregateCodeAsString(IFunctionBuilder functionBuilder)
		{
			functionBuilder.AddCode(string.Format(
				@"using (var mongoX = new Twenty57.Linx.Components.MongoDB.MongoDBX({0})){{
						mongoX.LogEvent += message => {1}.Log(message);
						var results = mongoX.AggregateAsJson({2},{3});",
				functionBuilder.GetParamName(MongoDBReadShared.Names.ConnectionString),
				functionBuilder.ContextParamName,
				functionBuilder.GetParamName(MongoDBReadShared.Names.Collection),
				functionBuilder.GetParamName(MongoDBReadShared.Names.AggregationPipeline)));
		}
	}
}