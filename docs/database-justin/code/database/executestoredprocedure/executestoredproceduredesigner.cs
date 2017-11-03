using System;
using System.Collections.Generic;
using System.Linq;
using Twenty57.Linx.Components.Database.Common;
using Twenty57.Linx.Components.Database.ExecuteStoredProcedure.Editors.ProcedureParametersEditor;
using Twenty57.Linx.Components.Database.ExecuteStoredProcedure.Editors.RefreshResultSets;
using Twenty57.Linx.Components.Database.ExecuteStoredProcedure.Editors.ResultSetEditor;
using Twenty57.Linx.Components.Database.ExecuteStoredProcedure.Editors.StoredProcedureEditor;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.Types;
using Twenty57.Linx.Plugin.Common.Validation;
using Twenty57.Linx.Plugin.Common.Validation.Custom;

namespace Twenty57.Linx.Components.Database.ExecuteStoredProcedure
{
	public class ExecuteStoredProcedureDesigner : DbDesignerBase, ICustomValidation
	{
		public ExecuteStoredProcedureDesigner(IDesignerContext context)
			: base(context)
		{
			Version = FunctionUpdater.Instance.CurrentVersion;

			Properties.Add(new Property(ExecuteStoredProcedureShared.StoredProcedurePropertyName, typeof(string), ValueUseOption.RuntimeRead, string.Empty));
			Properties.Add(new Property(ExecuteStoredProcedureShared.DesignTimeStoredProcedurePropertyName, typeof(string), ValueUseOption.DesignTime, string.Empty) { IsVisible = false });

			Properties.Add(new Property(ExecuteStoredProcedureShared.ParametersPropertyName, typeof(DatabaseModel.ProcedureParameters), ValueUseOption.DesignTime, new DatabaseModel.ProcedureParameters()));
			Properties.Add(new Property(ExecuteStoredProcedureShared.ResultSetCountPropertyName, typeof(int), ValueUseOption.DesignTime, 0));
			Properties.Add(new Property(ExecuteStoredProcedureShared.OutputOptionPropertyName, typeof(OutputOption), ValueUseOption.DesignTime, OutputOption.RowByRow));

			SetPropertyAttributes();
		}

		public ExecuteStoredProcedureDesigner(IFunctionData data, IDesignerContext context) : base(data, context)
		{
			foreach (var nextResultSet in ResultSets.Where(r => r.CustomType != null))
				nextResultSet.CustomType = context.CustomTypes.FirstOrDefault(t => t.Id == nextResultSet.CustomType.Id);
		}

		public string StoredProcedure
		{
			get { return Properties[ExecuteStoredProcedureShared.StoredProcedurePropertyName].GetValue<string>(); }
			set { Properties[ExecuteStoredProcedureShared.StoredProcedurePropertyName].Value = value; }
		}

		public string ResolvedStoredProcedure
		{
			get
			{
				object resolvedStoredProcedure;
				if (Context.TryParsePropertyValue(Properties[ExecuteStoredProcedureShared.StoredProcedurePropertyName], out resolvedStoredProcedure))
					return resolvedStoredProcedure as string;
				return null;
			}
		}

		public string DesignTimeStoredProcedure
		{
			get
			{
				string storedProcedure = Properties[ExecuteStoredProcedureShared.DesignTimeStoredProcedurePropertyName].GetValue<string>();
				return string.IsNullOrEmpty(storedProcedure) ? (ResolvedStoredProcedure ?? string.Empty) : storedProcedure;
			}
			set { Properties[ExecuteStoredProcedureShared.DesignTimeStoredProcedurePropertyName].Value = value; }
		}

		public DatabaseModel.ProcedureParameters Parameters
		{
			get { return Properties[ExecuteStoredProcedureShared.ParametersPropertyName].GetValue<DatabaseModel.ProcedureParameters>(); }
			set { Properties[ExecuteStoredProcedureShared.ParametersPropertyName].Value = value; }
		}

		public int ResultSetCount
		{
			get { return Math.Max(Properties[ExecuteStoredProcedureShared.ResultSetCountPropertyName].GetValue<int>(), 0); }
			set { Properties[ExecuteStoredProcedureShared.ResultSetCountPropertyName].Value = value; }
		}

