using System;
using Twenty57.Linx.Components.Database.Common;
using Twenty57.Linx.Components.Database.Mongo.Editors;
using Twenty57.Linx.Components.Database.Mongo.Editors.MongoJsonEditor.ViewModel;
using Twenty57.Linx.Components.MongoDB;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.Types;
using Twenty57.Linx.Plugin.Common.Validation;
using Twenty57.Linx.Plugin.Common.Validation.Custom;

namespace Twenty57.Linx.Components.Database.Mongo.MongoDBRead
{
	public class MongoDBReadDesigner : FunctionDesigner, MongoDBComponent, ICustomValidation
	{
		public MongoDBReadDesigner(IDesignerContext context)
			: base(context)
		{
			Version = MongoDBReadFunctionUpdater.Instance.CurrentVersion;

			Properties.Add(new Property(MongoDBReadShared.Names.ConnectionString, typeof(string), ValueUseOption.RuntimeRead, string.Empty));
			Properties.Add(new Property(MongoDBReadShared.Names.Collection, typeof(string), ValueUseOption.RuntimeRead, string.Empty));
			Properties.Add(new Property(MongoDBReadShared.Names.Query, typeof(string), ValueUseOption.RuntimeRead, string.Empty));
			Properties.Add(new Property(MongoDBReadShared.Names.AggregationPipeline, typeof(string), ValueUseOption.RuntimeRead, string.Empty));
			Properties.Add(new Property(MongoDBReadShared.Names.OutputType, typeof(ITypeReference), ValueUseOption.DesignTime, string.Empty));
			Properties.Add(new Property(MongoDBReadShared.Names.Operation, typeof(MongoDBReadShared.OperationType), ValueUseOption.DesignTime, MongoDBReadShared.OperationType.Find));
			Properties.Add(new Property(MongoDBReadShared.Names.Fields, typeof(string), ValueUseOption.RuntimeRead, string.Empty));
			Properties.Add(new Property(MongoDBReadShared.Names.Sort, typeof(string), ValueUseOption.RuntimeRead, string.Empty));
			Properties.Add(new Property(MongoDBReadShared.Names.Skip, typeof(int), ValueUseOption.RuntimeRead, 0));
			Properties.Add(new Property(MongoDBReadShared.Names.Limit, typeof(int), ValueUseOption.RuntimeRead, 0));
			Properties.Add(new Property(MongoDBReadShared.Names.ReturnOptionsPropertyName, typeof(MongoDBReadShared.ReturnModeType), ValueUseOption.DesignTime, MongoDBReadShared.ReturnModeType.RowByRow));

			Initialise();
		}

		public MongoDBReadDesigner(IFunctionData data, IDesignerContext context)
			: base(data, context)
		{
			Initialise();
		}

		public string ConnectionString
		{
			get { return Properties[MongoDBReadShared.Names.ConnectionString].Value.ToString(); }
		}

		public string Collection
		{
			get { return Properties[MongoDBReadShared.Names.Collection].Value.ToString(); }
		}

		public System.Collections.Generic.List<MongoDatabaseObject> GetDatabaseObjects()
		{
			return new MongoDBX(Properties[MongoDBReadShared.Names.ConnectionString].Value.ToString())
			.GetDatabaseObjects(Properties[MongoDBReadShared.Names.Collection].Value.ToString());
		}

		public MongoDBReadShared.ReturnModeType ReturnOptionsValue
		{
			get { return Properties[MongoDBReadShared.Names.ReturnOptionsPropertyName].GetValue<MongoDBReadShared.ReturnModeType>(); }
		}

		public void Validate(CustomValidationResults validations, ValidateDynamicValue validationMethod)
		{
			StringExpressionHelpers.Validate(Properties[MongoDBReadShared.Names.AggregationPipeline], Context, validations, validationMethod);
			StringExpressionHelpers.Validate(Properties[MongoDBReadShared.Names.Query], Context, validations, validationMethod);
			StringExpressionHelpers.Validate(Properties[MongoDBReadShared.Names.Fields], Context, validations, validationMethod);
			StringExpressionHelpers.Validate(Properties[MongoDBReadShared.Names.Sort], Context, validations, validationMethod);
			StringExpressionHelpers.Validate(Properties[MongoDBReadShared.Names.Skip], Context, validations, validationMethod);
			StringExpressionHelpers.Validate(Properties[MongoDBReadShared.Names.Limit], Context, validations, validationMethod);
		}

		protected override void InitializeProperties(System.Collections.Generic.IReadOnlyDictionary<string, IPropertyData> properties)
		{
			base.InitializeProperties(properties);
			StringExpressionHelpers.UpdateOnLoad(Properties[MongoDBReadShared.Names.Query], properties, MongoDBReadShared.Names.QueryExpressions, true);
			StringExpressionHelpers.UpdateOnLoad(Properties[MongoDBReadShared.Names.AggregationPipeline], properties, MongoDBReadShared.Names.AggregationPipelineExpressions, true);
			StringExpressionHelpers.UpdateOnLoad(Properties[MongoDBReadShared.Names.Fields], properties, MongoDBReadShared.Names.FieldsExpressions, true);
		}

		private void BuildOutput()
		{
			var outputType = Properties[MongoDBReadShared.Names.OutputType].GetValue<ITypeReference>();

			if (outputType == null)
				Output = null;
			else
				Output = (ReturnOptionsValue == MongoDBReadShared.ReturnModeType.ListOfRows) ? TypeReference.CreateList(outputType) : outputType;

			if ((ReturnOptionsValue == MongoDBReadShared.ReturnModeType.RowByRow))
			{
				if ((ExecutionPaths.Count == 0) || (!ExecutionPaths.Contains(MongoDBReadShared.Names.ExecutionPath)))
				{
					ExecutionPaths.Clear();
					ExecutionPaths.Add(MongoDBReadShared.Names.ExecutionPath, MongoDBReadShared.Names.ExecutionPath, Output, IterationHint.ZeroOrMore);
				}
				else
					ExecutionPaths[MongoDBReadShared.Names.ExecutionPath].Output = Output;
				Output = null;
			}
			else
				ExecutionPaths.Clear();
		}

