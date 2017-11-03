using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Twenty57.Linx.Components.Database.Common;
using Twenty57.Linx.Components.Database.ExecuteSQL.Editors.ResultTypeEditor;
using Twenty57.Linx.Components.Database.ExecuteSQL.Editors.SQLEditor;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.Types;
using Twenty57.Linx.Plugin.Common.Validation;
using Twenty57.Linx.Plugin.Common.Validation.Custom;

namespace Twenty57.Linx.Components.Database.ExecuteSQL
{
	public class ExecuteSQLDesigner : DbDesignerBase, ICustomValidation
	{
		public ExecuteSQLDesigner(IDesignerContext context)
			: base(context)
		{
			Version = FunctionUpdater.Instance.CurrentVersion;

			Properties.Add(new Property(ExecuteSQLShared.SqlStatementPropertyName, typeof(string), ValueUseOption.RuntimeRead, string.Empty));
			Properties.Add(new Property(ExecuteSQLShared.TimeoutPropertyName, typeof(int), ValueUseOption.DesignTime, 60));
			Properties.Add(new Property(ExecuteSQLShared.ReturnOptionsPropertyName, typeof(ExecuteSQLShared.ReturnModeType), ValueUseOption.DesignTime, ExecuteSQLShared.ReturnModeType.RowByRow));
			Properties.Add(new Property(ExecuteSQLShared.ResultTypePropertyName, typeof(ResultType), ValueUseOption.DesignTime, new ResultType()));

			SetPropertyAttributes();
			BuildExecutionPaths();
			BuildResultType();
		}

		public ExecuteSQLDesigner(IFunctionData functionData, IDesignerContext context)
			: base(functionData, context)
		{
			if (ResultTypeValue.CustomType != null)
				ResultTypeValue.CustomType = context.CustomTypes.FirstOrDefault(t => t.Id == ResultTypeValue.CustomType.Id);
		}

		public string ResolvedSqlStatementValue
		{
			get
			{
				object resolvedSqlValue;
				if (Context.TryParsePropertyValue(Properties[ExecuteSQLShared.SqlStatementPropertyName], out resolvedSqlValue))
					return resolvedSqlValue as string;
				return null;
			}
		}

		public ExecuteSQLShared.ReturnModeType ReturnOptionsValue
		{
			get { return Properties[ExecuteSQLShared.ReturnOptionsPropertyName].GetValue<ExecuteSQLShared.ReturnModeType>(); }
		}

		public ResultType ResultTypeValue
		{
			get { return Properties[ExecuteSQLShared.ResultTypePropertyName].GetValue<ResultType>(); }
			set { Properties[ExecuteSQLShared.ResultTypePropertyName].Value = value; }
		}

		protected override void InitializeProperties(IReadOnlyDictionary<string, IPropertyData> properties)
		{
			base.InitializeProperties(properties);
			StringExpressionHelpers.UpdateOnLoad(Properties[ExecuteSQLShared.SqlStatementPropertyName], properties, ExecuteSQLShared.SqlValuePropertyPrefix, false);
			SetPropertyAttributes();
		}

		protected override void InitializeExecutionPaths(IReadOnlyDictionary<string, IExecutionPathData> executionPaths)
		{
			base.InitializeExecutionPaths(executionPaths);
			BuildExecutionPaths();
		}

		protected override void InitializeOutput(ITypeReference output)
		{
			BuildDataOut();
		}

		public override IFunctionData GetFunctionData()
		{
			FunctionData functionData = new FunctionData { Version = FunctionUpdater.Instance.CurrentVersion };

			foreach (var nextProperty in Properties)
				if (!nextProperty.Name.StartsWith(ExecuteSQLShared.SqlValuePropertyPrefix))
					functionData.Properties.Add(nextProperty);

			if (!string.IsNullOrEmpty(ResolvedSqlStatementValue))
			{
				int i = 1;
				SqlStringHandler.GetSqlStringHandler(ResolvedSqlStatementValue).Expressions.ForEach(
					ex => functionData.Properties.Add(new Property(ExecuteSQLShared.SqlValuePropertyPrefix + (i++), typeof(object), ValueUseOption.RuntimeRead,
						Context.CreateExpression(ex.ExpressionText))
					{ IsVisible = false }));
			}

			functionData.ExecutionPaths.AddRange(ExecutionPaths);
			functionData.Output = Output;

			return functionData;
		}

		public void Validate(CustomValidationResults validations, ValidateDynamicValue validationMethod)
		{
			StringExpressionHelpers.Validate(Properties[ExecuteSQLShared.SqlStatementPropertyName], Context, validations, validationMethod);

			var resultType = ResultTypeValue;
			if (resultType.CustomType != null)
			{
				IEnumerable<string> missingFields = resultType.Fields.Select(f => f.Name).Where(s => resultType.CustomType.GetProperty(s) == null);
				if (missingFields.Any())
				{
					validations.AddValidationResult(
						ExecuteSQLShared.ResultTypePropertyName,
						string.Format("Custom type {0} has no field(s) named [{1}].", resultType.CustomType.Name, string.Join(", ", missingFields)),
						CustomValidationType.Error);
				}
			}
		}

