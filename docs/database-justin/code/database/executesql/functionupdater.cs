using System;
using System.Linq;
using Twenty57.Linx.Components.Database.Common;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.Types;
using Twenty57.Linx.Plugin.Utilities;

namespace Twenty57.Linx.Components.Database.ExecuteSQL
{
	internal class FunctionUpdater : IFunctionUpdater
	{
		private static FunctionUpdater instance;

		public string CurrentVersion { get { return "3"; } }

		public static IFunctionUpdater Instance
		{
			get
			{
				if (instance == null)
					instance = new FunctionUpdater();
				return instance;
			}
		}

		public IFunctionData Update(IFunctionData data)
		{
			if (data.Version == CurrentVersion)
				return data;

			bool didUpdate = false;
			if (string.IsNullOrEmpty(data.Version))
			{
				data = UpdateToVersion1(data);
				didUpdate = true;
			}
			if (data.Version == "1")
			{
				data = UpdateToVersion2(data);
				didUpdate = true;
			}
			if (data.Version == "2")
			{
				data = UpdateToVersion3(data);
				didUpdate = true;
			}

			if (didUpdate)
				return data;

			throw new Exception(string.Format("Unknown version [{0}] specified.", data.Version));
		}

		private IFunctionData UpdateToVersion1(IFunctionData data)
		{
			data = AddMissingProperties(data);
			data = AddTransactionProperty(data);
			data = TransformConnectionTypeProperty(data);

			return data.UpdateVersion("1");
		}

		private IFunctionData AddMissingProperties(IFunctionData data)
		{
			var connectionType = ConnectionType.OleDb;
			if (data.Properties[DbShared.ConnectionStringPropertyName].Value is string)
				connectionType = DatabaseAssistant.DetectConnectionType(data.Properties[DbShared.ConnectionStringPropertyName].Value as string) ?? ConnectionType.OleDb;

			if (data.FindPropertyById(DbShared.ConnectionTypePropertyName) == null)
				data = data.AddProperty(new Property(DbShared.ConnectionTypePropertyName, typeof(ConnectionType), ValueUseOption.DesignTime, ConnectionType.SqlServer)
				{
					Value = connectionType
				});

			if (data.FindPropertyById(DbShared.DesignTimeConnectionTypePropertyName) == null)
				data = data.AddProperty(new Property(DbShared.DesignTimeConnectionTypePropertyName, typeof(ConnectionType), ValueUseOption.DesignTime, ConnectionType.SqlServer)
				{
					Value = connectionType,
					IsVisible = false
				});

			if (data.FindPropertyById(DbShared.DesignTimeConnectionStringPropertyName) == null)
				data = data.AddProperty(new Property(DbShared.DesignTimeConnectionStringPropertyName, typeof(string), ValueUseOption.DesignTime, string.Empty) { IsVisible = false });

			return data;
		}

		private IFunctionData AddTransactionProperty(IFunctionData data)
		{
			if (data.FindPropertyById(DbShared.TransactionPropertyName) == null)
				data = data.AddProperty(new Property(DbShared.TransactionPropertyName, typeof(Transaction), ValueUseOption.DesignTime, null));

			return data;
		}

		private IFunctionData TransformConnectionTypeProperty(IFunctionData data)
		{
			IPropertyData connectionTypeProperty;
			if (data.TryFindPropertyById(DbShared.ConnectionTypePropertyName, out connectionTypeProperty))
			{
				if (connectionTypeProperty.TypeReference.IsCompiled && connectionTypeProperty.TypeReference.GetUnderlyingType() == typeof(ConnectionTypeSelection))
					return data;

				var newConnectionTypeProperty = new Property(DbShared.ConnectionTypePropertyName, typeof(ConnectionTypeSelection), ValueUseOption.DesignTime, ConnectionTypeSelection.SqlServer)
				{
					Value = connectionTypeProperty.GetValue<ConnectionType>().ToConnectionTypeSelection(),
					IsVisible = connectionTypeProperty.IsVisible
				};

				data = data.ReplaceProperty(connectionTypeProperty, newConnectionTypeProperty);
			}

			return data;
		}

		private IFunctionData UpdateToVersion2(IFunctionData data)
		{
			var resultFieldsProperty = data.Properties[ExecuteSQLShared.ResultTypePropertyName];
			var resultType = new ResultType();
			resultType.Fields.AddRange(resultFieldsProperty.GetValue<ResultTypeFields>());
			data = data.ReplaceProperty(resultFieldsProperty,
				new Property(ExecuteSQLShared.ResultTypePropertyName, typeof(ResultType), ValueUseOption.DesignTime, new ResultType()) { Value = resultType });

			return data.UpdateVersion("2");
		}

		private IFunctionData UpdateToVersion3(IFunctionData data)
		{
			var resultTypeProperty = data.Properties[ExecuteSQLShared.ResultTypePropertyName];

			var resultTypeValue = resultTypeProperty.GetValue<ResultType>();
			var requireUpdate = false;
			foreach (var field in resultTypeValue.Fields)
			{
				if (field.Type == typeof(Int32))
				{
					field.Type = typeof(Int64);
					requireUpdate = true;
				}
			}

			if (requireUpdate)
			{
				data = data.UpdateProperty(resultTypeProperty, resultTypeProperty.Id, resultTypeProperty.Name, resultTypeProperty.TypeReference, resultTypeValue, resultTypeProperty.IsVisible, resultTypeProperty.ValueUsage);

				var returnTypeValue = resultTypeValue.BuildRowTypeFromFields();
				if (returnTypeValue != null)
				{
					if (data.Output != null)
					{
						if (data.Output.IsList)
							data = data.UpdateOutput(TypeReference.CreateList(returnTypeValue));
						else
							data = data.UpdateOutput(returnTypeValue);
					}

					IExecutionPathData executionPath;
					if (data.TryFindExecutionPathByKey("ForEachRow", out executionPath) && executionPath.Output != null)
						data = data.UpdateExecutionPath(executionPath, executionPath.Key, executionPath.Name, returnTypeValue, executionPath.IterationHint);
				}
			}

			return data.UpdateVersion("3");
		}
	}
}
