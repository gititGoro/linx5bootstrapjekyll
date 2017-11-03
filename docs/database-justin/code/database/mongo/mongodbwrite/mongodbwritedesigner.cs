using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Twenty57.Linx.Components.Database.Common;
using Twenty57.Linx.Components.Database.ExecuteSQL.Editors.SQLEditor;
using Twenty57.Linx.Components.Database.Mongo.Editors;
using Twenty57.Linx.Components.Database.Mongo.Editors.MongoJsonEditor.ViewModel;
using Twenty57.Linx.Components.MongoDB;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.Validation;
using Twenty57.Linx.Plugin.Common.Validation.Custom;

namespace Twenty57.Linx.Components.Database.Mongo.MongoDBWrite
{
	public class MongoDBWriteDesigner : FunctionDesigner, MongoDBComponent, ICustomValidation
	{
		public MongoDBWriteDesigner(IDesignerContext context)
			: base(context)
		{
			Properties.Add(new Property(MongoDBWriteShared.Names.Operation, typeof(MongoDBWriteOperation), ValueUseOption.DesignTime, MongoDBWriteOperation.Insert));
			Properties.Add(new Property(MongoDBWriteShared.Names.InsertIfNotFound, typeof(bool), ValueUseOption.DesignTime, false));
			Properties.Add(new Property(MongoDBWriteShared.Names.UpdateOperation, typeof(string), ValueUseOption.RuntimeRead, string.Empty));
			Properties.Add(new Property(MongoDBWriteShared.Names.ConnectionString, typeof(string), ValueUseOption.RuntimeRead, string.Empty));
			Properties.Add(new Property(MongoDBWriteShared.Names.Collection, typeof(string), ValueUseOption.RuntimeRead, string.Empty));
			Properties.Add(new Property(MongoDBWriteShared.Names.Criteria, typeof(string), ValueUseOption.RuntimeRead, string.Empty));
			Properties.Add(new Property(MongoDBWriteShared.Names.Data, typeof(object), ValueUseOption.RuntimeRead, string.Empty));
			Initialise();
		}

		public MongoDBWriteDesigner(IFunctionData data, IDesignerContext context)
			: base(data, context)
		{
			Initialise();
		}

		public string ConnectionString
		{
			get { return Properties[MongoDBWriteShared.Names.ConnectionString].Value.ToString(); }
		}

		public string Collection
		{
			get { return Properties[MongoDBWriteShared.Names.Collection].Value.ToString(); }
		}

		public string UpdateOperation
		{
			get { return Properties[MongoDBWriteShared.Names.UpdateOperation].GetValue<string>(); }
			set { Properties[MongoDBWriteShared.Names.UpdateOperation].Value = value; }
		}

		public MongoDBWriteOperation Operation { get; set; }

		public void Validate(CustomValidationResults validations, ValidateDynamicValue validationMethod)
		{
			StringExpressionHelpers.Validate(Properties[MongoDBWriteShared.Names.UpdateOperation], Context, validations, validationMethod);
			StringExpressionHelpers.Validate(Properties[MongoDBWriteShared.Names.Criteria], Context, validations, validationMethod);
		}

		public List<MongoDatabaseObject> GetDatabaseObjects()
		{
			return new MongoDBX(Properties[MongoDBWriteShared.Names.ConnectionString].Value.ToString())
				.GetDatabaseObjects(Properties[MongoDBWriteShared.Names.Collection].Value.ToString());
		}

		protected override void InitializeProperties(System.Collections.Generic.IReadOnlyDictionary<string, IPropertyData> properties)
		{
			base.InitializeProperties(properties);
			StringExpressionHelpers.UpdateOnLoad(Properties[MongoDBWriteShared.Names.Criteria], properties, MongoDBWriteShared.Names.CriteriaExpressions, true);
			StringExpressionHelpers.UpdateOnLoad(Properties[MongoDBWriteShared.Names.UpdateOperation], properties, MongoDBWriteShared.Names.UpdateOperationExpressions, true);
		}