		public DatabaseModel.ResultSets ResultSets
		{
			get
			{
				return new DatabaseModel.ResultSets(Enumerable.Range(0, ResultSetCount).Select(i => GetResultSetProperty(i).GetValue<DatabaseModel.ResultSet>()));
			}
			set
			{
				ResultSetCount = value.Count();
				int i = 0;
				foreach (var nextResultSet in value)
					GetResultSetProperty(i++).Value = nextResultSet;
			}
		}

		public OutputOption OutputOption
		{
			get { return Properties[ExecuteStoredProcedureShared.OutputOptionPropertyName].GetValue<OutputOption>(); }
			set { Properties[ExecuteStoredProcedureShared.OutputOptionPropertyName].Value = value; }
		}

		private IndexAccessResultSetCollection ResultSetCollection
		{
			get { return new IndexAccessResultSetCollection(this); }
		}


		private class IndexAccessResultSetCollection
		{
			private ExecuteStoredProcedureDesigner designer;

			internal IndexAccessResultSetCollection(ExecuteStoredProcedureDesigner designer)
			{
				this.designer = designer;
			}

			public DatabaseModel.ResultSet this[int index]
			{
				get { return designer.GetResultSetProperty(index).GetValue<DatabaseModel.ResultSet>(); }
				set { designer.GetResultSetProperty(index).Value = value; }
			}
		}


		protected override void InitializeProperties(IReadOnlyDictionary<string, IPropertyData> properties)
		{
			base.InitializeProperties(properties);

			SetPropertyAttributes();
		}

		protected override void InitializeOutput(ITypeReference output)
		{
			base.InitializeOutput(output);

			BuildOutput();
		}

		protected override void InitializeExecutionPaths(IReadOnlyDictionary<string, IExecutionPathData> executionPaths)
		{
			base.InitializeExecutionPaths(executionPaths);

			RefreshExecutionPaths();
		}

		public void Validate(CustomValidationResults validations, ValidateDynamicValue validationMethod)
		{
			for (int i = 0; i < ResultSetCount; i++)
			{
				var nextResultSet = ResultSetCollection[i];
				if (nextResultSet.CustomType != null)
				{
					foreach (var nextField in nextResultSet.Fields)
						if (nextResultSet.CustomType.GetProperty(nextField.OutputName) == null)
						{
							validations.AddValidationResult(
								GetResultSetPropertyName(i),
								string.Format("Custom type {0} has no field named {1}.", nextResultSet.CustomType.Name, nextField.OutputName),
								CustomValidationType.Error);
						}
				}
			}
		}


		private Property GetResultSetProperty(int index)
		{
			return Properties[GetResultSetPropertyName(index)];
		}

		private string GetResultSetPropertyName(int index)
		{
			return string.Format(ExecuteStoredProcedureShared.ResultSetPropertyNameFormat, index + 1);
		}

