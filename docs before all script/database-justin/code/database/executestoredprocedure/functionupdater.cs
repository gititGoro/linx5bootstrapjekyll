using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Twenty57.Linx.Components.Database.Common;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.Types;
using Twenty57.Linx.Plugin.Utilities;

namespace Twenty57.Linx.Components.Database.ExecuteStoredProcedure
{
	internal class FunctionUpdater : IFunctionUpdater
	{
		private static FunctionUpdater instance;

		public string CurrentVersion { get { return "2"; } }

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

			if (string.IsNullOrEmpty(data.Version))
				data = UpdateToVersion1(data);

			if (data.Version == "1")
				data = UpdateToVersion2(data);

			if (data.Version == CurrentVersion)
				return data;

			throw new Exception(string.Format("Unknown version [{0}] specified.", data.Version));
		}

		private IFunctionData UpdateToVersion1(IFunctionData data)
		{
			data = ChangeExecutionPathHint(data);

			int resultSetCount = data.Properties[ExecuteStoredProcedureShared.ResultSetCountPropertyName].GetValue<int>();

			var executionPathOutputMap = Enumerable.Range(0, resultSetCount).ToDictionary(
				i => string.Format(ExecuteStoredProcedureShared.ResultSetPropertyNameFormat, i + 1),
				i => data.ExecutionPaths[string.Format(ExecuteStoredProcedureShared.ResultSetExecutionPathNameFormat, i + 1)].Output);

			data = FixProperties(data, executionPathOutputMap);

			data = AddTransactionProperty(data);
			data = TransformConnectionTypeProperty(data);

			return data.UpdateVersion("1");
		}

		private IFunctionData ChangeExecutionPathHint(IFunctionData data)
		{
			foreach (var executionPath in data.ExecutionPaths.Values.ToArray())
				if (executionPath.IterationHint != IterationHint.ZeroOrMore)
					data = data.ReplaceExecutionPath(executionPath, new ExecutionPath
					{
						Key = executionPath.Key,
						Name = executionPath.Name,
						Output = executionPath.Output,
						IterationHint = IterationHint.ZeroOrMore
					});

			return data;
		}

		private IFunctionData FixProperties(IFunctionData data, IDictionary<string, ITypeReference> executionPathOutputMap)
		{
			foreach (var property in data.Properties.Values.ToArray())
			{
				if (property.Name == DbShared.DesignTimeConnectionTypePropertyName)
					data = data.ReplaceProperty(property, FixDesignTimeConnectionTypeProperty(property));
				else if (property.Name == DbShared.ConnectionTypePropertyName)
					data = data.ReplaceProperty(property, FixConnectionTypeProperty(property));
				else if (property.Name == ExecuteStoredProcedureShared.ParametersPropertyName)
					data = data.ReplaceProperty(property, FixParametersProperty(property));
				else if (executionPathOutputMap.ContainsKey(property.Name))
					data = data.ReplaceProperty(property, FixResultSetProperty(property, executionPathOutputMap[property.Name]));
			}

			return data;
		}

		private IPropertyData FixDesignTimeConnectionTypeProperty(IPropertyData property)
		{
			if (!property.TypeReference.IsCompiled || property.TypeReference.GetUnderlyingType() != typeof(ConnectionType))
				property.TypeReference = TypeReference.Create(typeof(ConnectionType));
			if ((property.Value != null) && (!(property.Value is ConnectionType)))
				property.Value = Enum.Parse(typeof(ConnectionType), property.Value.ToString());

			return property;
		}

		private IPropertyData FixConnectionTypeProperty(IPropertyData property)
		{
			if (property.TypeReference.IsCompiled && property.TypeReference.GetUnderlyingType() == typeof(ConnectionTypeSelection))
				return property;

			return FixDesignTimeConnectionTypeProperty(property);
		}

		private IPropertyData FixParametersProperty(IPropertyData property)
		{
			property.TypeReference = TypeReference.Create(typeof(DatabaseModel.ProcedureParameters));
			property.Value = new DatabaseModel.ProcedureParameters(((IEnumerable<dynamic>)property.Value).Select(p =>
				new DatabaseModel.ProcedureParameter
				{
					Name = p.Name,
					Direction = Enum.Parse(typeof(DatabaseModel.ParameterDirection), p.Direction.ToString()),
					DataType = Enum.Parse(typeof(DatabaseModel.DataType), p.DataType.ToString()),
					Precision = p.Precision,
					Scale = p.Scale,
					Size = p.Size,
					IsNullable = p.IsNullable,
					Position = p.Position
				}));

			return property;
		}

		private IPropertyData FixResultSetProperty(IPropertyData property, ITypeReference executionPathOutput)
		{
			property.TypeReference = TypeReference.Create(typeof(DatabaseModel.ResultSet));

			var usesCustomType = ((dynamic)property.Value).CustomType != null;
			ITypeReference customType = usesCustomType ? executionPathOutput : null;

			property.Value = new DatabaseModel.ResultSet
			{
				Fields = new Collection<DatabaseModel.ResultSetField>(((IEnumerable<dynamic>)((dynamic)property.Value).Fields).Select(f =>
					new DatabaseModel.ResultSetField
					{
						ColumnName = f.ColumnName,
						DataType = Enum.Parse(typeof(DatabaseModel.DataType), f.DataType.ToString()),
						OutputName = f.OutputName
					}).ToList()),
				CustomType = customType
			};

			return property;
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
			IPropertyData parametersProperty;
			if (data.TryFindPropertyById(ExecuteStoredProcedureShared.ParametersPropertyName, out parametersProperty))
			{
				DatabaseModel.ProcedureParameters parameterValues = parametersProperty.Value as DatabaseModel.ProcedureParameters;
				if (parameterValues != null)
				{
					foreach (DatabaseModel.ProcedureParameter parameter in parameterValues)
					{
						IPropertyData parameterProperty;
						if (data.TryFindPropertyById(parameter.Name, out parameterProperty))
						{
							data = data.UpdateProperty(parameterProperty,
								parameter.DisplayPropertyId, parameter.DisplayPropertyName, parameterProperty.TypeReference, parameterProperty.Value, parameterProperty.IsVisible, parameterProperty.ValueUsage);
						}
					}
				}
			}

			return data.UpdateVersion("2");
		}
	}
}