		private void ChangeOperation(MongoDBWriteOperation operation)
		{
			new List<string>
			{
				MongoDBWriteShared.Names.InsertIfNotFound,
				MongoDBWriteShared.Names.UpdateOperation,
				MongoDBWriteShared.Names.Criteria,
				MongoDBWriteShared.Names.Data,
			}.ToList().ForEach(v => { if (Properties.Contains(v)) Properties[v].IsVisible = false; });

			var visible = new List<string>();
			switch (operation)
			{
				case MongoDBWriteOperation.Replace:
					visible = new List<string>
					{
						MongoDBWriteShared.Names.Data,
						MongoDBWriteShared.Names.Criteria,
						MongoDBWriteShared.Names.InsertIfNotFound
					};
					break;
				case MongoDBWriteOperation.Insert:
					visible = new List<string>
					{
						MongoDBWriteShared.Names.Data,
					};
					break;
				case MongoDBWriteOperation.Update:
					visible = new List<string>
					{
						MongoDBWriteShared.Names.Criteria,
						MongoDBWriteShared.Names.UpdateOperation,
						MongoDBWriteShared.Names.InsertIfNotFound,
					};
					break;
				case MongoDBWriteOperation.Delete:
					visible = new List<string>
					{
						MongoDBWriteShared.Names.Criteria,
					};
					break;
				case MongoDBWriteOperation.DeleteAll:
				default:
					break;
			}
			visible.ForEach(v =>
			{
				if (Properties.Contains(v))
				{
					Properties[v].IsVisible = true;
				}
			});
		}

		private void Initialise()
		{
			SetPropertyAttributes();
			ChangeOperation(Properties[MongoDBWriteShared.Names.Operation].GetValue<MongoDBWriteOperation>());
			RefreshExpressionParameters(MongoDBWriteShared.Names.CriteriaExpressions, MongoDBWriteShared.Names.Criteria);
			RefreshExpressionParameters(MongoDBWriteShared.Names.UpdateOperationExpressions, MongoDBWriteShared.Names.UpdateOperation);
		}

		private void SetPropertyAttributes()
		{
			Property connectionStringProperty = Properties[MongoDBWriteShared.Names.ConnectionString];
			connectionStringProperty.Description =
				"Configuration values required by the data provider to connect to the data source.";
			connectionStringProperty.Order = 0;
			connectionStringProperty.Validations.Add(new RequiredValidator());

			Property collection = Properties[MongoDBWriteShared.Names.Collection];
			collection.Description = "The colleciton to write to";
			collection.Order = 1;
			collection.Validations.Add(new RequiredValidator());

			Property operation = Properties[MongoDBWriteShared.Names.Operation];
			operation.ValueChanged += (property, e) => ChangeOperation((property as Property).GetValue<MongoDBWriteOperation>());
			operation.Order = 2;

			Property updateOperation = Properties[MongoDBWriteShared.Names.UpdateOperation];
			updateOperation.Description = "JSON object describing the changes to be made to each database document.";
			updateOperation.Order = 15;
			updateOperation.Validations.Add(new RequiredValidator());
			updateOperation.Editor = typeof(MongoUpdateEditor);
			updateOperation.ValueChanged +=
				(property, e) =>
					RefreshExpressionParameters(MongoDBWriteShared.Names.UpdateOperationExpressions,
						MongoDBWriteShared.Names.UpdateOperation);

			Property criteria = Properties[MongoDBWriteShared.Names.Criteria];
			criteria.Description = "JSON formatted object specifying the search criteria";
			criteria.Editor = typeof(MongoCriteriaEditor);
			criteria.Order = 10;
			criteria.ValueChanged +=
				(property, e) =>
					RefreshExpressionParameters(MongoDBWriteShared.Names.CriteriaExpressions,
						MongoDBWriteShared.Names.Criteria);

			Property insertIfNotFound = Properties[MongoDBWriteShared.Names.InsertIfNotFound];
			insertIfNotFound.Description = "Insert a new record, if no matches are found";
			insertIfNotFound.Order = 12;
			insertIfNotFound.Validations.Add(new RequiredValidator());

			Property data = Properties[MongoDBWriteShared.Names.Data];
			data.Description = "The object to be inserted";
			data.Order = 11;
			data.Validations.Add(new RequiredValidator());
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