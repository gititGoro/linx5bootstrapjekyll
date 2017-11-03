using System;
using System.Collections.Generic;
using System.Linq;
using Twenty57.Linx.Components.Database.Mongo.MongoDBRead;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Utilities;

namespace Twenty57.Linx.Components.MongoDB
{
	internal class MongoDBReadFunctionUpdater : IFunctionUpdater
	{
		private static MongoDBReadFunctionUpdater instance;

		public string CurrentVersion { get { return "2"; } }

		public static IFunctionUpdater Instance
		{
			get
			{
				if (instance == null)
					instance = new MongoDBReadFunctionUpdater();
				return instance;
			}
		}

		public IFunctionData Update(IFunctionData data)
		{
			if (data.Version == CurrentVersion)
				return data;

			if (string.IsNullOrEmpty(data.Version))
				data = UpdateToVersion1(data);

			if (data.Version == "1")
				data = UpdateToVersion2(data);

			if (data.Version == CurrentVersion)
				return data;

			throw new Exception($"Unknown version [{data.Version}] specified.");
		}

		private IFunctionData UpdateToVersion1(IFunctionData data)
		{
			data = AddMissingProperties(data);
			return data.UpdateVersion("1");
		}

		private IFunctionData UpdateToVersion2(IFunctionData data)
		{
			IPropertyData loopResultsProperty;

			if (data.FindPropertyById(MongoDBReadShared.Names.ReturnOptionsPropertyName) == null)
			{
				MongoDBReadShared.ReturnModeType returnModeType = MongoDBReadShared.ReturnModeType.ListOfRows;
				if (data.TryFindPropertyById("LoopResults", out loopResultsProperty))
				{
					returnModeType = loopResultsProperty.GetValue<bool>() ? MongoDBReadShared.ReturnModeType.RowByRow : MongoDBReadShared.ReturnModeType.ListOfRows;
					data = data.RemoveProperty(data.FindPropertyById("LoopResults"));
				}
				data = data.AddProperty(new Property(MongoDBReadShared.Names.ReturnOptionsPropertyName, typeof(MongoDBReadShared.ReturnModeType), ValueUseOption.DesignTime, returnModeType) { IsVisible = true });
			}

			data = ChangeExecutionPathKey(data);
			return data.UpdateVersion("2");
		}

		private IFunctionData ChangeExecutionPathKey(IFunctionData data)
		{
			foreach (var executionPath in data.ExecutionPaths.Values.ToArray())
			{
				data = data.UpdateExecutionPath(executionPath, MongoDBReadShared.Names.ExecutionPath, MongoDBReadShared.Names.ExecutionPath, executionPath.Output, executionPath.IterationHint);
			}

			return data;
		}

		private IFunctionData AddMissingProperties(IFunctionData data)
		{
			IPropertyData criteriaProperty;
			IPropertyData aggregationPipelineProperty;

			object criteria = string.Empty;
			object aggregationPipeline = string.Empty;
			MongoDBReadShared.OperationType operationType = MongoDBReadShared.OperationType.Find;

			if (data.TryFindPropertyById("Criteria", out criteriaProperty) &&
				data.TryFindPropertyById("Aggregation pipeline", out aggregationPipelineProperty))
			{
				criteria = criteriaProperty.Value;
				aggregationPipeline = aggregationPipelineProperty.Value;

				if (string.IsNullOrEmpty(criteria.ToString()) && !string.IsNullOrEmpty(aggregationPipeline.ToString()))
					operationType = MongoDBReadShared.OperationType.Aggregate;
				data = data.RemoveProperty(criteriaProperty);
				data = data.RemoveProperty(aggregationPipelineProperty);
			}

			if (data.FindPropertyById(MongoDBReadShared.Names.Operation) == null)
				data = data.AddProperty(new Property(MongoDBReadShared.Names.Operation, typeof(MongoDBReadShared.OperationType), ValueUseOption.DesignTime, operationType));

			if (data.FindPropertyById(MongoDBReadShared.Names.AggregationPipeline) == null)
				data = data.AddProperty(new Property(MongoDBReadShared.Names.AggregationPipeline, typeof(string), ValueUseOption.RuntimeRead, aggregationPipeline) { IsVisible = true });

			if (data.FindPropertyById(MongoDBReadShared.Names.Query) == null)
				data = data.AddProperty(new Property(MongoDBReadShared.Names.Query, typeof(string), ValueUseOption.RuntimeRead, criteria) { IsVisible = true });

			if (data.FindPropertyById(MongoDBReadShared.Names.Fields) == null)
				data = data.AddProperty(new Property(MongoDBReadShared.Names.Fields, typeof(string), ValueUseOption.RuntimeRead, string.Empty) { IsVisible = true });

			if (data.FindPropertyById(MongoDBReadShared.Names.Sort) == null)
				data = data.AddProperty(new Property(MongoDBReadShared.Names.Sort, typeof(string), ValueUseOption.RuntimeRead, string.Empty) { IsVisible = true });

			if (data.FindPropertyById(MongoDBReadShared.Names.Skip) == null)
				data = data.AddProperty(new Property(MongoDBReadShared.Names.Skip, typeof(int), ValueUseOption.RuntimeRead, 0) { IsVisible = true });

			if (data.FindPropertyById(MongoDBReadShared.Names.Limit) == null)
				data = data.AddProperty(new Property(MongoDBReadShared.Names.Limit, typeof(int), ValueUseOption.RuntimeRead, 0) { IsVisible = true });

			string criteriaExpressionKey = "CriteriaExpression";
			List<string> criteriaExpressionIDs = data.Properties.Keys.Where(key => key.StartsWith(criteriaExpressionKey)).ToList();
			foreach (string criteriaExpressionID in criteriaExpressionIDs)
			{
				IPropertyData criteriaExpressionProperty = data.Properties[criteriaExpressionID];
				string queryExpressionName = criteriaExpressionID.Replace(criteriaExpressionKey, MongoDBReadShared.Names.QueryExpressions);
				data = data.AddProperty(new Property(queryExpressionName, criteriaExpressionProperty.TypeReference, criteriaExpressionProperty.ValueUsage, criteriaExpressionProperty.Value) { IsVisible = criteriaExpressionProperty.IsVisible });
				data = data.RemoveProperty(criteriaExpressionProperty);
			}

			return data;
		}
	}
}
