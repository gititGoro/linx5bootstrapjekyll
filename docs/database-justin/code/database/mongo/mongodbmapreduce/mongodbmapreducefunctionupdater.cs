using System;
using System.Linq;
using Twenty57.Linx.Components.Database.Mongo.MongoDBMapReduce;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Utilities;

namespace Twenty57.Linx.Components.MongoDB
{
	internal class MongoDBMapReduceFunctionUpdater : IFunctionUpdater
	{
		private static MongoDBMapReduceFunctionUpdater instance;

		public string CurrentVersion { get { return "1"; } }

		public static IFunctionUpdater Instance
		{
			get
			{
				if (instance == null)
					instance = new MongoDBMapReduceFunctionUpdater();
				return instance;
			}
		}

		public IFunctionData Update(IFunctionData data)
		{
			if (data.Version == CurrentVersion)
				return data;

			if (string.IsNullOrEmpty(data.Version))
				data = UpdateToVersion1(data);

			if (data.Version == CurrentVersion)
				return data;

			throw new Exception($"Unknown version [{data.Version}] specified.");
		}

		private IFunctionData UpdateToVersion1(IFunctionData data)
		{
			data = AddMissingProperties(data);
			data = ChangeExecutionPathKey(data);
			return data.UpdateVersion("1");
		}

		private IFunctionData ChangeExecutionPathKey(IFunctionData data)
		{
			foreach (var executionPath in data.ExecutionPaths.Values.ToArray())
			{
				data = data.UpdateExecutionPath(executionPath, MongoDBMapReduceShared.Names.ExecutionPath, MongoDBMapReduceShared.Names.ExecutionPath, executionPath.Output, executionPath.IterationHint);
			}

			return data;
		}

		private IFunctionData AddMissingProperties(IFunctionData data)
		{
			IPropertyData loopResultsProperty;

			if (data.FindPropertyById(MongoDBMapReduceShared.Names.ReturnOptionsPropertyName) == null)
			{
				MongoDBMapReduceShared.ReturnModeType returnModeType = MongoDBMapReduceShared.ReturnModeType.ListOfRows;
				if (data.TryFindPropertyById("LoopResults", out loopResultsProperty))
				{
					returnModeType = loopResultsProperty.GetValue<bool>() ? MongoDBMapReduceShared.ReturnModeType.RowByRow : MongoDBMapReduceShared.ReturnModeType.ListOfRows;
					data = data.RemoveProperty(data.FindPropertyById("LoopResults"));
				}
				data = data.AddProperty(new Property(MongoDBMapReduceShared.Names.ReturnOptionsPropertyName, typeof(MongoDBMapReduceShared.ReturnModeType), ValueUseOption.DesignTime, returnModeType) { IsVisible = true });
			}

			return data;
		}
	}
}