		private void SetPropertyAttributes()
		{
			if (!UseTransaction)
			{
				Property connectionStringProperty = Properties[DbShared.ConnectionStringPropertyName];
				connectionStringProperty.ValueChanged += connectionStringProperty_ValueChanged;
			}

			Property sqlStatementProperty = Properties[ExecuteSQLShared.SqlStatementPropertyName];
			sqlStatementProperty.Description = "The SQL statement to execute";
			sqlStatementProperty.Order = 2;
			sqlStatementProperty.Validations.Add(new RequiredValidator());
			sqlStatementProperty.Editor = typeof(SQLEditorItem);
			sqlStatementProperty.ValueChanged += sqlStatementProperty_ValueChanged;

			Property timeoutProperty = Properties[ExecuteSQLShared.TimeoutPropertyName];
			timeoutProperty.Description = "Time (in seconds) to wait for the SQL to execute.";
			timeoutProperty.Order = 3;
			timeoutProperty.Validations.Add(new RangeValidator(1, Int32.MaxValue));

			Property resultTypeProperty = Properties[ExecuteSQLShared.ResultTypePropertyName];
			resultTypeProperty.Description = "Configure the output columns of the SQL statement.";
			resultTypeProperty.Order = 4;
			resultTypeProperty.Editor = typeof(ResultTypeEditorItem);
			resultTypeProperty.ValueChanged += resultTypeProperty_ValueChanged;

			Property returnOptionsProperty = Properties[ExecuteSQLShared.ReturnOptionsPropertyName];
			returnOptionsProperty.Description = "Select how the data is to be returned: only the first row, or all of the rows one by one, or all of the rows at once.";
			returnOptionsProperty.Order = 5;
			returnOptionsProperty.IsVisible = ResultTypeValue.Fields.Count != 0;
			returnOptionsProperty.ValueChanged += returnOptionsProperty_ValueChanged;
		}

		private void connectionStringProperty_ValueChanged(object sender, EventArgs e)
		{
			BuildResultType();
		}

		private void sqlStatementProperty_ValueChanged(object sender, EventArgs e)
		{
			BuildResultType();
		}

		private void returnOptionsProperty_ValueChanged(object sender, EventArgs e)
		{
			BuildDataOut();
			BuildExecutionPaths();
		}

		private void resultTypeProperty_ValueChanged(object sender, EventArgs e)
		{
			BuildDataOut();
			BuildExecutionPaths();
		}

		private void BuildExecutionPaths()
		{
			if ((ResultTypeValue.Fields.Count != 0) && (ReturnOptionsValue == ExecuteSQLShared.ReturnModeType.RowByRow))
			{
				if ((ExecutionPaths.Count == 0) || (!ExecutionPaths.Contains(ExecuteSQLShared.ExecutionPathName)))
				{
					ExecutionPaths.Clear();
					ExecutionPaths.Add(ExecuteSQLShared.ExecutionPathName, ExecuteSQLShared.ExecutionPathName, Output, IterationHint.ZeroOrMore);
				}
				else
					ExecutionPaths[ExecuteSQLShared.ExecutionPathName].Output = Output;
				Output = null;
			}
			else
				ExecutionPaths.Clear();
		}

		private void BuildDataOut()
		{
			var rowOutputType = ResultTypeValue.BuildRowTypeFromFields();
			if (rowOutputType == null)
				Output = null;
			else
				Output = ReturnOptionsValue == ExecuteSQLShared.ReturnModeType.ListOfRows ? TypeReference.CreateList(rowOutputType) : rowOutputType;
		}

		private void BuildResultType()
		{
			if (!string.IsNullOrEmpty(ResolvedConnectionString) && !string.IsNullOrEmpty(ResolvedSqlStatementValue))
			{
				string designTimeSql = SqlStringHandler.GetSqlStringHandler(ResolvedSqlStatementValue).GetExecutableDesignTimeSql();
				DataTable schema = null;
				try
				{
					schema = DatabaseHelpers.RetrieveSchema(ConnectionType, ResolvedConnectionString, designTimeSql);
				}
				catch { }
				if (schema != null)
				{
					var resultFields = ResultTypeValue.Fields;
					ResultTypeFields newResults = BuildResultType(schema, resultFields);
					resultFields.Clear();
					foreach (ResultTypeField nextField in newResults)
						resultFields.Add(nextField);
				}
			}
			BuildDataOut();
			BuildExecutionPaths();
		}

		public static ResultTypeFields BuildResultType(DataTable dataTable, ResultTypeFields reusableFields = null)
		{
			ResultTypeFields resultFields = new ResultTypeFields();
			foreach (DataColumn dataColumn in dataTable.Columns)
			{
				var oldModel = reusableFields == null ? null : reusableFields.FirstOrDefault(x => x.ColumnName == dataColumn.ColumnName);
				if (oldModel == null)
				{
					ResultTypeField newModel = new ResultTypeField();
					newModel.ColumnName = dataColumn.ColumnName;
					newModel.Name = DatabaseHelpers.GetValidName(dataColumn.ColumnName);
					newModel.Type = MapResultType(dataColumn.DataType);
					resultFields.Add(newModel);
				}
				else
					resultFields.Add(oldModel);
			}
			return resultFields;
		}

		private static Type MapResultType(Type type)
		{
			if (type == typeof(Int16) || type == typeof(Int32))
				return typeof(Int64);

			return type;
		}
	}
}