		private void SetPropertyAttributes()
		{
			Property storedProcedureProperty = Properties[ExecuteStoredProcedureShared.StoredProcedurePropertyName];
			storedProcedureProperty.Description = "The name of the stored procedure to call.";
			storedProcedureProperty.Order = 10;
			storedProcedureProperty.Validations.Add(new RequiredValidator());
			storedProcedureProperty.Editor = typeof(StoredProcedureEditorItem);
			storedProcedureProperty.ValueChanged += storedProcedureProperty_ValueChanged;

			Property parametersProperty = Properties[ExecuteStoredProcedureShared.ParametersPropertyName];
			parametersProperty.Description = "Configure the collection of parameters that the stored procedure expects.";
			parametersProperty.Order = 11;
			parametersProperty.Editor = typeof(ProcedureParametersEditorItem);
			parametersProperty.ValueChanged += parametersProperty_ValueChanged;

			foreach (DatabaseModel.ProcedureParameter nextParameter in Parameters)
			{
				if (Properties.ContainsId(nextParameter.DisplayPropertyId))
				{
					Property property = Properties[nextParameter.DisplayPropertyId];
					property.Description = string.Format("The value to pass for the {0} parameter.", nextParameter.Name);
					property.Category = ExecuteStoredProcedureShared.ParametersCategoryName;
					property.Order = nextParameter.Position;
				}
			}

			Property resultSetCountProperty = Properties[ExecuteStoredProcedureShared.ResultSetCountPropertyName];
			resultSetCountProperty.Description = "The number of result sets returned by the stored procedure.";
			resultSetCountProperty.Category = ExecuteStoredProcedureShared.ResultSetsCategoryName;
			resultSetCountProperty.Order = 0;
			resultSetCountProperty.Validations.Add(new RangeValidator(0, Int32.MaxValue));
			resultSetCountProperty.Editor = typeof(RefreshResultSetsItem);
			resultSetCountProperty.ValueChanged += resultSetCountProperty_ValueChanged;

			for (int i = 0; i < ResultSetCount; i++)
			{
				Property resultSetProperty = GetResultSetProperty(i);
				resultSetProperty.Description = string.Format("The fields for result set {0}.", i + 1);
				resultSetProperty.Category = ExecuteStoredProcedureShared.ResultSetsCategoryName;
				resultSetProperty.Order = i + 1;
				resultSetProperty.Editor = typeof(ResultSetEditorItem);
				resultSetProperty.ValueChanged += resultSetProperty_ValueChanged;
			}

			Property outputOptionProperty = Properties[ExecuteStoredProcedureShared.OutputOptionPropertyName];
			outputOptionProperty.Description = "Select how the data of each result set is to be returned: only the first row, or all of the rows one by one, or all of the rows at once.";
			outputOptionProperty.Category = ExecuteStoredProcedureShared.ResultSetsCategoryName;
			outputOptionProperty.Order = ResultSetCount + 1;
			outputOptionProperty.IsVisible = ResultSetCount > 0;
			outputOptionProperty.ValueChanged += outputOptionProperty_ValueChanged;
		}

		private void storedProcedureProperty_ValueChanged(object sender, EventArgs e)
		{
			if (StoredProcedure != null)
				DesignTimeStoredProcedure = StoredProcedure;
			else if (ResolvedStoredProcedure != null)
				DesignTimeStoredProcedure = string.Empty;
		}

		private void parametersProperty_ValueChanged(object sender, EventArgs e)
		{
			var inParameters = Parameters.Where(p => (p.Direction == DatabaseModel.ParameterDirection.In) || (p.Direction == DatabaseModel.ParameterDirection.InOut));

			for (int i = Properties.Count - 1; i >= 0; i--)
				if ((Properties[i].Category == ExecuteStoredProcedureShared.ParametersCategoryName) &&
						(!inParameters.Any(p => p.DisplayPropertyId == Properties[i].Id)))
					Properties.RemoveAt(i);

			foreach (DatabaseModel.ProcedureParameter nextParameter in inParameters)
			{
				Type type = nextParameter.DataType.GetSystemType();
				if (Properties.ContainsId(nextParameter.DisplayPropertyId))
					Properties[nextParameter.DisplayPropertyId].TypeReference = type.MapType();
				else
					Properties.Add(new Property(nextParameter.DisplayPropertyId, nextParameter.DisplayPropertyName, type.MapType(), ValueUseOption.RuntimeRead, type == typeof(DateTime) ? DatabaseModel.DefaultDateTime : type.GetDefaultValue()));
			}

			BuildOutput();
		}

		private void resultSetCountProperty_ValueChanged(object sender, EventArgs e)
		{
			for (int i = 0; i < ResultSetCount; i++)
			{
				string resultSetPropertyName = string.Format(ExecuteStoredProcedureShared.ResultSetPropertyNameFormat, i + 1);
				if (!Properties.Contains(resultSetPropertyName))
				{
					Property resultSetProperty = new Property(resultSetPropertyName, typeof(DatabaseModel.ResultSet), ValueUseOption.DesignTime, new DatabaseModel.ResultSet());
					Properties.Add(resultSetProperty);
					resultSetProperty.ValueChanged += resultSetProperty_ValueChanged;
				}
			}

			for (int i = ResultSetCount; ; i++)
			{
				string resultSetPropertyName = string.Format(ExecuteStoredProcedureShared.ResultSetPropertyNameFormat, i + 1);
				if (Properties.Contains(resultSetPropertyName))
				{
					Properties[resultSetPropertyName].ValueChanged -= resultSetProperty_ValueChanged;
					Properties.Remove(resultSetPropertyName);
				}
				else
					break;
			}

			if (OutputOption == OutputOption.RowByRow)
				RefreshExecutionPaths();
			else
				BuildOutput();
		}

