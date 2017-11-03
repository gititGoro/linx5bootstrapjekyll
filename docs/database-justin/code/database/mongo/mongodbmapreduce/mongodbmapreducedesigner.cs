using System;
using Twenty57.Linx.Components.Database.Common;
using Twenty57.Linx.Components.Database.Mongo.Editors;
using Twenty57.Linx.Components.Database.Mongo.Editors.MongoJsonEditor.ViewModel;
using Twenty57.Linx.Components.MongoDB;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.Types;
using Twenty57.Linx.Plugin.Common.Validation;
using Twenty57.Linx.Plugin.Common.Validation.Custom;

namespace Twenty57.Linx.Components.Database.Mongo.MongoDBMapReduce
{
	public class MongoDBMapReduceDesigner : FunctionDesigner, MongoDBComponent, ICustomValidation
	{
		public MongoDBMapReduceDesigner(IDesignerContext context)
			: base(context)
		{
			Version = MongoDBReadFunctionUpdater.Instance.CurrentVersion;

			Properties.Add(new Property(MongoDBMapReduceShared.Names.ConnectionString, typeof(string), ValueUseOption.RuntimeRead, string.Empty));
			Properties.Add(new Property(MongoDBMapReduceShared.Names.Collection, typeof(string), ValueUseOption.RuntimeRead, string.Empty));
			Properties.Add(new Property(MongoDBMapReduceShared.Names.Query, typeof(string), ValueUseOption.RuntimeRead, string.Empty));
			Properties.Add(new Property(MongoDBMapReduceShared.Names.OutputType, typeof(ITypeReference), ValueUseOption.DesignTime, string.Empty));
			Properties.Add(new Property(MongoDBMapReduceShared.Names.Sort, typeof(string), ValueUseOption.RuntimeRead, string.Empty));
			Properties.Add(new Property(MongoDBMapReduceShared.Names.Limit, typeof(int), ValueUseOption.RuntimeRead, 0));
			Properties.Add(new Property(MongoDBMapReduceShared.Names.Map, typeof(string), ValueUseOption.RuntimeRead, string.Empty));
			Properties.Add(new Property(MongoDBMapReduceShared.Names.Reduce, typeof(string), ValueUseOption.RuntimeRead, string.Empty));
			Properties.Add(new Property(MongoDBMapReduceShared.Names.Finalize, typeof(string), ValueUseOption.RuntimeRead, string.Empty));
			Properties.Add(new Property(MongoDBMapReduceShared.Names.ReturnOptionsPropertyName, typeof(MongoDBMapReduceShared.ReturnModeType), ValueUseOption.DesignTime, MongoDBMapReduceShared.ReturnModeType.RowByRow));

			Initialise();
		}

		public MongoDBMapReduceDesigner(IFunctionData data, IDesignerContext context)
			: base(data, context)
		{
			Initialise();
		}

		public string ConnectionString
		{
			get { return Properties[MongoDBMapReduceShared.Names.ConnectionString].Value.ToString(); }
		}

		public string Collection
		{
			get { return Properties[MongoDBMapReduceShared.Names.Collection].Value.ToString(); }
		}

		public System.Collections.Generic.List<MongoDatabaseObject> GetDatabaseObjects()
		{
			return new MongoDBX(Properties[MongoDBMapReduceShared.Names.ConnectionString].Value.ToString())
			.GetDatabaseObjects(Properties[MongoDBMapReduceShared.Names.Collection].Value.ToString());
		}

		public MongoDBMapReduceShared.ReturnModeType ReturnOptionsValue
		{
			get { return Properties[MongoDBMapReduceShared.Names.ReturnOptionsPropertyName].GetValue<MongoDBMapReduceShared.ReturnModeType>(); }
		}

		public void Validate(CustomValidationResults validations, ValidateDynamicValue validationMethod)
		{
			StringExpressionHelpers.Validate(Properties[MongoDBMapReduceShared.Names.Query], Context, validations, validationMethod);
			StringExpressionHelpers.Validate(Properties[MongoDBMapReduceShared.Names.Map], Context, validations, validationMethod);
			StringExpressionHelpers.Validate(Properties[MongoDBMapReduceShared.Names.Reduce], Context, validations, validationMethod);
			StringExpressionHelpers.Validate(Properties[MongoDBMapReduceShared.Names.Finalize], Context, validations, validationMethod);
		}

		protected override void InitializeProperties(System.Collections.Generic.IReadOnlyDictionary<string, IPropertyData> properties)
		{
			base.InitializeProperties(properties);

			StringExpressionHelpers.UpdateOnLoad(Properties[MongoDBMapReduceShared.Names.Query], properties, MongoDBMapReduceShared.Names.QueryExpressions, true);
			StringExpressionHelpers.UpdateOnLoad(Properties[MongoDBMapReduceShared.Names.Map], properties, MongoDBMapReduceShared.Names.MapExpressions, true);
			StringExpressionHelpers.UpdateOnLoad(Properties[MongoDBMapReduceShared.Names.Reduce], properties, MongoDBMapReduceShared.Names.ReduceExpressions, true);
			StringExpressionHelpers.UpdateOnLoad(Properties[MongoDBMapReduceShared.Names.Finalize], properties, MongoDBMapReduceShared.Names.FinalizeExpressions, true);
		}