		private void Initialise()
		{
			SetPropertyAttributes();
			RefreshExpressionParameters(MongoDBReadShared.Names.QueryExpressions, MongoDBReadShared.Names.Query);
			RefreshExpressionParameters(MongoDBReadShared.Names.AggregationPipelineExpressions, MongoDBReadShared.Names.AggregationPipeline);
			RefreshExpressionParameters(MongoDBReadShared.Names.FieldsExpressions, MongoDBReadShared.Names.Fields);
			BuildOutput();
		}

		private void SetPropertyAttributes()
		{
			bool isFindOperation = Properties[MongoDBReadShared.Names.Operation].GetValue<MongoDBReadShared.OperationType>() == MongoDBReadShared.OperationType.Find;

			Property connectionStringProperty = Properties[MongoDBReadShared.Names.ConnectionString];
			connectionStringProperty.Description = "Configuration values required by the data provider to connect to the data source.";
			connectionStringProperty.Category = "Database";
			connectionStringProperty.Order = 0;
			connectionStringProperty.Validations.Add(new RequiredValidator());

			Property collection = Properties[MongoDBReadShared.Names.Collection];
			collection.Description = "The collection to read from.";
			collection.Category = "Database";
			collection.Order = 1;
			collection.Validations.Add(new RequiredValidator());

			Property aggregationPipeline = Properties[MongoDBReadShared.Names.AggregationPipeline];
			aggregationPipeline.Editor = typeof(MongoAggregationEditor);
			aggregationPipeline.Description = "JSON formatted array of aggregation operations.";
			aggregationPipeline.Category = "Operation";
			aggregationPipeline.Order = 3;
			aggregationPipeline.IsVisible = !isFindOperation;
			aggregationPipeline.ValueChanged += (property, e) => RefreshExpressionParameters(MongoDBReadShared.Names.AggregationPipelineExpressions, MongoDBReadShared.Names.AggregationPipeline);
			aggregationPipeline.Validations.Add(new RequiredValidator());

			Property operation = Properties[MongoDBReadShared.Names.Operation];
			operation.Category = "Operation";
			operation.Order = 2;
			operation.ValueChanged += (sender, e) => BuildOutput();

			Property query = Properties[MongoDBReadShared.Names.Query];
			query.Editor = typeof(MongoCriteriaEditor);
			query.Description = "JSON formatted object specifying the selection criteria using query operators for determining the documents input to the read function.";
			query.Category = "Operation";
			query.Order = 3;
			query.IsVisible = isFindOperation;
			query.ValueChanged += (property, e) => RefreshExpressionParameters(MongoDBReadShared.Names.QueryExpressions, MongoDBReadShared.Names.Query);

			Property fields = Properties[MongoDBReadShared.Names.Fields];
			fields.Description = "The condition to determine which fields to be returned in the matching documents.";
			fields.Category = "Operation";
			fields.Order = 4;
			fields.IsVisible = isFindOperation;
			fields.ValueChanged += (property, e) => RefreshExpressionParameters(MongoDBReadShared.Names.FieldsExpressions, MongoDBReadShared.Names.Fields);

			Property sort = Properties[MongoDBReadShared.Names.Sort];
			sort.Description = "The condition to sort the input documents.";
			sort.Category = "Operation";
			sort.Order = 5;
			sort.IsVisible = isFindOperation;


			Property skip = Properties[MongoDBReadShared.Names.Skip];
			skip.Description = "The condition to skip the input documents.";
			skip.Category = "Operation";
			skip.Order = 6;
			skip.IsVisible = isFindOperation;

			Property limit = Properties[MongoDBReadShared.Names.Limit];
			limit.Description = "The condition to limit the number of documents in the result set.";
			limit.Category = "Operation";
			limit.Order = 7;
			limit.IsVisible = isFindOperation;

			Property outputType = Properties[MongoDBReadShared.Names.OutputType];
			outputType.Description = "The output type";
			outputType.Category = "Output";
			outputType.Order = 8;
			outputType.Validations.Add(new RequiredValidator());
			outputType.ValueChanged += (sender, e) => BuildOutput();

			Property returnOptionsProperty = Properties[MongoDBReadShared.Names.ReturnOptionsPropertyName];
			returnOptionsProperty.Description = "Select how the data is to be returned: only the first row, or all of the rows one by one, or all of the rows at once.";
			returnOptionsProperty.Order = 9;
			returnOptionsProperty.Category = "Output";
			returnOptionsProperty.ValueChanged += (sender, e) => BuildOutput();
		}

		private void RefreshExpressionParameters(string expressionName, string propertyName)
		{
			for (int i = 0; Properties.ContainsId(expressionName + i); i++)
				Properties.Remove(expressionName + i);

			if (!Properties.Contains(propertyName))
				return;

			var property = Properties[propertyName];
			var expressionText = property.Value as string ?? string.Empty;
			if (!String.IsNullOrEmpty(expressionText))
			{
				var index = 0;
				SqlStringHandler.GetSqlStringHandler(expressionText).Expressions.ForEach(
					expression =>
						Properties.Add(new Property(expressionName + (index++), typeof(object), ValueUseOption.RuntimeRead, Context.CreateExpression(expression.ExpressionText))
						{
							IsVisible = false
						}));
			}
		}
	}
}