using System;
using Twenty57.Linx.Components.Database.Mongo.Common;
using Twenty57.Linx.Components.MongoDB;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.CodeGeneration;

namespace Twenty57.Linx.Components.Database.Mongo.MongoDBMapReduce
{
	public class MongoDBMapReduceCodeGenerator : FunctionCodeGenerator
	{
		public MongoDBMapReduceCodeGenerator(IFunctionData data)
			: base(data)
		{ }

		public override void GenerateCode(IFunctionBuilder functionBuilder)
		{
			var outputTypeReference = FunctionData.Properties[MongoDBMapReduceShared.Names.OutputType].GetValue<ITypeReference>();
			string outputType = functionBuilder.GetTypeName(outputTypeReference);
			if (string.IsNullOrEmpty(outputType))
				throw new ArgumentException("MongoDBMapReduce has no OutputType set");

			functionBuilder.AddAssemblyReference(typeof(MongoDBX));

			MongoDBCodeGenerator.CompileExpression(functionBuilder,
				FunctionData,
				MongoDBMapReduceShared.Names.Query,
				MongoDBMapReduceShared.Names.QueryExpressions
				);

			MongoDBCodeGenerator.CompileExpression(functionBuilder,
				FunctionData,
				MongoDBMapReduceShared.Names.Map,
				MongoDBMapReduceShared.Names.MapExpressions
				);

			MongoDBCodeGenerator.CompileExpression(functionBuilder,
				FunctionData,
				MongoDBMapReduceShared.Names.Reduce,
				MongoDBMapReduceShared.Names.ReduceExpressions
				);

			MongoDBCodeGenerator.CompileExpression(functionBuilder,
				FunctionData,
				MongoDBMapReduceShared.Names.Finalize,
				MongoDBMapReduceShared.Names.FinalizeExpressions
				);

			GenerateMapReduceCode(functionBuilder, outputTypeReference);

			MongoDBMapReduceShared.ReturnModeType returnMode = FunctionData.Properties[MongoDBMapReduceShared.Names.ReturnOptionsPropertyName].GetValue<MongoDBMapReduceShared.ReturnModeType>();
			switch (returnMode)
			{
				case MongoDBMapReduceShared.ReturnModeType.RowByRow:
					{
						functionBuilder.AddCode(string.Format(@"{0} = results.Select(v => new Twenty57.Linx.Plugin.Common.CodeGeneration.NextResult(""{1}"", v));",
							functionBuilder.ExecutionPathOutParamName,
							 MongoDBMapReduceShared.Names.ExecutionPath));
						break;
					}
				case MongoDBMapReduceShared.ReturnModeType.ListOfRows:
					{
						functionBuilder.AddCode("return results.ToList();");
						break;
					}
				case MongoDBMapReduceShared.ReturnModeType.FirstRow:
					{
						functionBuilder.AddCode("if (results.Count() > 0) return results.First(); else throw new Exception(\"No rows returned by query.\");");
						break;
					}
				case MongoDBMapReduceShared.ReturnModeType.FirstRowElseEmptyRow:
					{
						functionBuilder.AddCode("return results.FirstOrDefault();");
						break;
					}
			}
			functionBuilder.AddCode("}");
		}

		private static void GenerateMapReduceCode(IFunctionBuilder functionBuilder, ITypeReference outputTypeReference)
		{
			if (outputTypeReference.GetUnderlyingType() == typeof(string))
			{
				GenerateMapReduceCodeAsString(functionBuilder);
			}
			else
			{
				GenerateMapReduceCode(functionBuilder, functionBuilder.GetTypeName(outputTypeReference));
			}
		}

		private static void GenerateMapReduceCodeAsString(IFunctionBuilder functionBuilder)
		{
			functionBuilder.AddCode(string.Format(
				@"using (var mongoX = new Twenty57.Linx.Components.MongoDB.MongoDBX({0})){{
							mongoX.LogEvent += message => {1}.Log(message);
							var results = mongoX.MapReduceAsJson({2},{3},{4},{5},{6},{7},{8});",
				functionBuilder.GetParamName(MongoDBMapReduceShared.Names.ConnectionString),
				functionBuilder.ContextParamName,
				functionBuilder.GetParamName(MongoDBMapReduceShared.Names.Collection),
				functionBuilder.GetParamName(MongoDBMapReduceShared.Names.Query),
				functionBuilder.GetParamName(MongoDBMapReduceShared.Names.Map),
				functionBuilder.GetParamName(MongoDBMapReduceShared.Names.Reduce),
				functionBuilder.GetParamName(MongoDBMapReduceShared.Names.Finalize),
				functionBuilder.GetParamName(MongoDBMapReduceShared.Names.Sort),
				functionBuilder.GetParamName(MongoDBMapReduceShared.Names.Limit)));
		}

		private static void GenerateMapReduceCode(IFunctionBuilder functionBuilder, string outputType)
		{
			functionBuilder.AddCode(string.Format(
				@"using (var mongoX = new Twenty57.Linx.Components.MongoDB.MongoDBX({0})){{
							mongoX.LogEvent += message => {1}.Log(message);
							var results = mongoX.MapReduce<{2}>({3},{4},{5},{6},{7},{8},{9});",
				functionBuilder.GetParamName(MongoDBMapReduceShared.Names.ConnectionString),
				functionBuilder.ContextParamName,
				outputType,
				functionBuilder.GetParamName(MongoDBMapReduceShared.Names.Collection),
				functionBuilder.GetParamName(MongoDBMapReduceShared.Names.Query),
				functionBuilder.GetParamName(MongoDBMapReduceShared.Names.Map),
				functionBuilder.GetParamName(MongoDBMapReduceShared.Names.Reduce),
				functionBuilder.GetParamName(MongoDBMapReduceShared.Names.Finalize),
				functionBuilder.GetParamName(MongoDBMapReduceShared.Names.Sort),
				functionBuilder.GetParamName(MongoDBMapReduceShared.Names.Limit)));
		}
	}
}