		private void BuildOutput()
		{
			var outputType = Properties[MongoDBMapReduceShared.Names.OutputType].GetValue<ITypeReference>();

			if (outputType == null)
				Output = null;
			else
				Output = (ReturnOptionsValue == MongoDBMapReduceShared.ReturnModeType.ListOfRows) ? TypeReference.CreateList(outputType) : outputType;

			if ((ReturnOptionsValue == MongoDBMapReduceShared.ReturnModeType.RowByRow))
			{
				if ((ExecutionPaths.Count == 0) || (!ExecutionPaths.Contains(MongoDBMapReduceShared.Names.ExecutionPath)))
				{
					ExecutionPaths.Clear();
					ExecutionPaths.Add(MongoDBMapReduceShared.Names.ExecutionPath, MongoDBMapReduceShared.Names.ExecutionPath, Output, IterationHint.ZeroOrMore);
				}
				else
					ExecutionPaths[MongoDBMapReduceShared.Names.ExecutionPath].Output = Output;
				Output = null;
			}
			else
				ExecutionPaths.Clear();
		}

		private void Initialise()
		{
			SetPropertyAttributes();
			RefreshExpressionParameters(MongoDBMapReduceShared.Names.QueryExpressions, MongoDBMapReduceShared.Names.Query);
			RefreshExpressionParameters(MongoDBMapReduceShared.Names.MapExpressions, MongoDBMapReduceShared.Names.Map);
			RefreshExpressionParameters(MongoDBMapReduceShared.Names.ReduceExpressions, MongoDBMapReduceShared.Names.Reduce);
			RefreshExpressionParameters(MongoDBMapReduceShared.Names.FinalizeExpressions, MongoDBMapReduceShared.Names.Finalize);

			BuildOutput();
		}

		private void SetPropertyAttributes()
		{
			Property connectionStringProperty = Properties[MongoDBMapReduceShared.Names.ConnectionString];
			connectionStringProperty.Description = "Configuration values required by the data provider to connect to the data source.";
			connectionStringProperty.Category = "Database";
			connectionStringProperty.Order = 0;
			connectionStringProperty.Validations.Add(new RequiredValidator());

			Property collection = Properties[MongoDBMapReduceShared.Names.Collection];
			collection.Description = "The collection to read from.";
			collection.Category = "Database";
			collection.Order = 1;
			collection.Validations.Add(new RequiredValidator());

			Property query = Properties[MongoDBMapReduceShared.Names.Query];
			query.Editor = typeof(MongoCriteriaEditor);
			query.Description = "JSON formatted object specifying the selection criteria using query operators for determining the documents input to the map function.";
			query.Category = "Operation";
			query.Order = 2;
			query.ValueChanged +=
							(property, e) => RefreshExpressionParameters(MongoDBMapReduceShared.Names.QueryExpressions, MongoDBMapReduceShared.Names.Query);

			Property sort = Properties[MongoDBMapReduceShared.Names.Sort];
			sort.Description = "The condition to sort the input documents.";
			sort.Category = "Operation";
			sort.Order = 3;

			Property limit = Properties[MongoDBMapReduceShared.Names.Limit];
			limit.Description = "The maximum number of documents for the input into the map function.";
			limit.Category = "Operation";
			limit.Order = 4;

			Property map = Properties[MongoDBMapReduceShared.Names.Map];
			map.Editor = typeof(MongoCriteriaEditor);
			map.Validations.Add(new RequiredValidator());
			map.Description = "A JavaScript function that associates or “maps” a value with a key and emits the key and value pair.";
			map.Category = "Operation";
			map.Order = 5;
			map.ValueChanged += (property, e) => RefreshExpressionParameters(MongoDBMapReduceShared.Names.MapExpressions, MongoDBMapReduceShared.Names.Map);

			Property reduce = Properties[MongoDBMapReduceShared.Names.Reduce];
			reduce.Editor = typeof(MongoCriteriaEditor);
			reduce.Validations.Add(new RequiredValidator());
			reduce.Description = "A JavaScript function that “reduces” to a single object all the values associated with a particular key.";
			reduce.Category = "Operation";
			reduce.Order = 6;
			reduce.ValueChanged += (property, e) => RefreshExpressionParameters(MongoDBMapReduceShared.Names.ReduceExpressions, MongoDBMapReduceShared.Names.Reduce);

			Property finalize = Properties[MongoDBMapReduceShared.Names.Finalize];
			finalize.Editor = typeof(MongoCriteriaEditor);
			finalize.Description = "A JavaScript function that follows the reduce method and modifies the output.";
			finalize.Category = "Operation";
			finalize.Order = 7;
			finalize.ValueChanged += (property, e) => RefreshExpressionParameters(MongoDBMapReduceShared.Names.FinalizeExpressions, MongoDBMapReduceShared.Names.Finalize);

			Property outputType = Properties[MongoDBMapReduceShared.Names.OutputType];
			outputType.Description = "The output type";
			outputType.Category = "Output";
			outputType.Order = 8;
			outputType.Validations.Add(new RequiredValidator());
			outputType.ValueChanged += (sender, e) => BuildOutput();

			Property returnOptionsProperty = Properties[MongoDBMapReduceShared.Names.ReturnOptionsPropertyName];
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
			if (!string.IsNullOrEmpty(expressionText))
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