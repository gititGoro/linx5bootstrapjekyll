using System;
using System.Collections.Generic;
using System.Linq;
using Twenty57.Linx.Components.Database.Common;
using Twenty57.Linx.Components.Database.DbBulkCopy.Editors.TableChooser;
using Twenty57.Linx.Components.Database.DbBulkCopy.Runtime;
using Twenty57.Linx.Plugin.Common;
using Twenty57.Linx.Plugin.Common.Types;
using Twenty57.Linx.Plugin.Common.Validation;

namespace Twenty57.Linx.Components.Database.DbBulkCopy
{
	public class DbBulkCopyDesigner : DbDesignerBase
	{
		public DbBulkCopyDesigner(IDesignerContext context)
			: base(context)
		{
			Version = FunctionUpdater.Instance.CurrentVersion;

			Properties[DbShared.ConnectionTypePropertyName].IsVisible = false;

			Properties.Add(new Property(DbBulkCopyShared.ConnectionTypePropertyName, typeof(ConnectionType), ValueUseOption.DesignTime, Database.DbBulkCopy.ConnectionType.SqlServer));
			Properties.Add(new Property(DbBulkCopyShared.TimeoutPropertyName, typeof(int), ValueUseOption.DesignTime, 30));
			Properties.Add(new Property(DbBulkCopyShared.TableNamePropertyName, typeof(string), ValueUseOption.DesignTime, string.Empty));
			Properties.Add(new Property(DbBulkCopyShared.ColumnsPropertyName, typeof(DatabaseModel.Columns), ValueUseOption.DesignTime, new DatabaseModel.Columns()) { IsVisible = false });
			Properties.Add(new Property(DbBulkCopyShared.BatchSizePropertyName, typeof(int), ValueUseOption.DesignTime, 10000));

			SetPropertyAttributes();
		}

		public DbBulkCopyDesigner(IFunctionData data, IDesignerContext context) : base(data, context) { }

		public override Common.ConnectionType[] SupportedConnectionTypes
		{
			get
			{
				return ((Database.DbBulkCopy.ConnectionType[])Enum.GetValues(typeof(Database.DbBulkCopy.ConnectionType)))
					.Select(t => (Common.ConnectionType)Enum.Parse(typeof(Common.ConnectionType), t.ToString())).ToArray();
			}
		}

		public string TableName
		{
			get { return Properties[DbBulkCopyShared.TableNamePropertyName].GetValue<string>(); }
			set { Properties[DbBulkCopyShared.TableNamePropertyName].Value = value; }
		}

		public DatabaseModel.Columns TableColumns
		{
			get { return Properties[DbBulkCopyShared.ColumnsPropertyName].GetValue<DatabaseModel.Columns>(); }
			set { Properties[DbBulkCopyShared.ColumnsPropertyName].Value = value; }
		}

		protected override void InitializeProperties(IReadOnlyDictionary<string, IPropertyData> properties)
		{
			base.InitializeProperties(properties);
			SetPropertyAttributes();
		}

		protected override void InitializeExecutionPaths(IReadOnlyDictionary<string, IExecutionPathData> executionPaths)
		{
			base.InitializeExecutionPaths(executionPaths);
			RefreshExecutionPath();
		}

		private void SetPropertyAttributes()
		{
			Properties[DbShared.ConnectionStringPropertyName].Editor = typeof(ConnectionEditorItem);

			Property connectionTypeProperty = Properties[DbBulkCopyShared.ConnectionTypePropertyName];
			connectionTypeProperty.Description = "The type of database to connect to.";
			connectionTypeProperty.Order = 0;
			connectionTypeProperty.ValueChanged += connectionTypeProperty_ValueChanged;

			Property timeoutProperty = Properties[DbBulkCopyShared.TimeoutPropertyName];
			timeoutProperty.Description = "Timeout in seconds. 0 indicates the bulk copy will wait indefinitely.";
			timeoutProperty.Order = 10;
			timeoutProperty.Validations.Add(new RangeValidator(0, Int32.MaxValue));

			Property tableNameProperty = Properties[DbBulkCopyShared.TableNamePropertyName];
			tableNameProperty.Order = 11;
			tableNameProperty.Description = "The name of the destination table.";
			tableNameProperty.Validations.Add(new RequiredValidator());
			tableNameProperty.Editor = typeof(TableChooserItem);
			tableNameProperty.ValueChanged += tableNameProperty_ValueChanged;

			Property batchSizeProperty = Properties[DbBulkCopyShared.BatchSizePropertyName];
			batchSizeProperty.Order = 12;
			batchSizeProperty.Description = "The number of rows to collect in a batch before it is sent to the server.";
		}

		private void connectionTypeProperty_ValueChanged(object sender, EventArgs e)
		{
			ConnectionType = (Common.ConnectionType)Enum.Parse(typeof(Common.ConnectionType), Properties[DbBulkCopyShared.ConnectionTypePropertyName].Value.ToString());
		}

		private void tableNameProperty_ValueChanged(object sender, EventArgs e)
		{
			RefreshExecutionPath();
		}

		private void RefreshExecutionPath()
		{
			var columns = Properties[DbBulkCopyShared.ColumnsPropertyName].GetValue<DatabaseModel.Columns>();
			if (columns.Any())
			{
				TypeReference outputTypeReference = new DynamicCompiledTypeReference(typeof(Loader), _ =>
				{
					var rowTypeBuilder = new TypeBuilder();
					foreach (var nextColumn in columns)
					{
						Type systemType = nextColumn.DataType.GetSystemType();
						bool isNullableType = systemType.IsClass;
						ITypeReference propertyType = isNullableType ? systemType.MapType() : TypeReference.Create(typeof(Nullable<>).MakeGenericType(systemType));

						rowTypeBuilder.AddProperty(
							DbBulkCopyShared.GetPropertyName(nextColumn.Name),
							propertyType);
					}
					return rowTypeBuilder.CreateTypeReference();
				});

				if (ExecutionPaths.Any())
					ExecutionPaths[DbBulkCopyShared.ExecutionPathName].Output = outputTypeReference;
				else
					ExecutionPaths.Add(DbBulkCopyShared.ExecutionPathName, DbBulkCopyShared.ExecutionPathName, outputTypeReference, IterationHint.Once);
			}
			else if (ExecutionPaths.Any())
				ExecutionPaths.Clear();
		}
	}
}