		private void resultSetProperty_ValueChanged(object sender, EventArgs e)
		{
			if (OutputOption == OutputOption.RowByRow)
				RefreshExecutionPaths();
			else
				BuildOutput();
		}

		private void outputOptionProperty_ValueChanged(object sender, EventArgs e)
		{
			RefreshExecutionPaths();
			BuildOutput();
		}

		private void BuildOutput()
		{
			var outParameters = Parameters.Where(p => (p.Direction != DatabaseModel.ParameterDirection.In) && (p.DataType != DatabaseModel.DataType.RefCursor));
			if ((outParameters.Any()) || ((OutputOption != OutputOption.RowByRow) && (ResultSetCount > 0)))
			{
				TypeBuilder typeBuilder = new TypeBuilder();

				if (outParameters.Any())
				{
					TypeBuilder outParametersTypeBuilder = new TypeBuilder();
					foreach (var nextParameter in outParameters)
						outParametersTypeBuilder.AddProperty(nextParameter.OutputPropertyName, nextParameter.DataType.GetSystemType().MapType());
					typeBuilder.AddProperty(ExecuteStoredProcedureShared.OutParametersOutputPropertyName, outParametersTypeBuilder);
				}

				if (OutputOption != OutputOption.RowByRow)
				{
					for (int i = 0; i < ResultSetCount; i++)
					{
						ITypeReference rowTypeReference = BuildResultSetTypeReference(ResultSetCollection[i]);
						string resultSetOutputPropertyName = string.Format(OutputOption == OutputOption.ListOfRows ? ExecuteStoredProcedureShared.ResultSetRowsOutputPropertyNameFormat : ExecuteStoredProcedureShared.ResultSetRowOutputPropertyNameFormat,
							ResultSetCount == 1 ? string.Empty : (i + 1).ToString());
						typeBuilder.AddProperty(resultSetOutputPropertyName, OutputOption == OutputOption.ListOfRows ? TypeReference.CreateList(rowTypeReference) : rowTypeReference);
					}
				}

				Output = typeBuilder.CreateTypeReference();
			}
			else
				Output = null;
		}

		private void RefreshExecutionPaths()
		{
			if (OutputOption == OutputOption.RowByRow)
			{
				for (int i = 0; i < ResultSetCount; i++)
				{
					ITypeReference outputType = BuildResultSetTypeReference(ResultSetCollection[i]);

					string executionPathKey = string.Format(ExecuteStoredProcedureShared.ResultSetExecutionPathNameFormat, i + 1);
					string executionPathName = ResultSetCount == 1 ? string.Format(ExecuteStoredProcedureShared.ResultSetExecutionPathNameFormat, string.Empty) : executionPathKey;
					if (ExecutionPaths.Contains(executionPathKey))
					{
						ExecutionPath executionPath = ExecutionPaths[executionPathKey];
						executionPath.Name = executionPathName;
						executionPath.Output = outputType;
						executionPath.IterationHint = IterationHint.ZeroOrMore;
					}
					else
						ExecutionPaths.Add(executionPathKey, executionPathName, outputType, IterationHint.ZeroOrMore);
				}

				for (int i = ResultSetCount; ; i++)
				{
					string executionPathKey = string.Format(ExecuteStoredProcedureShared.ResultSetExecutionPathNameFormat, i + 1);
					if (ExecutionPaths.Contains(executionPathKey))
						ExecutionPaths.Remove(executionPathKey);
					else
						break;
				}
			}
			else
				ExecutionPaths.Clear();
		}

		private static ITypeReference BuildResultSetTypeReference(DatabaseModel.ResultSet resultSet)
		{
			if (resultSet.CustomType != null)
				return resultSet.CustomType;

			TypeBuilder typeBuilder = new TypeBuilder();
			foreach (var nextField in resultSet.Fields)
				typeBuilder.AddProperty(nextField.OutputName, nextField.DataType.GetSystemType().MapType());
			return typeBuilder.CreateTypeReference();
		}
	}
}
